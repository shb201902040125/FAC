namespace FAC
{
    public abstract class Override<T>
    {
        public T OrigValue { get; }
        public Override(T value)
        {
            OrigValue = value;
        }
        public abstract T Value { get; set; }
        public abstract void Reset();
    }
    public class OverrideClass<T> : Override<T> where T : class
    {
        private T @override;
        public OverrideClass(T value) : base(value)
        {
        }
        public override T Value
        {
            get => @override ?? OrigValue;
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
        public static explicit operator OverrideClass<T>(T value)
        {
            return new OverrideClass<T>(value);
        }
    }
    public class OverrideValue<T> : Override<T> where T : struct
    {
        private T? @override;
        public OverrideValue(T value) : base(value)
        {
        }
        public override T Value
        {
            get => @override ?? OrigValue;
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
        public static explicit operator OverrideValue<T>(T value)
        {
            return new OverrideValue<T>(value);
        }
    }
}
