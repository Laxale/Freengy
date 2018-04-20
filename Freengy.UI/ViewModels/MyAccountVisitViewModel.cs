// Created by Laxale 20.04.2018
//
//

using Freengy.UI.Views;
using Freengy.Common.Models;
using Freengy.Base.ViewModels;
using Freengy.Networking.Interfaces;

using Catel.IoC;


namespace Freengy.UI.ViewModels 
{
    /// <summary>
    /// Viewmodel for <see cref="MyAccountVisitView"/>.
    /// </summary>
    public class MyAccountVisitViewModel : WaitableViewModel 
    {
        public MyAccountVisitViewModel() 
        {
            MyAccount = ServiceLocatorProperty.ResolveType<ILoginController>().CurrentAccount;
        }


        public UserAccount MyAccount { get; set; }


        /// <summary>
        /// This is called in InitializeAsync - force coderast to not init commands manually
        /// </summary>
        protected override void SetupCommands() 
        {
            throw new System.NotImplementedException();
        }
    }
}