using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public struct Shape
    {
        byte[][] data;

        public byte this[int x, int y]
        {
            get { return data[x][y]; }
            set { data[x][y] = value; }
        }

        public byte[] this[int x]
        {
            get { return data[x]; }
            set { data[x] = value; }
        }

        /// <summary>
        /// Initialize shape
        /// </summary>
        /// <param name="tiles">add tile info 1 is a tile 0 is null</param>
        public Shape(int xCount, int yCount, params byte[] tiles)
        {
            data = Util.GetInitialized2DArray<byte>(xCount, yCount);

            if (tiles != null)
                for (int x = 0; x < data.Length; x++)
                {
                    for (int y = 0; y < data[x].Length; y++)
                    {
                        if (tiles.Length > y * xCount + x)
                            data[x][y] = tiles[y * xCount + x];
                        else
                            data[x][y] = 0;
                    }
                }
        }

        public byte[][] Data
        {
            get { return data; }
            set { data = value; }
        }

        public void Rotate()
        {
            this = Util.RotateShape(this);
        }

        public Tile[][] ToTileShape(Point origin, Point tileSize, Color color)
        {
            Tile[][] t = Util.GetInitialized2DArray<Tile>(XCount, YCount);
            for (int x = 0; x < data.Length; x++)
            {
                for (int y = 0; y < data[x].Length; y++)
                {
                    if (data[x][y] != 0)
                        t[x][y] = new Tile(origin + new Point(x * tileSize.X, y * tileSize.Y), tileSize, color);
                    else
                        t[x][y] = null;
                }
            }
            return t;
        }

        public int XCount
        {
            get { return data.Length; }
        }

        public int YCount
        {
            get { return data[0].Length; }
        }
    }
}
