// Created by Laxale 19.10.2016
//
//


namespace Freengy.Networking.Interfaces 
{
    using System.Threading.Tasks;


    public interface ILoginController
    {
        bool IsLoggedIn { get; }

        Task LogIn(ILoginParameters loginParameters);
    }
}