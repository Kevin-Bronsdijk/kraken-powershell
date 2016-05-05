using System.Management.Automation;
using SeaMist;
using SeaMist.Http;

namespace kraken.powershell
{
    public abstract class PsOptimizeBase : PSCmdlet
    {
        [Parameter(
            DontShow = true
            )]
        public KrakenClient KrakenClient { get; set; }

        [Parameter(
            DontShow = true
            )]
        public KrakenConnection KrakenConnection { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 1
            )]
        public string Key { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 2
            )]
        public string Secret { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 3
            )]
        public bool Wait { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 4
            )]
        public string CallBackUrl { get; set; }

        protected override void BeginProcessing()
        {
            KrakenConnection = KrakenConnection.Create(Key, Secret);
            KrakenClient = new KrakenClient(KrakenConnection);

            base.BeginProcessing();
        }

        protected override void EndProcessing()
        {
            KrakenConnection.Dispose();
            KrakenClient.Dispose();

            base.EndProcessing();
        }
    }
}