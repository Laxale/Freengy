// Created by Laxale 17.04.2018
//
//

using System.Reflection;
using System.Configuration;


namespace Freengy.Networking.Constants 
{
    /// <summary>
    /// Holds server url constants.
    /// </summary>
    public static class Url 
    {
        private static readonly string stateAction;
        private static readonly string fromServerAction;
        private static readonly string informAction;
        private static readonly string chatAction;
        private static readonly string helloAction;
        private static readonly string replyAction;
        private static readonly string friendAction;
        private static readonly string requestAction;
        private static readonly string searchAction;
        private static readonly string usersAction;
        private static readonly string logInAction;
        private static readonly string registerAction;
        private static readonly string friendRequestAction;
        private static readonly Configuration networkingConfig;


        static Url() 
        {
            networkingConfig = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            stateAction = networkingConfig.AppSettings.Settings["StateActionName"].Value;
            fromServerAction = networkingConfig.AppSettings.Settings["FromServerActionName"].Value;
            informAction = networkingConfig.AppSettings.Settings["InformActionName"].Value;
            chatAction = networkingConfig.AppSettings.Settings["ChatActionName"].Value;
            replyAction = networkingConfig.AppSettings.Settings["ReplyActionName"].Value;
            requestAction = networkingConfig.AppSettings.Settings["RequestActionName"].Value;
            friendAction = networkingConfig.AppSettings.Settings["FriendActionName"].Value;
            helloAction = networkingConfig.AppSettings.Settings["HelloActionName"].Value;
            usersAction = networkingConfig.AppSettings.Settings["UsersActionName"].Value;
            searchAction = networkingConfig.AppSettings.Settings["SearchActionName"].Value;
            logInAction = networkingConfig.AppSettings.Settings["LogInActionName"].Value;
            registerAction = networkingConfig.AppSettings.Settings["RegistrationActionName"].Value;

            friendRequestAction = $"{friendAction}{requestAction}";
        }


        public static class Http 
        {
            public static string RootUrl { get; } = networkingConfig.AppSettings.Settings["FreengyServerHttpAddress"].Value;

            public static string HelloUrl { get; } = $"{ RootUrl }/{ helloAction }";

            public static string RegisterUrl { get; } = $"{ RootUrl }/{ registerAction }";

            public static string LogInUrl { get; } = $"{ RootUrl }/{ logInAction }";

            public static string SearchUsersUrl { get; } = $"{ RootUrl }/{ searchAction }/{ usersAction }";

            public static string SearchFriendRequestsUrl { get; } = $"{ RootUrl }/{ searchAction }/{ friendRequestAction }";

            public static string AddFriendUrl { get; } = $"{ RootUrl }/{ requestAction }/{ friendAction }";

            public static string ReplyFriendRequestUrl { get; } = $"{ RootUrl }/{ replyAction}/{ friendAction }{ requestAction }";


            public static class Chat 
            {
                public static string ChatSubRoute { get; }= $"/{chatAction}";
            }

            public static class FromServer 
            {
                public static string Root { get; } = $"/{fromServerAction}";

                public static string Inform { get; } = $"/{fromServerAction}/{ informAction }";

                public static string InformFriendState { get; } = $"/{fromServerAction}/{ informAction }/{ friendAction}/{ stateAction }";
            }
        }

        public static class Https 
        {
            public static string ServerRootHttpsUrl { get; } = networkingConfig.AppSettings.Settings["FreengyServerHttpsAddress"].Value;
        }
    }
}