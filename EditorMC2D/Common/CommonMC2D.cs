using EditorMC2D.Option;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Shell;
using TileStageFormat;
using TileStageFormat.Map.Square;
using TileStageFormat.Tile.Square;

namespace EditorMC2D.Common
{

    /// <summary>
    /// すべてのクラスから使用するシングルトンクラス
    /// </summary>
	public partial class CommonMC2D
    {
        public const float FRAME_RATE = 1.666666667e-2f;
        /// <summary>マップチップイメージ</summary>
        public const string TV_NAME_MAPCHIP_IMG = "ndMapChipImg";
        /// <summary>通常イメージ</summary>
        public const string TV_NAME_RECT_IMG    = "ndDefImg";
        /// <summary>アニメーション</summary>
        public const string TV_NAME_ANM_CHIP    = "ndChipAnmFolder";
        /// <summary>アニメーション</summary>
        public const string TV_NAME_ANM_RECT    = "ndRectAnmFolder";
        /// <summary>マップ</summary>
        public const string TV_NAME_MAP         = "ndMap";
        /// <summary>通常イメージ</summary>
        public const string TV_NAME_MAP_CHANGE = "ndMapChange";
        /// <summary>イメージRECTマップ</summary>
        public const string TV_NAME_IMG_RECT_MAP_F = "ndImgRectMapFolder";
        /// <summary>イメージRECTマップ</summary>
        public const string TV_NAME_IMG_RECT_MAP = "ndImgRectMap";
        /// <summary>コンフィグ</summary>
        private APPConfig m_config = null;
        private ProcessMC2D m_process = null;


        /// <summary>
        /// ステージタイル
        /// </summary>
        protected D2StageFile m_d2Stg = null;

        /// <summary>
		/// 
		/// </summary>
		private static readonly CommonMC2D instance = new CommonMC2D();
        private TreeView m_rTV;
		/// <summary>
		/// 自信のインスタンス
		/// </summary>
		public static CommonMC2D Instance
		{
			get 
			{
				return instance; 
			}
		}
        /// <summary>
        /// メインウィンドウフォーム
        /// </summary>
        public MainWindow MainWindow { get; set; }

        /// <summary>
        /// MC2D実行ファイルがあるディレクトリーパス
        /// </summary>
        public string DirPathMC2D { get { return MainWindow.DirPathMC2D; } }


        /// <summary>
        /// このアプリのコンフィグ（オプション）
        /// </summary>
        public APPConfig Config { get { return m_config; } }
        /// <summary>
        /// プロジェクトを読み込み中か
        /// </summary>
        public bool IsReadProject { get { return m_d2Stg.IsOpenFile; } }


        //private SortedDictionary<string, CStageBackground> m_mapBG = new SortedDictionary<string, CStageBackground>(StringComparer.CurrentCultureIgnoreCase);
        //private SortedDictionary<string, CStageImageRectMap> m_mapImgRectMap = new SortedDictionary<string, CStageImageRectMap>(StringComparer.CurrentCultureIgnoreCase);

        //----------------------------
        // Util
        //----------------------------
        private Bitmap[] m_aCollisionChipImg;
        private Image m_rectImg = null;

