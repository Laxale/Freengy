// Created by Laxale 13.11.2016
//
//


namespace Freengy.Base.Helpers 
{
    using System;


    public static class Account
    {
        private const string ForbiddenSymbol = " ";
        private const int MinimumPasswordLength = 10;


        public static bool IsGoodPassword(string password) 
        {
            if (string.IsNullOrWhiteSpace(password) ||
                password.Length < MinimumPasswordLength ||
                password.Contains(ForbiddenSymbol)) return false;

            return true;
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