// Created by Laxale 22.10.2016
//
//


namespace Freengy.Networking.DefaultImpl 
{
    using Freengy.Networking.Interfaces;


    public class LoginParameters : ILoginParameters 
    {
        public string Port { get; set; }

        public string Domain { get; set; }

        public string HostName { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }
    }
}