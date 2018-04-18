// Created by Laxale 18.04.2018
//
//


namespace Freengy.Common.Enums 
{
    /// <summary>
    /// Values of possible user account privileges.
    /// </summary>
    public enum AccountPrivilege 
    {
        /// <summary>
        /// Undefined.
        /// </summary>
        None,

        /// <summary>
        /// Common user account.
        /// </summary>
        Common,

        /// <summary>
        /// Account has some privileges.
        /// </summary>
        Privileged,

        /// <summary>
        /// Account has administrative privileges.
        /// </summary>
        Admin,

        /// <summary>
        /// Account is god-like devastator.
        /// </summary>
        UltramarineImperator
    }
}