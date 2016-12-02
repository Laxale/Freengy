// Created by Laxale 02.12.2016
//
//


namespace Freengy.SharedWebTypes.Objects 
{
    using System;

    using Freengy.SharedWebTypes.Interfaces;

    public class UserAccount : IUserAccount 
    {
        public Guid Id { get; }
        public int Level { get; }
        public string Name { get; }
    }
}