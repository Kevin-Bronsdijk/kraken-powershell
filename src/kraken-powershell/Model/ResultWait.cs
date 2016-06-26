namespace Kraken.Powershell.Model
{
    internal class ResultWait
    {
        public bool Success { get; set; }
        public string FileName { get; set; }
        public int OriginalSize { get; set; }
        public int KrakedSize { get; set; }
        public int SavedBytes { get; set; }
        public string KrakedUrl { get; set; }
        public int StatusCode { get; set; }
        public string Error { get; set; }
    }
}