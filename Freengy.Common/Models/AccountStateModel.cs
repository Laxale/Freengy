// Created by Laxale 18.04.2018
//
//

using Freengy.Common.Enums;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// User account state model.
    /// </summary>
    public class AccountStateModel 
    {
        /// <summary>
        /// User's current address is set by server.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// User account model.
        /// </summary>
        public UserAccountModel AccountModel { get; set; }

        /// <summary>
        /// User account online status.
        /// </summary>
        public AccountOnlineStatus OnlineStatus { get; set; }


        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() 
        {
            return $"{AccountModel?.Name} [{AccountModel?.Level}] {OnlineStatus}";
        }
    }
}