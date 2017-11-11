using System.Collections.Generic;


namespace TileStageFormat.Tile.Rect
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageRect : FImageRect
    {
        public List<FImageRectInfo> aSprite = null;
        public ImageRect()
        {
            aSprite = new List<FImageRectInfo>();
        }
    }
}
