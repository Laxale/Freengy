// Created by Laxale 16.11.2016
//
//

using System;
using System.Threading.Tasks;

using Freengy.Base.ViewModels;
using Freengy.Base.Interfaces;
using Freengy.Diagnostics.Interfaces;


namespace Freengy.Diagnostics.ViewModels 
{
    using Freengy.Base.Helpers.Commands;


    public class DiagnosticsUnitViewModel : WaitableViewModel 
    {
        private readonly IDiagnosticsUnit diagnosticsUnit;

        private bool isRunning;
        private bool succeeded;


        public DiagnosticsUnitViewModel(IDiagnosticsUnit diagnosticsUnit) 
        {
            this.diagnosticsUnit = diagnosticsUnit;

            // viewmodel is not created by Catel, so init in ctor. I stuck here wondering why command is not working
            CommandShowDetails = new MyCommand(() => IsShowingDetails = !IsShowingDetails);
        }


        public async void Run() 
        {
            void RunAction()
            {
                SetTestStartedFlags();

                Succeeded = diagnosticsUnit.UnitTest();

                UnitResult = diagnosticsUnit.ResultInfo;
            }

            Action<Task> runContinuator =
                parentTask =>
                {
                    SetTestFinishedFlags(parentTask.Exception);
                };

            await TaskerWrapper.Wrap(RunAction, runContinuator);
        }

        protected override void SetupCommands() 
        {
            
        }


        public MyCommand CommandShowDetails { get; private set; }


        #region properties

        public bool IsRunning 
        {
            get => isRunning;

            set
            {
                if (isRunning == value) return;

                isRunning = value;

                OnPropertyChanged();
            }
        }
        
        public bool Succeeded 
        {
            get => succeeded;

            set
            {
                if (succeeded == value) return;

                succeeded = value;

                OnPropertyChanged();
            }
        }


        private bool isFailed;

        public bool IsFailed 
        {
            get => isFailed;

            set
            {
                if (isFailed == value) return;

                isFailed = value;

                OnPropertyChanged();
            }
        }

        private bool isFinished;

        public bool IsFinished 
        {
            get => isFinished;

            set
            {
                if (isFinished == value) return;

                isFinished = value;

                OnPropertyChanged();
            }
        }


        private bool isShowingDetails;

        public bool IsShowingDetails
        {
            get => isShowingDetails;

            set
            {
                if (isShowingDetails == value) return;

                isShowingDetails = value;

                OnPropertyChanged();
            }
        }

        private string unitResult;

        public string UnitResult 
        {
            get => unitResult;

            set
            {
                if (unitResult == value) return;

                unitResult = value;

                OnPropertyChanged();
            }
        }

        public string UnitName => diagnosticsUnit.Name;


        #endregion properties


        private void SetTestStartedFlags() 
        {
            IsFailed = false;
            IsRunning = true;
            Succeeded = false;
            IsFinished = false;
        }

        private void SetTestFinishedFlags(Exception testException) 
        {
            IsRunning = false;
            IsFinished = true;

            if (testException != null)
            {
                Succeeded = false;
            }

            IsFailed = !Succeeded;
        }
    }
}