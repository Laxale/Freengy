// Created by Laxale 19.04.2018
//
//


namespace Freengy.Base.Messages.Collapse 
{
    /// <summary>
    /// Request for chat region visual collapsing.
    /// </summary>
    public class MessageCollapseChatRequest : MessageCollapseBase 
    {
        /// <summary>
        /// Construct new <see cref="MessageCollapseChatRequest" /> with a given collapse flag.
        /// </summary>
        /// <param name="beCollapsed">Do collapse if true.</param>
        public MessageCollapseChatRequest(bool beCollapsed) : base(beCollapsed) 
        {

        }
    }
}