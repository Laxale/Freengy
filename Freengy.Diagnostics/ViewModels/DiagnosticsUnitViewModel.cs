// Created by Laxale 16.11.2016
//
//


namespace Freengy.Diagnostics.ViewModels 
{
    using System;
    using System.Threading.Tasks;

    using Freengy.Base.ViewModels;
    using Freengy.Base.Interfaces;
    using Freengy.Diagnostics.Interfaces;
    
    using Catel.IoC;
    using Catel.Data;


    public class DiagnosticsUnitViewModel : WaitableViewModel 
    {
        private readonly IGuiDispatcher dispatcher;
        private readonly IDiagnosticsUnit diagnosticsUnit;
        

        public DiagnosticsUnitViewModel(IDiagnosticsUnit diagnosticsUnit) 
        {
            this.diagnosticsUnit = diagnosticsUnit;
            this.dispatcher = base.serviceLocator.ResolveType<IGuiDispatcher>();
        }


        public async void Run() 
        {
            Action runAction =
                () =>
                {
                    this.IsRunning = true;

                    this.IsSucceeded = this.diagnosticsUnit.UnitTest();
                };
            
            Action<Task> runContinuator =
                parentTask =>
                {
                    
                    this.IsRunning = false;
                    this.IsFinished = true;

                    if (parentTask.Exception != null)
                    {
                        this.IsFailed = true;
                        this.IsSucceeded = false;
                    }
                    else
                    {
                        this.IsFailed = false;
                        this.IsSucceeded = true;
                    }
                };

            await base.taskWrapper.Wrap(runAction, runContinuator);
        }

        protected override void SetupCommands() 
        {
            
        }

        

        public bool IsRunning 
        {
            get { return (bool)GetValue(IsRunningProperty); }
            private set { SetValue(IsRunningProperty, value); }
        }

        public bool IsSucceeded 
        {
            get { return (bool)GetValue(IsSucceededProperty); }
            private set { SetValue(IsSucceededProperty, value); }
        }

        public bool IsFailed 
        {
            get { return (bool)GetValue(IsFailedProperty); }
            private set { SetValue(IsFailedProperty, value); }
        }

        public bool IsFinished
        {
            get { return (bool)GetValue(IsFinishedProperty); }
            private set { SetValue(IsFinishedProperty, value); }
        }

        public string UnitName => this.diagnosticsUnit.Name;
        

        public static readonly PropertyData IsFailedProperty =
            RegisterProperty<DiagnosticsUnitViewModel, bool>(viewModel => viewModel.IsFailed, () => false);

        public static readonly PropertyData IsFinishedProperty =
            RegisterProperty<DiagnosticsUnitViewModel, bool>(viewModel => viewModel.IsFinished, () => false);

        public static readonly PropertyData IsSucceededProperty =
            RegisterProperty<DiagnosticsUnitViewModel, bool>(viewModel => viewModel.IsSucceeded, () => false);

        public static readonly PropertyData IsRunningProperty =
            RegisterProperty<DiagnosticsUnitViewModel, bool>(diagViewModel => diagViewModel.IsRunning, () => false);
    }
}