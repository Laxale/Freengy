// Created by Laxale 21.04.2018
//
//

using System;
using System.Text;
using System.Security.Cryptography;


namespace Freengy.Common.Helpers 
{
    /// <summary>
    /// Contains methods to work with hashes.
    /// </summary>
    public class Hasher 
    {
        /// <summary>
        /// Get base64 hash value of a given string.
        /// </summary>
        /// <param name="value">String to compute hash.</param>
        /// <returns>Hash value of a string.</returns>
        public string GetHash(string value) 
        {
            if(string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            using (SHA512 keccak = SHA512.Create())
            {
                byte[] passwordBytes = Encoding.ASCII.GetBytes(value);

                byte[] hash = keccak.ComputeHash(passwordBytes);

                string result = Convert.ToBase64String(hash);

                return result;
            }
        }
    }
}