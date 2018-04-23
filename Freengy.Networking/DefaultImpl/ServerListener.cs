// Created by Laxale 23.04.2018
//
//

using System;
using Freengy.Networking.Interfaces;
using NLog;

using Nancy;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// Client-side Freengy server listener.
    /// </summary>
    internal class ServerListener : IHttpClientParametersProvider 
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly string httpAddressNoPort = "http://localhost:";
        private static readonly object Locker = new object();
        private static readonly int portSelectStep = 100;
        private static readonly int maxStartTrials = 50;

        private static NancyHost host;
        private static ServerListener instance;

        private int initialPort = 12345;


        private ServerListener() 
        {
            StartClient();
        }


        /// <summary>
        /// Gets the client socket address.
        /// </summary>
        public string ClientAddress { get; private set; }


        /// <summary>
        /// Единственный инстанс <see cref="ServerListener"/>.
        /// </summary>
        public static IHttpClientParametersProvider Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new ServerListener());
                }
            }
        }


        private void StartClient() 
        {
            int trialsCount = 0;
            var baseUri = new Uri(GetCurrentClientAddress());
            INancyBootstrapper booter = CreateBootstrapper();
            HostConfiguration configuration = CreateConfiguration();

            bool started = TryStartHost(booter, configuration, baseUri);
            while (!started)
            {
                trialsCount++;

                if (trialsCount > maxStartTrials)
                {
                    throw new InvalidOperationException($"Failed to start client host after { maxStartTrials } retries");
                }

                initialPort += portSelectStep;
                string newAddress = GetCurrentClientAddress();
                baseUri = new Uri(newAddress);
                started = TryStartHost(booter, configuration, baseUri);
            }
        }
        
        private string GetCurrentClientAddress() 
        {
            return $"{ httpAddressNoPort }{ initialPort }";
        }


        private HostConfiguration CreateConfiguration() 
        {
            var config = new HostConfiguration
            {
                EnableClientCertificates = true,
                UrlReservations = new UrlReservations
                {
                    CreateAutomatically = true
                }
            };

            config.UnhandledExceptionCallback = ex =>
            {
                logger.Error(ex.GetRootException(), "Unhandled exception in client host");
            };

            return config;
        }

        private INancyBootstrapper CreateBootstrapper() 
        {
            return new DefaultNancyBootstrapper();
        }

        private bool TryStartHost(INancyBootstrapper booter, HostConfiguration configuration, Uri baseUri)
        {
            string address = baseUri.AbsoluteUri;
            try
            {
                host = new NancyHost(booter, configuration, baseUri);
                host.Start();
                logger.Info($"Started client host on { address }");

                ClientAddress = address;

                return true;
            }
            catch (Exception ex)
            {
                host.Dispose();
                logger.Error(ex, $"Failed to start client host on { address }");
                return false;
            }
        }
    }
}