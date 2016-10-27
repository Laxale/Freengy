// Created by Laxale 20.10.2016
//
//


namespace Freengy.UI.Helpers
{
    using System;
    using System.Windows;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    using Freengy.Base.Interfaces;


    /// <summary>
    /// Do not add dispatcher.Invoke() - hangs in deadlock
    /// </summary>
    internal sealed class UiDispatcher : IGuiDispatcher
    {
        private static readonly Dispatcher uiDispatcher;


        #region Singleton

        private static UiDispatcher instance;
        

        static UiDispatcher()
        {
            uiDispatcher = Application.Current.MainWindow.Dispatcher;
        }

        private UiDispatcher()
        {

        }

        public static UiDispatcher Instance => UiDispatcher.instance ?? (UiDispatcher.instance = new UiDispatcher());

        #endregion Singleton


        public void InvokeOnGuiThread(Action method) 
        {
            Invoke(method);
        }

        internal static void Invoke(Action method) 
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            uiDispatcher.BeginInvoke(method);
        }
    }
}