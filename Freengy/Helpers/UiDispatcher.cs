// Created 20.10.2016
//
//


namespace Freengy.UI.Helpers 
{
    using System;
    using System.Windows;
    using System.Threading.Tasks;
    using System.Windows.Threading;


    public static class UiDispatcher 
    {
        private static readonly Dispatcher uiDispatcher;


        #region ctor

        static UiDispatcher() 
        {
            uiDispatcher = Application.Current.MainWindow.Dispatcher;
        }

        #endregion ctor


        public static void Invoke(Action method) 
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            uiDispatcher.BeginInvoke(method);
        }

        public static T Invoke<T>(Func<T> function) 
        {
            if (function == null) throw new ArgumentNullException(nameof(function));

            T result = (T)uiDispatcher.Invoke(DispatcherPriority.Background, function);

            return result;
        }
    }
}