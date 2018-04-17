using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freengy.Base.Interfaces;

namespace Freengy.Base.DefaultImpl
{
    public class UserAccount : IUserAccount 
    {
        public string Name { get; }
    }
}
