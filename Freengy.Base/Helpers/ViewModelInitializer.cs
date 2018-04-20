using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Freengy.Base.Messages;

using NLog;

using Catel.Messaging;


namespace Freengy.Base.Helpers 
{
    /// <summary>
    /// Асинхронный инициализатор вьюмоделей.
    /// </summary>
    public class ViewModelInitializer 
    {
        private static readonly object Locker = new object();
        private static readonly Lazy<Logger> lazyLogger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);

        private static ViewModelInitializer instance;


        private ViewModelInitializer() 
        {
            MessageMediator.Default.Register<MessageInitializeModelRequest>(this, OnInitializeRequest);
        }


        /// <summary>
        /// Единственный инстанс <see cref="ViewModelInitializer"/>.
        /// </summary>
        public static ViewModelInitializer Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new ViewModelInitializer());
                }
            }
        }


        private void OnInitializeRequest(MessageInitializeModelRequest initializeRequest) 
        {
            Task
                .Factory
                .StartNew(() => initializeRequest.RequesterModel.Initialize(initializeRequest.InitializingMessage))
                .ContinueWith(task =>
                {
                    if (task.Exception == null) return;

                    string message = task.Exception.GetRootException().Message;
                    System.Windows.MessageBox.Show(message);

                    lazyLogger.Value.Warn(message);
                });
        }
    }
}