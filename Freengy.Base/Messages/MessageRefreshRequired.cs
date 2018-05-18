// Created by Laxale 18.05.2018
//
//

using System;

using Freengy.Base.Interfaces;
using Freengy.Base.Messages.Base;


namespace Freengy.Base.Messages 
{
    /// <summary>
    /// Сообщение о том, что создан объект, требующий обновления своего состояния при повторном логине.
    /// </summary>
    public class MessageRefreshRequired : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageRefreshRequired"/> для заданного обновляемого объекта.
        /// </summary>
        /// <param name="refreshable">Объект, требующий обновления при повторном логине.</param>
        public MessageRefreshRequired(IRefreshable refreshable) 
        {
            Refreshable = refreshable ?? throw new ArgumentNullException(nameof(refreshable));
        }


        /// <summary>
        /// Возвращает ссылку на объект, требующий обновления при повторном логине.
        /// </summary>
        public IRefreshable Refreshable { get; }
    }
}