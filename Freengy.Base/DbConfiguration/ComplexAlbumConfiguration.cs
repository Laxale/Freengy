// Created by Laxale 04.05.2018
//
//

using System.Data.Entity.ModelConfiguration;
using Freengy.Base.Models.Readonly;
using Freengy.Common.Models;


namespace Freengy.Base.DbConfiguration 
{
    /// <summary>
    /// EF-конфигурация для работы с <see cref="AlbumModel"/>.
    /// </summary>
    public class ComplexAlbumConfiguration : EntityTypeConfiguration<AlbumModel> 
    {
        public ComplexAlbumConfiguration() 
        {
            ToTable($"{nameof(Album)}s");

            HasRequired(albumModel => albumModel.OwnerAccountModel)
                .WithMany(accountModel => accountModel.Albums)
                .HasForeignKey(albumModel => albumModel.OwnerAccountId)
                .WillCascadeOnDelete(true);
        }
    }
}