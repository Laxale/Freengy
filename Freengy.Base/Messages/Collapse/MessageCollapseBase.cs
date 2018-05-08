// Created by Laxale 19.04.2018
//
//

using Freengy.Base.Messages.Base;


namespace Freengy.Base.Messages.Collapse 
{
    /// <summary>
    /// Request for region visual collapsing (if region supports collapsing).
    /// </summary>
    public abstract class MessageCollapseBase : MessageBase 
    {
        /// <summary>
        /// Construct new <see cref="MessageCollapseBase"/> with a given collapse flag.
        /// </summary>
        /// <param name="beCollapsed">Do collapse if true.</param>
        protected MessageCollapseBase(bool beCollapsed) 
        {
            BeCollapsed = beCollapsed;
        }


        /// <summary>
        /// Gets the flag - does sender want to collapse or UNcollapse region?
        /// </summary>
        public bool BeCollapsed { get; }
    }
}