// Created by Laxale 24.04.2018
//
//


using System;

using Freengy.Common.Exceptions;
using Freengy.Networking.Interfaces;

using NLog;

using Nancy;

using Catel.IoC;
using Catel.Messaging;
using Freengy.Common.Helpers;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.Networking.Messages;
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
        private static readonly ILoginController loginController = ServiceLocator.Default.ResolveType<ILoginController>();


        public FromServerModule() 
        {
            Before.AddItemToStartOfPipeline(ValidateRequest);
            Post[Url.Http.FromServer.InformFriendState] = OnFriendStateInform;
        }


        private Response ValidateRequest(NancyContext nancyContext) 
        {
            string serverToken = nancyContext.Request.Headers.Authorization;

            if (serverToken != loginController.ServerSessionToken)
            {
                string message = 
                    $"Got request on server module. Request token mismatch found" +
                    Environment.NewLine +
                    $"Cached token: { loginController.ServerSessionToken }" +
                    $"Actual token: { serverToken }";

                logger.Warn(message);

                throw new ServerNotAuthorizedException();
            }

            return nancyContext.Response;
        }
        
        private dynamic OnFriendStateInform(dynamic arg) 
        {
            var stateModel = new SerializeHelper().DeserializeObject<AccountStateModel>(Request.Body);

            var state = new AccountState(stateModel);
            mediator.SendMessage(new MessageFriendStateUpdate(state));

            return HttpStatusCode.OK;
        }
    }
}