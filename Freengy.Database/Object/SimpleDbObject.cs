// Created by Laxale 17.04.2018
//
//


using Freengy.Common.Database;

namespace Freengy.Database.Object 
{
    /// <summary>
    /// Базовый класс для наследования объектов простых настроек. 
    /// Простые настройки содержат только простые базовые типы данных и не содержат коллекций.
    /// Простые настройки не нуждаются в маппинге.
    /// </summary>
    public abstract class SimpleDbObject : DbObject 
    {

    }
}