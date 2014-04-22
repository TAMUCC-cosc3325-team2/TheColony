using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheColony
{
    public static class TileEngine
    {

        static float tileHeight = 260.5f;
        static float tileWidth = 260.5f;
        static int offsetX = -1300;
        static int offsetY = -1300;

        static Rectangle tileArea = new Rectangle(offsetX, offsetY, (int)tileWidth * 10, (int)tileHeight * 10);

        public static Rectangle TileArea
        {
            get { return tileArea; }
        }

        public static float TileHeight
        {
            get { return tileHeight; }
        }

        public static float TileWidth
        {
            get { return tileWidth; }
        }

        public static int OffsetX
        {
            get { return offsetX; }
        }

        public static int OffsetY
        {
            get { return offsetY; }
        }

        public static Vector2 TileNumber(Vector2 position)
        {
            return new Vector2((int)((position.X - offsetX) / tileWidth), (int)((position.Y - offsetY) / tileHeight));
        }

        public static Vector2 TilePosition(Vector2 position)
        {
            Vector2 temp = TileNumber(position);
            return new Vector2(temp.X * TileWidth + offsetX, temp.Y * TileHeight + offsetY);
        }

        public static bool TileContains(Vector2 position)
        {
            if(TileArea.Contains((int)Math.Ceiling(position.X), (int)Math.Ceiling(position.Y)))
                return true;
            return false;
        }
    }
}