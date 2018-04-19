// Created by Laxale 17.04.2018
//
//

using System.Configuration;
using System.Reflection;


namespace Freengy.Networking.Constants 
{
    /// <summary>
    /// Holds server url constants.
    /// </summary>
    public static class Url 
    {
        private static readonly string helloAction;
        private static readonly string searchAction;
        private static readonly string usersAction;
        private static readonly string logInAction;
        private static readonly string registerAction;
        private static readonly Configuration networkingConfig;


        static Url() 
        {
            networkingConfig = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            helloAction = networkingConfig.AppSettings.Settings["HelloActionName"].Value;
            usersAction = networkingConfig.AppSettings.Settings["UsersActionName"].Value;
            searchAction = networkingConfig.AppSettings.Settings["SearchActionName"].Value;
            logInAction = networkingConfig.AppSettings.Settings["LogInActionName"].Value;
            registerAction = networkingConfig.AppSettings.Settings["RegistrationActionName"].Value;
        }


        public static class Http 
        {
            public static string ServerRootHttpUrl { get; } = networkingConfig.AppSettings.Settings["FreengyServerHttpAddress"].Value;

            public static string ServerHttpHelloUrl { get; } = $"{ ServerRootHttpUrl }/{ helloAction }";

            public static string ServerHttpRegisterUrl { get; } = $"{ ServerRootHttpUrl }/{ registerAction }";

            public static string ServerHttpLogInUrl { get; } = $"{ ServerRootHttpUrl }/{ logInAction }";

            public static string ServerHttpSearchUsersUrl { get; } = $"{ ServerRootHttpUrl }/{ searchAction }/{ usersAction }";
        }

        public static class Https 
        {
            public static string ServerRootHttpsUrl { get; } = networkingConfig.AppSettings.Settings["FreengyServerHttpsAddress"].Value;
        }
    }
}