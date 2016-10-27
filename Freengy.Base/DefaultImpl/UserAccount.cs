// Created by Laxale 23.10.2016
//
//


namespace Freengy.Base.DefaultImpl 
{
    using System;

    using Freengy.Base.Interfaces;


    public class UserAccount : IUserAccount 
    {
        public UserAccount() 
        {
            this.Name = Guid.NewGuid().ToString();
            this.DisplayedName = $"Displayed: { Guid.NewGuid() }";
        }


        public Guid Id { get; }

        public string Name { get; }

        public string DisplayedName { get; }
    }
}