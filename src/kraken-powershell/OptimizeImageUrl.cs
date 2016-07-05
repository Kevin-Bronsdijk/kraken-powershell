using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace Kraken.Powershell
{
    [Cmdlet(VerbsCommon.Optimize, "ImageUrl")]
    public class OptimizeImageUrl : PsOptimizeBase
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string[] FileUrl { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            MessageAdapter adapter = CreateMessageAdapter(FileUrl.Count());

            Task.Factory.StartNew(() => 
            {
                for (int x = 0; x < FileUrl.Count(); x++)
                {
                    if (Wait)
                    {
                        var request = new Kraken.Model.OptimizeWaitRequest(new Uri(FileUrl[x]))
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
                        var request = new Kraken.Model.OptimizeRequest(new Uri(FileUrl[x]), new Uri(CallBackUrl))
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