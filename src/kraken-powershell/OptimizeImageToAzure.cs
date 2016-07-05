using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Kraken.Model.Azure;

namespace Kraken.Powershell
{
    [Cmdlet(VerbsCommon.Optimize, "ImageToAzure")]
    public class OptimizeImageToAzure : PsOptimizeAzureBase
    {
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string[] FilePath { get; set; }

        private void Validation()
        {
            FilePathOrOptimizeImageItemsRequired();

            CantUseFilePathAndOptimizeImageItems();
        }

        private void CantUseFilePathAndOptimizeImageItems()
        {
            if (FilePath != null && OptimizeImageItems != null)
            {
                throw new ArgumentException(Consts.CantUseFilePathAndOptimizeImageItemsMessage);
            }
        }

        private void FilePathOrOptimizeImageItemsRequired()
        {
            if (FilePath == null && OptimizeImageItems == null)
            {
                throw new ArgumentException(Consts.FilePathOrOptimizeImageItemsRequiredMesssage);
            }
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            Validation();

            var items = OptimizeImageItems;
            if (FilePath != null)
            {
                items = HelperFunctions.CreateOptimizeImageItems(FilePath).ToArray();
            }

            MessageAdapter adapter = CreateMessageAdapter(items.Count());

            Task.Factory.StartNew(() => 
            {
                for (int x = 0; x < items.Count(); x++)
                {
                    if (Wait)
                    {
                        var request = new OptimizeUploadWaitRequest(
                            CreateDataStore(items[x]))
                        {
                            WebP = WebP,
                            Lossy = Lossy,
                            AutoOrient = AutoOrient,
                            SamplingScheme = HelperFunctions.ConvertSamplingScheme(SamplingScheme)
                        };

                        var task = Client.OptimizeWait(items[x].Path, request);
                        adapter.WriteObject(task.Result);
                    }
                    else
                    {
                        var request = new OptimizeUploadRequest(new Uri(CallBackUrl),
                            CreateDataStore(items[x]))
                        {
                            WebP = WebP,
                            Lossy = Lossy,
                            AutoOrient = AutoOrient,
                            SamplingScheme = HelperFunctions.ConvertSamplingScheme(SamplingScheme)
                        };

                        var task = Client.Optimize(items[x].Path, request);
                        adapter.WriteObject(task.Result);
                    }
                }
                adapter.Finished = true;
            });
            adapter.Listen();
        }
    }
}