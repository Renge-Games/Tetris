using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public static class Util
    {
        public static T[][] GetInitialized2DArray<T>(int xLength, int yLength)
        {
            T[][] array = new T[xLength][];
            for (int i = 0; i < xLength; i++)
            {
                array[i] = new T[yLength];
            }
            return array;
        }

        public static Shape RotateShape(Shape shape)
        {
            Shape rotated = shape;
            rotated = TransposeShape(rotated);
            List<byte[]> tmp = rotated.Data.ToList();
            tmp.Reverse();
            rotated.Data = tmp.ToArray();

            return rotated;
        }

        public static Shape TransposeShape(Shape shape)
        {
            Shape transposed = new Shape(shape.YCount, shape.XCount);
            for (int x = 0; x < shape.YCount; x++)
            {
                for (int y = 0; y < shape.XCount; y++)
                {
                    transposed[x, y] = shape[y, x];
                }
            }

            return transposed;
        }
    }
}
