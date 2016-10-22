// Created by Laxale 22.10.2016
//
//


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Represents objects that register services/types/whatever in catel infrastructure
    /// </summary>
    public interface IRegistrator 
    {
        /// <summary>
        /// Register some desired stuff in catel infrastructure
        /// </summary>
        void Register();

        /// <summary>
        /// Remove all registered stuff
        /// </summary>
        void Unregister();

        /// <summary>
        /// Registration is not considered to be done more than once
        /// </summary>
        bool IsRegistered { get; }
    }
}