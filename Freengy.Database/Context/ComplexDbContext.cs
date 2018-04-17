// Created by Laxale 17.04.2018
//
//

using Freengy.Database.Object;


namespace Freengy.Database.Context 
{
    /// <summary>
    /// Базовый класс EF-контекст базы данных PKI Client для работы со сложными настройками - требующими маппинга и содержащими вложенные типы.
    /// <para>Для работы с конкретным сложным типом нужно мапить его в оверрайде метода OnModelCreating().</para>
    /// </summary>
    /// <typeparam name="T">Тип объекта сложных настроек - наследник <see cref="DbObject"/>.</typeparam>
    public abstract class ComplexDbContext<T> : DbContextBase<T> where T : DbObject, new() 
    {

    }
}