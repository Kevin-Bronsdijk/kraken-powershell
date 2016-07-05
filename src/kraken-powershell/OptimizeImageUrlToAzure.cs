using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using OptimizeRequest = Kraken.Model.Azure.OptimizeRequest;
using OptimizeWaitRequest = Kraken.Model.Azure.OptimizeWaitRequest;

namespace Kraken.Powershell
{
    [Cmdlet(VerbsCommon.Optimize, "ImageUrlToAzure")]
    public class OptimizeImageUrlToAzure : PsOptimizeAzureBase
    {
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string[] FileUrl { get; set; }
        
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public bool KeepPath { get; set; } = false;

        private void Validation()
        {
            CantUseKeepPathAndAzurePath();

            FileUrlOrOptimizeImageItemsRequired();

            CantUseFileUrlAndOptimizeImageItems();
        }

        private void CantUseKeepPathAndAzurePath()
        {
            if (KeepPath && !string.IsNullOrEmpty(AzurePath))
            {
                throw new ArgumentException(Consts.CantUseKeepPathAndAzurePathMessage);
            }
        }

        private void FileUrlOrOptimizeImageItemsRequired()
        {
            if (FileUrl == null && OptimizeImageItems == null)
            {
                throw new ArgumentException(Consts.FileUrlOrOptimizeImageItemsRequiredMesssage);
            }
        }

        private void CantUseFileUrlAndOptimizeImageItems()
        {
            if (FileUrl != null && OptimizeImageItems != null)
            {
                throw new ArgumentException(Consts.CantUseFileUrlAndOptimizeImageItemsMessage);
            }
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            Validation();

            var items = OptimizeImageItems;
            if (FileUrl != null)
            {
                items = HelperFunctions.CreateOptimizeImageItems(FileUrl).ToArray();
            }

            MessageAdapter adapter = CreateMessageAdapter(items.Count());

            Task.Factory.StartNew(() => 
            {
                for (int x = 0; x < items.Count(); x++)
                {
                    if (Wait)
                    {
                        var request = new OptimizeWaitRequest(new Uri(items[x].Path),
                            CreateDataStore(items[x], KeepPath))
                        {
                            WebP = WebP,
                            Lossy = Lossy,
                            AutoOrient = AutoOrient,
                            SamplingScheme = HelperFunctions.ConvertSamplingScheme(SamplingScheme)
                        };

                        var task = Client.OptimizeWait(request);
                        adapter.WriteObject(task.Result);
                    }
                    else
                    {
                        var request = new OptimizeRequest(new Uri(items[x].Path), new Uri(CallBackUrl),
                            CreateDataStore(items[x], KeepPath))
                        {
                            WebP = WebP,
                            Lossy = Lossy,
                            AutoOrient = AutoOrient,
                            SamplingScheme = HelperFunctions.ConvertSamplingScheme(SamplingScheme)
                        };

                        var task = Client.Optimize(request);
                        adapter.WriteObject(task.Result);
                    }
                }
                adapter.Finished = true;
            });
            adapter.Listen();
        }
    }
}