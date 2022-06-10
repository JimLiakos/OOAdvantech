using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech
{
    /// <MetaDataID>{09d102c4-69a8-4fb2-afb2-65e878048005}</MetaDataID>
    public class SerializeTaskScheduler
    {
        /// <MetaDataID>{64e5f331-062a-4eb4-b473-46c5b67ccdbc}</MetaDataID>
        Queue<Func<Task<bool>>> Tasks = new Queue<Func<Task<bool>>>();
        bool _Runs = true;

        bool Runs
        {
            get
            {
                lock (this)
                    return _Runs;
            }
            set
            {
                lock (this)
                    _Runs=value;
            }
        }

        Task ActiveTask;

        bool SerializeTaskActive;
        /// <MetaDataID>{7582855a-67f6-414d-b930-e6e5d47a3198}</MetaDataID>
        public void RunAsync()
        {
            if (SerializeTaskActive)
                return;
            Task.Run(() =>
            {
                SerializeTaskActive = true;
                while (Runs)
                {
                    lock (Tasks)
                    {
                        if (Tasks.Count > 0)
                        {
                            try
                            {
                                Func<Task<bool>> function = Tasks.Dequeue();
                                var task = Task<bool>.Run(function);
                                task.Wait();
                            }
                            catch (Exception error)
                            {
                            }
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
                SerializeTaskActive = false;

            });
        }

        /// <MetaDataID>{d2b6a2c6-235f-440f-bb23-1e03b90e1d1d}</MetaDataID>
        public void Stop()
        {
            Runs = false;
        }

        /// <MetaDataID>{a01a0261-97b0-43a0-9891-ecccf13eaffb}</MetaDataID>
        public void AddTask(Func<Task<bool>> function)
        {
            Tasks.Enqueue(function);
        }
    }
}
