// Created by Laxale 13.11.2016
//
//

using System;
using System.Security;


namespace Freengy.Base.Helpers 
{
    public static class Account 
    {
        private const string ForbiddenSymbol = " ";

        public const int MinimumPasswordLength = 10;


        public static bool IsGoodPassword(string password) 
        {
            if(password == null) throw new ArgumentNullException(nameof(password));

            return password.Length >= MinimumPasswordLength && !password.Contains(ForbiddenSymbol);
        }

        public static bool IsValidEmail(string email) 
        {
            if (string.IsNullOrWhiteSpace(email) ||
                email.Contains(ForbiddenSymbol) ||
                email.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries).Length != 2) return false;

            return true;
        }
    }
}