// Created by Laxale 08.05.2018
//
//

using Microsoft.Practices.Unity;


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Интерфейс для сокрытия излишней логики DI контейнеров, не исопльзуемой в приложении.
    /// </summary>
    public interface IMyServiceLocator 
    {
        /// <summary>
        /// Получить инстанс объекта типа <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Тип объекта для получения.</typeparam>
        /// <returns>Инстанс типа <see cref="T"/>.</returns>
        T Resolve<T>() where T : class;

        T ResolveWithParameters<T>(params object[] parameters) where T : class;

        /// <summary>
        /// Зарегистрировать единственный инстанс объекта типа <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Тип объекта для единственной регистрации.</typeparam>
        /// <param name="instance">Объект типа <see cref="T"/> для единственной регистрации.</param>
        void RegisterInstance<T>(T instance) where T : class;
    }
}