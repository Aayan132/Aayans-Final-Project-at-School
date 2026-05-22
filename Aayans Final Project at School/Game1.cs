using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Aayans_Final_Project_at_School
{
    public class Game1 : Game
    {
        enum Screen
        {
            Intro,
            Single,
            Duo,
            End
        }


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Screen currentScreen;
        Rectangle window;
        MouseState mouseState;
        KeyboardState previousKeyboard;
        Texture2D introScreen;
        Texture2D backgroundTexture;
        Texture2D shipTexture;
        Texture2D laserTexture;
        Texture2D barrierTexture;

        Rectangle barrierrect;
        Rectangle ship;
        int shipSpeed = 5;
        List<Rectangle> lasers = new List<Rectangle>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            window = new Rectangle(0, 0, 800, 600);

            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            currentScreen = Screen.Intro;
            ship = new Rectangle(window.Width / 2 - 40, 500, 80, 80);
            barrierrect = new Rectangle(80, 400, 180, 60);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            introScreen = Content.Load<Texture2D>("SpaceInvadersIntro.");
            backgroundTexture = Content.Load<Texture2D>("SpaceInvadersBackground");
            shipTexture = Content.Load<Texture2D>("Spaceship");
            laserTexture = Content.Load<Texture2D>("lazer_image");
            barrierTexture = Content.Load<Texture2D>("OGBarrier");
        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                keyboard.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (currentScreen == Screen.Intro)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    currentScreen = Screen.Single;
                }
            }

            else if (currentScreen == Screen.Single)
            {
                if (keyboard.IsKeyDown(Keys.Left) && ship.X > 0)
                {
                    ship.X -= shipSpeed;
                }

                if (keyboard.IsKeyDown(Keys.Right) && ship.Right < window.Width)
                {
                    ship.X += shipSpeed;
                }

                if (keyboard.IsKeyDown(Keys.Space) && previousKeyboard.IsKeyUp(Keys.Space))
                {
                    lasers.Add(new Rectangle(ship.X + ship.Width / 2 - 5, ship.Y, 10, 20));
                }

                for (int i = lasers.Count - 1; i >= 0; i--)
                {
                    lasers[i] = new Rectangle(lasers[i].X, lasers[i].Y - 8, lasers[i].Width, lasers[i].Height);

                    if (lasers[i].Y < 0)
                        lasers.RemoveAt(i);
                }
            }

            previousKeyboard = keyboard;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            if (currentScreen == Screen.Intro)
            {
                _spriteBatch.Draw(introScreen, window, Color.White);
            }
            else if (currentScreen == Screen.Single)
            {
                _spriteBatch.Draw(backgroundTexture, window, Color.White);
                _spriteBatch.Draw(shipTexture, ship, Color.White);
                _spriteBatch.Draw(barrierTexture, barrierrect, Color.MediumPurple);

                for (int i = 0; i < lasers.Count; i++)
                {
                    _spriteBatch.Draw(laserTexture, lasers[i], Color.White);
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}