// Created by Laxale 03.05.2018
//
//

using System;
using System.Windows;

using Freengy.Base.Messages;
using Freengy.Base.Interfaces;

using Catel.Messaging;


namespace Freengy.Base.DefaultImpl 
{
    /// <summary>
    /// Класс для выполнения некоего действия при затемнённом занавеской содержимом главного окна.
    /// </summary>
    internal sealed class CurtainedExecutor : ICurtainedExecutor 
    {
        private static readonly object Locker = new object();

        private static CurtainedExecutor instance;

        private readonly IMessageMediator mediator = MessageMediator.Default;


        private CurtainedExecutor() { }

        /// <summary>
        /// Единственный инстанс <see cref="CurtainedExecutor"/>.
        /// </summary>
        public static ICurtainedExecutor Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new CurtainedExecutor());
                }
            }
        }


        /// <summary>
        /// Выполнить действие при затемнённом содержимом целевого окна. Занавеска с окна снимается по завершении действия.
        /// </summary>
        /// <param name="acceptorId">Уникальный идентификатор получателя запроса.</param>
        /// <param name="method">Метод для выполнения.</param>
        public void ExecuteWithCurtain(Guid acceptorId, Action method) 
        {
            lock (Locker)
            {
                var request = new MessageCurtainRequest(acceptorId);
                
                try
                {
                    mediator.SendMessage(request);
                    method();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                finally
                {
                    mediator.SendMessage(request);
                }
            }
        }
    }
}