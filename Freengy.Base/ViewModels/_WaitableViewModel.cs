// Created by Laxale 19.10.2016
//
//

using System.Threading.Tasks;

using Freengy.Base.Interfaces;
using Freengy.Base.Extensions;

using Catel.IoC;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using Catel.Messaging;

using NLog;


namespace Freengy.Base.ViewModels 
{
    public abstract class _WaitableViewModel : ViewModelBase, IRefreshable 
    {
        protected readonly ITaskWrapper taskWrapper;
        protected readonly ITypeFactory typeFactory;
        protected readonly IGuiDispatcher guiDispatcher;
        protected readonly IUIVisualizerService uiVisualizer;
        protected readonly Logger logger = LogManager.GetCurrentClassLogger();
        protected readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        protected readonly IMessageMediator messageMediator = MessageMediator.Default;


        protected _WaitableViewModel() 
        {
            typeFactory = this.GetTypeFactory();
            taskWrapper = serviceLocator.ResolveType<ITaskWrapper>();
            guiDispatcher = serviceLocator.ResolveType<IGuiDispatcher>();
            uiVisualizer = serviceLocator.ResolveType<IUIVisualizerService>();

            DeferValidationUntilFirstSaveCall = false;
        }


        public virtual void Refresh() { }

        public virtual void ReportMessage(string information) 
        {
            Information = information;
        }


        /// <summary>
        /// Set the IsWaiting flag and report message.
        /// </summary>
        protected void SetBusySilent() 
        {
            IsWaiting = true;
        }

        /// <summary>
        /// Set the IsWaiting flag and report message.
        /// </summary>
        protected void SetBusy(string message) 
        {
            IsWaiting = true;
            ReportMessage(message);
        }

        /// <summary>
        /// Clear the IsWaiting flag.
        /// </summary>
        protected void ClearBusyState() 
        {
            IsWaiting = false;
        }

        /// <summary>
        /// This is called in InitializeAsync - force coderast to not init commands manually
        /// </summary>
        protected abstract void SetupCommands();

        protected virtual void InitializationContinuator(Task parentTask) 
        {
            if (parentTask.Exception != null)
            {
                // show error dialog?
                logger.Error(parentTask.Exception.GetReallyRootException(), $"{ GetType() } initialization failed");
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// To auto-initialize use Catel controls for viewmodels - they call InitializeAsync() internally
        /// </summary>
        /// <returns>Initializing <see cref="Task"/>.</returns>
        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            SetupCommands();
        }

        public string Information 
        {
            get => (string)GetValue(InformationProperty);

            protected set
            {
                SetValue(InformationProperty, value);

                SetValue(HasInformationProperty, !string.IsNullOrWhiteSpace(value));
            }
        }

        /// <summary>
        /// Represents long-running task flag of the viewmodel.
        /// </summary>
        public bool IsWaiting 
        {
            get => (bool)GetValue(IsWaitingProperty);

            protected set => SetValue(IsWaitingProperty, value);
        }

        public bool HasInformation 
        {
            get => (bool)GetValue(HasInformationProperty);

            protected set => SetValue(HasInformationProperty, value);
        }


        #region property data

        public static readonly PropertyData IsWaitingProperty =
            RegisterProperty<WaitableViewModel, bool>(waitViewModel => waitViewModel.IsWaiting, () => false);
        
        public static readonly PropertyData InformationProperty =
            RegisterProperty<WaitableViewModel, string>(waitViewModel => waitViewModel.Information, () => string.Empty);

        public static readonly PropertyData HasInformationProperty =
            RegisterProperty<_WaitableViewModel, bool>(waitViewModel => waitViewModel.HasInformation, () => false);

        #endregion property data
    }
}