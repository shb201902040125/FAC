using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAC
{
    public abstract class Override<T>
    {
        protected T value;
        public Override(T value)
        {
            this.value = value;
        }
        public abstract T Value { get; set; }
        public abstract void Reset();
    }
    public class OverrideClass<T> : Override<T> where T : class
    {
        T @override;
        public OverrideClass(T value) : base(value)
        {
        }
        public override T Value
        {
            get => @override ?? value;
            set => @override = value;
        }
        public override void Reset()
        {
            @override = null;
        }
        public static implicit operator T(OverrideClass<T> @override)
        {
            return @override.Value;
        }
    }
    public class OverrideValue<T> : Override<T> where T : struct
    {
        T? @override;
        public OverrideValue(T value) : base(value)
        {
        }
        public override T Value
        {
            get => @override ?? value;
            set => @override = value;
        }
        public override void Reset()
        {
            @override = null;
        }
        public static implicit operator T(OverrideValue<T> @override)
        {
            return @override.Value;
        }
    }
}
