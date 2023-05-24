namespace FAC.Compontents.Auxiliaries
{
    public abstract class Auxiliary : Compontent
    {
        public virtual bool CanApplyTo(Compontent compontent) => compontent.Foundation == Foundation;
        public virtual void Apply(Compontent compontent) { }
    }
    public abstract class Auxiliary<T> : Auxiliary where T : Compontent
    {
        public sealed override bool CanApplyTo(Compontent compontent)
        {
            return base.CanApplyTo(compontent) && compontent is T;
        }
    }
}
