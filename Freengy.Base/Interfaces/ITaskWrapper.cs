// Created by Laxale 19.10.2016


namespace Freengy.Base.Interfaces 
{
    using System;
    using System.Threading.Tasks;


    public interface ITaskWrapper 
    {
        Task Wrap(Action method, Action<Task> continuator);
    }
}