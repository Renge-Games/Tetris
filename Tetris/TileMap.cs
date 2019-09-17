using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public class TileMap
    {
        Tile[][] tiles;
        Point origin;
        Color[] bg;
        Texture2D bgT;
        Point bgOffset;
        int xClearIndex;
        List<int> yClearIndices;

        public TileMap(int xCount, int yCount, Point origin, Color bgColor, Point bgOffset)
        {
            tiles = Util.GetInitialized2DArray<Tile>(xCount, yCount);
            Clear();
            this.origin = origin;
            bg = new Color[] { bgColor };
            this.bgOffset = bgOffset;
            xClearIndex = 0;
            yClearIndices = new List<int>();
        }

        public bool LinesAreBeingCleared { get; protected set; }

        public void Clear()
        {
            for (int x = 0; x < tiles.Length; x++)
                for (int y = 0; y < tiles[x].Length; y++)
                {
                    tiles[x][y] = new Tile(new Point(origin.X + x * Level.tileSize, origin.Y + y * Level.tileSize), new Point(Level.tileSize), Level.voidColor);
                    tiles[x][y].SetGridPosition(x, y);
                }
        }

        public void ClearLine(int yIndex)
        {
            for (int x = 0; x < tiles.Length; x++)
                tiles[x][yIndex].Color = Level.voidColor;
            if (yIndex - 1 < 0)
                return;
            for (int y = yIndex - 1; y >= 0; y--)
            {
                for (int x = 0; x < tiles.Length; x++)
                    tiles[x][y + 1].Color = tiles[x][y].Color;
            }
        }

        public void SetTile(int xIndex, int yIndex, Color color)
        {
            tiles[xClearIndex][yIndex].Color = color;
        }

        public Point GetGridPosition(int x, int y)
        {
            Point p = new Point(x, y);
            return new Point((p.X - origin.X) / Level.tileSize, (p.Y - origin.Y) / Level.tileSize);
        }

        public Point GetGridPosition(Point p)
        {
            return new Point((p.X - origin.X) / Level.tileSize, (p.Y - origin.Y) / Level.tileSize);
        }

        public bool CheckCollisionOnGP(int x, int y)
        {
            if (AllBoundsCollision(x, y))
                return false;
            if (tiles[x][y].Color != Level.voidColor)
                return true;
            return false;
        }

        public bool AllBoundsCollision(int x, int y)
        {
            if (x < 0 || y < 0 || x > tiles.Length - 1 || y > tiles[0].Length - 1)
                return true;
            return false;
        }

        public bool BoundsCollision(int x, int y)
        {
            if (x < 0 || x > tiles.Length - 1 || y > tiles[0].Length - 1)
                return true;
            return false;
        }

        public void AddEntity(Entity entity)
        {
            Point gp = entity.GetGridPosition();
            for (int x = 0; x < entity.Size.X; x++)
            {
                for (int y = 0; y < entity.Size.Y; y++)
                {
                    if (entity[x, y] != null && !AllBoundsCollision(x + gp.X, y + gp.Y))
                        tiles[x + gp.X][y + gp.Y] = entity[x, y];
                }
            }
        }

        int clearCounter = 0;
        public void Update(ProgressionHandler p, int clearDelay)
        {
            if (!LinesAreBeingCleared)
            {
                for (int y = 0; y < tiles[0].Length; y++)
                {
                    bool lineFull = true;
                    for (int x = 0; x < tiles.Length; x++)
                    {
                        if (tiles[x][y].Color == Level.voidColor)
                        {
                            lineFull = false;
                            break;
                        }
                    }
                    if (lineFull)
                    {
                        LinesAreBeingCleared = true;
                        yClearIndices.Add(y);
                    }
                }
            }

            if (clearCounter >= clearDelay && LinesAreBeingCleared)
            {
                clearCounter = 0;
                for (int i = 0; i < yClearIndices.Count; i++)
                {
                    SetTile(xClearIndex, yClearIndices[i], new Color(100, 100, 100, 255));
                }
                xClearIndex++;
                if (xClearIndex >= tiles.Length)
                {
                    xClearIndex = 0;
                    for (int i = 0; i < yClearIndices.Count; i++)
                        ClearLine(yClearIndices[i]);
                    LinesAreBeingCleared = false;
                    yClearIndices.Clear();
                }
            }
            clearCounter++;
        }

        public void Draw(SpriteBatch sb)
        {
            if (bgT == null)
            {
                bgT = new Texture2D(sb.GraphicsDevice, 1, 1);
                bgT.SetData(bg);
            }

            sb.Draw(bgT, new Rectangle(origin - bgOffset, new Point(tiles.Length * Level.tileSize + bgOffset.X * 2, tiles[0].Length * Level.tileSize + bgOffset.Y * 2)), Color.White);
            for (int x = 0; x < tiles.Length; x++)
                for (int y = 0; y < tiles[x].Length; y++)
                    if (tiles[x][y] != null)
                        tiles[x][y].Draw(sb);
        }
    }
}