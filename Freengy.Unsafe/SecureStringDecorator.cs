// Created by Laxale 13.11.2016
//
//


namespace Freengy.Unsafe 
{
    using System;
    using System.Linq;
    using System.Security;
    using System.ComponentModel;
    using System.Runtime.InteropServices;


    public class SecureStringDecorator 
    {
        private SecureString SecureString { get; set; }

        [DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private unsafe string StringRepresentation 
        {
            [SecurityCritical]
            get
            {
                string result;

                IntPtr intPtr = Marshal.SecureStringToBSTR(this.SecureString);

                try
                {
                    result = new string((char*)((void*)intPtr));
                }
                finally
                {
                    Marshal.ZeroFreeBSTR(intPtr);
                }

                return result;
            }

            [SecurityCritical]
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.SecureString.Clear();

                foreach (char symbol in value)
                {
                    this.SecureString.AppendChar(symbol);
                }
            }
        }


        public string GetSecureString() 
        {
            return this.StringRepresentation;
        }
        public void SetSecureString(char[] argument) 
        {
            if (argument == null) throw new ArgumentNullException(nameof(argument));
            if (!argument.Any()) throw new ArgumentException("Array is empty");

            this.SetSecureStringImpl(argument);
        }
        public void SetSecureString(string argument) 
        {
            if (string.IsNullOrWhiteSpace(argument)) throw new ArgumentNullException(nameof(argument));

            this.SetSecureStringImpl(argument.ToCharArray());
        }


        private unsafe void SetSecureStringImpl(char[] argument) 
        {
            fixed (char* unsafeString = &argument[0])
            {
                this.SecureString = new SecureString(unsafeString, argument.Length);
            }
        }
    }
}