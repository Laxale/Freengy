// Created by Laxale 19.10.2016
//
//


namespace Freengy.Networking.Interfaces 
{
    public interface ILoginController
    {
        void LogIn(ILoginParameters loginParameters);

        bool IsLoggedIn { get; }
    }
}