        /// <summary> </summary>
        public D2StageFile D2Stage { get { return m_d2Stg; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private CommonMC2D()
		{
            m_d2Stg = new D2StageFile();
            // 衝突判定用チップイメージ
            m_aCollisionChipImg = new Bitmap[36];
            int i;
            for (i = 0; i < 36; ++i)
            {
                m_aCollisionChipImg[i] = CopyCatRectangleBitmap(
                    global::EditorMC2D.Properties.Resources.colli_chip,
                    new Rectangle(i % 12 * 40, i / 12 * 40, 40, 40),
                    40
                );
            }

            //-------------------------------------------
            // コンフィグ作成
            //-------------------------------------------
            m_config = APPConfig.Read();

            //--
            m_rectImg = global::EditorMC2D.Properties.Resources.gura;

            m_process = new ProcessMC2D();
            InitOutputWindowDatas();

        }
        ~CommonMC2D()
        {
            m_process.StopMC2dApp();
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            //m_mapMap.Clear();
            //m_mapBG.Clear();
        }
        public void SetTreeviewReference(TreeView rTV)
        {
            m_rTV = rTV;
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  便利なもの
        //##
        //##############################################################################
        //##############################################################################
        public ProcessMC2D Process { get { return m_process; } }
        //##############################################################################
        //##############################################################################
        //##
        //##  ビットマップ関係
        //##
        //##############################################################################
        //##############################################################################
        #region ビットマップ関係
        /// <summary>
        /// してした衝突チップを反転または回転させたビットマップを作る
        /// </summary>
        /// <param name="nCollisionChipNo">衝突チップ番号</param>
        /// <param name="nCollisionChipFlgs">衝突チップフラグ</param>
        /// <returns>ビットマップ</returns>
        public Bitmap GetCollisionChipBitmap(byte nCollisionChipNo, byte nCollisionChipFlgs)
        {
            Bitmap tmp = (Bitmap)m_aCollisionChipImg[nCollisionChipNo].Clone();

            //回転する
            if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_ROT_L270) != 0)
            {
                if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
                    tmp.RotateFlip(RotateFlipType.Rotate90FlipX);
                else
                    tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_ROT_L180) != 0)
            {
                if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
                    tmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                else
                    tmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
            else if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_ROT_L90) != 0)
            {
                if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
                    tmp.RotateFlip(RotateFlipType.Rotate270FlipX);
                else
                    tmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
            else if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
            {
                tmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            return tmp;
        }
        /// <summary>
        /// 指定した衝突チップを反転または回転させたビットマップを作る
        /// </summary>
        /// <param name="nCollisionChipNo">衝突チップ番号</param>
        /// <param name="nCollisionChipFlgs">衝突チップフラグ</param>
        /// <returns>ビットマップ</returns>
        public static Bitmap GetImageChipBitmap(ImageSquareTile tlImgF, Image rImg, int tileNo, int flag)
        {
            int x = tileNo % tlImgF.blockX;
            int y = tileNo / tlImgF.blockX;
            Rectangle rc = new Rectangle(x * 40, y * 40, 40, 40);
            return GetImageChipBitmap(rImg, rc, flag);
        }
        /// <summary>
        /// 指定した衝突チップを反転または回転させたビットマップを作る
        /// </summary>
        /// <param name="nCollisionChipNo">衝突チップ番号</param>
        /// <param name="nCollisionChipFlgs">衝突チップフラグ</param>
        /// <returns>ビットマップ</returns>
        public static Bitmap GetImageChipBitmap(Image bitmap, Rectangle rc, int flag)
        {
            //Bitmap tmp = CopyCatRectangleBitmap((Bitmap)bitmap, rc, 40);

            ////回転する
            //if ((flag & FSquareTileInfo.COLLI_ROT_R90) != 0)
            //{
            //    if ((flag & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
            //        tmp.RotateFlip(RotateFlipType.Rotate90FlipX);
            //    else
            //        tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //}
            //else if ((flag & FSquareTileInfo.COLLI_ROT_R180) != 0)
            //{
            //    if ((flag & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
            //        tmp.RotateFlip(RotateFlipType.Rotate180FlipX);
            //    else
            //        tmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
            //}
            //else if ((flag & FSquareTileInfo.COLLI_ROT_R270) != 0)
            //{
            //    if ((flag & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
            //        tmp.RotateFlip(RotateFlipType.Rotate270FlipX);
            //    else
            //        tmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            //}
            //else if ((flag & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
            //{
            //    tmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            //}
            //return tmp;
            return null;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <param name="blockX"></param>
        ///// <param name="blockY"></param>
        ///// <param name="framePos"></param>
        ///// <param name="g"></param>
        ///// <returns></returns>
        //public int DrawAnimationFrame(int x, int y, int blockX, int blockY, int framePos, Graphics g)
        //{
        //    //---------------------------------------
        //    // 選択されているブロックを示す
        //    //---------------------------------------
        //    int blockPosX = blockX * 40;
        //    int blockPosY = blockY * 40;
        //    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
        //    g.DrawImage(m_rectImg, new Rectangle(x + blockPosX,       y+blockPosY, 38, 2), new Rectangle(0, framePos, 38, 2), GraphicsUnit.Pixel);
        //    g.DrawImage(m_rectImg, new Rectangle(x + blockPosX + 38,  y+blockPosY, 2, 38), new Rectangle(framePos, 0, 2, 38), GraphicsUnit.Pixel);
        //    g.DrawImage(m_rectImg, new Rectangle(x + blockPosX +  2,  y+blockPosY + 38, 38, 2), new Rectangle(0, 6 - framePos, 38, 2), GraphicsUnit.Pixel);
        //    g.DrawImage(m_rectImg, new Rectangle(x + blockPosX +  0,  y+blockPosY + 2, 2, 38), new Rectangle(6 - framePos, 0, 2, 38), GraphicsUnit.Pixel);
        //    if ((framePos += 2) >= 8) framePos = 0;
        //    return framePos;
        //}
        /// <summary>
        /// 指定したビットマップから切り出し、新たなビットマップを作る
        /// </summary>
        /// <param name="source">元となるビットマップ</param>
        /// <param name="part">範囲</param>
        /// <param name="size">幅高さの範囲</param>
        /// <returns>ビットマップ</returns>
        public static Bitmap CopyCatRectangleBitmap(Bitmap source, Rectangle part, int size)
        {
            //Graphics gs = Graphics.FromImage(source);
            //Bitmap bmp = new Bitmap(size, size, gs);
            //gs.Dispose();
            //Graphics g = Graphics.FromImage(bmp);
            //g.DrawImage(source, 0, 0, part, GraphicsUnit.Pixel);
            //g.Dispose();
            //return bmp;
            return null;
        }
        /// <summary>
        /// コリジョンチップ情報取得
        /// </summary>
        /// <param name="srcColliFlg">チップ番号</param>
        /// <param name="mapColliFlg">送信先テクスチャーのX座標</param>
        /// <returns></returns>
        public static int GetCollisionMapChip(int srcColliFlg, int mapColliFlg)
        {
            int[,] sRotTbl_A = new int[,]{
		        {0,1,2,3}, {1,2,3,0}, {2,3,0,1}, {3,0,1,2}
            };
            int[,] sRotTbl_B = new int[,]{
		        {4,5,6,7}, {7,4,5,6}, {6,7,4,5}, {5,6,7,4}
	        };
            int tmp,s,d,n,r;

            n = s = d = r = 0;
            tmp = 0;
            s = (srcColliFlg & 0x07);
            if (s > 2)
                s = 3;
            d = (mapColliFlg & 0x07);
            if (d > 2)
                d = 3;

            if (((srcColliFlg ^ mapColliFlg) & FSquareTileInfoMap.FLIPHORIZONTAL) != 0)
            {
                tmp = FSquareTileInfo.COLLI_FLIPHORIZONTAL;
                r = sRotTbl_B[s,d];
            }
            else
            {
                r = sRotTbl_A[s,d];
            }
            switch (r)
            {
                case 0:
                case 4:
                    tmp |= (int)FSquareTileInfo.COLLI_ROT_0;
                    break;
                case 1:
                case 5:
                    tmp |= (int)FSquareTileInfo.COLLI_ROT_L90;
                    break;
                case 2:
                case 6:
                    tmp |= (int)FSquareTileInfo.COLLI_ROT_L180;
                    break;
                case 3:
                case 7:
                    tmp |= (int)FSquareTileInfo.COLLI_ROT_L270;
                    break;
            }

	        return tmp;
        }
        #endregion
        //##########################################################################
        //##########################################################################
        //##
        //## 　チェック関連
        //##
        //##########################################################################
        //##########################################################################
        #region チェック関連


        #endregion
        //##########################################################################
        //##########################################################################
        //##
        //## 作成
        //##
        //##########################################################################
        //##########################################################################
        #region 作成関係

        ///// <summary>
        ///// イメージRECTマップを作成する
        ///// </summary>
        ///// <param name="name">イメージRECTマップ名</param>
        //public bool CreateImageRectMap(string name)
        //{
        //    if (m_mapImgRectMap.ContainsKey(name))
        //    {
        //        MessageBox.Show("同じイメージRECTマップ名が既にあります。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return false;
        //    }
        //    CStageImageRectMap map = new CStageImageRectMap(name);
        //    m_mapImgRectMap.Add(name, map);

        //    TreeNode tNode;
        //    tNode = m_rTV.Nodes[0].Nodes[6].Nodes.Add(name);
        //    tNode.ImageIndex = 8;
        //    tNode.SelectedImageIndex = 9;
        //    tNode.Name = TV_NAME_IMG_RECT_MAP;

        //    return true;
        //}
        #endregion
        //##########################################################################
        //##########################################################################
        //##
        //## 取得関係
        //##
        //##########################################################################
        //##########################################################################



        ///// <summary>
        ///// 指定時間（nowTime）から指定位置のフレーム、イメージ、チップ番号導きだし渡す
        ///// </summary>
        ///// <param name="nowTime">経過した時間</param>
        ///// <param name="nAChipNo">短径番号</param>
        ///// <param name="pFrameNo">時間から指定位置のフレーム番号を渡す</param>
        ///// <param name="pImageNo">イメージ番号を渡す</param>
        ///// <param name="pChipNo">チップ番号を渡す</param>
        ///// <returns></returns>
        //public int GetChipAnmFramePosition(float time, int nAChipNo, SquareTileAnimationInfo rCAI) 
        //{
        //    AnmSquareTile r = null;
	       // int i,nMax;
        //    float f1, f2;


        //    r = m_aChipAnimations[nAChipNo];
	       // nMax = r.ChipCount-1;
        //    if (rCAI.state == SquareTileAnimationInfo.PLAY)
        //    {
        //        rCAI.nowTime += time;
        //        while (rCAI.nowTime >= r.period)
        //        {
        //            rCAI.nowTime -= r.period;
        //            if (rCAI.loopNum != -1) --rCAI.loopNum;
        //        }
        //    }
        //    else if (rCAI.state == SquareTileAnimationInfo.INV_PLAY)
        //    {
        //        rCAI.nowTime -= time;
        //        while (rCAI.nowTime <= 0)
        //        {
        //            rCAI.nowTime += r.period;
        //            if (rCAI.loopNum != -1) --rCAI.loopNum;
        //        }
        //    }
        //    if (rCAI.loopNum == 0)
        //    {
        //        rCAI.state = SquareTileAnimationInfo.STOP;
        //        return 0;
        //    }
        //    f1 = 0;
        //    f2 = r.aTile[0].time;
        //    for (i = 0; i < r.ChipCount; ++i)
        //    {
        //        if ( f1 <= rCAI.nowTime && f2 >= rCAI.nowTime)
        //        {
        //            rCAI.frameNum = i;
        //            return i;
        //        }
        //        if (i == r.ChipCount) break;
        //        f1 = f2;
        //        f2 = r.aTile[i+1].time;
        //    }
        //    rCAI.frameNum = nMax;

        //    return nMax;
        //}
        //public bool GetRectImageNames(ref ComboBox obj)
        //{
        //    for (int i = 0; i < m_imagesRect.Count; ++i)

        //    obj.Items.Add(m_imagesRect[i].ToString());
        //    return obj.Items.Count > 0;
        //}

    }
}
