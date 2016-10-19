// Created 19.10.2016
//
//


namespace Freengy.Base.DefaultImpl 
{
    using System;
    using System.Threading.Tasks;

    using Freengy.Base.Interfaces;


    /// <summary>
    /// Default <see cref="ITaskWrapper"/> implementer
    /// </summary>
    public class TaskWrapper : ITaskWrapper 
    {
        public Task Wrap(Action action, Action<Task> continuator) 
        {
            var wrapperTask = Task.Factory.StartNew(action).ContinueWith(continuator);

            return wrapperTask;
        }
    }
}