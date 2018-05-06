// Created by Laxale 06.05.2018
//
//

using System.ComponentModel.DataAnnotations.Schema;

using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Models 
{
    /// <summary>
    /// Internal user account data model to store in client database. Contains password information.
    /// </summary>
    [Table(nameof(UserAccount) + "s")]
    public class PrivateAccountModel : UserAccountModel 
    {
        /// <summary>
        /// Password salt obtained from server during last login or registration action.
        /// </summary>
        public string NextLoginSalt { get; set; }
    }
}