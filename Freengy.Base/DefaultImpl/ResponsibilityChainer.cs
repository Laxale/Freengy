// Created by Laxale 23.10.2016
//
//


namespace Freengy.Base.DefaultImpl 
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    
    using Freengy.Base.Interfaces;


    /// <summary>
    /// Default <see>
    ///         <cref>IResponsibilityChainer</cref>
    ///     </see>
    ///     implementer
    /// </summary>
    /// <typeparam name="TObjectType">Type of a processing target</typeparam>
    public class ResponsibilityChainer<TObjectType> : IResponsibilityChainer<TObjectType> 
    {
        private readonly List<Func<TObjectType, bool>> chain = new List<Func<TObjectType, bool>>();


        public IResponsibilityChainer<TObjectType> AddHandler(Func<TObjectType, bool> handler) 
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            if (this.chain.Contains(handler)) return this;

            this.chain.Add(handler);

            return this;
        }

        public IResponsibilityChainer<TObjectType> RemoveHandler(Func<TObjectType, bool> handler) 
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            if (!this.chain.Contains(handler)) return this;

            this.chain.Remove(handler);

            return this;
        }

        public bool Handle(TObjectType targetToProcess) 
        {
            bool handled = false;

            foreach (var handler in this.chain)
            {
                handled |= handler(targetToProcess);

                if (handled) break;
            }

            return handled;
        }

        public async Task<bool> HandleAsync(TObjectType targetToProcess) 
        {
            if (targetToProcess == null) throw new ArgumentNullException(nameof(targetToProcess));

            return await Task.Run(() => this.Handle(targetToProcess));
        }
    }
}