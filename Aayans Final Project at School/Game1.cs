using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

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

        Random rng = new Random();

        List<Rectangle> aliens = new List<Rectangle>();
        List<Color> alienColor = new List<Color>();
        List<Rectangle> alienLasers = new List<Rectangle>();
        List<Rectangle> shipLasers = new List<Rectangle>();
        List<Rectangle> barriers = new List<Rectangle>();
        List<Texture2D> alienTextures = new List<Texture2D>();
        List<int> barriersHealth = new List<int>();



        Color neonGreen;
        SpriteFont font;
        Screen currentScreen;
        MouseState mouseState;
        KeyboardState previousKeyboard;

        Rectangle window;
        Rectangle barrierrect;
        Rectangle ship;
        Rectangle ship2;

        Texture2D introScreen;
        Texture2D backgroundTexture;
        Texture2D shipTexture;
        Texture2D laserTexture;
        Texture2D ogBarrier;
        Texture2D slightBarrier;
        Texture2D lastBarrier;
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
        int score = 0;

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
            barriers.Add(new Rectangle(80, 400, 180, 60));
            barriers.Add(new Rectangle(313, 400, 180, 60));
            barriers.Add(new Rectangle(545, 400, 180, 60));
            barriersHealth.Add(500);
            barriersHealth.Add(500);
            barriersHealth.Add(500);
            neonGreen = new Color(57, 255, 20);

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    aliens.Add(new Rectangle(startX + col * (alienWidth + spacingX), 30 + row * 50, alienWidth, alienHeight));

                    if (row == 0)
                    {
                        alienColor.Add(Color.Lime);
                    }
                    else if (row == 1)
                    {
                        alienColor.Add(Color.Cyan);
                    }
                    else if (row == 2)
                    {
                        alienColor.Add(Color.Magenta);
                    }
                    else if (row == 3)
                    {
                        alienColor.Add(Color.Yellow);
                    }
                    else if (row == 4)
                    {
                        alienColor.Add(Color.Orange);
                    }
                    else
                        alienColor.Add(Color.Red);
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
            ogBarrier = Content.Load<Texture2D>("OGBarrier");
            slightBarrier = Content.Load<Texture2D>("SlightBarrier");
            lastBarrier = Content.Load<Texture2D>("LastBarrier");
            font = Content.Load<SpriteFont>("font");
            alien1 = Content.Load<Texture2D>("Alien 1");
            alien2 = Content.Load<Texture2D>("Alien 2");
            alien3 = Content.Load<Texture2D>("Alien 3");
            alien4 = Content.Load<Texture2D>("Alien 4");
            alien5 = Content.Load<Texture2D>("Alien 5");
            alien6 = Content.Load<Texture2D>("Alien 6");

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (row == 0)
                    {
                        alienTextures.Add(alien1);
                    }
                    else if (row == 1)
                    {
                        alienTextures.Add(alien2);
                    }
                    else if (row == 2)
                    {
                        alienTextures.Add(alien3);
                    }
                    else if (row == 3)
                    {
                        alienTextures.Add(alien4);
                    }
                    else if (row == 4)
                    {
                        alienTextures.Add(alien5);
                    }
                    else
                    {
                        alienTextures.Add(alien6);
                    }
                }
            }
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
                    shipLasers.Add(new Rectangle(ship.X + ship.Width / 2 - 5, ship.Y, 10, 20));
                }

                for (int i = shipLasers.Count - 1; i >= 0; i--)
                {
                    shipLasers[i] = new Rectangle(shipLasers[i].X, shipLasers[i].Y - 8, shipLasers[i].Width, shipLasers[i].Height);

                    if (shipLasers[i].Y < 0)
                        shipLasers.RemoveAt(i);
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

                for (int l = shipLasers.Count - 1; l >= 0; l--)
                {
                    for (int a = aliens.Count - 1; a >= 0; a--)
                    {
                        if (shipLasers[l].Intersects(aliens[a]))
                        {
                            shipLasers.RemoveAt(l);
                            aliens.RemoveAt(a);
                            alienColor.RemoveAt(a);
                            alienTextures.RemoveAt(a);
                            score += 40;
                            break;
                        }
                    }
                }



                for (int l = shipLasers.Count - 1; l >= 0; l--)
                {
                    for (int b = barriers.Count - 1; b >= 0; b--)
                    {
                        if (shipLasers[l].Intersects(barriers[b]))
                        {
                            shipLasers.RemoveAt(l);
                            barriersHealth[b] -= 25;

                            if (barriersHealth[b] <= 0)
                            {
                                barriers.RemoveAt(b);
                                barriersHealth.RemoveAt(b);
                            }

                            break;

                        }

                    }
                }
                
                

                previousKeyboard = keyboard;
                base.Update(gameTime);
            }
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
                _spriteBatch.DrawString(font, "Score: " + score, new Vector2(338, 10), Color.White);

                for (int i = 0; i < barriers.Count; i++)
                {
                    if (barriersHealth[i] >= 300)
                    {
                        _spriteBatch.Draw(ogBarrier, barriers[i], Color.MediumPurple);
                    }

                    else if (barriersHealth[i] >= 150)
                    {
                        _spriteBatch.Draw(slightBarrier, barriers[i], Color.MediumPurple);
                    }
                    else if (barriersHealth[i] <= 150)
                    {
                        _spriteBatch.Draw(lastBarrier, barriers[i], Color.MediumPurple);

                    }
                }

                for (int i = 0; i < shipLasers.Count; i++)
                {
                    _spriteBatch.Draw(laserTexture, shipLasers[i], Color.White);
                }

                for (int i = 0; i < aliens.Count; i++)
                {
                    _spriteBatch.Draw(alienTextures[i], aliens[i], alienColor[i]);
                }

            }

            else if (currentScreen == Screen.Duo)
            {

            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}