// Created by Laxale 23.10.2016
//
//


namespace Freengy.Networking.DefaultImpl 
{
    using System;

    using Freengy.Networking.Interfaces;


    public class UserAccount : IUserAccount 
    {
        public Guid Id { get; }

        public string Name { get; }
    }
}