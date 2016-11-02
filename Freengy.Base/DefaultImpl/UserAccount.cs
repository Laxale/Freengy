// Created by Laxale 23.10.2016
//
//


namespace Freengy.Base.DefaultImpl 
{
    using System;

    using Freengy.Base.Interfaces;


    internal class UserAccount : IUserAccount 
    {
        public UserAccount() 
        {
            this.Name = Guid.NewGuid().ToString();
            this.DisplayedName = $"Displayed: { Guid.NewGuid() }";
        }


        public string Name { get; }

        public string DisplayedName { get; }

        public Guid Id { get; } = Guid.NewGuid();
    }
}