// Created by Laxale 26.11.2016
//
//


namespace Freengy.Settings.Interfaces 
{
    /// <summary>
    /// Must be implemented by a database model-class to expose a primary key
    /// </summary>
    public interface IObjectWithLongId 
    {
        long Id { get; set; }
    }
}