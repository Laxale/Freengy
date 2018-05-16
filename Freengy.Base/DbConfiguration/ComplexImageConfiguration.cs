// Created by Laxale 04.05.2018
//
//

using System.Data.Entity.ModelConfiguration;

using Freengy.Common.Models;


namespace Freengy.Base.DbConfiguration 
{
    /// <summary>
    /// EF-конфигурация для работы с <see cref="ImageModel"/>.
    /// </summary>
    public class ComplexImageConfiguration : EntityTypeConfiguration<ImageModel> 
    {
        public ComplexImageConfiguration() 
        {
            ToTable("Images");

            HasRequired(imageModel => imageModel.NavigationParent)
                .WithMany(albumModel => albumModel.Images)
                .HasForeignKey(imageModel => imageModel.ParentId)
                .WillCascadeOnDelete(true);
        }
    }
}