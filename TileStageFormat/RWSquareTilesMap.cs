using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TileStageFormat.Events;
using TileStageFormat.Map.Square;

namespace TileStageFormat
{
    /// <summary>
    /// 主に、スクエアマップを扱う
    /// </summary>
    public partial class D2StageFile
    {
        /// <summary>
        /// スクエアータイルマップ作成後、呼び出すイベント
        /// </summary>
        public event CreateSquareTilesMapHandler CreateSquareTilesMapCall;


        private SortedDictionary<string, SquareTilesMap> m_squareTileMapDic = new SortedDictionary<string, SquareTilesMap>(StringComparer.CurrentCultureIgnoreCase);


        /// <summary>
        /// 指定したマップ名がすでに存在するか？
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool ExistenceSquareTileMapName(string s)
        {
            if (m_squareTileMapDic.ContainsKey(s))
                return true;
            return false;

        }
        /// <summary>
        /// マップ名からSquareTilesMapを参照する
        /// </summary>
        /// <param name="mapName">スクエアタイル群マップ名</param>
        /// <returns></returns>
        public SquareTilesMap FindSquareTileMapFromName(string mapName)
        {
            if (!m_squareTileMapDic.ContainsKey(mapName))
            {
                MessageBox.Show("マップ名(" + mapName + ")からImageSquareTileを取得できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return m_squareTileMapDic[mapName];
        }

        /// <summary>
        /// マップチップのマップを作成する
        /// </summary>
        /// <param name="tileNumX"></param>
        /// <param name="tileNumY"></param>
        /// <param name="strMapName"></param>
        public void CreateMap(int tileNumX, int tileNumY, string strMapName)
        {
            SquareTilesMap map = new SquareTilesMap(tileNumX, tileNumY, strMapName);
            m_squareTileMapDic.Add(strMapName, map);

            //TreeNode tNode;
            //tNode = m_rTV.Nodes[0].Nodes[4].Nodes.Add(strMapName);
            //tNode.ImageIndex = 8;
            //tNode.SelectedImageIndex = 9;
            //tNode.Name = TV_NAME_MAP;
        }
        public void DeleteMap(string strName)
        {
            m_squareTileMapDic.Remove(strName);
        }
    }
}
