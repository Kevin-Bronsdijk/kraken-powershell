using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using OptimizeRequest = Kraken.Model.S3.OptimizeRequest;
using OptimizeWaitRequest = Kraken.Model.S3.OptimizeWaitRequest;

namespace Kraken.Powershell
{
    [Cmdlet(VerbsCommon.Optimize, "ImageUrlToS3")]
    public class OptimizeImageUrlToS3 : PsOptimizeS3Base
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
            base.ProcessRecord();

            if (KeepPath && !string.IsNullOrEmpty(S3Path))
            {
                throw new ArgumentNullException("Can't use KeepPath and S3Path at the same time");
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
                            CreateDataStore(AmazonKey, AmazonSecret, AmazonBucket,
                                HelperFunctions.BuildPath(FileUrl[x], KeepPath, S3Path, AmazonBucket)
                            ));

                        var task = Client.OptimizeWait(request);
                        adapter.WriteObject(task.Result);
                    }
                    else
                    {
                        var request = new OptimizeRequest(new Uri(FileUrl[x]), new Uri(CallBackUrl),
                            CreateDataStore(AmazonKey, AmazonSecret, AmazonBucket,
                                HelperFunctions.BuildPath(FileUrl[x], KeepPath, S3Path, AmazonBucket)
                            ));

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