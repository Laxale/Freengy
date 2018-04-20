// Created by Laxale 19.04.2018
//
//

using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Freengy.Common.Helpers;
using Freengy.Networking.Interfaces;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// <see cref="IHttpActor"/> implementer.
    /// </summary>
    internal class HttpActor : IHttpActor 
    {
        private readonly MediaTypes mediaTypes = new MediaTypes();

        private string address;


        /// <inheritdoc />
        /// <summary>
        /// Set the HTTP address to send request to.
        /// </summary>
        /// <param name="requestAddress">HTTP address to send request to.</param>
        /// <returns>this.</returns>
        public IHttpActor SetAddress(string requestAddress) 
        {
            //if (string.IsNullOrWhiteSpace(requestAddress)) throw new ArgumentNullException(nameof(requestAddress));

            address = requestAddress;

            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Execute GET method with a given message payload.
        /// </summary>
        /// <typeparam name="TRequest">Message payload type.</typeparam>
        /// <typeparam name="TResponce">Type of expected request to deserialize.</typeparam>
        /// <param name="request"><see cref="!:TRequest" /> instance.</param>
        /// <returns><see cref="!:TResponce" /> deserialized instance.</returns>
        public Task<TResponce> GetAsync<TRequest, TResponce>(TRequest request) where TRequest : class, new() where TResponce : class, new() 
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Execute POST method with a given message payload.
        /// </summary>
        /// <typeparam name="TRequest">Message payload type.</typeparam>
        /// <typeparam name="TResponce">Type of expected request to deserialize.</typeparam>
        /// <param name="request"><see cref="!:TRequest" /> instance.</param>
        /// <returns><see cref="!:TResponce" /> deserialized instance.</returns>
        public Task<TResponce> PostAsync<TRequest, TResponce>(TRequest request) where TRequest : class, new() where TResponce : class, new()
        {
            //await Task.Run(async () => { });
            //Task.Factory.StartNew()

            return Task.Factory.StartNew(() =>
            //return await Task.Run(async () =>
            {
                using (HttpClientHandler handler = CreateHandler())
                using (var client = new HttpClient(handler))
                {
                    var serializeHelper = new SerializeHelper();
                    string jsonRequest = serializeHelper.Serialize(request);
                    string jsonMediaType = mediaTypes.GetStringValue(MediaTypesEnum.Json);
                    var httpRequest = new StringContent(jsonRequest, Encoding.UTF8, jsonMediaType);

                    HttpResponseMessage responseMessage = client.PostAsync(address, httpRequest).Result;

                    Stream responceStream = responseMessage.Content.ReadAsStreamAsync().Result;

                    var responce = serializeHelper.DeserializeObject<TResponce>(responceStream);

                    return responce;
                }
            });
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }


        private HttpClientHandler CreateHandler() 
        {
            var handler = new HttpClientHandler
            {
                UseCookies = true
            };

            return handler;
        }
    }
}