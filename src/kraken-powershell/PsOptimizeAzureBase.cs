using System.Collections;
using System.Management.Automation;
using Kraken.Model.Azure;

namespace Kraken.Powershell
{
    public abstract class PsOptimizeAzureBase : PsOptimizeBase
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 10
            )]
        public string AzureAccount { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 11
            )]
        public string AzureKey { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 12
            )]
        public string AzureContainer { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 13
            )]
        public string AzurePath { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 14
        )]
        public Hashtable Headers { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 15
        )]
        public Hashtable Metadata { get; set; }

        internal DataStore CreateDataStore(string account, string key, string container, string path)
        {
            var ds = new DataStore(account, key, container, path);

            if (Headers != null)
            {
                foreach (DictionaryEntry kvp in Headers)
                {
                    var eKey = kvp.Key as string;
                    var eVal = kvp.Value as string;

                    if (!string.IsNullOrEmpty(eKey) && !string.IsNullOrEmpty(eVal))
                    {
                        ds.AddHeaders(eKey, eVal);
                    }
                }
            }

            if (Metadata != null)
            {
                foreach (DictionaryEntry kvp in Metadata)
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