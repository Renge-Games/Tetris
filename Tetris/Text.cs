using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public class Text
    {
        SpriteFont sf;
        string text;
        Color color;
        Point position;
        float scale;
        Vector2 origin;

        public Text(SpriteFont spriteFont, string t, Color c, Point pos, float zoom, Vector2? origin)
        {
            sf = spriteFont;
            text = t;
            color = c;
            position = pos;
            scale = zoom;
            if (origin.HasValue)
                this.origin = origin.Value;
            else
                this.origin = sf.MeasureString(text) / 2.0f;
        }

        public void SetText(string t, Vector2? origin)
        {
            text = t;
            if (origin.HasValue)
                this.origin = origin.Value;
            else
                this.origin = sf.MeasureString(text) / 2.0f;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(sf, text, position.ToVector2(), color, 0, origin, scale, SpriteEffects.None, 0);
        }
    }
}
