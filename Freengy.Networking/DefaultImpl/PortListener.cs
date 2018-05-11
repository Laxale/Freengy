// Created by Laxale 23.04.2018
//
//

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using Freengy.Base.Interfaces;
using Freengy.Base.Chat.Interfaces;
using Freengy.Common.Constants;
using Freengy.Common.Helpers;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.Networking.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Common.Extensions;
using Freengy.Networking.Messages;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// Client-side port listener.
    /// </summary>
    internal class PortListener : IHttpClientParametersProvider 
    {
        private static readonly string httpAddressNoPort = "localhost";
        private static readonly object Locker = new object();
        private static readonly ushort maxStartTrials = 50;
        
        private static PortListener instance;

        private readonly IChatHub chatHub = MyServiceLocator.Instance.Resolve<IChatHub>();
        private readonly IChatSessionFactory sessionFactory = MyServiceLocator.Instance.Resolve<IChatSessionFactory>();
        private readonly IChatMessageFactory messageFactory = MyServiceLocator.Instance.Resolve<IChatMessageFactory>();
        private readonly IFriendStateController friendController = MyServiceLocator.Instance.Resolve<IFriendStateController>();

        private Task startupTask;
        private string clientAddress;


        private PortListener() { }
     

        /// <summary>
        /// Единственный инстанс <see cref="PortListener"/>.
        /// </summary>
        public static IHttpClientParametersProvider ExposedInstance => InternalInstance;
        

        /// <summary>
        /// Internal instance for nonclient usage.
        /// </summary>
        internal static PortListener InternalInstance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new PortListener());
                }
            }
        }


        /// <summary>
        /// Gets the client socket address.
        /// </summary>
        public async Task<string> GetClientAddressAsync() 
        {
            return await Task.Run(() =>
            {
                if (clientAddress == null)
                {
                    startupTask.Wait();
                }

                return clientAddress;
            });
        }


        internal async Task InitInternalAsync() 
        {
            startupTask = Task.Run(() =>
            {
                string machineAddress = GetMachineAddress();

                clientAddress =
                    new ServerStartupBuilder()
                        .SetBaseAddress(machineAddress)
                        .SetInitialPort(StartupConst.InitialClientPort)
                        .SetPortStep(StartupConst.PortCheckingStep)
                        .SetTrialsCount(maxStartTrials)
                        .UseHttps(false)
                        .Build();
            });

            await startupTask;
        }

        private static string GetMachineAddress() 
        {
            //string machineAddress = GetMachineIpAddress();
            string machineAddress = httpAddressNoPort;

            return machineAddress;
        }

        private static string GetMachineIpAddress() 
        {
            IPAddress[] ipAddresses = Dns.GetHostAddressesAsync(Environment.MachineName).Result;

            var machineAddress =
                ipAddresses
                    .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork) // Internetwork is IPv4
                    //.Skip(1)
                    .First(ip => ip.IsLocal()) // for debugging local server
                    //.First(ip => !ip.IsLocal()) // for real remote server
                    .ToString();

            return machineAddress;
        }

        internal void InformOfANewMessage(ChatMessageModel messageModel) 
        {
            AccountState friendState = friendController.GetFriendState(messageModel.AuthorAccount.Id);
            if (friendState == null)
            {
                throw new InvalidOperationException($"Friend '{ messageModel.AuthorAccount.Id }' state is not found");
            }

            Guid sessionId = messageModel.SessionId;
            IChatSession session = chatHub.GetSession(sessionId);

            if (session == null)
            {
                string sessionName = messageModel.AuthorAccount.Name;
                session = 
                    sessionFactory
                        .SetSessionId(messageModel.SessionId)
                        .CreateInstance(sessionName, sessionName);

                chatHub.AddSession(session);
            }

            session.AddToChat(friendState);
            messageFactory.Author = friendState.Account;
            IChatMessage message = messageFactory.CreateMessage(messageModel.MessageText);
            session.AcceptMessage(message);

            this.Publish(new MessageReceivedMessage(friendState.Account.Id, sessionId));
        }
    }
}