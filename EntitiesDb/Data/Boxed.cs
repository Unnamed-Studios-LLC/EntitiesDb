namespace EntitiesDb;

internal sealed class Boxed<T> where T : unmanaged
{
    public T Value;
}
