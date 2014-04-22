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
        GraphicsDevice graphicsDevice;

        //set camera to 0,0
        public Camera(GraphicsDevice graphicsDevice)
        {
            position = new Vector2(0, 0);
            this.graphicsDevice = graphicsDevice;
        }

        //Used to move the camera whenever mouse is near edge of world's viewport
        public void Pan(Vector2 direction)
        {
            position += new Vector2(10 * direction.X, 10 * direction.Y);
        }

        //Used for the game world's viewport to make the square 2D background image appear isometric
        //First it takes the square background and rotates it by 45 degrees, 
        //then it scales the now diamond shaped image vertically to 63%.
        //This allows for the square image to be viewed from an isometric perspective 
        public Matrix getTransformation()
        {
            Viewport viewport = graphicsDevice.Viewport;
            Vector3 axis = new Vector3(3 * MathHelper.ToRadians(45), MathHelper.ToRadians(45), 2 * MathHelper.ToRadians(45));
            axis.Normalize();
            transformation =
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                                                Matrix.CreateRotationZ(MathHelper.ToRadians(45)) *      //rotate 45 degrees
                                                Matrix.CreateScale(new Vector3(1.0f, 0.5f, 1.0f)) *     //scale vertical by 63%
                                                Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));

            return transformation;
        }

        //Will eventually convert mouse clicks into world coordinates
        //Untested and currently unused
        //Still trying to learn how to a matrix
        public Vector2 ScreenToWorldPos(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, Matrix.Invert(getTransformation()));
        }

        public Vector2 WorldToScreenPos(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, getTransformation());
        }
    }
}
