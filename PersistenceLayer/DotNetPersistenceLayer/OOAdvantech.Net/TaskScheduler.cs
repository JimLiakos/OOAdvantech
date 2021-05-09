using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech
{
    public class SerializeTaskScheduler
    {
        Queue<System.Threading.Tasks.Task> Tasks = new Queue<System.Threading.Tasks.Task>();
        bool Runs = true;
        public void RunAsync()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                while (Runs)
                {
                    if (Tasks.Count > 0)
                    {
                        var task = Tasks.Dequeue();
                        task.Start();
                        task.Wait();
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            });
        }

        public void Stop()
        {
            Runs = false;
        }

        public void AddTask(System.Threading.Tasks.Task task )
        {
            Tasks.Enqueue(task);
        }
    }
}
