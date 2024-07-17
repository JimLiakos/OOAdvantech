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
        public SerializeTaskScheduler()
        {

        }
        /// <MetaDataID>{64e5f331-062a-4eb4-b473-46c5b67ccdbc}</MetaDataID>
        Queue<Func<Task<bool>>> Tasks = new Queue<Func<Task<bool>>>();
        bool _Runs = true;
        Queue<ISerializableTask> Tasks_b = new Queue<ISerializableTask>();


        public bool Runs
        {
            get
            {
                lock (this)
                    return _Runs;
            }
            set
            {
                lock (this)
                    _Runs = value;
            }
        }

        Task ActiveTask;

        bool SerializeTaskActive;
        private TaskCompletionSource<bool> TaskCompletionSource;

        object TaskSchedulerLock = new object();





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
                    lock (Tasks_b)
                    {
                        if (Tasks_b.Count > 0)
                        {
                            //try
                            //{

                            //    Func<Task<bool>> function = null;
                            //    lock (TaskSchedulerLock)
                            //        function = Tasks.Dequeue();
                            //    var task = Task<bool>.Run(function);
                            //    task.Wait();
                            //}
                            //catch (Exception error)
                            //{
                            //}
                            //lock (TaskSchedulerLock)
                            //{
                            //    if(Tasks.Count==0)
                            //        TaskCompletionSource.SetResult(true);
                            //}

                            try
                            {
                                ISerializableTask serializableTask = null;

                                lock (TaskSchedulerLock)
                                    serializableTask = Tasks_b.Dequeue();



                                serializableTask.Run();

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

        ///// <MetaDataID>{a01a0261-97b0-43a0-9891-ecccf13eaffb}</MetaDataID>
        //public void AddTask(Func<Task<bool>> function)
        //{
        //    lock (TaskSchedulerLock)
        //    {
        //        if (TaskCompletionSource == null || TaskCompletionSource.Task.Status != TaskStatus.Running)
        //            TaskCompletionSource = new System.Threading.Tasks.TaskCompletionSource<bool>();
        //        Tasks.Enqueue(function);
        //    }
        //}

        public Task<TResult> AddTask<TResult>(Func<Task<TResult>> function)
        {
            lock (TaskSchedulerLock)
            {
                if (TaskCompletionSource == null || TaskCompletionSource.Task.Status != TaskStatus.Running)
                    TaskCompletionSource = new System.Threading.Tasks.TaskCompletionSource<bool>();

                SerializableTask<TResult> serializableTask = new SerializableTask<TResult>() { asynchFunction = function, taskCompletionSource = new TaskCompletionSource<TResult>() };
                Tasks_b.Enqueue(serializableTask);
                return serializableTask.taskCompletionSource.Task;
            }

        }

        public Task<TResult> AddTask<TResult>(Func<TResult> function)
        {

            lock (TaskSchedulerLock)
            {
                if (TaskCompletionSource == null || TaskCompletionSource.Task.Status != TaskStatus.Running)
                    TaskCompletionSource = new System.Threading.Tasks.TaskCompletionSource<bool>();

                SerializableTask<TResult> serializableTask = new SerializableTask<TResult> { synchFunction = function, taskCompletionSource = new TaskCompletionSource<TResult>() };
                Tasks_b.Enqueue(serializableTask);
                return serializableTask.taskCompletionSource.Task;
            }

            //lock (TaskSchedulerLock)
            //{
            //    if (TaskCompletionSource == null || TaskCompletionSource.Task.Status != TaskStatus.Running)
            //        TaskCompletionSource = new System.Threading.Tasks.TaskCompletionSource<bool>();
            //    Tasks.Enqueue(function);
            //}
            return null;
        }


        public bool Wait(TimeSpan timeout)
        {
            TaskCompletionSource<bool> taskCompletionSource = null;

            lock (TaskSchedulerLock)
                taskCompletionSource = TaskCompletionSource;

            if (taskCompletionSource != null)
                return taskCompletionSource.Task.Wait(timeout);

            return true;

        }
        public void Wait()
        {
            TaskCompletionSource<bool> taskCompletionSource = null;

            lock (TaskSchedulerLock)
                taskCompletionSource = TaskCompletionSource;

            if (taskCompletionSource != null)
                taskCompletionSource.Task.Wait();

            return;

        }
    }

    /// <MetaDataID>{5ce5220a-6f15-4bd3-98e0-9df5c3f0a205}</MetaDataID>
    interface ISerializableTask
    {
        void Run();
    }

    /// <MetaDataID>{5cecddd7-e56f-470a-b902-43113b123832}</MetaDataID>
    class SerializableTask<TResult> : ISerializableTask
    {
        public Func<Task<TResult>> asynchFunction;

        public Func<TResult> synchFunction;
        public TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
        public void Run()
        {
            try
            {
                if (asynchFunction != null)
                {
                    var task = asynchFunction();
                    task.Wait();
                    taskCompletionSource.SetResult(task.Result);

                }
                else if (taskCompletionSource != null)
                {
                    var task = Task<TResult>.Run(synchFunction);
                    task.Wait();
                    taskCompletionSource.SetResult(synchFunction());
                }
            }
            catch (AggregateException agrError)
            {
                taskCompletionSource.SetException(agrError.InnerException);
            }
            catch (Exception error)
            {
                taskCompletionSource.SetException(error);
            }

        }

    }
}
