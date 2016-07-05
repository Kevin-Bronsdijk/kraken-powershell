using System.Collections;

namespace Kraken.Powershell
{
    public class OptimizeImageItem
    {
        public string ExternalStoragePath { get; set; } 
        public Hashtable Headers { get; set; }
        public Hashtable Metadata { get; set; }
        public string Path { get; set; } 
    }
}
