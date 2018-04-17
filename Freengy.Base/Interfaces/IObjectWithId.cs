using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freengy.Base.Interfaces
{
    public interface IObjectWithId 
    {
        Guid Id { get; }
    }
}
