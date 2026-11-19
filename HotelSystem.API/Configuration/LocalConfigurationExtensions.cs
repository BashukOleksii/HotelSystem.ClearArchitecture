namespace HotelSystem.API.Configuration;

public static class LocalConfigurationExtensions
{
    public static ConfigurationManager AddLocalDevelopmentConfiguration(
        this ConfigurationManager configuration, IHostEnvironment environment, string[] args)
    {
        if (!environment.IsDevelopment()) return configuration;

        configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: false);
        // Explicit process/deployment settings keep priority over the local development file.
        configuration.AddEnvironmentVariables();
        if (args.Length > 0) configuration.AddCommandLine(args);
        return configuration;
    }
}
