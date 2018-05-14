// Created by Laxale 24.04.2018
//
//


using System;
using System.Threading.Tasks;

using Freengy.Base.DefaultImpl;
using Freengy.Base.Messages;
using Freengy.Common.Exceptions;
using Freengy.Networking.Interfaces;
using Freengy.Common.Helpers;
using Freengy.Common.Models;
using Freengy.Common.Extensions;
using Freengy.Networking.DefaultImpl;
using Freengy.Base.Messages.Notification;
using Freengy.Networking.Messages;
using NLog;

using Nancy;

using Url = Freengy.Networking.Constants.Url;


namespace Freengy.Networking.Modules 
{
    /// <summary>
    /// Module for handling server requests.
    /// </summary>
    public class FromServerModule : NancyModule 
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
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
            Before.AddItemToStartOfPipeline(ValidateServerMessage);

            Get[Url.Http.FromServer.ReplyState] = OnStateReplyRequest;

            Post[Url.Http.FromServer.SyncExp] = OnExpAdded;
            Post[Url.Http.FromServer.InformFriendState] = OnFriendStateInform;
            Post[Url.Http.FromServer.InformFriendRequestState] = OnFriendRequestReply;
            Post[Url.Http.FromServer.InformFriendRequest] = OnNewFriendRequest;
        }


        private Response ValidateServerMessage(NancyContext nancyContext) 
        {
            loginController = loginController ?? MyServiceLocator.Instance.Resolve<ILoginController>();

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
                //throw new ServerNotAuthorizedException();
                return HttpStatusCode.Unauthorized;
            }

            return null;
        }

        private dynamic OnNewFriendRequest(dynamic arg) 
        {
            var newRequest = new SerializeHelper().DeserializeObject<FriendRequest>(Request.Body);

            Task.Run(() => this.Publish(new MessageNewFriendRequest(newRequest)));

            return HttpStatusCode.OK;
        }

        private dynamic OnFriendStateInform(dynamic arg) 
        {
            var stateModel = new SerializeHelper().DeserializeObject<AccountStateModel>(Request.Body);

            FriendStateController.InternalInstance.UpdateFriendState(stateModel);

            return HttpStatusCode.OK;
        }

        private dynamic OnFriendRequestReply(dynamic arg) 
        {
            var reply = new SerializeHelper().DeserializeObject<FriendRequestReply>(Request.Body);

            Task.Run(() => this.Publish(new MessageFriendRequestState(reply)));

            return HttpStatusCode.OK;
        }

        private dynamic OnStateReplyRequest(dynamic arg) 
        {
            // каждый запрос от сервера даёт нам понять, что сервер онлайн
            // если по истечении таймаута инвокер не был перезапущен, он запостит сообщение о статусе сервера
            delayedInvoker.RemoveDelayedEventRequest();
            delayedInvoker.RequestDelayedEvent();

            delayedInvoker.Publish(new MessageServerOnlineStatus(true));

            return HttpStatusCode.OK;
        }

        private dynamic OnExpAdded(dynamic arg) 
        {
            var expModel = new SerializeHelper().DeserializeObject<GainExpModel>(Request.Body);

            this.Publish(new MessageExpirienceGained(expModel.GainReason, expModel.Amount));

            return HttpStatusCode.OK;
        }


        private static void OnDelayedEvent() 
        {
            delayedInvoker.Publish(new MessageServerOnlineStatus(false));
        }
    }
}