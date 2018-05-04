// Created by Laxale 04.05.2018
//
//

using System;


namespace Freengy.Base.Messages 
{
    /// <summary>
    /// Запрос на добавление изображения в альбом.
    /// </summary>
    public class MessageAddImageRequest : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageAddImageRequest"/> с заданным удалённым или локальным путём к изображению.
        /// </summary>
        /// <param name="imageUri">Удалённый или локальный путь к изображению.</param>
        public MessageAddImageRequest(string imageUri) 
        {
            if(string.IsNullOrWhiteSpace(imageUri)) throw new ArgumentNullException(nameof(imageUri));

            ImageUri = imageUri;
        }


        /// <summary>
        /// Удалённый или локальный путь к изображению.
        /// </summary>
        public string ImageUri { get; }
    }
}