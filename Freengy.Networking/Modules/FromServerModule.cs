// Created by Laxale 24.04.2018
//
//


using System;

using Freengy.Common.Exceptions;
using Freengy.Networking.Interfaces;
using Freengy.Common.Helpers;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.Networking.Messages;
using Freengy.Common.Extensions;
using Freengy.Networking.DefaultImpl;

using NLog;

using Nancy;

using Catel.IoC;
using Catel.Messaging;

using Url = Freengy.Networking.Constants.Url;


namespace Freengy.Networking.Modules 
{
    /// <summary>
    /// Module for handling server requests.
    /// </summary>
    public class FromServerModule : NancyModule 
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly IMessageMediator mediator = MessageMediator.Default;

        private static string clientToken;
        private static ILoginController loginController;


        public FromServerModule() 
        {
            // only work when logged in
            if (string.IsNullOrWhiteSpace(clientToken))
            {
                return;
            }

            Before.AddItemToStartOfPipeline(ValidateRequest);

            string sessionChannel = $"{Url.Http.FromServer.InformFriendState}/{clientToken}";
            Post[sessionChannel] = OnFriendStateInform;
        }


        /// <summary>
        /// Set the client session token to create a private client-server channel.
        /// </summary>
        /// <param name="token">Client session token.</param>
        internal static void SetClientSessionToken(string token) 
        {
            clientToken = token;
        }


        private Response ValidateRequest(NancyContext nancyContext) 
        {
            loginController = loginController ?? ServiceLocator.Default.ResolveType<ILoginController>();

            SessionAuth serverAuth = nancyContext.Request.Headers.GetSessionAuth();

            if (serverAuth.ServerToken != loginController.ServerSessionToken)
            {
                string message = 
                    $"Got request on server module. Request token mismatch found" +
                    Environment.NewLine +
                    $"Cached token: { loginController.ServerSessionToken }" +
                    $"Actual token: { serverAuth.ServerToken }";

                logger.Warn(message);

                throw new ServerNotAuthorizedException();
            }

            return nancyContext.Response;
        }
        
        private dynamic OnFriendStateInform(dynamic arg) 
        {
            var stateModel = new SerializeHelper().DeserializeObject<AccountStateModel>(Request.Body);

            FriendStateController.InternalInstance.UpdateFriendState(stateModel);

            return HttpStatusCode.OK;
        }
    }
}