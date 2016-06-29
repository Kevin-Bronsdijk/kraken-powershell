using System;
using System.IO;
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
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0
            )]
        public string[] FilePath { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            MessageAdapter adapter = new MessageAdapter(this, FilePath.Count())
            {
                Message = Consts.ProgressMessage,
                Formatter = new ApiResultFormatter()
            };

            Task.Factory.StartNew(() => 
            {
                for (int x = 0; x < FilePath.Count(); x++)
                {
                    if (Wait)
                    {
                        var request = new OptimizeUploadWaitRequest(
                            CreateDataStore(AzureAccount, AzureKey, AzureContainer,
                            AzurePath + Path.GetFileName(FilePath[x]))
                            )
                        {
                            WebP = WebP,
                            Lossy = Lossy,
                            AutoOrient = AutoOrient,
                            SamplingScheme = HelperFunctions.ConvertSamplingScheme(SamplingScheme)
                        };

                        var task = Client.OptimizeWait(FilePath[x], request);
                        adapter.WriteObject(task.Result);
                    }
                    else
                    {
                        var request = new OptimizeUploadRequest(new Uri(CallBackUrl),
                            CreateDataStore(AzureAccount, AzureKey, AzureContainer,
                            AzurePath + Path.GetFileName(FilePath[x]))
                            )
                        {
                            WebP = WebP,
                            Lossy = Lossy,
                            AutoOrient = AutoOrient,
                            SamplingScheme = HelperFunctions.ConvertSamplingScheme(SamplingScheme)
                        };

                        var task = Client.Optimize(FilePath[x], request);
                        adapter.WriteObject(task.Result);
                    }
                }
                adapter.Finished = true;
            });
            adapter.Listen();
        }
    }
}