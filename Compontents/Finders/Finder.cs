namespace FAC.Compontents.Finders
{
    public abstract class Finder<T> : Compontent
    {
        public abstract bool TryFindTarget(out T target);
    }
}
