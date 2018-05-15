// Created by Laxale 23.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Freengy.Base.Messages;
using Freengy.Base.Chat.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Base.Interfaces;
using Freengy.Common.Helpers.Result;


namespace Freengy.Base.Chat.DefaultImpl 
{
    /// <summary>
    /// An <see cref="IChatHub"/> implementer.
    /// </summary>
    internal class ChatHub : IChatHub, IUserActivity 
    {
        private static readonly object Locker = new object();

        private static ChatHub instance;

        private readonly Dictionary<Guid, IChatSession> chatSessions = new Dictionary<Guid, IChatSession>();


        private ChatHub() 
        {
            this.Publish(new MessageActivityChanged(this, true));
        }
        

        /// <summary>
        /// Единственный инстанс <see cref="ChatHub"/>.
        /// </summary>
        public static IChatHub Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new ChatHub());
                }
            }
        }


        /// <summary>
        /// Возвращает значение - можно ли остановить данную активити без ведома юзера.
        /// </summary>
        public bool CanCancelInSilent { get; private set; } = true;

        /// <summary>
        /// Возвращает описание активности в контексте её остановки.
        /// </summary>
        public string CancelDescription 
        {
            get
            {
                lock (Locker)
                {
                    if (!chatSessions.Any())
                    {
                        return string.Empty;
                    }

                    string desc = $"Chat with {string.Join(",", chatSessions.Values.Select(session => session.Name))}";

                    return desc;
                }
            }
        }


        /// <summary>
        /// Get a chat session by identifier.
        /// </summary>
        /// <param name="sessionId">Chat session identifier.</param>
        /// <returns>Chat session or null.</returns>
        public IChatSession GetSession(Guid sessionId) 
        {
            lock (Locker)
            {
                return chatSessions.ContainsKey(sessionId) ? chatSessions[sessionId] : null;
            }
        }

        /// <summary>
        /// Add new session.
        /// </summary>
        /// <param name="session">Chat session to add.</param>
        public void AddSession(IChatSession session) 
        {
            lock (Locker)
            {
                if (chatSessions.ContainsKey(session?.Id ?? throw new ArgumentNullException(nameof(session))))
                {
                    return;
                }

                chatSessions.Add(session.Id, session);                  

                var message = new MessageChatSessionChanged(session, true);
                this.Publish(message);

                CanCancelInSilent = false;
            }
        }

        /// <summary>
        /// Remove a session.
        /// </summary>
        /// <param name="session">Chat session to remove.</param>
        public void RemoveSession(IChatSession session) 
        {
            lock (Locker)
            {
                if (!chatSessions.ContainsKey(session?.Id ?? throw new ArgumentNullException(nameof(session))))
                {
                    return;
                }

                chatSessions.Remove(session.Id);

                var message = new MessageChatSessionChanged(session, false);
                this.Publish(message);

                if (!chatSessions.Any())
                {
                    CanCancelInSilent = true;
                }
            }
        }

        /// <summary>
        /// Cancel activity.
        /// </summary>
        /// <returns>Result of a cancel attempt.</returns>
        public Result Cancel() 
        {
            lock (Locker)
            {
                chatSessions.Clear();
            }

            return Result.Ok();
        }
    }
}