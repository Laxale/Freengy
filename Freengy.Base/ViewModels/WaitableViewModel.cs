// Created by Laxale 19.10.2016
//
//


namespace Freengy.Base.ViewModels 
{
    using System.Threading.Tasks;

    using Freengy.Base.Interfaces;

    using Catel.IoC;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Messaging;


    public abstract class WaitableViewModel : ViewModelBase, IRefreshable 
    {
        #region vars

        protected readonly ITaskWrapper taskWrapper;
        protected readonly ITypeFactory typeFactory;
        protected readonly IGuiDispatcher guiDispatcher;
        protected readonly IUIVisualizerService uiVisualizer;
        protected readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        protected readonly IMessageMediator messageMediator = MessageMediator.Default;

        #endregion vars


        #region ctor

        protected WaitableViewModel() 
        {
            this.typeFactory = this.GetTypeFactory();
            this.taskWrapper = this.serviceLocator.ResolveType<ITaskWrapper>();
            this.guiDispatcher = this.serviceLocator.ResolveType<IGuiDispatcher>();
            this.uiVisualizer = this.serviceLocator.ResolveType<IUIVisualizerService>();
        }

        #endregion ctor


        #region override

        /// <summary>
        /// To auto-initialize use Catel controls for viewmodels - they call InitializeAsync() internally
        /// </summary>
        /// <returns>Initializing <see cref="Task"/></returns>
        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            this.SetupCommands();
        }

        #endregion override


        #region virtual

        public virtual void Refresh() 
        {

        }

        public virtual async Task UninitializeAsync() 
        {

        }

        /// <summary>
        ///     This is called in InitializeAsync - force coderast to not init commands manually
        /// </summary>
        protected abstract void SetupCommands();

        public virtual void ReportMessage(string information) 
        {

        }

        protected virtual void InitializationContinuator(System.Threading.Tasks.Task parentTask)
        {
            if (parentTask.Exception != null)
            {
                // show error dialog?
            }
        }

        #endregion virtual


        #region properties

        public string Name 
        {
            get { return (string)this.GetValue(WaitableViewModel.NameProperty); }

            set { this.SetValue(WaitableViewModel.NameProperty, value); }
        }

        /// <summary>
        ///     Represents long-running task state of the viewmodel
        /// </summary>
        public bool IsWaiting 
        {
            get { return (bool)this.GetValue(WaitableViewModel.IsWaitingProperty); }

            set { this.SetValue(WaitableViewModel.IsWaitingProperty, value); }
        }
        
        #endregion properties


        #region property data

        public static readonly PropertyData IsWaitingProperty =
            ModelBase.RegisterProperty<WaitableViewModel, bool>(loginViewModel => loginViewModel.IsWaiting, () => false);

        public static readonly PropertyData NameProperty =
            ModelBase.RegisterProperty<WaitableViewModel, string>(waitViewModel => waitViewModel.Name, () => string.Empty);

        #endregion property data
    }
}