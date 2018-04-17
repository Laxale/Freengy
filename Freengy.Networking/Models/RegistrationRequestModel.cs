// Created by Laxale 17.04.2018
//
//

using System;


namespace Freengy.Networking.Models 
{
    /// <summary>
    /// Model to serialize and send to server new user registration request.
    /// </summary>
    public class RegistrationRequestModel 
    {
        /// <summary>
        /// Creates new <see cref="RegistrationRequestModel"/> with a given user Name.
        /// </summary>
        /// <param name="userName">Desired new user name to register.</param>
        public RegistrationRequestModel(string userName) 
        {
            if(string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));

            UserName = userName;

            RequestTime = DateTime.Now;
        }


        public DateTime RequestTime { get; }

        public string UserName { get; }
    }
}