// Created by Laxale 08.05.2018
//
//

using System;
using System.Linq;
using System.Collections.Generic;


namespace Freengy.Base.DefaultImpl 
{
    /// <summary>
    /// Хаб обмена сообщениями.
    /// </summary>
    public class MessageHub 
    {
        internal class Handler 
        {
            public Type Type { get; set; }

            public Delegate Action { get; set; }

            public WeakReference Sender { get; set; }
        }


        internal readonly object locker = new object();

        internal readonly List<Handler> handlers = new List<Handler>();


        /// <summary>
        /// Allow publishing directly onto this MessageHub.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void Publish<T>(T data = default(T))
        {
            Publish(this, data);
        }

        public void Publish<T>( object sender, T data = default( T ) )
        {
            var handlerList = new List<Handler>( handlers.Count );
            var handlersToRemoveList = new List<Handler>( handlers.Count );

            lock ( this.locker )
            {
                var messageType = typeof(T);

                foreach ( var handler in handlers )
                {
                    if ( !handler.Sender.IsAlive )
                    {
                        handlersToRemoveList.Add( handler );
                    }
                    else if ( handler.Type.IsAssignableFrom(messageType))
                    {
                        handlerList.Add(handler);
                    }
                }

                foreach ( var l in handlersToRemoveList )
                {
                    handlers.Remove( l );
                }
            }

            foreach ( var l in handlerList )
            {
                ( (Action<T>) l.Action )( data );
            }
        }

        /// <summary>
        /// Allow subscribing directly to this MessageHub.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void Subscribe<T>(Action<T> handler)
        {
            Subscribe(this, handler);
        }

        public void Subscribe<T>( object sender, Action<T> handler )
        {
            var item = new Handler
            {
                Action = handler,
                Sender = new WeakReference( sender ),
                Type = typeof( T )
            };

            lock ( this.locker )
            {
                this.handlers.Add( item );
            }
        }

        /// <summary>
        /// Allow unsubscribing directly to this MessageHub.
        /// </summary>
        public void Unsubscribe()
        {
            Unsubscribe(this);
        }

        public void Unsubscribe( object sender )
        {
            lock ( this.locker )
            {
                var query = this.handlers.Where( a => !a.Sender.IsAlive || a.Sender.Target.Equals( sender ) );

                foreach ( var h in query.ToList() )
                {
                    this.handlers.Remove( h );
                }
            }
        }

        /// <summary>
        /// Allow unsubscribing directly to this MessageHub.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Unsubscribe<T>()
        {
            Unsubscribe<T>(this);
        }

        /// <summary>
        /// Allow unsubscribing directly to this MessageHub.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void Unsubscribe<T>(Action<T> handler = null)
        {
            Unsubscribe<T>(this, handler);
        }

        public void Unsubscribe<T>( object sender, Action<T> handler = null )
        {
            lock ( this.locker )
            {
                var query = this.handlers
                    .Where( a => !a.Sender.IsAlive ||
                                 ( a.Sender.Target.Equals( sender ) && a.Type == typeof( T ) ) );

                if ( handler != null )
                {
                    query = query.Where( a => a.Action.Equals( handler ) );
                }

                foreach ( var h in query.ToList() )
                {
                    this.handlers.Remove( h );
                }
            }
        }
    }
}