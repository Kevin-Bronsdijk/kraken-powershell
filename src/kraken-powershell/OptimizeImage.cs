using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using SeaMist.Model;

namespace kraken.powershell
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
            if (Wait)
            {
                var tasks = (from file in FilePath
                    let image = File.ReadAllBytes(file)
                    let imageName = Path.GetFileName(file)
                    select KrakenClient.OptimizeWait(image, imageName, new OptimizeUploadWaitRequest()
                        )).ToList();

                Task.WaitAll(tasks.Cast<Task>().ToArray(), Consts.TimeoutInMilliseconds);

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
                    select KrakenClient.Optimize(image, imageName, new OptimizeUploadRequest(new Uri(CallBackUrl))
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