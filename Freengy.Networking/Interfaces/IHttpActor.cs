// Created by Laxale 19.04.2018
//
//

using System;
using System.Threading.Tasks;


namespace Freengy.Networking.Interfaces 
{
    /// <summary>
    /// Interface to hide HTTP actions implementation.
    /// </summary>
    public interface IHttpActor : IDisposable 
    {
        /// <summary>
        /// Set the HTTP address to send request to.
        /// </summary>
        /// <param name="requestAddress">HTTP address to send request to.</param>
        /// <returns>this.</returns>
        IHttpActor SetAddress(string requestAddress);

        /// <summary>
        /// Execute GET method with a given message payload.
        /// </summary>
        /// <typeparam name="TRequest">Message payload type.</typeparam>
        /// <typeparam name="TResponce">Type of expected request to deserialize.</typeparam>
        /// <param name="request"><see cref="TRequest"/> instance.</param>
        /// <returns><see cref="TResponce"/> deserialized instance.</returns>
        Task<TResponce> GetAsync<TRequest, TResponce>(TRequest request) 
            where TRequest : class, new()
            where TResponce : class, new();
        
        /// <summary>
        /// Execute POST method with a given message payload.
        /// </summary>
        /// <typeparam name="TRequest">Message payload type.</typeparam>
        /// <typeparam name="TResponce">Type of expected request to deserialize.</typeparam>
        /// <param name="request"><see cref="TRequest"/> instance.</param>
        /// <returns><see cref="TResponce"/> deserialized instance.</returns>
        Task<TResponce> PostAsync<TRequest, TResponce>(TRequest request) 
            where TRequest : class, new()
            where TResponce : class, new();
    }
}