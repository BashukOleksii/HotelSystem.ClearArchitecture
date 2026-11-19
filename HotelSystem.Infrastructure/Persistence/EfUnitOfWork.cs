using System.Data;
using HotelSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace HotelSystem.Infrastructure.Persistence;

public sealed class EfUnitOfWork(HotelDbContext db, ILogger<EfUnitOfWork> logger) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => db.SaveChangesAsync(cancellationToken);

    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);
        db.EnsureUsable(); 
        cancellationToken.ThrowIfCancellationRequested();

        if (db.Database.CurrentTransaction is not null) throw new InvalidOperationException("Вкладені транзакції цим контрактом не підтримуються.");
        if (db.ChangeTracker.HasChanges()) throw new InvalidOperationException("Зміни мають починатися всередині callback транзакції.");
        

        db.ChangeTracker.Clear();
        IDbContextTransaction? transaction = null;
        try
        {
            transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            await action(cancellationToken);
            if (db.ChangeTracker.HasChanges())
                throw new InvalidOperationException("Callback повинен явно зберегти всі зміни перед завершенням транзакції.");
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception error)
        {
            db.MarkFaulted();
            if (transaction is not null)
            {
                try { await transaction.RollbackAsync(CancellationToken.None); }
                catch (Exception rollbackError) { logger.LogWarning(rollbackError, "Rollback failed; discard this unit of work."); }
            }
            if (PersistenceErrors.Translate(error) is { } translated) throw translated;
            throw;
        }
        finally { if (transaction is not null) await transaction.DisposeAsync(); }
    }
}
