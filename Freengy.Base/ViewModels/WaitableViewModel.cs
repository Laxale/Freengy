// Created by Laxale 19.10.2016
//
//

using System.Threading.Tasks;

using Freengy.Base.Interfaces;

using Catel.IoC;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using Catel.Messaging;


namespace Freengy.Base.ViewModels 
{
    public abstract class WaitableViewModel : ViewModelBase, IRefreshable 
    {
        protected readonly ITaskWrapper taskWrapper;
        protected readonly ITypeFactory typeFactory;
        protected readonly IGuiDispatcher guiDispatcher;
        protected readonly IUIVisualizerService uiVisualizer;
        protected readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        protected readonly IMessageMediator messageMediator = MessageMediator.Default;


        protected WaitableViewModel() 
        {
            typeFactory = this.GetTypeFactory();
            taskWrapper = serviceLocator.ResolveType<ITaskWrapper>();
            guiDispatcher = serviceLocator.ResolveType<IGuiDispatcher>();
            uiVisualizer = serviceLocator.ResolveType<IUIVisualizerService>();
        }


        #region override

        /// <summary>
        /// To auto-initialize use Catel controls for viewmodels - they call InitializeAsync() internally
        /// </summary>
        /// <returns>Initializing <see cref="Task"/></returns>
        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            SetupCommands();
        }

        #endregion override


        #region virtual

        public virtual void Refresh() { }

        public virtual async Task UninitializeAsync() 
        {
            await Task.FromResult(0);
        }

        /// <summary>
        ///     This is called in InitializeAsync - force coderast to not init commands manually
        /// </summary>
        protected abstract void SetupCommands();

        public virtual void ReportMessage(string information) 
        {
            Information = information;
        }

        protected virtual void InitializationContinuator(Task parentTask) 
        {
            if (parentTask.Exception != null)
            {
                // show error dialog?
                
            }
        }

        #endregion virtual


        public string Name 
        {
            get => (string)GetValue(NameProperty);

            set => SetValue(NameProperty, value);
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

        public static readonly PropertyData NameProperty =
            RegisterProperty<WaitableViewModel, string>(waitViewModel => waitViewModel.Name, () => string.Empty);

        public static readonly PropertyData InformationProperty =
            RegisterProperty<WaitableViewModel, string>(waitViewModel => waitViewModel.Information, () => string.Empty);

        public static readonly PropertyData HasInformationProperty =
            RegisterProperty<WaitableViewModel, bool>(waitViewModel => waitViewModel.HasInformation, () => false);

        #endregion property data
    }
}