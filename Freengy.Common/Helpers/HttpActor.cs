// Created by Laxale 19.04.2018
//
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Freengy.Common.Constants;
using Freengy.Common.ErrorReason;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Interfaces;

using Nancy.Responses;

using NLog;


namespace Freengy.Common.Helpers 
{
    /// <summary>
    /// <see cref="IHttpActor"/> implementer.
    /// </summary>
    public class HttpActor : IHttpActor 
    {
        private readonly MediaTypes mediaTypes = new MediaTypes();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, string> addedHeaders = new Dictionary<string, string>();

        private string address;


        /// <summary>
        /// Gets the last responce message.
        /// </summary>
        public HttpResponseMessage ResponceMessage { get; private set; }


        /// <summary>
        /// Add HTTP header to sender.
        /// </summary>
        /// <param name="headerName">Header name.</param>
        /// <param name="headerValue">Header value.</param>
        /// <returns>this.</returns>
        public IHttpActor AddHeader(string headerName, string headerValue) 
        {
            if (string.IsNullOrWhiteSpace(headerName)) throw new ArgumentNullException(nameof(headerName));

            if (!addedHeaders.ContainsKey(headerName))
            {
                addedHeaders.Add(headerName, headerValue);
            }

            return this;
        }

        /// <summary>
        /// Set the HTTP address to send request to.
        /// </summary>
        /// <param name="requestAddress">HTTP address to send request to.</param>
        /// <returns>this.</returns>
        public IHttpActor SetRequestAddress(string requestAddress) 
        {
            if (string.IsNullOrWhiteSpace(requestAddress)) throw new ArgumentNullException(nameof(requestAddress));

            address = requestAddress;

            return this;
        }

        /// <summary>
        /// Execute HTTP GET and return responce message asynchronously.
        /// </summary>
        /// <returns>Responce message.</returns>
        public Task<HttpResponseMessage> GetAsync() 
        {
            return Task.Factory.StartNew(() =>
            {
                using (HttpClientHandler handler = CreateHandler())
                using (var client = new HttpClient(handler))
                {
                    AttachHeadersTo(client);

                    ResponceMessage = client.GetAsync(address).Result;

                    return ResponceMessage;
                }
            });
        }

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

        /// <summary>
        /// Execute POST method with a given message payload.
        /// </summary>
        /// <typeparam name="TRequest">Message payload type.</typeparam>
        /// <typeparam name="TResponce">Type of expected request to deserialize.</typeparam>
        /// <param name="request"><see cref="!:TRequest" /> instance.</param>
        /// <returns><see cref="!:TResponce" /> deserialized instance.</returns>
        public Task<Result<TResponce>> PostAsync<TRequest, TResponce>(TRequest request) where TRequest : class, new() where TResponce : class, new() 
        {
            return Task.Factory.StartNew(() =>
            {
                using (HttpClientHandler handler = CreateHandler())
                using (var client = new HttpClient(handler))
                {
                    AttachHeadersTo(client);

                    var serializeHelper = new SerializeHelper();
                    string jsonRequest = serializeHelper.Serialize(request);
                    string jsonMediaType = mediaTypes.GetStringValue(MediaTypesEnum.Json);
                    var httpRequest = new StringContent(jsonRequest, Encoding.UTF8, jsonMediaType);

                    ResponceMessage = client.PostAsync(address, httpRequest).Result;

                    if (IsKnownBadResponceCode(out Common.ErrorReason.ErrorReason reason))
                    {
                        return Result<TResponce>.Fail(reason);
                    }

                    try
                    {
                        bool parsed = TryGetAsRawJson<TResponce>(serializeHelper, out TResponce tResponce);

                        if (parsed)
                        {
                            return Result<TResponce>.Ok(tResponce);
                        }

                        //throw new Exception();
                        parsed = TryGetAsJsonResponce(serializeHelper, out TResponce jsonTresponce);

                        return Result<TResponce>.Ok(tResponce);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Failed to post request");

                        return Result<TResponce>.Fail(new UnexpectedErrorReason(ex.Message));
                    }
                }
            });
        }

        private bool IsKnownBadResponceCode(out ErrorReason.ErrorReason errorReason) 
        {
            errorReason = null;

            switch (ResponceMessage.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    errorReason = new NoServerConnectionErrorReason();
                    return true;

                case HttpStatusCode.Forbidden:
                    errorReason = new InvalidPasswordErrorReason();
                    return true;

                case HttpStatusCode.InternalServerError:
                    errorReason = new UnexpectedErrorReason("Unexpected server error");
                    return true;
            }

            return false;
        }

        private bool TryGetAsRawJson<TResponce>(SerializeHelper serializeHelper, out TResponce tresponce) 
            where TResponce : class, new()
        {
            tresponce = null;
            try
            {
                Stream responceStream = ResponceMessage.Content.ReadAsStreamAsync().Result;

                tresponce = serializeHelper.DeserializeObject<TResponce>(responceStream);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private bool TryGetAsJsonResponce<TResponce>(SerializeHelper serializeHelper, out TResponce tresponce) 
            where TResponce : class, new()
        {
            tresponce = null;
            try
            {
                Stream responceStream = ResponceMessage.Content.ReadAsStreamAsync().Result;

                var ttt = ResponceMessage.Content.ReadAsStringAsync().Result;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
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

        private void AttachHeadersTo(HttpClient client) 
        {
            foreach (KeyValuePair<string, string> pair in addedHeaders)
            {
                client.DefaultRequestHeaders.Add(pair.Key, pair.Value);
            }
        }
    }
}