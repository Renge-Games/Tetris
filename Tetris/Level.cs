using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public static class Level
    {
        public static int tileSize;
        public static int gridWidth;
        public static int gridHeight;
        public static int windowWidth;
        public static int windowHeight;
        public static int originX;
        public static int originY;
        public static int nOriginY1;
        public static int nOriginY2;
        public static int nOriginY3;
        public static int nOriginX;
        public static Point hOrigin;
        public static Point cOrigin;
        public static Texture2D tt;
        static TileMap map;
        static Color[] colors;
        static Shape[] shapes;
        public static Color voidColor;
        public static Entity heldEntity;
        public static Entity currentEntity;
        public static Entity[] nextEntities;
        public static Random r = new Random();
        public static bool hToggled = false;

        public static void Init(Texture2D tex)
        {
            InitColors();
            InitBaseValues(tex);
            InitShapes();
            InitMap();
            InitEntities();
        }

        public static void InitColors()
        {
            voidColor = new Color(255, 255, 255, 15);
            colors = new Color[10];
            colors[0] = Color.Red;
            colors[1] = Color.Green;
            colors[2] = Color.Blue;
            colors[3] = Color.Yellow;
            colors[4] = Color.Orange;
            colors[5] = Color.Purple;
            colors[6] = Color.DarkRed;
            colors[7] = Color.CornflowerBlue;
            colors[8] = Color.DarkGreen;
            colors[9] = Color.Gold;
        }

        public static void InitBaseValues(Texture2D tex)
        {
            tileSize = 50;
            gridWidth = 10;
            gridHeight = 16;
            windowWidth = tileSize * gridWidth * 2;
            windowHeight = tileSize * gridHeight + tileSize;
            originX = tileSize * 5;
            originY = tileSize / 2;
            nOriginX = (int)(tileSize * 17.5f);
            nOriginY1 = originY + 50;
            nOriginY2 = nOriginY1 + (int)(tileSize * 0.8) * 4 + 10;
            nOriginY3 = nOriginY2 + (int)(tileSize * 0.5) * 4 + 10;
            hOrigin = new Point(originY, originY + 50);
            cOrigin = new Point(originX + tileSize * 3, originY - tileSize * 3);
            tt = tex;
        }

        public static void InitShapes()
        {
            shapes = new Shape[7];
            shapes[0] = new Shape(2, 2,
                1, 1,
                1, 1);
            shapes[1] = new Shape(3, 2,
                1, 1, 0,
                0, 1, 1);
            shapes[2] = new Shape(3, 2,
                0, 1, 1,
                1, 1, 0);
            shapes[3] = new Shape(2, 3,
                1, 0,
                1, 0,
                1, 1);
            shapes[4] = new Shape(2, 3,
                0, 1,
                0, 1,
                1, 1);
            shapes[5] = new Shape(3, 2,
                0, 1, 0,
                1, 1, 1);
            shapes[6] = new Shape(1, 4,
                1,
                1,
                1,
                1);
        }

        public static void InitMap()
        {
            Point origin = new Point(originX, originY);
            Color color = new Color(150, 150, 150, 150);
            Point bgOffset = new Point(10, 10);
            int width = gridWidth;
            int height = gridHeight;
            map = new TileMap(width, height, origin, color, bgOffset);
        }

        public static void InitEntities()
        {
            heldEntity = null;
            currentEntity = new Entity(shapes[r.Next(0, shapes.Length)], cOrigin, colors[r.Next(0, colors.Length)]);
            nextEntities = new Entity[3];

            Point size = new Point((int)(tileSize * 0.8));
            Shape s = shapes[r.Next(0, shapes.Length)];
            Point p = new Point(nOriginX - (int)((s.XCount / 2.0f) * size.X), nOriginY1);

            nextEntities[0] = new Entity(s, p, colors[r.Next(0, colors.Length)]);
            nextEntities[0].ReSizeTiles(size);

            size = new Point((int)(tileSize * 0.5));
            s = shapes[r.Next(0, shapes.Length)];
            p = new Point(nOriginX - (int)((s.XCount / 2.0f) * size.X), nOriginY2);

            nextEntities[1] = new Entity(s, p, colors[r.Next(0, colors.Length)]);
            nextEntities[1].ReSizeTiles(size);

            s = shapes[r.Next(0, shapes.Length)];
            p = new Point(nOriginX - (int)((s.XCount / 2.0f) * size.X), nOriginY3);

            nextEntities[2] = new Entity(s, p, colors[r.Next(0, colors.Length)]);
            nextEntities[2].ReSizeTiles(size);
        }

        public static void PushEntities()
        {
            currentEntity = nextEntities[0];
            currentEntity.ResetTileSize();
            currentEntity.SetPosition(cOrigin);

            Point size = new Point((int)(tileSize * 0.8));
            Point p = new Point(nOriginX - (int)((nextEntities[1].Size.X / 2.0f) * size.X), nOriginY1);

            nextEntities[0] = nextEntities[1];
            nextEntities[0].ReSizeTiles(size);
            nextEntities[0].SetPosition(p);

            size = new Point((int)(tileSize * 0.5));
            p = new Point(nOriginX - (int)((nextEntities[2].Size.X / 2.0f) * size.X), nOriginY2);

            nextEntities[1] = nextEntities[2];
            nextEntities[1].ReSizeTiles(size);
            nextEntities[1].SetPosition(p);

            Shape s = shapes[r.Next(0, shapes.Length)];
            p = new Point(nOriginX - (int)((s.XCount / 2.0f) * size.X), nOriginY3);

            nextEntities[2] = new Entity(s, p, colors[r.Next(0, colors.Length)]);
            nextEntities[2].ReSizeTiles(size);
        }

        public static bool IsWithinLevelBounds(int xG, int yG)
        {
            return !map.BoundsCollision(xG, yG);
        }

        public static bool IsWithinAllLevelBounds(int xG, int yG)
        {
            return !map.AllBoundsCollision(xG, yG);
        }

        public static bool BottomCollision(int xG, int yG)
        {
            if (yG >= gridHeight) return true;
            return false;
        }

        public static bool Collision(int xG, int yG)
        {
            return map.CheckCollisionOnGP(xG, yG);
        }

        public static Point GetGridPosition(int x, int y)
        {
            return map.GetGridPosition(x, y);
        }

        public static void AddToMap(Entity entity)
        {
            map.AddEntity(entity);
        }

        public static void RotateCurrentEntity()
        {
            currentEntity.Rotate();
        }

        public static void ToggleHold()
        {
            if (!hToggled)
            {
                if (heldEntity == null)
                {
                    heldEntity = currentEntity;
                    heldEntity.SetPosition(hOrigin);
                    PushEntities();
                }
                else
                {
                    Entity tmp = currentEntity;
                    currentEntity = heldEntity;
                    currentEntity.SetPosition(tmp.Position);
                    heldEntity = tmp;
                    heldEntity.SetPosition(hOrigin);
                }
                currentEntity.GetOutOfCollision();
                hToggled = true;
            }
        }

        public static void MoveCurrentEntityX(int dir)
        {
            currentEntity.MoveX(dir);
        }

        public static void MoveCurrentEntityDown()
        {
            currentEntity.MoveY(1);
        }

        public static void Update(ProgressionHandler p)
        {
            if (!map.LinesAreBeingCleared)
            {
                if (p.UpdateReady)
                {
                    currentEntity.MoveY(1);
                }

                if (currentEntity.DeadOnArrival)
                    Environment.Exit(1337);
                if (currentEntity.Dead)
                {
                    PushEntities();
                    hToggled = false;
                }
            }

            map.Update(p, 5);
        }

        public static void Draw(SpriteBatch sb)
        {
            map.Draw(sb);
            if (currentEntity != null)
                currentEntity.Draw(sb);
            if (heldEntity != null)
                heldEntity.Draw(sb, true);
            for (int i = 0; i < nextEntities.Length; i++)
            {
                if (nextEntities[i] != null)
                    nextEntities[i].Draw(sb, true);
            }
        }
    }
}