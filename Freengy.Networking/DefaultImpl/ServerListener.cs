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

using NLog;

using Catel.IoC;
using Freengy.Common.Extensions;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// Client-side Freengy server listener.
    /// </summary>
    internal class ServerListener : IHttpClientParametersProvider 
    {
        //private static readonly string httpAddressNoPort = "localhost";
        private static readonly object Locker = new object();
        private static readonly ushort maxStartTrials = 50;
        
        private static ServerListener instance;

        private readonly IChatHub chatHub = ServiceLocator.Default.ResolveType<IChatHub>();
        private readonly IChatSessionFactory sessionFactory = ServiceLocator.Default.ResolveType<IChatSessionFactory>();
        private readonly IChatMessageFactory messageFactory = ServiceLocator.Default.ResolveType<IChatMessageFactory>();
        private readonly IFriendStateController friendController = ServiceLocator.Default.ResolveType<IFriendStateController>();

        private Task startupTask;
        private string clientAddress;


        private ServerListener() { }
     

        /// <summary>
        /// Единственный инстанс <see cref="ServerListener"/>.
        /// </summary>
        public static IHttpClientParametersProvider ExposedInstance => InternalInstance;
        

        /// <summary>
        /// Internal instance for nonclient usage.
        /// </summary>
        internal static ServerListener InternalInstance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new ServerListener());
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
            startupTask = Task.Run(async () =>
            {
                IPAddress[] ipAddresses = await Dns.GetHostAddressesAsync(Environment.MachineName);

                var machineAddress = 
                    ipAddresses
                        .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork) // Internetwork is IPv4
                        //.Skip(1)
                        .First(ip => ip.IsLocal()) // for debugging local server
                        //.First(ip => !ip.IsLocal()) // for real remote server
                        .ToString();

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
        }
    }
}