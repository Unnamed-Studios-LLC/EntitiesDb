namespace EntitiesDb.Data
{
    internal unsafe ref struct DataList<T> where T : unmanaged
    {
        private readonly T* _ptr;

        public DataList(void* ptr)
        {
            _ptr = (T*)ptr;
        }

        public DataList(T* ptr)
        {
            _ptr = ptr;
        }

        public T* this[int index] => _ptr + index;
    }
}
