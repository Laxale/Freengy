// Created by Laxale 20.10.2016
//
//


namespace Freengy.UI.Constants 
{
    using Freengy.UI.Views;


    public static class ViewNames
    {
        private static string loginViewName;
        private static string shellViewName;
        
        public static string LoginViewName => ViewNames.loginViewName ?? (ViewNames.loginViewName = typeof(LoginView).FullName);
        public static string ShellViewName => ViewNames.shellViewName ?? (ViewNames.shellViewName = typeof(ShellView).FullName);
    }
}