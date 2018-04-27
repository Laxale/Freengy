// Created by Laxale 04.12.2016
//
//

using System.Collections.Generic;

namespace Freengy.Common.Helpers 
{
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
            return mediaTypes[mediaType];
        }
    }
}