﻿using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using SeaMist;
using SeaMist.Http;
using SeaMist.Model;

namespace kraken.powershell
{
    [Cmdlet(VerbsCommon.Optimize, "Image")]
    public class OptimizeImage : PSCmdlet
    {
        private KrakenClient _krakenClient;
        private KrakenConnection _krakenConnection;

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0
            )]
        public string[] FilePath { get; set; }

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
            _krakenConnection = KrakenConnection.Create(Key, Secret);
            _krakenClient = new KrakenClient(_krakenConnection);

            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            if (Wait)
            {
                var tasks = (from file in FilePath
                    let image = File.ReadAllBytes(file)
                    let imageName = Path.GetFileName(file)
                    select _krakenClient.OptimizeWait(image, imageName, new OptimizeUploadWaitRequest()
                        )).ToList();

                Task.WaitAll(tasks.Cast<Task>().ToArray());

                foreach (var task in tasks)
                {
                    WriteObject(task.Result.Body);
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
                    select _krakenClient.Optimize(image, imageName, new OptimizeUploadRequest(new Uri(CallBackUrl))
                        )).ToList();

                Task.WaitAll(tasks.Cast<Task>().ToArray());

                foreach (var task in tasks)
                {
                    WriteObject(task.Result.Body);
                }
            }

            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            _krakenConnection.Dispose();
            _krakenClient.Dispose();

            base.EndProcessing();
        }
    }
}