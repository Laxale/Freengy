// Created by Laxale 16.05.2018
//
//

using System.Data.Entity;

using Freengy.Base.DbConfiguration;
using Freengy.Base.Models;
using Freengy.Common.Models.Avatar;
using Freengy.Database.Context;


namespace Freengy.Base.DbContext 
{
    /// <summary>
    /// ORM context for <see cref="UserAvatarModel"/>.
    /// </summary>
    internal class UserAvatarContext : ComplexDbContext<UserAvatarModel> 
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserAvatarConfiguration());

            CreateTable(modelBuilder);
        }
    }
}