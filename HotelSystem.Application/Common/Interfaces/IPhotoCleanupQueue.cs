namespace HotelSystem.Application.Common.Interfaces;
public interface IPhotoCleanupQueue
{
    void Enqueue(string storageKey);
}
