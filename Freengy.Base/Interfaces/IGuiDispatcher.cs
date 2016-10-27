// Created by Laxale 25.10.2016
//
//


namespace Freengy.Base.Interfaces 
{
    using System;


    public interface IGuiDispatcher 
    {
        void InvokeOnGuiThread(Action method);

        //T InvokeOnGuiThread<T>(Func<T> function);
    }
}