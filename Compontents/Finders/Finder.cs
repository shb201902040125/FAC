using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAC.Compontents.Finders
{
    public abstract class Finder<T> : Compontent
    {
        public abstract bool TryFindTarget(out T target);
    }
}
