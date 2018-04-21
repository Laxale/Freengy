// Created by Laxale 21.04.2018
//
//

using Freengy.Common.Database;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Model of a request that is available only after authorization.
    /// </summary>
    public abstract class AuthorizedRequest : DbObject 
    {
        /// <summary>
        /// User's session token.
        /// </summary>
        public string UserToken { get; set; }
    }
}