// Created by Laxale 19.10.2016
//
//


namespace Freengy.Networking.Interfaces 
{
    using System.Threading.Tasks;

    using Freengy.SharedWebTypes.Objects;

    public interface ILoginController
    {
        bool IsLoggedIn { get; }

        bool Register(LoginModel loginParameters);

        Task LogInAsync(LoginModel loginParameters);
    }
}