// Created by Laxale 22.10.2016
//
//


namespace Freengy.Networking.Interfaces 
{
    using System.Security;


    public interface ILoginParameters 
    {
        string Port { get; set; }

        string Domain { get; set; }

        string HostName { get; set; }

        string UserName { get; set; }

        /// <summary>
        /// TODO: replace with <see cref="SecureString"/>
        /// </summary>
        string Password { get; set; }
    }
}