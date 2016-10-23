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


        public void AddHandler(Func<TObjectType, bool> handler) 
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            if (this.chain.Contains(handler)) return;

            this.chain.Add(handler);
        }

        public void RemoveHandler(Func<TObjectType, bool> handler) 
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            if (!this.chain.Contains(handler)) return;

            this.chain.Remove(handler);
        }

        public async Task<bool> HandleAsync(TObjectType targetToProcess) 
        {
            if (targetToProcess == null) throw new ArgumentNullException(nameof(targetToProcess));

            Func<bool> handleFunc =
                () =>
                {
                    bool handled = false;

                    foreach (var handler in this.chain)
                    {
                        handled |= handler(targetToProcess);

                        if (handled) break;
                    }

                    return handled;
                };

            return await Task.Run(handleFunc);
        }
    }
}