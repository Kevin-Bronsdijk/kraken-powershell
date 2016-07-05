using System.Collections;
using System.Management.Automation;
using Kraken.Model;

namespace Kraken.Powershell
{
    public abstract class PsOptimizeExternalStorageBase : PsOptimizeBase
    {
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public OptimizeImageItem[] OptimizeImageItems { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        public Hashtable Headers { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        public Hashtable Metadata { get; set; }

        internal IDataStore SetHeadersMetadata(IDataStore ds, Hashtable headers, Hashtable metadata)
        {
            if (headers != null)
            {
                foreach (DictionaryEntry kvp in headers)
                {
                    var eKey = kvp.Key as string;
                    var eVal = kvp.Value as string;

                    if (!string.IsNullOrEmpty(eKey) && !string.IsNullOrEmpty(eVal))
                    {
                        ds.AddHeaders(eKey, eVal);
                    }
                }
            }

            if (metadata != null)
            {
                foreach (DictionaryEntry kvp in metadata)
                {
                    var eKey = kvp.Key as string;
                    var eVal = kvp.Value as string;

                    if (!string.IsNullOrEmpty(eKey) && !string.IsNullOrEmpty(eVal))
                    {
                        ds.AddMetadata(eKey, eVal);
                    }
                }
            }

            return ds;
        }
    }
}
