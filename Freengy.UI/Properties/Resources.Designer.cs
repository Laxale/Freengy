﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Freengy.UI.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Freengy.UI.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create account.
        /// </summary>
        public static string CreateAccountText {
            get {
                return ResourceManager.GetString("CreateAccountText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create new account.
        /// </summary>
        public static string CreateNewAccountText {
            get {
                return ResourceManager.GetString("CreateNewAccountText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No spaces, template: good@email.
        /// </summary>
        public static string EmailRequirementsText {
            get {
                return ResourceManager.GetString("EmailRequirementsText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Log in.
        /// </summary>
        public static string LogInText {
            get {
                return ResourceManager.GetString("LogInText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Log in and kawabanga!.
        /// </summary>
        public static string LogInWelcomeText {
            get {
                return ResourceManager.GetString("LogInWelcomeText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email is not not required, but you will be unable to recover forgotten password without it.
        /// </summary>
        public static string NewAccountEmailHintText {
            get {
                return ResourceManager.GetString("NewAccountEmailHintText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minimum length of 10 symbols and no spaces.
        /// </summary>
        public static string PasswordRequirementsText {
            get {
                return ResourceManager.GetString("PasswordRequirementsText", resourceCulture);
            }
        }
    }
}
