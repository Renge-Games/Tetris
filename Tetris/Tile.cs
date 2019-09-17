using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public class Tile
    {
        Point position;
        Point size;
        Color color;

        public Tile(Point pos, Point size, Color color)
        {
            this.position = pos;
            this.size = size;
            this.color = color;
        }

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }
        public Point Size
        {
            get { return size; }
            set { size = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public void SetGridPosition(int x, int y)
        {
            position = new Point(Level.originX + Level.tileSize * x, Level.originY + Level.tileSize * y);
        }

        public Point GetGridPosition()
        {
            return Level.GetGridPosition(position.X, position.Y);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Level.tt, new Rectangle(position, size), color);
        }
    }
}