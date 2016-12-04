// Created by Laxale 04.12.2016
//
//


namespace Freengy.Networking.DefaultImpl 
{
    using System.Collections.Generic;


    internal enum MediaTypesEnum 
    {
        Json,
        Xml
    }

    internal class MediaTypes 
    {
        private static readonly IDictionary<MediaTypesEnum, string> mediaTypes = new Dictionary<MediaTypesEnum, string>
        {
            { MediaTypesEnum.Xml, "application/xml" },
            { MediaTypesEnum.Json, "application/json" }
        };


        public string GetStringValue(MediaTypesEnum mediaType) 
        {
            return MediaTypes.mediaTypes[mediaType];
        }
    }
}