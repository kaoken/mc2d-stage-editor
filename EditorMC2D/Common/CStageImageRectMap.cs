using System.Collections.Generic;
using TileStageFormat.Tile.Square;
using TileStageFormat.Animation.Rect;
using TileStageFormat.Map.Rect;
using MC2DUtil.ChunkFormat;

namespace Common
{
    class CStageImageRectMap
    {
        protected string m_Name;
        protected List<ImageSquareTile> m_rvImage;
        protected List<AnimationRect> m_rvAnimaRECT;	
        protected List<FImageRectMapSprite> m_aIRMS;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CStageImageRectMap(string name)
        {
            m_aIRMS = new List<FImageRectMapSprite>();
            m_Name = name;
        }
        //###########################################################################
        //##
        //## 読み込み
        //##
        //###########################################################################
        public bool FF_LoadStageFile(
            string name,
            List<ImageSquareTile> rvImageFile,
            List<AnimationRect> rvAnimaRECT,
            LoadChank lChank
        )
        {
            m_rvImage = rvImageFile;
            m_rvAnimaRECT = rvAnimaRECT;
            m_Name = name;
            return true;
        }
        //###########################################################################
        //##
        //## 書き込み
        //##
        //###########################################################################
        /// <summary>
        /// ファイル書き込み
        /// </summary>
        /// <param name="objSave"></param>
        /// <returns></returns>
        public bool FF_WriteStageFile(SaveState objSave)
        {
            return true;
        }
    }
}
