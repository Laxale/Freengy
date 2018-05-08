// Created by Laxale 17.04.2018
//
//

using System;
using System.Diagnostics;


namespace Freengy.Common.Helpers.Result 
{
    /// <summary>
    /// Расширения для <see cref="Result"/>.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Выполнить в любом случае.
        /// </summary>
        /// <param name="result">Резкльтат.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static Result OnBoth(this Result result, Action<Result> action) 
        {
            action(result);

            return result;
        }

        /// <summary>
        /// Выполнить в любом случае.
        /// </summary>
        /// <typeparam name="T">Тип значения результата.</typeparam>
        /// <param name="result">Результат.</param>
        /// <param name="func">Функция.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static T OnBoth<T>(this Result result, Func<Result, T> func)
        {
            return func(result);
        }

        /// <summary>
        /// Выполнить в случае провала.
        /// </summary>
        /// <typeparam name="T">Тип значения результата.</typeparam>
        /// <param name="result">Результат.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static Result<T> OnFailure<T>(this Result<T> result, Action action)
        {
            if (result.Failure)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// Выполнить в случае провала.
        /// </summary>
        /// <param name="result">Результат.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static Result OnFailure(this Result result, Action action)
        {
            if (result.Failure)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// Выполнить в случае провала.
        /// </summary>
        /// <param name="result">Результат.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static Result OnFailure(this Result result, Action<Common.ErrorReason.ErrorReason> action)
        {
            if (result.Failure)
            {
                action(result.Error);
            }

            return result;
        }

        /// <summary>
        /// Выполнить в случае провала.
        /// </summary>
        /// <typeparam name="TResult">Тип значения результата.</typeparam>
        /// <param name="result">Результат.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static Result<TResult> OnFailure<TResult>(this Result<TResult> result, Func<Common.ErrorReason.ErrorReason, TResult> action)
        {
            if (result.Failure)
            {
                TResult onFailureResult = action(result.Error);

                return Result<TResult>.Ok(onFailureResult);
            }

            return result;
        }

        /// <summary>
        /// Выполнить в случае провала.
        /// </summary>
        /// <typeparam name="TResult">Тип значения результата.</typeparam>
        /// <param name="result">Результат.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static Result<TResult> OnFailure<TResult>(this Result<TResult> result,
            Func<Common.ErrorReason.ErrorReason, Result<TResult>> action)
        {
            if (result.Failure)
            {
                return action(result.Error);
            }

            return result;
        }

        /// <summary>
        /// Выполнить в случае успеха.
        /// </summary>
        /// <param name="result">Результат.</param>
        /// <param name="func">Функция.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static Result OnSuccess(this Result result, Func<Result> func)
        {
            if (result.Failure)
            {
                return result;
            }

            return func();
        }

        /// <summary>
        /// Выполнить в случае успеха.
        /// </summary>
        /// <param name="result">Результат.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.Failure)
            {
                return result;
            }

            action();

            return Result.Ok();
        }

        /// <summary>
        /// Выполнить в случае успеха.
        /// </summary>
        /// <typeparam name="T">Тип значения результата.</typeparam>
        /// <param name="result">Результат.</param>
        /// <param name="action">Действие.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static Result OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.Failure)
            {
                return result;
            }

            action(result.Value);

            return Result.Ok();
        }

        /// <summary>
        /// Убедиться, что результат успешен.
        /// </summary>
        /// <typeparam name="T">Тип значения результата.</typeparam>
        /// <param name="result">Результат.</param>
        /// <param name="errorMessage">Сообщение об ошибке.</param>
        /// <returns>Значение результата.</returns>
        [DebuggerStepThrough]
        public static T EnsureSuccess<T>(this Result<T> result, string errorMessage)
        {
            if (result.Success)
            {
                return result.Value;
            }

            throw new Exception(errorMessage);
        }

        /// <summary>
        /// Выполнить в случае успеха.
        /// </summary>
        /// <typeparam name="TValue">Тип значения.</typeparam>
        /// <typeparam name="TResult">Тип результата.</typeparam>
        /// <param name="result">Результат.</param>
        /// <param name="func">Функция.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static Result<TResult> OnSuccess<TValue, TResult>(this Result<TValue> result, Func<TValue, Result<TResult>> func) 
        {
            if (result.Failure)
            {
                return Result<TResult>.Fail(result.Error);
            }

            return func(result.Value);
        }

        /// <summary>
        /// Выполнить в случае успеха.
        /// </summary>
        /// <typeparam name="TValue">Тип значения.</typeparam>
        /// <param name="result">Результат.</param>
        /// <param name="func">Функция.</param>
        /// <returns>Результат.</returns>
        [DebuggerStepThrough]
        public static Result OnSuccess<TValue>(this Result<TValue> result, Func<TValue, Result> func)
        {
            if (result.Failure)
            {
                return result.Error;
            }

            return func(result.Value);
        }
    }
}
