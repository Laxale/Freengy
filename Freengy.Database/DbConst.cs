// Created by Laxale 17.04.2018
//
//


namespace Freengy.Database 
{
    /// <summary>
    /// Названия таблиц базы данных Freengy.
    /// </summary>
    public class DbConst
    {
        /// <summary>
        /// Название файла базы данных Freengy.
        /// </summary>
        public const string DbFileName = "freengy-database.db";


        /// <summary>
        /// Содержит названия таблиц базы PKI Client.
        /// </summary>
        public static class TableNames
        {
            /// <summary>
            /// Настройки для точек распространения.
            /// </summary>
            public const string CertificateDistributionSettings = "cdp_settings";

            /// <summary>
            /// Список истории файлов.
            /// </summary>
            public const string FileHistory = "file_history";
        }
    }
}