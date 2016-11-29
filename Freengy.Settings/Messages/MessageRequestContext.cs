// Created by Laxale 29.11.2016
//
//


namespace Freengy.Settings.Messages 
{
    using System;
    using System.Windows;

    using Freengy.Base.Messages;


    /// <summary>
    /// 
    /// </summary>
    internal class MessageRequestContext : MessageBase 
    {
        /// <summary>
        /// Create new <see cref="MessageRequestContext"/> with a type name or caller type name by default
        /// </summary>
        /// <param name="instance">Instance of a datacontext requester</param>
        public MessageRequestContext(FrameworkElement instance) 
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            this.RequesterInstance = instance;
        }


        public FrameworkElement RequesterInstance { get; }
    }
}