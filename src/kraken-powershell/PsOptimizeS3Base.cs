using System.Collections;
using System.Management.Automation;
using Kraken.Model.S3;

namespace Kraken.Powershell
{
    public abstract class PsOptimizeS3Base : PsOptimizeBase
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 10
            )]
        public string AmazonKey { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 11
            )]
        public string AmazonSecret { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 12
            )]
        public string AmazonBucket { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 13
            )]
        public string S3Path { get; set; }

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

        internal DataStore CreateDataStore(string key, string secret, string bucket, string path)
        {
            var ds = new DataStore(key, secret, bucket, string.Empty)
            {
                Path = path
            };

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