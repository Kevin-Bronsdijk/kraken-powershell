namespace Kraken.Powershell
{
    internal static class Consts
    {
        public const int TimeoutInMilliseconds = 1800000;
        public const string AzureBlobUrl = "blob.core.windows.net";

        // No Localization
        public const string ProgressMessage = "Calling Kraken.io API";
        public const string FileDownloadProgressMessage = "Downloading files";
        public const string CallBackUrlRequiredMesssage = "CallBackUrl can\'t be empty when Wait is false";
        public const string FilePathOrOptimizeImageItemsRequiredMesssage = "Use -FilePath or -OptimizeImageItems";
        public const string CantUseFilePathAndOptimizeImageItemsMessage = "Can't use -FilePath and -OptimizeImageItems at the same time";
        public const string CantUseKeepPathAndAzurePathMessage = "Can't use -KeepPath and -AzurePath at the same time";
        public const string FileUrlOrOptimizeImageItemsRequiredMesssage = "Use -FileUrl or -OptimizeImageItems";
        public const string CantUseFileUrlAndOptimizeImageItemsMessage = "Can't use -FileUrl and -OptimizeImageItems at the same time";
        public const string CantUseKeepPathAndS3PathMessage = "Can't use -KeepPath and -S3Path at the same time";

        public const string CantUseS3SharedPropertiesAndOptimizeImageItemsMessage = "Can't use shared properties (-Headers, -Metadata -S3Path) when using -OptimizeImageItems";
        public const string CantUseAzureSharedPropertiesAndOptimizeImageItemsMessage = "Can't use shared properties (-Headers, -Metadata -AzurePath) when using -OptimizeImageItems";
    }
}