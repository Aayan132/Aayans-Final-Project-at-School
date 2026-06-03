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

        //a is for alien
        //l is for laser

        List<Rectangle> aliens = new List<Rectangle>();
        List<Color> alienColor = new List<Color>();
        List<Rectangle> lasers = new List<Rectangle>();

        Color neonGreen;
        SpriteFont font;
        Screen currentScreen;
        MouseState mouseState;
        KeyboardState previousKeyboard;

        Rectangle window;
        Rectangle barrierrect1, barrierrect2, barrierrect3;
        Rectangle ship;
        Rectangle ship2;

        Texture2D introScreen;
        Texture2D backgroundTexture;
        Texture2D shipTexture;
        Texture2D laserTexture;
        Texture2D barrierTexture;
        Texture2D alien1;
        Texture2D alien2;
        Texture2D alien3;
        Texture2D alien4;
        Texture2D alien5;
        Texture2D alien6;

        int shipSpeed = 5;
        int menuChoice = 0;
        int alienSpeed = 1;
        int alienDirection = 1;
        int alienWidth = 40;
        int alienHeight = 30;
        int startX = 100;
        int spacingX = 8;
        int columns = 9;

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
            ship2 = new Rectangle(130, 500, 80, 80);
            barrierrect1 = new Rectangle(80, 400, 180, 60);
            barrierrect2 = new Rectangle(313, 400, 180, 60);
            barrierrect3 = new Rectangle(545, 400, 180, 60);
            neonGreen = new Color(57, 255, 20);

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    aliens.Add(new Rectangle(startX + col * (alienWidth + spacingX), 10 + row * 50, alienWidth, alienHeight));
                    
                    if (row == 0)
                        alienColor.Add(neonGreen);
                    else if (row == 1)
                        alienColor.Add(Color.Blue);
                    else if (row == 2)
                        alienColor.Add(Color.Purple);
                    else if (row == 3)
                        alienColor.Add(Color.Yellow);
                    else if (row == 4)
                        alienColor.Add(Color.Red);
                    else
                        alienColor.Add(Color.YellowGreen);

                }
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            introScreen = Content.Load<Texture2D>("SIIntroScreen");
            backgroundTexture = Content.Load<Texture2D>("SpaceInvadersBackground");
            shipTexture = Content.Load<Texture2D>("Spaceship");
            laserTexture = Content.Load<Texture2D>("lazer_image");
            barrierTexture = Content.Load<Texture2D>("OGBarrier");
            font = Content.Load<SpriteFont>("font");
            alien1 = Content.Load<Texture2D>("Alien 1");
            alien2 = Content.Load<Texture2D>("Alien 2");
            alien3 = Content.Load<Texture2D>("Alien 3");
            alien4 = Content.Load<Texture2D>("Alien 4");
            alien5 = Content.Load<Texture2D>("Alien 5");
            alien6 = Content.Load<Texture2D>("Alien 6");
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
                if (keyboard.IsKeyDown(Keys.Up) && previousKeyboard.IsKeyUp(Keys.Up))
                {
                    menuChoice--;
                    if (menuChoice < 0)
                        menuChoice = 1;
                }
                if (keyboard.IsKeyDown(Keys.Down) && previousKeyboard.IsKeyUp(Keys.Down))
                {
                    menuChoice++;
                    if (menuChoice > 1)
                        menuChoice = 0;
                }
                if ((keyboard.IsKeyDown(Keys.Enter)) && previousKeyboard.IsKeyUp(Keys.Enter))
                {
                    if (menuChoice == 0)
                    {
                        currentScreen = Screen.Single;
                    }
                    else if (menuChoice == 1)
                    {
                        currentScreen = Screen.Duo;
                    }
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

                for (int i = 0; i < aliens.Count; i++)
                {
                    aliens[i] = new Rectangle(aliens[i].X + alienSpeed * alienDirection, aliens[i].Y, aliens[i].Width, aliens[i].Height);
                }

                bool hitWall = false;

                for (int i = 0; i < aliens.Count; i++)
                {
                    if (aliens[i].X <= 0 || aliens[i].X + aliens[i].Width >= window.Width)
                    {
                        hitWall = true;
                    }
                }

                if (hitWall == true)
                {
                    alienDirection *= -1;

                    for (int i = 0; i < aliens.Count; i++)
                    {
                        aliens[i] = new Rectangle(aliens[i].X, aliens[i].Y + 20, aliens[i].Width, aliens[i].Height);
                    }
                }

                for (int l = lasers.Count - 1; l >= 0; l--)
                {
                    for (int a = aliens.Count - 1; a >= 0; a--)
                    {
                        if (lasers[l].Intersects(aliens[a]))
                        {
                            lasers.RemoveAt(l);
                            aliens.RemoveAt(a);
                            alienColor.RemoveAt(a);
                            break;
                        }
                    }
                }
            }

            if (currentScreen == Screen.Duo)
            {
                if (keyboard.IsKeyDown(Keys.Up) && previousKeyboard.IsKeyUp(Keys.Up))
                {
                    menuChoice--;
                    if (menuChoice < 0)
                        menuChoice = 1;
                }
                if (keyboard.IsKeyDown(Keys.Down) && previousKeyboard.IsKeyUp(Keys.Down))
                {
                    menuChoice++;
                    if (menuChoice > 1)
                        menuChoice = 0;
                }
                if ((keyboard.IsKeyDown(Keys.Enter)) && previousKeyboard.IsKeyUp(Keys.Enter))
                {
                    if (menuChoice == 0)
                    {
                        currentScreen = Screen.Single;
                    }
                    else if (menuChoice == 1)
                    {
                        currentScreen = Screen.Duo;
                    }
                }


            }

            if (currentScreen == Screen.Duo)
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

                if (menuChoice == 0)
                {
                    _spriteBatch.DrawString(font, "> 1: SI Game", new Vector2(290, 420), neonGreen);
                    _spriteBatch.DrawString(font, " 2: Tutorial", new Vector2(290, 445), Color.White);

                }
                else
                {
                    _spriteBatch.DrawString(font, " 1: SI Game", new Vector2(290, 420), Color.White);
                    _spriteBatch.DrawString(font, "> 2: Tutorial", new Vector2(290, 445), neonGreen);
                }
            }
            else if (currentScreen == Screen.Single)
            {
                _spriteBatch.Draw(backgroundTexture, window, Color.White);
                _spriteBatch.Draw(shipTexture, ship, Color.White);
                _spriteBatch.Draw(barrierTexture, barrierrect1, Color.MediumPurple);
                _spriteBatch.Draw(barrierTexture, barrierrect2, Color.MediumPurple);
                _spriteBatch.Draw(barrierTexture, barrierrect3, Color.MediumPurple);

                for (int i = 0; i < lasers.Count; i++)
                {
                    _spriteBatch.Draw(laserTexture, lasers[i], Color.White);
                }

                for (int i = 0; i < aliens.Count; i++)
                {
                    _spriteBatch.Draw(alien1, aliens[i], alienColor[i]);
                }
            }

            else if (currentScreen == Screen.Duo)
            {
                _spriteBatch.Draw(backgroundTexture, window, Color.White);
                _spriteBatch.Draw(shipTexture, ship, Color.White);
                _spriteBatch.Draw(barrierTexture, barrierrect1, Color.MediumPurple);
                _spriteBatch.Draw(barrierTexture, barrierrect2, Color.MediumPurple);
                _spriteBatch.Draw(barrierTexture, barrierrect3, Color.MediumPurple);

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