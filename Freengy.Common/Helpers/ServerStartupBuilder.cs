// Created by Laxale 24.04.2018
//
//

using System;

using NLog;

using Nancy;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;


namespace Freengy.Common.Helpers 
{
    /// <summary>
    /// Web host startup builder.
    /// </summary>
    public class ServerStartupBuilder 
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ushort minimumTrials = 1;
        private readonly ushort minPort = 1000;
        private readonly ushort maxPortStep = 1000;

        private NancyHost host;
        private string baseIpAddress;
        private ushort maxTrials;
        private ushort initialPort;
        private ushort portSelectStep;
        private bool useHttps;


        /// <summary>
        /// Set flag to use or not use HTTPS.
        /// </summary>
        /// <param name="useHttps">True - to use HTTPS.</param>
        /// <returns>this.</returns>
        public ServerStartupBuilder UseHttps(bool useHttps) 
        {
            this.useHttps = useHttps;

            return this;
        }

        /// <summary>
        /// Set initial host port to try to start on.
        /// </summary>
        /// <param name="initialPort">Initial host port.</param>
        /// <returns>this.</returns>
        public ServerStartupBuilder SetInitialPort(ushort initialPort) 
        {
            this.initialPort =
                initialPort > minPort ?
                    initialPort :
                    throw new InvalidOperationException($"Initial port must be greater than { minPort }");

            return this;
        }

        /// <summary>
        /// Set port checking step. It will be added to current port value in case of startup failure.
        /// </summary>
        /// <param name="portStep">Port selection step value.</param>
        /// <returns>this.</returns>
        public ServerStartupBuilder SetPortStep(ushort portStep) 
        {
            portSelectStep =
                portStep < maxPortStep ?
                    portStep :
                    throw new InvalidOperationException($"Port step must be lesser than { maxPortStep }");

            return this;
        }

        /// <summary>
        /// Set maximum startup trials. Exception will be thrown in case of maximum startup failures.
        /// </summary>
        /// <param name="trialsCount">Startup trials count.</param>
        /// <returns>this.</returns>
        public ServerStartupBuilder SetTrialsCount(ushort trialsCount) 
        {
            this.maxTrials =
                trialsCount > minimumTrials ?
                    trialsCount :
                    throw new InvalidOperationException($"Trials count must be greater than { minimumTrials }");

            return this;
        }

        /// <summary>
        /// Set base host IP address without port. It can be 'localhost' or 'xxx.xxx.xxx.xxx'.
        /// </summary>
        /// <param name="baseAddress">Base host IP address.</param>
        /// <returns>this.</returns>
        public ServerStartupBuilder SetBaseAddress(string baseAddress) 
        {
            if (string.IsNullOrWhiteSpace(baseAddress)) throw new ArgumentNullException(nameof(baseAddress));

            baseIpAddress = baseAddress;

            return this;
        }


        /// <summary>
        /// Build and start host with selected configuration.
        /// </summary>
        /// <returns>Full host startup address.</returns>
        public string Build() 
        {
            int trialsCount = 0;
            var baseUri = new Uri(GetCurrentHostAddress());
            INancyBootstrapper booter = CreateBootstrapper();
            HostConfiguration configuration = CreateConfiguration();

            string startedAddress = TryStartHost(booter, configuration, baseUri);
            while (string.IsNullOrWhiteSpace(startedAddress))
            {
                trialsCount++;

                if (trialsCount > maxTrials)
                {
                    throw new InvalidOperationException($"Failed to start host after { maxTrials } retries");
                }

                initialPort += portSelectStep;
                string newAddress = GetCurrentHostAddress();
                baseUri = new Uri(newAddress);
                startedAddress = TryStartHost(booter, configuration, baseUri);
            }

            return startedAddress;
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
                logger.Error(ex, "Unhandled exception in client host");
            };

            return config;
        }

        private INancyBootstrapper CreateBootstrapper() 
        {
            return new DefaultNancyBootstrapper();
        }

        private string GetCurrentHostAddress() 
        {
            string schema = useHttps ? "https" : "http";

            return $"{ schema }://{ baseIpAddress }:{ initialPort }";
        }

        private string TryStartHost(INancyBootstrapper booter, HostConfiguration configuration, Uri baseUri) 
        {
            string address = baseUri.AbsoluteUri;
            try
            {
                logger.Info($"Trying to start host on { address }");
                host = new NancyHost(booter, configuration, baseUri);
                host.Start();
                logger.Info($"Started host on { address }");

                return address;
            }
            catch (Exception ex)
            {
                host.Dispose();
                logger.Error(ex, $"Failed to start host on { address }");
                return string.Empty;
            }
        }
    }
}