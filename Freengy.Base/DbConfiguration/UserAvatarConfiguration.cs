// Created by Laxale 16.05.2018
//
//

using System.Data.Entity.ModelConfiguration;

using Freengy.Base.Models;


namespace Freengy.Base.DbConfiguration 
{
    /// <summary>
    /// EF-configuration for working with <see cref="UserAvatarModel"/> models.
    /// </summary>
    internal class UserAvatarConfiguration : EntityTypeConfiguration<UserAvatarModel> 
    {
        public UserAvatarConfiguration() 
        {
            ToTable("UserAvatars");

            HasOptional(avatar => avatar.NavigationParent)
                .WithMany(acc => acc.Avatars)
                .HasForeignKey(pass => pass.ParentId)
                //                // в связи с дикими проблемами наладить отношение один-к-одному пришлось вхерачить коллекцию
                //                // в этой коллекции всегда один аватар или пусто
                .WillCascadeOnDelete(true);
        }
    }
}