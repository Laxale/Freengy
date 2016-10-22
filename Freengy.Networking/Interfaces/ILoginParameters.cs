// Created by Laxale 22.10.2016
//
//


namespace Freengy.Networking.Interfaces 
{
    public interface ILoginParameters 
    {
        string Port { get; set; }

        string Domain { get; set; }

        string HostName { get; set; }

        string UserName { get; set; }

        string Password { get; set; }
    }
}