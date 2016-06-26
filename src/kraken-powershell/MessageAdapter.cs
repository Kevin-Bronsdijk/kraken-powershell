using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;

namespace Kraken.Powershell
{
    public class MessageAdapter
    {
        private Cmdlet Cmdlet { get; set; }
        private Queue<object> Queue { get; set; } = new Queue<object>();
        private object LockToken { get; set; } = new object();
        public bool Finished { get; set; } = false;
        public int Total { get; set; }
        public int Count { get; set; }
        public string Message { get; set; }
        public IResultFormatter Formatter { get; set; }

        public MessageAdapter(Cmdlet cmdlet, int total)
        {
            Cmdlet = cmdlet;
            Total = total;
        }

        public void Listen()
        {
            ProgressRecord progress = new ProgressRecord(1, Message, " ");

            while (!Finished || Queue.Count > 0)
            {
                while (Queue.Count > 0)
                {
                    progress.PercentComplete = Count * 100 / Total;
                    progress.StatusDescription = (Count) + "//" + Total;
                    Cmdlet.WriteProgress(progress);

                    Cmdlet.WriteObject(Formatter.FormatObject(Queue.Dequeue()));

                    progress.PercentComplete = ++Count * 100 / Total;
                    progress.StatusDescription = (Count) + "/" + Total;
                    Cmdlet.WriteProgress(progress);
                }
            }

            Thread.Sleep(500);
        }

        public void WriteObject(object obj)
        {
            lock (LockToken)
                Queue.Enqueue(obj);
        }
    }
}
