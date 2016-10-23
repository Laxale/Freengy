// Created by Laxale 19.10.2016


namespace Freengy.Base.Interfaces 
{
    using System;
    using System.Threading.Tasks;


    /// <summary>
    /// Implementer is capable of attaching continuator async task to a worker task
    /// </summary>
    public interface ITaskWrapper 
    {
        /// <summary>
        /// Attach <paramref name="continuator"/> async task to a <paramref name="method"/> worker task
        /// </summary>
        /// <param name="method">Some worker action</param>
        /// <param name="continuator">Continuator task to handle possible exceptions or whatever</param>
        /// <returns><see cref="Task"/> object that wrapped worker action</returns>
        Task Wrap(Action method, Action<Task> continuator);
    }
}