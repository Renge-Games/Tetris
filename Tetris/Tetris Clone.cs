using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class TetrisClone : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ProgressionHandler p;
        SpriteFont sf;
        Text scoreText;
        Text holdText;
        Text nextText;
        Text levelText;

        public TetrisClone()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Level.Init(Content.Load<Texture2D>("tile"));
            p = new ProgressionHandler(30, 50, 5, 1000);
            graphics.PreferredBackBufferWidth = Level.windowWidth;
            graphics.PreferredBackBufferHeight = Level.windowHeight;
            graphics.ApplyChanges();
            sf = Content.Load<SpriteFont>("font12");
            levelText = new Text(sf, "Level: " + p.Level, Color.Black, new Point(10, Level.windowHeight - 100), 2.5f, Vector2.Zero);
            scoreText = new Text(sf, "Score: " + p.Score.ToString() + "/" + p.MaxScoreForLevel, Color.Black, new Point(10, Level.windowHeight - 50), 1.5f, Vector2.Zero);
            holdText = new Text(sf, "Hold", Color.Black, new Point(Level.hOrigin.X + 100, 25), 3.0f, null);
            nextText = new Text(sf, "Next Tile", Color.Black, new Point(Level.nOriginX, 25), 3.0f, null);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            p.Update();
            Level.Update(p);
            InputHandler.HandleInput();
            levelText.SetText("Level: " + p.Level, Vector2.Zero);
            scoreText.SetText("Score: " + p.Score.ToString() + "/" + p.MaxScoreForLevel, Vector2.Zero);
            p.ResetTimerIfReady();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, null);
            Level.Draw(spriteBatch);
            p.Draw(spriteBatch, Color.White);
            scoreText.Draw(spriteBatch);
            levelText.Draw(spriteBatch);
            holdText.Draw(spriteBatch);
            nextText.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}