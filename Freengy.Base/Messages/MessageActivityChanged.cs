// Created by Laxale 20.10.2016
//
//

using System;

using Freengy.Base.Interfaces;
using Freengy.Base.Messages.Base;


namespace Freengy.Base.Messages 
{
    /// <summary>
    /// Message to inform that <see cref="IUserActivity"/> is started or finished.
    /// </summary>
    public class MessageActivityChanged : MessageBase 
    {
        /// <summary>
        /// Construct a new <see cref="MessageActivityChanged"/> with a given state flag.
        /// </summary>
        /// <param name="activity">Activity that changed its state.</param>
        /// <param name="isStarted">True if activity has started. False if finished.</param>
        public MessageActivityChanged(IUserActivity activity, bool isStarted) 
        {
            Activity = activity ?? throw new ArgumentNullException(nameof(activity));

            IsStarted = isStarted;
        }


        /// <summary>
        /// Gets activity state flag.
        /// </summary>
        public bool IsStarted { get; }

        /// <summary>
        /// Activity that changed its state.
        /// </summary>
        public IUserActivity Activity { get; }
    }
}