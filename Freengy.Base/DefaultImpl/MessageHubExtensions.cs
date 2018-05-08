// Created by Laxale 08.05.2018
//
//

using System;


namespace Freengy.Base.DefaultImpl 
{
    public static class MessageHubExtensions 
    {
        private static readonly MessageHub MessageHub = new MessageHub();

        public static bool Exists<T>( this object obj )
        {
            foreach ( var h in MessageHub.handlers )
            {
                if ( Equals(h.Sender.Target, obj ) &&
                    typeof(T) == h.Type )
                {
                    return true;
                }
            }

            return false;
        }

        public static void Publish<T>( this object obj )
        {
            MessageHub.Publish( obj, default( T ) );
        }

        public static void Publish<T>( this object obj, T data )
        {
            MessageHub.Publish( obj, data );
        }

        public static void Subscribe<T>( this object obj, Action<T> handler )
        {
            MessageHub.Subscribe( obj, handler );
        }

        public static void Unsubscribe( this object obj )
        {
            MessageHub.Unsubscribe( obj );
        }

        public static void Unsubscribe<T>( this object obj )
        {
            MessageHub.Unsubscribe( obj, (Action<T>) null );
        }

        public static void Unsubscribe<T>( this object obj, Action<T> handler )
        {
            MessageHub.Unsubscribe( obj, handler );
        }
    }
}