// Created by Laxale 17.04.2018
//
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freengy.Common.Interfaces;


namespace Freengy.Common.Database 
{
    /// <summary>
    /// Базовый класс для хранящихся в базе объектов.
    /// </summary>
    public abstract class DbObject : IObjectWithId 
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        protected DbObject() 
        {
            //Id = Guid.NewGuid().ToString();
            Id = Guid.NewGuid();
        }


        /// <summary>
        /// Уникальный идентификатор объекта в базе.
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public string Id { get; set; }
        public Guid Id { get; set; }
    }
}