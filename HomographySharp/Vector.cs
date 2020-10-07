namespace HomographySharp
{
    public struct Point2<T> where T : struct
    {
        public T X;
        public T Y;

        public Point2(T x, T y)
        {
            X = x;
            Y = y;
        }
    }
}