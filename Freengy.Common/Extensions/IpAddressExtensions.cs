// Created by Laxale 28.04.2018
//
//

using System;
using System.Net;
using System.Text.RegularExpressions;


namespace Freengy.Common.Extensions 
{
    /// <summary>
    /// Contains extension methods for <see cref="IPAddress"/>.
    /// </summary>
    public static class IpAddressExtensions 
    {
        /// <summary>
        /// Check is <see cref="IPAddress"/> is local.
        /// </summary>
        /// <param name="ipAddress">Address to check.</param>
        /// <returns>True if address is local - not used for outer web.</returns>
        public static bool IsLocal(this IPAddress ipAddress) 
        {
            string addressString = ipAddress?.ToString() ?? throw new ArgumentNullException(nameof(ipAddress));
            string localPattern = "192.168.*.*";

            bool isLocal = Regex.IsMatch(addressString, localPattern);

            return isLocal;
        }
    }
}