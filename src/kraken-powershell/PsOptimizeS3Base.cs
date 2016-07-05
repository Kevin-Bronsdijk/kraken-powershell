using System.Management.Automation;
using Kraken.Model.S3;
using System;

namespace Kraken.Powershell
{
    public abstract class PsOptimizeS3Base : PsOptimizeExternalStorageBase
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string AmazonKey { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string AmazonSecret { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string AmazonBucket { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string S3Path { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string AmazonRegion { get; set; } = string.Empty;

        private void Validation()
        {
            CantUseS3SharedPropertiesAndOptimizeImageItems();
        }

        private void CantUseS3SharedPropertiesAndOptimizeImageItems()
        {
            if (OptimizeImageItems != null)
            {
                if (Headers != null || Metadata != null || S3Path != null)
                {
                    throw new ArgumentException(
                        Consts.CantUseS3SharedPropertiesAndOptimizeImageItemsMessage);
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
                path = HelperFunctions.BuildPathS3(item.Path, keepPath, item.ExternalStoragePath, AmazonBucket);
            }
            else
            {
                path = HelperFunctions.BuildPathS3(item.Path, keepPath, S3Path, AmazonBucket);
            }

            var ds = new DataStore(AmazonKey, AmazonSecret, AmazonBucket, AmazonRegion)
            {
                Path = path
            };
            
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