using MC2DUtil.ChunkFormat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TileStageFormat.Map.Square
{
    /// <summary>
    /// スクエアタイル群のマップ
    /// </summary>
    public class SquareTilesMap
    {
        public const int TOP_LEFT = 0;
        public const int TOP_CENTER = 1;
        public const int TOP_RIGHT = 2;
        public const int CENTER_LEFT = 3;
        public const int CENTER_CENTER = 4;
        public const int CENTER_RIGHT = 5;
        public const int BOTTOM_LEFT = 6;
        public const int BOTTOM_CENTER = 7;
        public const int BOTTOM_RIGHT = 8;
        /// <summary>
        /// 1フレームあたりの秒数
        /// </summary>
        public const float FRAME_RATE = 1.666666667e-2f;
        //-------------------------
        // メンバ変数
        //-------------------------
        /// <summary>
        /// 置き換えタイル群
        /// </summary>
        private SortedDictionary<string, SquareTilesTranspose> m_transPoseTiles = null;
        /// <summary>
        /// 置き換えタイル群のアニメーション
        /// </summary>
        private SortedDictionary<string, SquareTilesTransposeAnimation> m_replacementAnimations = null;
        /// <summary>
        /// イベントRECT
        /// </summary>
        private SortedDictionary<string, FSquareTileMapRect> m_eventRect = null;
        /// <summary>
        /// スクエアタイルマップの名前
        /// </summary>
        private string m_mapName;
        /// <summary>
        /// X軸のタイル数
        /// </summary>
        private short m_tileCountX;
        /// <summary>
        /// Y軸のタイル数
        /// </summary>
        private short m_tileCountY;
        /// <summary>
        /// タイル数の合計
        /// </summary>
        private int m_allTileCount;
        /// <summary>
        /// MAP上で使うスクエア・タイル群の情報
        /// </summary>
        private D2ArrayObject<FSquareTileInfoMap> m_mapTiles = null;



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SquareTilesMap()
        {
            m_transPoseTiles = new SortedDictionary<string, SquareTilesTranspose>(StringComparer.CurrentCultureIgnoreCase);
            m_replacementAnimations = new SortedDictionary<string, SquareTilesTransposeAnimation>(StringComparer.CurrentCultureIgnoreCase);
            m_eventRect = new SortedDictionary<string, FSquareTileMapRect>(StringComparer.CurrentCultureIgnoreCase);

        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tileCountX">X軸のタイル数</param>
        /// <param name="tileCountY">Y軸のタイル数</param>
        /// <param name="name">マップ名</param>
        public SquareTilesMap(int tileCountX, int tileCountY, string name)
        {
            m_transPoseTiles = new SortedDictionary<string, SquareTilesTranspose>(StringComparer.CurrentCultureIgnoreCase);
            m_replacementAnimations = new SortedDictionary<string, SquareTilesTransposeAnimation>(StringComparer.CurrentCultureIgnoreCase);
            m_eventRect = new SortedDictionary<string, FSquareTileMapRect>(StringComparer.CurrentCultureIgnoreCase);
            m_tileCountX = (short)tileCountX;
            m_tileCountY = (short)tileCountY;
            m_mapName = name;

            m_mapTiles = new D2ArrayObject<FSquareTileInfoMap>(m_tileCountX, m_tileCountY);
        }
        /// <summary>
        /// X軸のタイル数
        /// </summary>
        public int TileNumX { get { return m_tileCountX; } }
        /// <summary>
        /// Y軸のタイル数
        /// </summary>
        public int TileNumY { get { return m_tileCountY; } }
        /// <summary>
        /// タイル数の合計
        /// </summary>
        public int AallTileCount { get { return m_allTileCount; } }
        /// <summary>
        /// マップ名
        /// </summary>
        public string Name { get { return m_mapName; } }
        public D2ArrayObject<FSquareTileInfoMap> GetMapchipAryReference()
        {
            return m_mapTiles;
        }
        public FSquareTileInfoMap GetMapchipReference(int x, int y)
        {
            return m_mapTiles[y, x];
        }
        public FSquareTileInfoMap GetMapchipClone(int x, int y)
        {
            return m_mapTiles[y, x].Clone();
        }
        public D2ArrayObject<FSquareTileInfoMap> GetCat(RangeSquareTiles r)
        {
            D2ArrayObject<FSquareTileInfoMap> ret = new D2ArrayObject<FSquareTileInfoMap>(r.widthBlock, r.hightBlock);
            FSquareTileInfoMap rMIM0 = null, rMIM1 = null;
            int i, j, k, l, nX, nY;
            nX = r.startPosBlockX + r.widthBlock;
            nY = r.startPosBlockY + r.hightBlock;

            for (i = r.startPosBlockY, k = 0; i < nY; ++i, ++k)
            {
                for (j = r.startPosBlockX, l = 0; j < nX; ++j, ++l)
                {
                    rMIM0 = ret[k, l];
                    rMIM1 = m_mapTiles[i, j];
                    rMIM0.L00._n = rMIM1.L00._n;
                    rMIM0.L01._n = rMIM1.L01._n;
                }
            }

            return ret;
        }
        /// <summary>
        /// タイル群をペーストする
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <param name="rAA"></param>
        /// <param name="rMap"></param>
        /// <param name="catFlg"></param>
        /// <param name="pasteFlg"></param>
        public void PasteTiles(int x, int y, RangeSquareTiles r, D2ArrayObject<FSquareTileInfoMap> rAA, SquareTilesMap rMap, int catFlg, int pasteFlg)
        {
            FSquareTileInfoMap rMIM0 = null, rMIM1 = null;
            int i, j, k, l, nX, nY;
            nX = x + r.widthBlock;
            nY = y + r.hightBlock;
            nX = nX > m_tileCountX ? m_tileCountX : nX;
            nY = nY > m_tileCountY ? m_tileCountY : nY;

            for (i = y, k = 0; i < nY; ++i, ++k)
            {
                for (j = x, l = 0; j < nX; ++j, ++l)
                {
                    rMIM0 = m_mapTiles[i, j];
                    rMIM1 = rAA[k, l];
                    if (catFlg == CatAndPasteSquareTiles.TARGET_LAYER_ALL)
                    {
                        rMIM0.L00._n = rMIM1.L00._n;
                        rMIM0.L01._n = rMIM1.L01._n;
                    }
                    else if (catFlg == CatAndPasteSquareTiles.TARGET_LAYER00)
                    {
                        if (pasteFlg == CatAndPasteSquareTiles.TARGET_LAYER00)
                        {
                            rMIM0.L00._n = rMIM1.L00._n;
                        }
                        else if (pasteFlg == CatAndPasteSquareTiles.TARGET_LAYER01)
                        {
                            rMIM0.L01._n = rMIM1.L00._n;
                        }
                    }
                    else if (catFlg == CatAndPasteSquareTiles.TARGET_LAYER01)
                    {
                        if (pasteFlg == CatAndPasteSquareTiles.TARGET_LAYER00)
                        {
                            rMIM0.L00._n = rMIM1.L01._n;
                        }
                        else if (pasteFlg == CatAndPasteSquareTiles.TARGET_LAYER01)
                        {
                            rMIM0.L01._n = rMIM1.L01._n;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 現行のタイルマップのサイズを変更した際のデータを取得する。現行のタイルマップの影響は無い
        /// </summary>
        /// <param name="tileCountX">X軸のタイル数</param>
        /// <param name="tileCountY">Y軸のタイル数</param>
        /// <param name="type">リサイズタイプ</param>
        /// <param name="aabb"></param>
        /// <param name="diffTileCountX">変更前のX軸のタイル群の差</param>
        /// <param name="diffTileCountY">変更前のY軸のタイル群の差</param>
        /// <returns>常にtrueを返す</returns>
        private bool ReSizeAABB(int tileCountX, int tileCountY, int type, BLOCK_AABB aabb, out int diffTileCountX, out int diffTileCountY)
        {
            int tmpW, tmpH;
            aabb.vMin.Set(0, 0);
            aabb.vMax.Set(m_tileCountX - 1, m_tileCountY - 1);

            diffTileCountX = tmpW = m_tileCountX - tileCountX;
            diffTileCountY = tmpH = m_tileCountY - tileCountY;
            switch (type)
            {
                // １行目
                case TOP_LEFT:    // 左上
                    aabb.vMin.x += tmpW;
                    aabb.vMin.y += tmpH;
                    break;
                case TOP_CENTER:    // 中央上
                    tmpW /= 2;
                    aabb.vMin.x += tmpW;
                    aabb.vMax.x -= tmpW;
                    aabb.vMin.y += tmpH;
                    break;
                case TOP_RIGHT:    // 右上
                    aabb.vMax.x -= tmpW;
                    aabb.vMin.y += tmpH;
                    break;
                // ２行目
                case CENTER_LEFT:
                    aabb.vMin.x += tmpW;
                    tmpH /= 2;
                    aabb.vMin.y += tmpH;
                    aabb.vMax.y -= tmpH;
                    break;
                case CENTER_CENTER:
                    tmpW /= 2;
                    aabb.vMin.x += tmpW;
                    aabb.vMax.x -= tmpW;
                    tmpH /= 2;
                    aabb.vMin.y += tmpH;
                    aabb.vMax.y -= tmpH;
                    break;
                case CENTER_RIGHT:
                    aabb.vMax.x -= tmpW;
                    tmpH /= 2;
                    aabb.vMin.y += tmpH;
                    aabb.vMax.y -= tmpH;
                    break;
                // ３行目
                case BOTTOM_LEFT:   // 左下
                    aabb.vMin.x += tmpW;
                    aabb.vMax.y -= tmpH;
                    break;
                case BOTTOM_CENTER:   // 中央下
                    aabb.vMax.y -= tmpH;
                    tmpW /= 2;
                    aabb.vMin.x += tmpW;
                    aabb.vMax.x -= tmpW;
                    break;
                case BOTTOM_RIGHT:   // 右下
                    aabb.vMax.x -= tmpW;
                    aabb.vMax.y -= tmpH;
                    break;
            }
            return true;
        }
        /// <summary>
        /// 現行のタイルマップのサイズを変更する
        /// </summary>
        /// <param name="tileCountX">X軸のタイル数</param>
        /// <param name="tileCountY">Y軸のタイル数</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Resize(int tileCountX, int tileCountY, int type)
        {
            int i, j, k, l, n, kk, ll;
            int tmpW = 0, tmpH = 0;
            FSquareTileInfoMap rMIM0 = null, rMIM1 = null;
            BLOCK_AABB aabbNow = new BLOCK_AABB();
            BLOCK_AABB aabb = new BLOCK_AABB();
            aabbNow.vMin.Set(0, 0);
            aabbNow.vMax.Set(m_tileCountX - 1, m_tileCountY - 1);
            this.ReSizeAABB(tileCountX, tileCountY, type, aabb, out tmpW, out tmpH);


            D2ArrayObject<FSquareTileInfoMap> newMapChip = new D2ArrayObject<FSquareTileInfoMap>(tileCountX, tileCountY);
            for (i = aabb.vMin.y, k = 0; i <= aabb.vMax.y; ++i, ++k)
            {
                for (j = aabb.vMin.x, l = 0; j <= aabb.vMax.x; ++j, ++l)
                {
                    if (i >= aabbNow.vMin.y && i <= aabbNow.vMax.y &&
                        j >= aabbNow.vMin.x && j <= aabbNow.vMax.x)
                    {
                        rMIM0 = newMapChip[k, l];
                        rMIM1 = m_mapTiles[i, j];
                        rMIM0.L00._n = rMIM1.L00._n;
                        rMIM0.L01._n = rMIM1.L01._n;
                    }
                }
            }
            m_mapTiles = newMapChip;
            m_tileCountX = (short)m_mapTiles.LengthX;
            m_tileCountY = (short)m_mapTiles.LengthY;

            List<String> aStrDel = new List<string>();
            List<String> aStrCat = new List<string>();
            BLOCK_AABB aabbR = new BLOCK_AABB();
            BLOCK_AABB aabbOut = new BLOCK_AABB();
            SquareTilesTransposeFrame rTCF;
            //aabb.vMin.Set(0, 0);
            //aabb.vMax.Set(m_tileNumX - 1, m_tileNumY - 1);
            foreach (KeyValuePair<string, SquareTilesTranspose> kvp in m_transPoseTiles)
            {
                aabbR.vMin.Set(kvp.Value.tilePosX, kvp.Value.tilePosY);
                aabbR.vMax.Set(aabbR.vMin);
                aabbR.vMax.x += kvp.Value.tileNumX - 1;
                aabbR.vMax.y += kvp.Value.tileNumY - 1;
                if (aabb.BooleanOperation_AND(aabbOut, aabbR))
                {
                    if (!aabbR.Equal(aabbOut))
                    {
                        kvp.Value.tileNumX = (short)aabbOut.GetWidthBlockCount();
                        kvp.Value.tileNumY = (short)aabbOut.GetHeightBlockCount();

                        // X
                        if (aabbOut.vMin.x <= aabb.vMin.x)
                        {
                            kvp.Value.tilePosX = 0;
                        }
                        else if (aabb.vMin.x < 0)
                        {
                            kvp.Value.tilePosX += (short)(Math.Abs(aabb.vMin.x));
                        }
                        // Y
                        if (aabbOut.vMin.y <= aabb.vMin.y)
                        {
                            kvp.Value.tilePosY = 0;
                        }
                        else if (aabb.vMin.y < 0)
                        {
                            kvp.Value.tilePosY += (short)(Math.Abs(aabb.vMin.y));
                        }
                        for (n = 0; n < kvp.Value.aTCF.Count; ++n)
                        {
                            newMapChip = new D2ArrayObject<FSquareTileInfoMap>(kvp.Value.tileNumX, kvp.Value.tileNumY);
                            rTCF = kvp.Value.aTCF[n];
                            kk = -1;
                            bool bRY = false;
                            for (i = aabbR.vMin.y, k = 0; i <= aabbR.vMax.y; ++i, ++k)
                            {
                                ll = 0;
                                if (i >= aabbOut.vMin.y && i <= aabbOut.vMax.y)
                                {
                                    kk++;
                                    bRY = true;
                                }
                                for (j = aabbR.vMin.x, l = 0; j <= aabbR.vMax.x; ++j, ++l)
                                {
                                    if (bRY && j >= aabbOut.vMin.x && j <= aabbOut.vMax.x)
                                    {
                                        rMIM0 = newMapChip[kk, ll++];
                                        rMIM1 = rTCF.aaMapChip[k, l];
                                        rMIM0.L00._n = rMIM1.L00._n;
                                        rMIM0.L01._n = rMIM1.L01._n;
                                    }
                                }
                            }
                            rTCF.aaMapChip = newMapChip;
                        }
                    }
                    else
                    {
                        // 
                        if (aabb.vMin.x < 0)
                        {
                            kvp.Value.tilePosX += (short)(Math.Abs(aabb.vMin.x));
                        }
                        // Y
                        if (aabb.vMin.y < 0)
                        {
                            kvp.Value.tilePosY += (short)(Math.Abs(aabb.vMin.y));
                        }
                    }
                }
                else
                {
                    // 削除する置き換え
                    aStrDel.Add(kvp.Key);
                }
            }// foreach

            for (i = 0; i < aStrDel.Count; ++i)
                m_transPoseTiles.Remove(aStrDel[i]);

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileCountX">X軸のタイル数</param>
        /// <param name="tileCountY">Y軸のタイル数</param>
        /// <param name="type"></param>
        /// <param name="outCount"></param>
        /// <param name="strOut"></param>
        /// <returns></returns>
        public bool CheckReplaceLap(int tileCountX, int tileCountY, int type, out int outCount, out string strOut)
        {
            outCount = -1;
            strOut = "";
            if (tileCountX >= m_tileCountX && tileCountY >= m_tileCountY)
            {
                return false;
            }
            int tmpW = 0, tmpH = 0;
            BLOCK_AABB aabb = new BLOCK_AABB();
            this.ReSizeAABB(tileCountX, tileCountY, type, aabb, out tmpW, out tmpH);

            outCount = 0;
            string strFrameOut = "", strFrameCat = "";
            BLOCK_AABB aabbR = new BLOCK_AABB();
            BLOCK_AABB aabbOut = new BLOCK_AABB();
            foreach (KeyValuePair<string, SquareTilesTranspose> kvp in m_transPoseTiles)
            {
                aabbR.vMin.Set(kvp.Value.tilePosX, kvp.Value.tilePosY);
                aabbR.vMax.Set(aabbR.vMin);
                aabbR.vMax.x += kvp.Value.tileNumX - 1;
                aabbR.vMax.y += kvp.Value.tileNumY - 1;
                if (aabb.BooleanOperation_AND(aabbOut, aabbR))
                {
                    if (!aabbR.Equal(aabbOut))
                    {
                        strFrameCat += "  " + kvp.Key + "\n";
                        ++outCount;
                    }
                }
                else
                {
                    strFrameOut += "  " + kvp.Key + "\n";
                    ++outCount;
                }
            }
            if (strFrameCat.Length > 0)
                strOut += "切り取られるのは\n" + strFrameCat;
            if (strFrameOut.Length > 0)
                strOut += "\n削除されるのは\n" + strFrameOut;

            return true;
        }






        //###########################################################################
        //##
        //## 置き換え関係
        //##
        //###########################################################################
        #region 置き換え関係
        public int ReplaseChipsCount
        {
            get
            {
                return m_transPoseTiles.Count;
            }

        }
        /// <summary>
        /// 置き換えのリファレンスを取得
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, SquareTilesTranspose> GetTransPoseChipsReference()
        {
            return m_transPoseTiles;
        }
        /// <summary>
        /// 新規追加
        /// </summary>
        /// <param name="name">置き換え名</param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="tileCountX">X軸のタイル数</param>
        /// <param name="tileCountY">Y軸のタイル数</param>
        /// <param name="isCopy"></param>
        /// <returns>登録されている名前があるとfalseを返す</returns>
        public bool TransPoseTilesAdd(string name, int startX, int startY, int tileCountX, int tileCountY, bool isCopy)
        {
            if (m_transPoseTiles.ContainsKey(name))
                return false;

            SquareTilesTranspose tmp = new SquareTilesTranspose();
            tmp.tilePosX = (short)startX;
            tmp.tilePosY = (short)startY;
            tmp.tileNumX = (short)tileCountX;
            tmp.tileNumY = (short)tileCountY;
            tmp.SetString(name);
            if (isCopy)
                tmp.CopyAdd(m_mapTiles);
            else
                tmp.NewAdd();

            m_transPoseTiles.Add(name, tmp);
            return true;
        }
        /// <summary>
        /// 置き換え名からSquareTilesTransposeリファレンスを取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SquareTilesTranspose FindSquareTilesTransposeFromName(string name)
        {
            if (m_transPoseTiles.ContainsKey(name))
                return m_transPoseTiles[name];

            return null;
        }
        /// <summary>
        /// リストビューに項目をセットする
        /// </summary>
        /// <param name="rL"></param>
        public void SetTransPoseListViewItems(ListView rL)
        {
            ListViewItem listVI;
            rL.Items.Clear();
            foreach (KeyValuePair<string, SquareTilesTranspose> kvp in m_transPoseTiles)
            {
                listVI = new ListViewItem();
                listVI.Text = kvp.Key;
                listVI.Name = kvp.Key;
                rL.Items.Add(listVI);
            }
        }
        /// <summary>
        /// 置き換え名からSquareTilesTransposeを削除する
        /// </summary>
        /// <param name="name">削除に成功した場合trueを返す。</param>
        /// <returns></returns>
        public bool DeleteTransPose(string name)
        {
            if (m_transPoseTiles.ContainsKey(name))
            {
                m_transPoseTiles.Remove(name);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 置き換えタイル群数
        /// </summary>
        public int TransPoseCount
        {
            get
            {
                return m_transPoseTiles.Count;
            }
        }
        #endregion






        //###########################################################################
        //##
        //## 読み込み
        //##
        //###########################################################################
        #region 読み込み
        /// <summary>
        /// スクエアタイルマップをファイルから読み込み開始する
        /// </summary>
        /// <param name="rMap"></param>
        /// <param name="lChank"></param>
        /// <returns></returns>
        internal bool FF_LoadStageFile(ref FSquareTileMapHeader rMap, LoadChank lChank)
        {
            FSquareTileInfoMap rMIM;
            LoadChank lChank02;
            LoadData lData;
            Chunk chank = new Chunk();
            int i, j;


            // 名前
            m_mapName = rMap.GetString();

            //
            m_tileCountX = rMap.tileNumX;	// X軸のマップチップ数
            m_tileCountY = rMap.tileNumY;	// Y軸のマップチップ数
            //m_nChipNum	 = m_tileNumX*m_tileNumY;			// チップ数の合計

            // サイズ
            //m_aabbSize.vMin.Init();
            //m_aabbSize.vMax.x =   (float)m_tileNumX * STAGE_CHIPSIZE;
            //m_aabbSize.vMin.y = -((float)m_tileNumY * STAGE_CHIPSIZE);

            m_mapTiles = new D2ArrayObject<FSquareTileInfoMap>(m_tileCountX, m_tileCountY);

            if (lChank != null)
            {

                lChank.GetBlockChank(chank);
                // チャンクブロックチェックしない
                // IDから処理選択
                while ((lData = lChank.FindChank(chank)) != null)
                {

                    if (chank.id == FSquareTileInfoMap.ID)
                    {
                        //----------------------------
                        // MAP上で使う一つのマップチップ
                        //----------------------------
                        FSquareTileInfoMap mapChip = new FSquareTileInfoMap();
                        // チェック
                        if (chank.num == 0)
                        {
                            throw new IOException("チャンク数が以上です。");
                        }
                        m_allTileCount = 0;
                        while ((lChank02 = lData.FindData(mapChip)) != null || !lData.GetFindEnd())
                        {
                            // データ内部部分書き込み
                            i = m_allTileCount / m_tileCountX;
                            j = m_allTileCount % m_tileCountX;
                            rMIM = m_mapTiles[i, j];
                            rMIM.L00._n = mapChip.L00._n;
                            rMIM.L01._n = mapChip.L01._n;
                            // 登録
                            //if ( (rMIM.L00.flags & FSquareTileInfoMap.ANMCHIP) != 0 )
                            //    rAGM.RegistrationChipAnimation(rMIM.L00.anmGroupNo, rMIM.L00.anmTileNo);
                            //if ( (rMIM.L01.flags & FSquareTileInfoMap.ANMCHIP) != 0 )
                            //    rAGM.RegistrationChipAnimation(rMIM.L01.anmGroupNo, rMIM.L01.anmTileNo);
                            // これ以上入れ子は今のところない予定なので入れ子は無視
                            if (lChank02 != null)
                            {
                                throw new IOException("入れ子");
                            }
                            ++m_allTileCount;
                        }
                        break;
                    }
                    else if (chank.id == FSquareTileMapRect.ID)
                    {   //----------------------------
                        // イベントなどに使用する長方形
                        //----------------------------
                        FSquareTileMapRect eRect = new FSquareTileMapRect();
                        while ((lChank02 = lData.FindData(eRect)) != null || !lData.GetFindEnd())
                        {
                            // これ以上入れ子は今のところない予定なので入れ子は無視
                            if (lChank02 != null)
                            {
                                lChank02 = null;
                                throw new IOException("存在してはいけない入れ子を発見");
                            }
                            m_eventRect.Add(eRect.GetString(), eRect);
                            eRect = new FSquareTileMapRect();
                        }
                        break;
                    }
                    else if (chank.id == FSquareTilesTransposeHeader.ID)
                    {   //----------------------------
                        // 置き換えるチップの集まり
                        //----------------------------
                        SquareTilesTranspose stTC = new SquareTilesTranspose();

                        // チェック
                        if (chank.num == 0)
                        {
                            throw new IOException("FSquareTilesTransposeHeader チャンク数が異常です。");
                        }
                        while ((lChank02 = lData.FindData(stTC)) != null || !lData.GetFindEnd())
                        {
                            m_transPoseTiles.Add(stTC.GetString(), stTC);
                            // AABB
                            //m_aabbSize.vMin.Init();
                            //m_aabbSize.vMax.x =   (float)m_tileNumX * STAGE_CHIPSIZE;
                            //m_aabbSize.vMin.y = -((float)m_tileNumY * STAGE_CHIPSIZE);

                            //stTC.aabb.vMin.x = (float)(tChip.tilePosX*STAGE_CHIPSIZE);
                            //stTC.aabb.vMax.x = stTC.aabb.vMin.x+(float)(tChip.tileNumX*STAGE_CHIPSIZE);
                            //stTC.aabb.vMax.y = -(float)(tChip.tilePosY*STAGE_CHIPSIZE);
                            //stTC.aabb.vMin.y = -(float)(tChip.tileNumY*STAGE_CHIPSIZE)+stTC.aabb.vMax.y;
                            if (lChank02 != null)
                            {
                                if (!this.FF_LoadStageFileTC(ref stTC, lChank02))
                                {
                                    throw new IOException("FSquareTilesTransposeHeader が異常です。");
                                }
                                lChank02 = null;
                            }
                            stTC = new SquareTilesTranspose();
                        }
                        break;
                    }
                    else
                    {
                        // FFID_MAPCHIP_INFO以外のIDが今現在存在しないで
                        // 怪しい値を見つけたらエラーをはき出すようにする
                        throw new IOException("未対応のIDが呼ばれた。");
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 置き換えデータの読み込み
        /// </summary>
        /// <param name="rTC"></param>
        /// <param name="lChank"></param>
        /// <returns></returns>
        private bool FF_LoadStageFileTC(ref SquareTilesTranspose rTC, LoadChank lChank)
        {
            LoadChank lChank02;
            LoadData lData;
            Chunk chank = new Chunk();
            int nCnt;

            if (lChank != null)
            {
                lChank.GetBlockChank(chank);
                // チャンクブロックチェックしない
                // IDから処理選択
                while ((lData = lChank.FindChank(chank)) != null)
                {
                    if (chank.id == FSquareTilesTransposeFrame.ID)
                    {
                        SquareTilesTransposeFrame stTCF = new SquareTilesTransposeFrame();
                        // チェック
                        if (chank.num == 0)
                        {
                            throw new IOException("FSquareTilesTransposeHeader チャンク数が異常です。");
                        }

                        nCnt = 0;
                        while ((lChank02 = lData.FindData(stTCF)) != null || !lData.GetFindEnd())
                        {
                            rTC.Add(stTCF);
                            if (lChank02 != null)
                            {
                                if (!this.FF_LoadStageFileTCF(ref rTC, ref stTCF, lChank02))
                                {
                                    return false;
                                }
                            }
                            lChank02 = null;
                            ++nCnt;
                            stTCF = new SquareTilesTransposeFrame();
                        }
                        break;
                    }
                    else
                    {
                        // FFID_MAPCHIP_INFO以外のIDが今現在存在しないで
                        // 怪しい値を見つけたらエラーをはき出すようにする
                        throw new IOException("未対応のIDが呼ばれた。");
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 置き換えタイル群の1フレーム読み込み
        /// </summary>
        /// <param name="rTC"></param>
        /// <param name="rTCF"></param>
        /// <param name="lChank"></param>
        /// <returns></returns>
        private bool FF_LoadStageFileTCF(ref SquareTilesTranspose rTC, ref SquareTilesTransposeFrame rTCF, LoadChank lChank)
        {
            FSquareTileInfoMap ffMIM;
            LoadChank lChank02;
            LoadData lData;
            Chunk chank = new Chunk();
            int cnt;
            int i, j;

            if (lChank != null)
            {
                lChank.GetBlockChank(chank);
                // チャンクブロックチェックしない
                // IDから処理選択
                while ((lData = lChank.FindChank(chank)) != null)
                {
                    if (chank.id == FSquareTileInfoMap.ID)
                    {
                        ffMIM = new FSquareTileInfoMap();
                        // チェック
                        if (chank.num == 0)
                        {
                            throw new IOException("FSquareTilesTransposeHeader チャンク数が異常です。");
                        }
                        rTCF.aaMapChip = new D2ArrayObject<FSquareTileInfoMap>(rTC.tileNumX, rTC.tileNumY);
                        cnt = 0;
                        i = j = 0;
                        while ((lChank02 = lData.FindData(rTCF.aaMapChip[i, j])) != null || !lData.GetFindEnd())
                        {
                            if (++cnt >= chank.num) break;
                            i = cnt / rTC.tileNumX;
                            j = cnt % rTC.tileNumX;
                        }
                        break;
                    }
                    else
                    {
                        // 上記以外のIDが今現在存在しないで
                        // 怪しい値を見つけたらエラーをはき出すようにする
                        throw new IOException("未対応のIDが呼ばれた。");
                    }
                }
            }
            return true;
        }

        #endregion
        //bool StartAnimation(const TCHAR *tszName);
        //HRESULT PreUpdate(float fElapsedTime );
        //bool ChangeMapChipCollisionChecku(const MCVECTOR2& rMovePos, const MC_AABB2D& rTagetAABB);
        //int MapChipDraw(
        //    const VP_IMAGEFILE &rvpImgFile,
        //    const MC_AABB2D* pBOA,
        //    int nX, int nY
        //);

        //###########################################################################
        //##
        //## 書き込み
        //##
        //###########################################################################
        #region 書き込み
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objSave"></param>
        /// <returns></returns>
        internal bool FF_WriteStageFile(SaveState objSave)
        {
            int i, j, k;
            //----------------
            // マップ
            //----------------
            FSquareTileMapHeader map = new FSquareTileMapHeader();
            // マップ
            map.SetString(m_mapName);
            map.tileNumX = m_tileCountX;
            map.tileNumY = m_tileCountY;

            objSave.Write(map);
            //----------------
            // 一つのマップチップ
            //----------------
            objSave.BlockStart(FSquareTileInfoMap.ID);
            {
                for (i = 0; i < m_mapTiles.LengthY; ++i)
                {
                    for (j = 0; j < m_mapTiles.LengthX; ++j)
                    {
                        objSave.Write(m_mapTiles[i, j]);
                    }
                }
            }
            objSave.BlockEnd();

            //----------------
            // イベントRECT
            //----------------
            if (m_eventRect.Count > 0)
            {
                FSquareTileMapRect eRect = new FSquareTileMapRect();
                objSave.BlockStart(FSquareTileMapRect.ID);
                {
                    foreach (KeyValuePair<string, FSquareTileMapRect> kvp in m_eventRect)
                    {
                        objSave.Write(kvp.Value);
                    }
                }
                objSave.BlockEnd();
            }
            //----------------
            // マップ置き換え
            //----------------
            if (m_transPoseTiles.Count > 0)
            {
                objSave.BlockStart(FSquareTilesTransposeHeader.ID);
                {

                    foreach (KeyValuePair<string, SquareTilesTranspose> kvp in m_transPoseTiles)
                    {
                        objSave.Write(kvp.Value);
                        objSave.BlockStart(FSquareTilesTransposeFrame.ID);
                        {
                            for (i = 0; i < kvp.Value.aTCF.Count; ++i)
                            {
                                objSave.Write(kvp.Value.aTCF[i]);
                                // マップチップデータ
                                objSave.BlockStart(FSquareTileInfoMap.ID);
                                {
                                    for (j = 0; j < kvp.Value.aTCF[i].aaMapChip.LengthY; ++j)
                                    {
                                        for (k = 0; k < kvp.Value.aTCF[i].aaMapChip.LengthX; ++k)
                                        {
                                            objSave.Write(kvp.Value.aTCF[i].aaMapChip[j, k]);
                                        }
                                    }
                                }
                                objSave.BlockEnd();
                            }
                        }
                        objSave.BlockEnd();
                    }
                }
                objSave.BlockEnd();
            }
            return true;
        }
        #endregion
    }
}
