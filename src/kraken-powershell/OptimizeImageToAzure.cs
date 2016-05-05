using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using SeaMist.Model.Azure;

namespace kraken.powershell
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
            if (Wait)
            {
                var tasks = (from file in FilePath
                    let image = File.ReadAllBytes(file)
                    let imageName = Path.GetFileName(file)
                    select KrakenClient.OptimizeWait(image, imageName,
                        new OptimizeUploadWaitRequest(
                            AzureAccount, AzureKey, AzureContainer, AzurePath + imageName)
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

                var tasks = (from file in FilePath
                    let image = File.ReadAllBytes(file)
                    let imageName = Path.GetFileName(file)
                    select KrakenClient.Optimize(image, imageName,
                        new OptimizeUploadRequest(new Uri(CallBackUrl),
                            AzureAccount, AzureKey, AzureContainer, AzurePath + imageName)
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