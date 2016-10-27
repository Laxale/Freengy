// Created by Laxale 25.10.2016
//
//


namespace Freengy.Base.Interfaces 
{
    using System;


    public interface IGuiDispatcher 
    {
        void InvokeOnGuiThread(Action method);

        // doesnt contain Func<T> overload due to problems with Dispatcher deadlock
        // when calling Dispatcher.Invoke(Func<T>)
        // asynchronous Dispatcher.BeginInvoke() doesnt work here as we need immediate Func result
        //T InvokeOnGuiThread<T>(Func<T> function);
    }
}