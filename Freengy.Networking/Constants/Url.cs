﻿// Created by Laxale 17.04.2018
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
        private static readonly string friendAction;
        private static readonly string requestAction;
        private static readonly string searchAction;
        private static readonly string usersAction;
        private static readonly string logInAction;
        private static readonly string registerAction;
        private static readonly Configuration networkingConfig;


        static Url() 
        {
            networkingConfig = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            requestAction = networkingConfig.AppSettings.Settings["RequestActionName"].Value;
            friendAction = networkingConfig.AppSettings.Settings["FriendActionName"].Value;
            helloAction = networkingConfig.AppSettings.Settings["HelloActionName"].Value;
            usersAction = networkingConfig.AppSettings.Settings["UsersActionName"].Value;
            searchAction = networkingConfig.AppSettings.Settings["SearchActionName"].Value;
            logInAction = networkingConfig.AppSettings.Settings["LogInActionName"].Value;
            registerAction = networkingConfig.AppSettings.Settings["RegistrationActionName"].Value;
        }


        public static class Http 
        {
            public static string RootUrl { get; } = networkingConfig.AppSettings.Settings["FreengyServerHttpAddress"].Value;

            public static string HelloUrl { get; } = $"{ RootUrl }/{ helloAction }";

            public static string RegisterUrl { get; } = $"{ RootUrl }/{ registerAction }";

            public static string LogInUrl { get; } = $"{ RootUrl }/{ logInAction }";

            public static string SearchUsersUrl { get; } = $"{ RootUrl }/{ searchAction }/{ usersAction }";

            public static string AddFriendUrl { get; } = $"{ RootUrl }/{ requestAction }/{ friendAction }";
        }

        public static class Https 
        {
            public static string ServerRootHttpsUrl { get; } = networkingConfig.AppSettings.Settings["FreengyServerHttpsAddress"].Value;
        }
    }
}