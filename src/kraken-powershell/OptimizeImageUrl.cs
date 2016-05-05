using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using SeaMist.Model;

namespace kraken.powershell
{
    [Cmdlet(VerbsCommon.Optimize, "ImageUrl")]
    public class OptimizeImageUrl : PsOptimizeBase
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0
            )]
        public string[] FileUrl { get; set; }

        protected override void ProcessRecord()
        {
            if (Wait)
            {
                var tasks = FileUrl.Select(url => KrakenClient.OptimizeWait(
                    new OptimizeWaitRequest(new Uri(url)))).ToList();

                Task.WaitAll(tasks.Cast<Task>().ToArray());

                foreach (var task in tasks)
                {
                    WriteObject(HelperFunctions.CreateReturnObject(task.Result));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(CallBackUrl))
                {
                    throw new ArgumentNullException($"CallBackUrl can\'t be empty when Wait is false");
                }

                var tasks = FileUrl.Select(url => KrakenClient.Optimize(
                    new OptimizeRequest(new Uri(url), new Uri(CallBackUrl)))).ToList();

                Task.WaitAll(tasks.Cast<Task>().ToArray());

                foreach (var task in tasks)
                {
                    WriteObject(HelperFunctions.CreateReturnObject(task.Result));
                }
            }

            base.ProcessRecord();
        }
    }
}