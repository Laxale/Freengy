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
    using Catel.MVVM;


    public class DiagnosticsUnitViewModel : WaitableViewModel 
    {
        private readonly IDiagnosticsUnit diagnosticsUnit;
        

        public DiagnosticsUnitViewModel(IDiagnosticsUnit diagnosticsUnit) 
        {
            this.diagnosticsUnit = diagnosticsUnit;

            // viewmodel is not created by Catel, so init in ctor. I stuck here wondering why command is not working
            this.CommandShowDetails = new Command(() => this.IsShowingDetails = true);
        }


        public async void Run() 
        {
            Action runAction =
                () =>
                {
                    this.SetTestStartedFlags();

                    this.IsSucceeded = this.diagnosticsUnit.UnitTest();
                };
            
            Action<Task> runContinuator =
                parentTask =>
                {
                    this.SetTestFinishedFlags(parentTask.Exception);
                };

            await base.taskWrapper.Wrap(runAction, runContinuator);
        }

        protected override void SetupCommands() 
        {
            
        }


        public Command CommandShowDetails { get; private set; }


        #region properties
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
        public bool IsShowingDetails 
        {
            get { return (bool)GetValue(IsShowingDetailsProperty); }
            set { SetValue(IsShowingDetailsProperty, value); }
        }
        public string UnitName => this.diagnosticsUnit.Name;
        

        public static readonly PropertyData IsFailedProperty =
            ModelBase.RegisterProperty<DiagnosticsUnitViewModel, bool>(viewModel => viewModel.IsFailed, () => false);

        public static readonly PropertyData IsFinishedProperty =
            ModelBase.RegisterProperty<DiagnosticsUnitViewModel, bool>(viewModel => viewModel.IsFinished, () => false);

        public static readonly PropertyData IsSucceededProperty =
            ModelBase.RegisterProperty<DiagnosticsUnitViewModel, bool>(viewModel => viewModel.IsSucceeded, () => false);

        public static readonly PropertyData IsShowingDetailsProperty =
            ModelBase.RegisterProperty<DiagnosticsUnitViewModel, bool>(viewModel => viewModel.IsShowingDetails, () => false);

        public static readonly PropertyData IsRunningProperty =
            ModelBase.RegisterProperty<DiagnosticsUnitViewModel, bool>(diagViewModel => diagViewModel.IsRunning, () => false);
        
        #endregion properties


        private void SetTestStartedFlags() 
        {
            this.IsFailed = false;
            this.IsRunning = true;
            this.IsSucceeded = false;
            this.IsFinished = false;
        }
        private void SetTestFinishedFlags(Exception testException) 
        {
            this.IsRunning = false;
            this.IsFinished = true;
            this.IsSucceeded = testException == null;
            this.IsFailed = !this.IsSucceeded;
        }
    }
}