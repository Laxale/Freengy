﻿// Created by Laxale 17.04.2018
//
//


namespace Freengy.Base.Helpers 
{
    /// <summary>
    /// Результат операции.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        protected Result()
        {
            Error = ErrorReason.None;
        }

        /// <summary>
        /// Успешен ли результат.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Ошибка.
        /// </summary>
        public ErrorReason Error { get; private set; }

        /// <summary>
        /// Провальный ли результат.
        /// </summary>
        public bool Failure
        {
            get { return !Success; }
        }


        /// <summary>
        /// Провальный результат.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <returns>Result.</returns>
        public static Result Fail(ErrorReason error)
        {
            return new Result().Failed(error);
        }

        /// <summary>
        /// Провальный результат.
        /// </summary>
        /// <typeparam name="T">Тип значения.</typeparam>
        /// <param name="error">Ошибка.</param>
        /// <returns>Result.</returns>
        public static Result<T> Fail<T>(ErrorReason error)
        {
            return Result<T>.Fail(error);
        }

        /// <summary>
        /// Успешный результат.
        /// </summary>
        /// <returns>Result.</returns>
        public static Result Ok()
        {
            return new Result().Succeeded();
        }

        /// <summary>
        /// Успешный результат.
        /// </summary>
        /// <typeparam name="T">Тип значения.</typeparam>
        /// <param name="value">Значение.</param>
        /// <returns>Result.</returns>
        public static Result<T> Ok<T>(T value)
        {
            return Result<T>.Ok(value);
        }

        /// <summary>
        /// Преведение ErrorReason к Result.
        /// </summary>
        /// <param name="reason">Ошибка.</param>
        public static implicit operator Result(ErrorReason reason)
        {
            return Fail(reason);
        }

        /// <summary>
        /// Провал.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <returns>Result.</returns>
        protected Result Failed(ErrorReason error)
        {
            Success = false;
            Error = error;

            return this;
        }

        /// <summary>
        /// Успех.
        /// </summary>
        /// <returns>Result.</returns>
        protected Result Succeeded()
        {
            Success = true;
            return this;
        }
    }
}