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

        public Camera()
        {
            position = new Vector2(0, 0);
        }

        public void Pan(Vector2 direction)
        {
            position += new Vector2(10 * direction.X, 10 * direction.Y);
        }

        public Matrix getTransformation(GraphicsDevice graphicsDevice)
        {
            Viewport viewport = graphicsDevice.Viewport;
            Vector3 axis = new Vector3(3 * MathHelper.ToRadians(45), MathHelper.ToRadians(45), 2 * MathHelper.ToRadians(45));
            axis.Normalize();
            transformation =
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                                                Matrix.CreateRotationZ(MathHelper.ToRadians(45)) *
                                                Matrix.CreateScale(new Vector3(1.0f, 0.5f, 1.0f)) *
                                                Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));

            return transformation;
        }
    }
}
