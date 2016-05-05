using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using OptimizeRequest = SeaMist.Model.Azure.OptimizeRequest;
using OptimizeWaitRequest = SeaMist.Model.Azure.OptimizeWaitRequest;

namespace kraken.powershell
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
            Position = 8
            )]
        public bool KeepPath { get; set; } = false;

        protected override void ProcessRecord()
        {
            if (Wait)
            {
                var tasks = (from url in FileUrl
                    select KrakenClient.OptimizeWait(
                        new OptimizeWaitRequest(new Uri(url),
                            AzureAccount, AzureKey, AzureContainer,
                            HelperFunctions.BuildAzurePath(url, KeepPath, AzurePath))
                        )).ToList();

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

                var tasks = (from url in FileUrl
                    select KrakenClient.Optimize(
                        new OptimizeRequest(new Uri(url), new Uri(CallBackUrl),
                            AzureAccount, AzureKey, AzureContainer,
                            HelperFunctions.BuildAzurePath(url, KeepPath, AzurePath))
                        )).ToList();

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