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

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 5
            )]
        public bool Lossy { get; set; } = false;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 6
            )]
        public bool WebP { get; set; } = false;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 7
            )]
        public bool AutoOrient { get; set; } = false;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 8
            )]
        [ValidateSet ("4:2:0", "4:2:2", "4:4:4")]
        public string SamplingScheme { get; set; } = "4:2:0";

        protected override void BeginProcessing()
        {
            Connection = Connection.Create(Key, Secret);
            Client = new Client(Connection);

            if (!Wait && string.IsNullOrEmpty(CallBackUrl))
            {
                throw new ArgumentNullException(Consts.CallBackUrlRequiredMesssage);
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