using System;
using MathNet.Numerics.LinearAlgebra.Single;

namespace HomographySharp.Single
{
    /// <summary>
    /// 
    /// </summary>
    public class HomographyMatrix
    {
        /// <summary>
        /// 
        /// </summary>
        public DenseMatrix Source { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="homographyMatrix"></param>
        internal HomographyMatrix(DenseMatrix homographyMatrix)
        {
            Source = homographyMatrix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcX"></param>
        /// <param name="srcY"></param>
        /// <returns></returns>
        public (float dstX, float dstY) Translate(float srcX, float srcY)
        {
            var rawArray = Source.Values;

            var dst1 = rawArray[0] * srcX + rawArray[3] * srcY + rawArray[6];
            var dst2 = rawArray[1] * srcX + rawArray[4] * srcY + rawArray[7];
            var dst3 = rawArray[2] * srcX + rawArray[5] * srcY + rawArray[8];

            return (dst1 / dst3, dst2 / dst3);
        }
    }
}