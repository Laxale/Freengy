// Created by Laxale 16.05.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using Freengy.Base.Extensions;
using Freengy.Base.Models.Readonly;
using Freengy.Common.Enums;
using Freengy.Common.Models;


namespace Freengy.Base.Helpers 
{
    /// <summary>
    /// Содержит вспомогательные методы для работы с <see cref="FriendRequest"/>.
    /// </summary>
    public static class FriendRequestHelper 
    {
        /// <summary>
        /// Create ready for use <see cref="FriendRequest"/> instance.
        /// </summary>
        /// <param name="sender">Request sender account.</param>
        /// <param name="target">Request target account.</param>
        /// <returns><see cref="FriendRequest"/> instance.</returns>
        public static FriendRequest Create(UserAccount sender, UserAccount target) 
        {
            var request = new FriendRequest
            {
                RequesterAccount = sender.ToModel(),
                TargetAccount = target.ToModel(),
                CreationDate = DateTime.Now
            };

            return request;
        }
    }
}