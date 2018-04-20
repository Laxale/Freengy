// Created by Laxale 17.04.2018
//
//

using System;

using Freengy.Common.Enums;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Model to serialize and send to server new user registration request.
    /// </summary>
    public class RegistrationRequest 
    {
        /// <summary>
        /// Default ctor for deserializing purposes.
        /// </summary>
        public RegistrationRequest() 
        {
            
        }

        /// <summary>
        /// Creates new <see cref="RegistrationRequest"/> with a given user Name.
        /// </summary>
        /// <param name="userName">Desired new user name to register.</param>
        public RegistrationRequest(string userName) 
        {
            if(string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));

            UserName = userName;

            RequestTime = DateTime.Now;
        }


        /// <summary>
        /// Request creation timestamp.
        /// </summary>
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// Desired user name to register.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Registration status is set by server when processed request.
        /// </summary>
        public RegistrationStatus Status { get; set; }

        /// <summary>
        /// Account model is created by server in case of success.
        /// </summary>
        public UserAccountModel CreatedAccount { get; set; }
    }
}