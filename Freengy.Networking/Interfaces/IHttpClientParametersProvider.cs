// Created by Laxale 23.04.2018
//
//


namespace Freengy.Networking.Interfaces 
{
    /// <summary>
    /// Interface to expose client-side Freengy server listener parameters.
    /// </summary>
    public interface IHttpClientParametersProvider 
    {
        /// <summary>
        /// Gets the client socket address.
        /// </summary>
        string ClientAddress { get; }
    }
}