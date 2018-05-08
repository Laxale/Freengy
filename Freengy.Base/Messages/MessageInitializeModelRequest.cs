// Created by Laxale 20.04.2018
//
//

using System;
using Freengy.Base.Messages.Base;
using Freengy.Base.ViewModels;


namespace Freengy.Base.Messages 
{
    /// <summary>
    /// Сообщени-запрос на инициализацию (подразумевается, что асинхронную) вьюмодели.
    /// </summary>
    public class MessageInitializeModelRequest : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageInitializeModelRequest"/> с заданной вьюмоделью, которую нужно инициализировать.
        /// </summary>
        /// <param name="requesterModel">Вьюмодель, которую нужно инициализировать.</param>
        /// <param name="initializingMessage">Сообщение, поясняющее смысл инициализации.</param>
        public MessageInitializeModelRequest(WaitableViewModel requesterModel, string initializingMessage) 
        {
            InitializingMessage = initializingMessage;
            RequesterModel = requesterModel ?? throw new ArgumentNullException(nameof(requesterModel));
        }


        /// <summary>
        /// Вьюмодель, которую нужно инициализировать.
        /// </summary>
        public WaitableViewModel RequesterModel { get; }

        /// <summary>
        /// Сообщение, поясняющее смысл инициализации.
        /// </summary>
        public string InitializingMessage { get; }
    }
}