using FAC.Compontents.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAC.Compontents.Finders
{
    public abstract class RangeFinder<T> : Worker<T>
    {
        public OverrideValue<float> MaxRange { get; protected set; }
        public OverrideValue<float> MinRange { get; protected set; }
    }
}
