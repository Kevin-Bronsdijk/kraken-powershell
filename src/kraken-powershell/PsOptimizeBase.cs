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