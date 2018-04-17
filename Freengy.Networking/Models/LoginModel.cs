using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freengy.Networking.Models
{
    /// <summary>
    /// Log-in process model.
    /// </summary>
    public class LoginModel 
    {
        public string UserName { get; set; }

        public string PasswordHash { get; set; }
    }
}