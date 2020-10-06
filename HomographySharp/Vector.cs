namespace HomographySharp
{
    public struct Vector2<T> where T : struct
    {
        public T X;
        public T Y;

        public Vector2(T x, T y)
        {
            X = x;
            Y = y;
        }
    }
}