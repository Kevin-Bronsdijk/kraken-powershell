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
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0
            )]
        public string[] FileUrl { get; set; }
        
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 9
            )]
        public bool KeepPath { get; set; } = false;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (KeepPath && !string.IsNullOrEmpty(AzurePath))
            {
                throw new ArgumentNullException("Can't use KeepPath and AzurePath at the same time");
            }

            MessageAdapter adapter = new MessageAdapter(this, FileUrl.Count())
            {
                Message = Consts.ProgressMessage,
                Formatter = new ApiResultFormatter()
            };

            Task.Factory.StartNew(() => 
            {
                for (int x = 0; x < FileUrl.Count(); x++)
                {
                    if (Wait)
                    {
                        var request = new OptimizeWaitRequest(new Uri(FileUrl[x]),
                            CreateDataStore(AzureAccount, AzureKey, AzureContainer,
                                HelperFunctions.BuildPath(FileUrl[x], KeepPath, AzurePath, AzureContainer)
                            ))
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
                        var request = new OptimizeRequest(new Uri(FileUrl[x]), new Uri(CallBackUrl),
                            CreateDataStore(AzureAccount, AzureKey, AzureContainer,
                                HelperFunctions.BuildPath(FileUrl[x], KeepPath, AzurePath, AzureContainer)
                            ))
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