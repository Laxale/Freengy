// Created by Laxale 25.10.2016
//
//

using System;


namespace Freengy.Base.Interfaces 
{
    public interface IGuiDispatcher 
    {
        void InvokeOnGuiThread(Action method);

        void BeginInvokeOnGuiThread(Action method);

        // doesnt contain Func<T> overload due to problems with Dispatcher deadlock
        // when calling Dispatcher.Invoke(Func<T>)
        // asynchronous Dispatcher.BeginInvoke() doesnt work here as we need immediate Func result
        //T InvokeOnGuiThread<T>(Func<T> function);
    }
}