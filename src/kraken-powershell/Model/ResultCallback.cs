namespace Kraken.Powershell.Model
{
    internal class ResultCallback
    {
        public bool Success { get; set; }
        public string Id { get; set; }
        public int StatusCode { get; set; }
        public string Error { get; set; }
    }
}