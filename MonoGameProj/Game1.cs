using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameProj
{
    public class Game1 : Game
    {
        Texture2D ball;
        Model man;
        //Vector2 ballPos;
        float distance;
        //Vector3 ballVel;
        Vector3 InitVel;
        Vector3 ShipVel;
        Vector3 ShipPos;
        Vector3 ShipRotation;
        //Vector3 ShipUp;
        
        Matrix view;
        Matrix world;
        Matrix ship;
        Matrix shipReposition;
        Matrix projection;
        
        VertexBuffer vertexBuffer;
        VertexPositionColor[] vertices;
        BasicEffect effectLine;

        SpriteFont font;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //ballPos = new Vector2 (_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            //ballVel = new Vector3(3.0f, 3.0f, -2.0f);
            distance = 10f;
            world = Matrix.CreateWorld(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -1f), Vector3.Up);
            shipReposition = Matrix.CreateRotationX(MathHelper.ToRadians(-90f)) * Matrix.CreateRotationY(MathHelper.ToRadians(180f));
            view = Matrix.CreateLookAt(new Vector3(0f, 10f, 10f), new Vector3(0f, 0f, 0f), Vector3.Up);//Vector3.Cross(new Vector3(1f, 0f, 0f), new Vector3(0f, 1f, 1f)));
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 
                                                            _graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight,
                                                            0.1f,
                                                            100f);
            InitVel = new Vector3(0f, 0f, -0.2f);
            ShipVel = InitVel;
            ShipPos = new Vector3(0f, 0f, 0f);
            ShipRotation = new Vector3(0f, 0f, 0f);
            
            vertices = new VertexPositionColor[8];
            vertices[0] = new VertexPositionColor(Vector3.Zero, Color.Yellow);
            vertices[1] = new VertexPositionColor(ShipVel * 5, Color.Yellow);
            
            vertices[2] = new VertexPositionColor(Vector3.Zero, Color.Red);
            vertices[3] = new VertexPositionColor(Vector3.UnitX * 10, Color.Red);

            vertices[4] = new VertexPositionColor(Vector3.Zero, Color.Green);
            vertices[5] = new VertexPositionColor(Vector3.UnitY * 10, Color.Green);

            vertices[6] = new VertexPositionColor(Vector3.Zero, Color.Purple);
            vertices[7] = new VertexPositionColor(Vector3.UnitZ * 10, Color.Purple);

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData(vertices);

            effectLine = new BasicEffect(GraphicsDevice);
            effectLine.VertexColorEnabled = true;
            //ShipUp = new Vector3(1f,0f,0f);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ball = Content.Load<Texture2D>("ball");
            man = Content.Load<Model>("Ship");
            font = Content.Load<SpriteFont>("File");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            /*if (ballPos.X + ball.Width > _graphics.PreferredBackBufferWidth || ballPos.X - ball.Width < 0)
            {
                ballVel.X *= -1;
                ballVel.Z *= -1;
            }
            if (ballPos.Y + ball.Height > _graphics.PreferredBackBufferHeight || ballPos.Y - ball.Height < 0)
            {
                ballVel.Y *= -1;
            }
            else
            {
                ballVel.Y += 0.3f;
            }

            ballPos.X += ballVel.X;
            ballPos.Y += ballVel.Y;*/

            const float turn_speed = 1f;

            Vector3 camera = new Vector3(0f, 10f, 10f);

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                ShipPos += ShipVel;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                ShipPos -= ShipVel;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                ShipRotation.Y += turn_speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                ShipRotation.Y -= turn_speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                ShipRotation.X += turn_speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                ShipRotation.X -= turn_speed;
            }
            // Пересчитывать скорость корабля постоянно, так же как и мировые координаты.
            //world = Matrix.CreateTranslation(ShipPos);
            ShipVel = Vector3.Transform(InitVel,
                Matrix.CreateRotationX(MathHelper.ToRadians(ShipRotation.X)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(ShipRotation.Y)));
            ship = world * shipReposition *
                Matrix.CreateRotationX(MathHelper.ToRadians(ShipRotation.X)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(ShipRotation.Y)) *
                Matrix.CreateTranslation(ShipPos);
            view = Matrix.CreateLookAt(camera, ShipPos, Vector3.Up);//Vector3.Cross(Vector3.Cross(camera - ShipPos, new Vector3(0f, 0f, 1f)), camera - ShipPos));

            vertices[1] = new VertexPositionColor(ShipVel * 5, Color.Yellow);

            vertexBuffer.SetData(vertices);

            //r = (r + 0.1f >= 360f) ? r + 0.1f - 360f : r + 0.1f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //_spriteBatch.Draw(ball, 
            //                ballPos, 
            //                null,
            //                Color.White,
            //                r,
            //                new Vector2(ball.Width / 2, ball.Height / 2),
            //                new Vector2(2f, 2f),
            //                SpriteEffects.None,
            //                0f);
            //_spriteBatch.
            _spriteBatch.DrawString(font, $"Velocity( X:{ShipVel.X}\nY:{ShipVel.Y}\nZ:{ShipVel.Z} )", new Vector2(10, 10), Color.Black);
            _spriteBatch.DrawString(font, $"Position( X:{ShipPos.X}\nY:{ShipPos.Y}\nZ:{ShipPos.Z} )", new Vector2(10, 100), Color.Black);
            _spriteBatch.End();
            foreach (ModelMesh mesh in man.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.World = ship;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }

            effectLine.World = world * Matrix.CreateTranslation(ShipPos);
            effectLine.View = view;
            effectLine.Projection = projection;

            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (EffectPass pass in effectLine.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                    (PrimitiveType.LineList, vertices, 0, 4);
            }

            base.Draw(gameTime);
        }
    }
}