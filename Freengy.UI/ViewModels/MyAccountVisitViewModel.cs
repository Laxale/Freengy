// Created by Laxale 20.04.2018
//
//

using Freengy.UI.Views;
using Freengy.Common.Models;
using Freengy.Base.ViewModels;
using Freengy.Networking.Interfaces;
using Freengy.Common.Models.Readonly;


namespace Freengy.UI.ViewModels 
{
    /// <summary>
    /// Viewmodel for <see cref="MyAccountVisitView"/>.
    /// </summary>
    public class MyAccountVisitViewModel : WaitableViewModel 
    {
        public MyAccountVisitViewModel() 
        {
            MyAccountState = ServiceLocator.Resolve<ILoginController>().MyAccountState;
        }


        /// <summary>
        /// State of my account.
        /// </summary>
        public AccountState MyAccountState { get; }


        /// <inheritdoc />
        /// <summary>
        /// This is called in InitializeAsync - force coderast to not init commands manually
        /// </summary>
        protected override void SetupCommands() 
        {
            throw new System.NotImplementedException();
        }
    }
}