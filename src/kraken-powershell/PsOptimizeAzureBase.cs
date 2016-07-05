using System.Management.Automation;
using Kraken.Model.Azure;
using System;

namespace Kraken.Powershell
{
    public abstract class PsOptimizeAzureBase : PsOptimizeExternalStorageBase
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string AzureAccount { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string AzureKey { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string AzureContainer { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string AzurePath { get; set; }


        private void Validation()
        {
            CantUseAzureSharedPropertiesAndOptimizeImageItems();
        }

        private void CantUseAzureSharedPropertiesAndOptimizeImageItems()
        {
            if (OptimizeImageItems != null)
            {
                if (Headers != null || Metadata != null || AzurePath != null)
                {
                    throw new ArgumentException(
                        Consts.CantUseAzureSharedPropertiesAndOptimizeImageItemsMessage);
                }
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            Validation();
        }

        internal DataStore CreateDataStore(OptimizeImageItem item, bool keepPath = false)
        {
            string path;

            if (OptimizeImageItems != null)
            {
                //note: ExternalStoragePath will be ignored when using keepPath
                path = HelperFunctions.BuildPathAzure(item.Path, keepPath, item.ExternalStoragePath, AzureContainer);
            }
            else
            {
                path = HelperFunctions.BuildPathAzure(item.Path, keepPath, AzurePath, AzureContainer);
            }

            var ds = new DataStore(AzureAccount, AzureKey, AzureContainer, path);

            //todo: refactor
            if (OptimizeImageItems != null) 
            {
                ds = SetHeadersMetadata(ds, item.Headers, item.Metadata) as DataStore;
            }
            else
            {
                ds = SetHeadersMetadata(ds, Headers, Metadata) as DataStore;
            }

            return ds;
        }
    }
}