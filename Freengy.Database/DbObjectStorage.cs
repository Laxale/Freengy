// Created by Laxale 17.04.2018
//
//

using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

using Freengy.Common.Database;
using Freengy.Common.Helpers.Result;
using Freengy.Database.Context;
using Freengy.Database.Object;

using NLog;


namespace Freengy.Database 
{
    /// <summary>
    /// Репозиторий настроек приложения - объекты настроек хранятся в единственном экземпляре.
    /// </summary>
    public class DbObjectStorage : IObjectStorage 
    {
        private static readonly object Locker = new object();

        private readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Возвращает объект настроек типа <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Тип объекта для сохранения.</typeparam>
        /// <returns>Результат с объектом настройки.</returns>
        public Result<T> GetSingleSimple<T>() where T : SimpleDbObject, new()
        {
            return GetDbObjectSynchronized<T>();
        }

        /// <summary>
        /// Вернуть объект сложных настроек типа <see cref="TComplexObject"/>.
        /// </summary>
        /// <typeparam name="TComplexObject">Тип сложных настроек.</typeparam>
        /// <returns>Результат получения объекта сложных настроек.</returns>
        public Result<TComplexObject> GetSingleComplex<TComplexObject>()
            where TComplexObject : ComplexDbObject, new()
        {
            return GetDbObjectSynchronized<TComplexObject>();
        }

        /// <summary>
        /// Сохранить простые настройки - не содержащие пропертей в виде вложенных типов.
        /// </summary>
        /// <typeparam name="TSimpleObject">Тип простого объекта для сохранения.</typeparam>
        /// <param name="objectToSave">Объект простых настроек.</param>
        /// <returns>Результат сохранения.</returns>
        public Result SaveSingleSimple<TSimpleObject>(TSimpleObject objectToSave) where TSimpleObject : SimpleDbObject, new() 
        {
            try
            {
                return SaveSimpleImpl(objectToSave);
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233079)
                {
                    // сообщение о последствиях тупого двойного инсерта, который EF пытался задолбить в базу.
                    // второй инсерт мы игнорили, так что в базе одна запись - всё хорошо
                    return Result.Ok();
                }

                logger.Error(ex, $"Не удалось сохранить объект типа '{ typeof(TSimpleObject).Name }'");

                return Result.Fail(new DBStorageErrorReason(ex.Message));
            }
        }

        /// <summary>
        /// Сохранить настройки - не содержащие пропертей в виде вложенных типов.
        /// </summary>
        /// <typeparam name="TComplexObject">Тип сложного объекта для сохранения.</typeparam>
        /// <param name="objectToSave">Объект сложных настроек.</param>
        /// <returns>Результат сохранения.</returns>
        public Result SaveSingleComplex<TComplexObject>(TComplexObject objectToSave) where TComplexObject : ComplexDbObject, new()
        {
            try
            {
                //var efLogs = new List<string>();
                //RemoveCascade<TComplexObject>();

                var contextType = GetContextType<TComplexObject>();
                var complexContext = CreateContext<TComplexObject>(contextType);

                using (complexContext)
                {
                    //complexContext.Database.Log = efLogs.Add;
                    var allObjects = complexContext.Objects.ToList();
                    complexContext.Objects.RemoveRange(allObjects);

                    var entry = complexContext.Entry(objectToSave.PrepareMappedProps());
                    entry.State = EntityState.Added;

                    complexContext.SaveChanges();

                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233079)
                {
                    // последствие тупого двойного инсерта, который EF пытался задолбить в базу.
                    // второй инсерт мы игнорили, так что в базе одна запись - всё хорошо
                    return Result.Ok();
                }

                return Result.Fail(new DBStorageErrorReason(ex.Message));
            }
        }


        private Result SaveSimpleImpl<TSimpleObject>(TSimpleObject objectToSave)
            where TSimpleObject : SimpleDbObject, new()
        {
            //var efLogs = new List<string>();
            using (var dbContext = new SimpleDbContext<TSimpleObject>())
            {
                //dbContext.Database.Log = efLogs.Add;

                var allObjects = dbContext.Objects.Cast<TSimpleObject>().ToArray();

                dbContext.Objects.RemoveRange(allObjects);

                var entry = dbContext.Entry(objectToSave);
                entry.State = EntityState.Added;

                dbContext.SaveChanges();

                return Result.Ok();
            }
        }

        private DbContextBase<TObject> CreateContext<TObject>(Type contextType) where TObject : DbObject, new()
        {
            var ctors = contextType.GetConstructors();
            var defaultCtor = ctors.FirstOrDefault(ctor => ctor.IsPublic && !ctor.GetParameters().Any());

            var context = defaultCtor.Invoke(null);

            return (DbContextBase<TObject>)context;
        }

        private Result<TObject> GetDbObjectSynchronized<TObject>() where TObject : DbObject, new()
        {
            lock (Locker)
            {
                return TryGetDbObject<TObject>();
            }
        }

        private Result<TObject> TryGetDbObject<TObject>() where TObject : DbObject, new() 
        {
            try
            {
                var contextType = GetContextType<TObject>();

                using (var dbContext = CreateContext<TObject>(contextType))
                {
                    //var logs = new List<string>();
                    //dbContext.Database.Log = logs.Add;

                    var foundProxy =
                        dbContext.Objects.Local.SingleOrDefault() ??
                        dbContext.Objects.SingleOrDefault();

                    TObject foundObject =
                        (TObject)((foundProxy as ComplexDbObject)?.CreateFromProxy(foundProxy) ?? foundProxy);

                    return
                        foundObject == null ?
                            Result<TObject>.Fail(new DBStorageErrorReason($"В базе нет объекта типа '{ typeof(TObject).Name }'")) :
                            Result<TObject>.Ok(foundObject);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Не удалось получить объект типа '{ typeof(TObject).Name }'");
                return Result<TObject>.Fail(new DBStorageErrorReason(ex.Message));
            }
        }

        private Type GetContextType<TObject>() where TObject : DbObject, new()
        {
            var objectType = typeof(TObject);
            string objectTypeName = objectType.Name;

            var contextTypeAttribute = objectType.GetCustomAttributes<RelationalContextAttribute>().FirstOrDefault();

            var basicType = typeof(object);
            var simpleType = typeof(SimpleDbObject);

            if (contextTypeAttribute == null)
            {
                while (objectType.BaseType != basicType)
                {
                    if (objectType.BaseType == simpleType)
                    {
                        return typeof(SimpleDbContext<TObject>);
                    }

                    objectType = objectType.BaseType;
                }

                ThrowInvalidObjectError(objectTypeName);
            }

            return contextTypeAttribute.ContextType;
        }

        private void ThrowInvalidObjectError(string objectTypeName)
        {
            string error =
                $"Для определения контекста тип '{ objectTypeName }' должен наследовать { nameof(SimpleDbObject) } " +
                $"или быть помеченным атрибутом { nameof(RelationalContextAttribute) }";

            throw new InvalidOperationException(error);
        }

        private bool IsEnc<TObject>()
        {
            return typeof(TObject).Name.ToLower().Contains("encryption");
        }
    }
}
