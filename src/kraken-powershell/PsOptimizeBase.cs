using System.Management.Automation;
using Kraken.Http;
using System;

namespace Kraken.Powershell
{
    public abstract class PsOptimizeBase : PSCmdlet
    {
        [Parameter(
            DontShow = true
            )]
        public Client Client { get; set; }

        [Parameter(
            DontShow = true
            )]
        public Connection Connection { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string Key { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string Secret { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public bool Wait { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public string CallBackUrl { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public bool Lossy { get; set; } = false;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public bool WebP { get; set; } = false;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        public bool AutoOrient { get; set; } = false;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
            )]
        [ValidateSet ("4:2:0", "4:2:2", "4:4:4")]
        public string SamplingScheme { get; set; } = "4:2:0";

        internal MessageAdapter CreateMessageAdapter(int count)
        {
            MessageAdapter adapter = new MessageAdapter(this, count)
            {
                Message = Consts.ProgressMessage,
                Formatter = new ApiResultFormatter()
            };

            return adapter;
        }

        protected override void BeginProcessing()
        {
            Connection = Connection.Create(Key, Secret);
            Client = new Client(Connection);

            if (!Wait && string.IsNullOrEmpty(CallBackUrl))
            {
                throw new ArgumentException(Consts.CallBackUrlRequiredMesssage);
            }

            base.BeginProcessing();
        }

        protected override void EndProcessing()
        {
            Connection.Dispose();
            Client.Dispose();

            base.EndProcessing();
        }
    }
}