namespace Kraken.Powershell
{
    internal static class Consts
    {
        public const int TimeoutInMilliseconds = 1800000;

        // No Localization
        public const string ProgressMessage = "Calling Kraken.io API";
        public const string FileDownloadProgressMessage = "Downloading files";
        public const string CallBackUrlRequiredMesssage = "CallBackUrl can\'t be empty when Wait is false";
    }
}