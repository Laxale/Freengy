// Created by Laxale 23.04.2018
//
//

using System.Threading.Tasks;


namespace Freengy.Networking.Interfaces 
{
    /// <summary>
    /// Interface to expose client-side Freengy server listener parameters.
    /// </summary>
    public interface IHttpClientParametersProvider 
    {
        /// <summary>
        /// Get the client socket address asynchronously.
        /// </summary>
        Task<string> GetClientAddressAsync();
    }
}