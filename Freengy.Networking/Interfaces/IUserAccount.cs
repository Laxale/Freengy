// Created by Laxale 23.10.2016
//
//


namespace Freengy.Networking.Interfaces 
{
    using System;


    /// <summary>
    /// Exposes user 
    /// </summary>
    public interface IUserAccount 
    {
        Guid Id { get; }

        string Name { get; }
    }
}