// Created by Laxale 20.04.2018
//
//

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Freengy.Base.Interfaces;

using NLog;

using Catel.IoC;
using Catel.Messaging;
using Catel.Services;


namespace Freengy.Base.ViewModels 
{
    public abstract class WaitableViewModel : INotifyPropertyChanged 
    {
        private bool isWaiting;
        private string busyMessage;
        private string information;


        protected WaitableViewModel() 
        {
            Factory = this.GetTypeFactory();
            TaskerWrapper = ServiceLocatorProperty.ResolveType<ITaskWrapper>();
            GUIDispatcher = ServiceLocatorProperty.ResolveType<IGuiDispatcher>();
        }


        protected ITaskWrapper TaskerWrapper { get; }

        protected ITypeFactory Factory { get; }

        protected IGuiDispatcher GUIDispatcher { get; }

        protected Logger NLogger { get; } = LogManager.GetCurrentClassLogger();

        protected IServiceLocator ServiceLocatorProperty { get; } = ServiceLocator.Default;

        protected IMessageMediator Mediator { get; } = MessageMediator.Default;


        /// <summary>
        /// Событие изменения значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Флаг, обратный флагу IsWaiting. Свободна ли вьюмодель от работы.
        /// </summary>
        public bool IsReady => !IsWaiting;

        /// <summary>
        /// Занята ли работой в данный момент вьюмодель.
        /// </summary>
        public bool IsWaiting 
        {
            get => isWaiting;

            private set
            {
                if (isWaiting == value) return;

                isWaiting = value;

                OnPropertyChanged(nameof(IsReady));
                OnPropertyChanged(nameof(IsWaiting));
            }
        }

        /// <summary>
        /// Текст для отображения во время занятости вьюмодели работой или по результату работы.
        /// </summary>
        public string Information 
        {
            get => information;

            private set
            {
                if (information == value) return;

                information = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasInformation));
            }
        }

        public string BusyMessage 
        {
            get => busyMessage;

            private set
            {
                if (busyMessage == value) return;

                busyMessage = value;

                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Returns true if Information property is not empty.
        /// </summary>
        public bool HasInformation => !string.IsNullOrWhiteSpace(Information);


        /// <summary>
        /// Инициализирует вьюмодель. Сюда должна быть вынесена вся длительная логика инициализации, дабы не загружать конструктор (и гуи-поток).
        /// </summary>
        public void Initialize(string initializingMessage) 
        {
            SetBusyState(initializingMessage);

            SetupCommands();

            try
            {
                InitializeImpl();
            }
            finally
            {
                ClearBusyState();
            }
        }


        /// <summary>
        /// Report some message. Can be used in GUI.
        /// </summary>
        /// <param name="message">Some message to show in any way.</param>
        protected void ReportMessage(string message) 
        {
            Information = message;
        }

        /// <summary>
        /// Вызов события изменения значения свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetBusySilent() 
        {
            IsWaiting = true;
        }

        /// <summary>
        /// Установить флаг занятости вьюмодели работой с пояснением.
        /// </summary>
        /// <param name="busyTemplateMessage">Пояснение, какой работой занята вьюмодель.</param>
        protected void SetBusyState(string busyTemplateMessage) 
        {
            IsWaiting = true;
            BusyMessage = busyTemplateMessage;
        }

        /// <summary>
        /// Очистить флаг занятости работой.
        /// </summary>
        protected void ClearBusyState() 
        {
            IsWaiting = false;
            BusyMessage = null;
        }

        /// <summary>
        /// Очистить информационное сообщение вьюмодели.
        /// </summary>
        protected void ClearInformation() 
        {
            Information = null;
        }

        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected virtual void InitializeImpl() { }

        /// <summary>
        /// Инициализировать команды вьюмодели.
        /// </summary>
        protected virtual void SetupCommands() { }
    }
}
