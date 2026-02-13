namespace BootSentry.Core;

public static class Constants
{
    public static class Registry
    {
        public const string DisabledKeyPath = @"Software\BootSentry\Disabled";
    }

    public static class Messages
    {
        // Internal messages that might be displayed if translation fails or for logging
        public const string SuspiciousCommandLine = "Suspicious command line detected";
    }
}
