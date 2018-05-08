using System;
using System.Threading.Tasks;

using Freengy.Base.Messages;
using Freengy.Base.DefaultImpl;

using NLog;


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
            this.Subscribe<MessageInitializeModelRequest>(OnInitializeRequest);
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