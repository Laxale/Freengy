// Created by Laxale 19.04.2018
//
//

using System;
using System.Net.Http;
using System.Threading.Tasks;

using Freengy.Common.Helpers.Result;


namespace Freengy.Common.Interfaces 
{
    /// <summary>
    /// Interface to hide HTTP actions implementation.
    /// </summary>
    public interface IHttpActor : IDisposable 
    {
        /// <summary>
        /// Gets the last responce message.
        /// </summary>
        HttpResponseMessage ResponceMessage { get; }

        /// <summary>
        /// Execute HTTP GET and return responce message asynchronously.
        /// </summary>
        /// <returns>Responce message.</returns>
        Task<HttpResponseMessage> GetAsync();

        /// <summary>
        /// Add HTTP header to sender.
        /// </summary>
        /// <param name="headerName">Header name.</param>
        /// <param name="headerValue">Header value.</param>
        /// <returns>this.</returns>
        IHttpActor AddHeader(string headerName, string headerValue);
        
        /// <summary>
        /// Set the HTTP address to send request to.
        /// </summary>
        /// <param name="requestAddress">HTTP address to send request to.</param>
        /// <returns>this.</returns>
        IHttpActor SetRequestAddress(string requestAddress);

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
        Task<Result<TResponce>> PostAsync<TRequest, TResponce>(TRequest request) 
            where TRequest : class, new()
            where TResponce : class, new();
    }
}