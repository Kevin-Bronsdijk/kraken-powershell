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
            Position = 5
            )]
        public string AzureAccount { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 6
            )]
        public string AzureKey { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 6
            )]
        public string AzureContainer { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 7
            )]
        public string AzurePath { get; set; }

        //[Parameter(
        //Mandatory = false,
        //ValueFromPipelineByPropertyName = true,
        //ValueFromPipeline = true,
        //Position = 9
        //)]
        //public Hashtable Headers { get; set; }

        //[Parameter(
        //Mandatory = false,
        //ValueFromPipelineByPropertyName = true,
        //ValueFromPipeline = true,
        //Position = 10
        //)]
        //public Hashtable Metadata { get; set; }

        internal DataStore CreateDataStore(string account, string key, string container, string path)
        {
            var ds = new DataStore(account, key, container, path);

            //foreach (DictionaryEntry kvp in Headers)
            //{
            //    var eKey = kvp.Key as string;
            //    var eVal = kvp.Value as string;

            //    if (string.IsNullOrEmpty(eKey) && string.IsNullOrEmpty(eVal))
            //    {
            //        ds.AddHeaders(eKey, eVal);
            //    }
            //}

            //foreach (DictionaryEntry kvp in Metadata)
            //{
            //    var eKey = kvp.Key as string;
            //    var eVal = kvp.Value as string;
            //    if (string.IsNullOrEmpty(eKey) && string.IsNullOrEmpty(eVal))
            //    {
            //        ds.AddHeaders(eKey, eVal);
            //    }
            //}

            return ds;
        }
    }
}