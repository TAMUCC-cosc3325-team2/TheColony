using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheColony
{
    
    class Camera
    {
        public Vector2 position;
        public Matrix transformation;

        //set camera to 0,0
        public Camera()
        {
            position = new Vector2(0, 0);
        }

        //Used to move the camera whenever mouse is near edge of world's viewport
        public void Pan(Vector2 direction)
        {
            position += new Vector2(10 * direction.X, 10 * direction.Y);
        }

        //Used for the game world's viewport to make the square 2D background image appear isometric
        public Matrix getTransformation(GraphicsDevice graphicsDevice)
        {
            Viewport viewport = graphicsDevice.Viewport;
            Vector3 axis = new Vector3(3 * MathHelper.ToRadians(45), MathHelper.ToRadians(45), 2 * MathHelper.ToRadians(45));
            axis.Normalize();
            transformation =
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                                                Matrix.CreateRotationZ(MathHelper.ToRadians(45)) *      //rotate 45 degrees
                                                Matrix.CreateScale(new Vector3(1.0f, 0.5f, 1.0f)) *     //scale vertical by 50%
                                                Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));

            return transformation;
        }


        //Will eventually convert mouse clicks into world coordinates
        //Untested and currently unused
        //Still trying to learn how to a matrix
        public Matrix screenToWorld(GraphicsDevice graphicsDevice)
        {
            Viewport viewport = graphicsDevice.Viewport;
            transformation = new Matrix();
            transformation.M11 = 1 / 2 * viewport.Width;
            transformation.M12 = 1 / 2 * viewport.Height;
            transformation.M13 = -position.X / 2 * viewport.Width - position.Y / 2 * viewport.Height;
            transformation.M21 = -1 / 2 * viewport.Width;
            transformation.M22 = 1 / 2 * viewport.Height;
            transformation.M23 = position.X / 2 * viewport.Width - position.Y / 2 * viewport.Height;
            transformation.M31 = 0;
            transformation.M32 = 0;
            transformation.M33 = 1;

            return transformation;
        }
    }
}
