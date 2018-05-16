// Created by Laxale 06.05.2018
//
//

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Freengy.Base.Models.Readonly;
using Freengy.Common.Models;


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

        /// <summary>
        /// Возвращает или задаёт пользовательский аватар.
        /// </summary>
        [NotMapped]
        public UserAvatarModel Avatar { get; set; }

        /// <summary>
        /// Возвращает или задаёт коллекцию пользовательских аватаров.
        /// </summary>
        public List<UserAvatarModel> Avatars { get; set; } = new List<UserAvatarModel>();
    }
}