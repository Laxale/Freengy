// Created by Laxale 17.04.2018
//
//

using Freengy.Base.Helpers;
using Freengy.Database.Object;


namespace Freengy.Database 
{
    /// <summary>
    /// Интерфейс репозитория настроек.
    /// </summary>
    public interface IObjectStorage
    {
        /// <summary>
        /// Вернуть единственный простой объект типа <see cref="TSimpleObject"/>.
        /// </summary>
        /// <typeparam name="TSimpleObject">Тип простого объекта для получения.</typeparam>
        /// <returns>Результат с объектом, полученным из базы (объект может быть null).</returns>
        Result<TSimpleObject> GetSingleSimple<TSimpleObject>() where TSimpleObject : SimpleDbObject, new();

        /// <summary>
        /// Вернуть единственный сложный объект типа <see cref="TComplexObject"/>.
        /// </summary>
        /// <typeparam name="TComplexObject">Тип сложных настроек.</typeparam>
        /// <returns>Результат получения объекта сложных настроек.</returns>
        Result<TComplexObject> GetSingleComplex<TComplexObject>() where TComplexObject : ComplexDbObject, new();

        /// <summary>
        /// Сохранить единственный простой объект - не содержащий пропертей в виде вложенных типов.
        /// </summary>
        /// <typeparam name="TSimpleObject">Тип простого объекта для сохранения.</typeparam>
        /// <param name="objectToSave">Объект простого типа. Если его нет в базе, вернётся null.</param>
        /// <returns>Результат сохранения.</returns>
        Result SaveSingleSimple<TSimpleObject>(TSimpleObject objectToSave) where TSimpleObject : SimpleDbObject, new();

        /// <summary>
        /// Сохранить сложный объект - содержащий проперти в виде вложенных типов.
        /// </summary>
        /// <typeparam name="TComplexObject">Тип сложного объекта для сохранения.</typeparam>
        /// <param name="objectToSave">Объект сложного типа.</param>
        /// <returns>Результат сохранения.</returns>
        Result SaveSingleComplex<TComplexObject>(TComplexObject objectToSave) where TComplexObject : ComplexDbObject, new();
    }
}
