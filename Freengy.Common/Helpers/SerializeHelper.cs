﻿// Created by Laxale 17.04.2018
//
//

using System.IO;

using Newtonsoft.Json;


namespace Freengy.Common.Helpers 
{
    /// <summary>
    /// Contains some methods to (de)serialize objects from streams to JSON.
    /// </summary>
    public class SerializeHelper 
    {
        /// <summary>
        /// Serialize the object using accepted serializing settings.
        /// </summary>
        /// <param name="target">Object to serialize.</param>
        /// <returns>Serialized object representation.</returns>
        public string Serialize(object target) 
        {
            string serialized = JsonConvert.SerializeObject(target, Formatting.Indented);

            return serialized;
        }

        /// <summary>
        /// Deserialize <see cref="T"/> object from a json stream.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <param name="inputJsonStream">Stream containing json string.</param>
        /// <returns>New instance of a <see cref="T"/>.</returns>
        public T DeserializeObject<T>(Stream inputJsonStream) where T : class, new() 
        {
            var serializer = new JsonSerializer();

            using (var streamReader = new StreamReader(inputJsonStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                T deserialized = serializer.Deserialize<T>(jsonReader);

                return deserialized;
            }
        }
    }
}