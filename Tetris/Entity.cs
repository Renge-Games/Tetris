using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public class Entity
    {
        Shape shape;
        Tile[][] tiles;
        Color tc;
        Point ts;
        Point position;
        bool xMoving;

        public Tile this[int x, int y]
        {
            get { return tiles[x][y]; }
            set { tiles[x][y] = value; }
        }

        public Entity(Shape shape, Point position, Color color)
        {
            this.shape = shape;
            tiles = Util.GetInitialized2DArray<Tile>(shape.XCount, shape.YCount);
            this.position = position;
            ts = new Point(Level.tileSize);
            tc = color;
            xMoving = false;

            tiles = shape.ToTileShape(position, new Point(Level.tileSize), color);
        }

        public Point Position
        {
            get { return position; }
        }
        public Point Size
        {
            get { return new Point(tiles.Length, tiles[0].Length); }
        }
        public Point TileSize
        {
            get { return ts; }
        }
        public Color TileColor
        {
            get { return tc; }
        }
        public bool DeadOnArrival { get; private set; }
        public bool Dead { get; private set; }

        public void ReSizeTiles(Point size)
        {
            ts = size;
            for (int x = 0; x < tiles.Length; x++)
                for (int y = 0; y < tiles[x].Length; y++)
                    if (tiles[x][y] != null)
                        tiles[x][y].Size = size;
            RearangeTiles();
        }

        public void ResetTileSize()
        {
            ts = new Point(Level.tileSize);
            for (int x = 0; x < tiles.Length; x++)
                for (int y = 0; y < tiles[x].Length; y++)
                    if (tiles[x][y] != null)
                        tiles[x][y].Size = new Point(Level.tileSize);
        }

        public void SetPosition(Point pos)
        {
            position = pos;
            RearangeTiles();
        }

        public void SetPosition(int x, int y)
        {
            position = new Point(x, y);
            RearangeTiles();
        }

        public void SetGridPosition(int x, int y)
        {
            position = new Point(Level.originX + Level.tileSize * x, Level.originY + Level.tileSize * y);
            RearangeTiles();
        }

        public Point GetGridPosition()
        {
            return Level.GetGridPosition(position.X, position.Y);
        }

        private void RearangeTiles()
        {
            for (int x = 0; x < tiles.Length; x++)
                for (int y = 0; y < tiles[x].Length; y++)
                    if (tiles[x][y] != null)
                        tiles[x][y].Position = new Point(position.X + x * tiles[x][y].Size.X, position.Y + y * tiles[x][y].Size.Y);
        }

        public bool CheckLevelForCollisions(int xDir, int yDir)
        {
            for (int x = 0; x < tiles.Length; x++)
                for (int y = 0; y < tiles[x].Length; y++)
                    if (tiles[x][y] != null)
                        if (Level.Collision(tiles[x][y].GetGridPosition().X + xDir, tiles[x][y].GetGridPosition().Y + yDir))
                            return true;
                        else if (Level.BottomCollision(tiles[x][y].GetGridPosition().X + xDir, tiles[x][y].GetGridPosition().Y + yDir))
                            return true;
                        else if (!Level.IsWithinLevelBounds(tiles[x][y].GetGridPosition().X + xDir, tiles[x][y].GetGridPosition().Y + yDir))
                            return true;
            return false;
        }

        public void Rotate()
        {
            int d = shape.YCount - shape.XCount;
            d = Math.Min(d, GetGridPosition().X);
            shape.Rotate();
            tiles = shape.ToTileShape(Position, TileSize, TileColor);
            HardMove(-d, 0);
            GetOutOfCollision();
            xMoving = true;
        }

        public void MoveX(int dir)
        {
            if (!CheckLevelForCollisions(dir, 0))
            {
                SetGridPosition(GetGridPosition().X + dir, GetGridPosition().Y);
                xMoving = true;
            }
        }

        public void MoveY(int dir)
        {
            if (!CheckLevelForCollisions(0, dir))
                SetGridPosition(GetGridPosition().X, GetGridPosition().Y + dir);
            else if (xMoving)
            {
                xMoving = false;
                return;
            }
            else
            {
                if (!Level.IsWithinAllLevelBounds(GetGridPosition().X, GetGridPosition().Y))
                    DeadOnArrival = true;

                Dead = true;
                Level.AddToMap(this);
            }
        }

        public void HardMove(int xDir, int yDir)
        {
            SetGridPosition(GetGridPosition().X + xDir, GetGridPosition().Y + yDir);
        }

        public void GetOutOfCollision()
        {
            Point gp = GetGridPosition();
            for (int x = 0; x < tiles.Length; x++)
                for (int y = 0; y < tiles[x].Length; y++)
                    if (!Level.IsWithinLevelBounds(gp.X + x, gp.Y + y))
                    {
                        while (GetGridPosition().X + x >= Level.gridWidth)
                            HardMove(-1, 0);
                        while (GetGridPosition().X + x < 0)
                            HardMove(1, 0);
                    }
                        
            while (CheckLevelForCollisions(0, 0))
            {
                HardMove(0, -1);
            }
        }


        /// <summary>
        /// Draw the entity
        /// </summary>
        /// <param name="sb">SpriteBatch for drawing</param>
        /// <param name="hardDraw">Draw no matter what</param>
        public void Draw(SpriteBatch sb, bool hardDraw = false)
        {
            for (int x = 0; x < tiles.Length; x++)
                for (int y = 0; y < tiles[x].Length; y++)
                    if ((Level.IsWithinAllLevelBounds(GetGridPosition().X + x, GetGridPosition().Y + y) || hardDraw) && tiles[x][y] != null)
                        tiles[x][y].Draw(sb);
        }

    }
}