using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using System;
using System.Net;

namespace Kraken.Powershell
{
    [Cmdlet(VerbsCommon.Optimize, "Image")]
    public class OptimizeImage : PsOptimizeBase
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
                        var task = Client.OptimizeWait(FilePath[x]);
                        adapter.WriteObject(task.Result);
                    }
                    else
                    {
                        var task = Client.Optimize(FilePath[x], new Uri(CallBackUrl));
                        adapter.WriteObject(task.Result);
                    }
                }
            adapter.Finished = true;
            });
            adapter.Listen();
        }
    }
}