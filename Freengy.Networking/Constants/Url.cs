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
        private static readonly string expAction;
        private static readonly string syncAction;
        private static readonly string editAction;
        private static readonly string accountAction;
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

            expAction = networkingConfig.AppSettings.Settings["ExpActionName"].Value;
            syncAction = networkingConfig.AppSettings.Settings["SyncActionName"].Value;
            stateAction = networkingConfig.AppSettings.Settings["StateActionName"].Value;
            editAction = networkingConfig.AppSettings.Settings["EditActionName"].Value;
            accountAction = networkingConfig.AppSettings.Settings["AccountActionName"].Value;
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
            public static string RootServerUrl { get; } = networkingConfig.AppSettings.Settings["FreengyServerHttpAddress"].Value;

            public static string HelloUrl { get; } = $"{ RootServerUrl }/{ helloAction }";

            public static string RegisterUrl { get; } = $"{ RootServerUrl }/{ registerAction }";

            public static string LogInUrl { get; } = $"{ RootServerUrl }/{ logInAction }";

            public static string SearchUsersUrl { get; } = $"{ RootServerUrl }/{ searchAction }/{ usersAction }";

            public static string SearchFriendRequestsUrl { get; } = $"{ RootServerUrl }/{ searchAction }/{ friendRequestAction }";

            public static string AddFriendUrl { get; } = $"{ RootServerUrl }/{ requestAction }/{ friendAction }";

            public static string ReplyFriendRequestUrl { get; } = $"{ RootServerUrl }/{ replyAction}/{ friendAction }{ requestAction }";


            public static class Chat 
            {
                public static string ChatSubRoute { get; }= $"/{chatAction}";
            }

            public static class FromServer 
            {
                public static string Root { get; } = $"/{fromServerAction}";

                /// <summary>
                /// Subroute on wich server requests client availability.
                /// </summary>
                public static string ReplyState { get; } = $"{ Root }/{ replyAction }{ stateAction }";
                
                public static string Inform { get; } = $"{ Root }/{ informAction }";

                /// <summary>
                /// Subroute on which server posts client's friends status updates.
                /// </summary>
                public static string InformFriendState { get; } = $"{ Root }/{ informAction }/{ friendAction}/{ stateAction }";

                /// <summary>
                /// Subroute on which server posts new incoming friendrequest.
                /// </summary>
                public static string InformFriendRequest { get; } = $"{ Root }/{ informAction }/{ friendAction}{ requestAction }";

                /// <summary>
                /// Subroute on which server posts friendrequest reply.
                /// </summary>
                public static string InformFriendRequestState { get; } = $"{ InformFriendRequest }/{ stateAction }";

                public static readonly string SyncExp = $"{ Inform }/{ expAction }";
            }

            public static class Edit 
            {
                public static readonly string EditRoot = $"{ RootServerUrl }/{ editAction }";

                public static readonly string EditAccount = $"{ EditRoot }/{ accountAction }";
            }

            public static class Sync 
            {
                public static readonly string SyncRoot = $"{ RootServerUrl }/{ syncAction }";

                public static readonly string SyncAccount = $"{ SyncRoot }/{ accountAction }";
            }
        }

        public static class Https 
        {
            public static string ServerRootHttpsUrl { get; } = networkingConfig.AppSettings.Settings["FreengyServerHttpsAddress"].Value;
        }
    }
}