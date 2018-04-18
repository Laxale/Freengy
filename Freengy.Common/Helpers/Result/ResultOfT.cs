// Created by Laxale 17.04.2018
//
//


namespace Freengy.Common.Helpers.Result 
{
    /// <summary>
    /// Результат операции.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    public sealed class Result<T> : Result 
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        private Result() 
        {

        }


        /// <summary>
        /// Значение.
        /// </summary>
        public T Value { get; private set; }


        /// <summary>
        /// Провал.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <returns>Result.</returns>
        public new static Result<T> Fail(ErrorReason.ErrorReason error) 
        {
            var result = new Result<T>();
            result.Failed(error);
            return result;
        }

        /// <summary>
        /// Успешный результат.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Result.</returns>
        public static Result<T> Ok(T value) 
        {
            var result = new Result<T>();
            result.Succeeded();
            result.Value = value;
            return result;
        }
    }
}