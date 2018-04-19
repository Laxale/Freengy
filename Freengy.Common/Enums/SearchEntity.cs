// Created by Laxale 18.04.2018
//
//


namespace Freengy.Common.Enums 
{
    /// <summary>
    /// Contains searchable entity types.
    /// </summary>
    public enum SearchEntity 
    {
        /// <summary>
        /// Undefined.
        /// </summary>
        None,

        /// <summary>
        /// Entity to search is user(s).
        /// </summary>
        Users,

        /// <summary>
        /// Entity to search is friend(s).
        /// </summary>
        Friends,

        /// <summary>
        /// Entity to search is game session(s).
        /// </summary>
        GameSessions
    }
}