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
    using Freengy.Base.Messages;


    /// <summary>
    /// Module for handling server requests.
    /// </summary>
    public class FromServerModule : NancyModule 
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly IMessageMediator mediator = MessageMediator.Default;
        private static readonly DelayedEventInvoker delayedInvoker;
        private static readonly int statusPublishDelay = 3000;

        private static ILoginController loginController;


        static FromServerModule() 
        {
            delayedInvoker = new DelayedEventInvoker(statusPublishDelay);
            delayedInvoker.DelayedEvent += OnDelayedEvent;
        }

        public FromServerModule() 
        {
            Before.AddItemToStartOfPipeline(ValidateRequest);

            Get[Url.Http.FromServer.ReplyState] = OnStateReplyRequest;
            Post[Url.Http.FromServer.InformFriendState] = OnFriendStateInform;
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

                //TODO пока что это нормально - один раз упасть тут после логина. На второй запрос токены уже синхронизируются
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

        private static dynamic OnStateReplyRequest(dynamic arg) 
        {
            // каждый запрос от сервера даёт нам понять, что сервер онлайн
            // если по истечении таймаута инвокер не был перезапущен, он запостит сообщение о статусе сервера
            delayedInvoker.RemoveDelayedEventRequest();
            delayedInvoker.RequestDelayedEvent();

            mediator.SendMessage(new MessageServerOnlineStatus(true));

            return HttpStatusCode.OK;
        }

        private static void OnDelayedEvent() 
        {
            mediator.SendMessage(new MessageServerOnlineStatus(false));
        }
    }
}