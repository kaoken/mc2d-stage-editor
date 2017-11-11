using MC2DUtil.ChunkFormat;
using System;
using System.Collections.Generic;
using System.IO;
using TileStageFormat.Tile.Square;
using TileStageFormat.Events;

namespace TileStageFormat
{
    public partial class D2StageFile
    {
        /// <summary>
        /// スクエア・タイル画像を呼び終えた後、呼び出すイベント
        /// </summary>
        private event ReadedSquareImageHandler ReadedSquareImageEventCall;



        /// <summary>イメージNoからスクエアタイル画像ファイル名でのインデックス</summary>
        private List<string> m_squareTileIndexs = new List<string>();
        /// <summary>イメージファイル名からスクエアタイルを取得する辞書</summary>
        private SortedDictionary<string, ImageSquareTile> m_squareTileFileDic = new SortedDictionary<string, ImageSquareTile>(StringComparer.CurrentCultureIgnoreCase);
        /// <summary>整数ポインタからスクエアタイルを取得する辞書</summary>
        private Dictionary<IntPtr, ImageSquareTile> m_ptrSquareTileImageFileDic = new Dictionary<IntPtr, ImageSquareTile>();


        /// <summary>スクエアアニメーションチップリスト</summary>
        private List<AnmSquareTile> m_animationTile = new List<AnmSquareTile>();



        /// <summary>
        /// クリア
        /// </summary>
        internal void SquareTilesClear()
        {
            m_squareTileIndexs.Clear();
            m_squareTileFileDic.Clear();
            m_ptrSquareTileImageFileDic.Clear();
            m_animationTile.Clear();
        }

        /// <summary>
        /// ファイル名からスクエアタイルデータを取得する
        /// </summary>
        /// <param name="filePath">相対ファイルパス名</param>
        /// <returns></returns>
        public ImageSquareTile FindSquareTileFromFilePaht(string filePath)
        {
            if (m_squareTileFileDic.ContainsKey(filePath)) return m_squareTileFileDic[filePath];
            return null;
        }
        /// <summary>
        /// インデックスからスクエアタイルデータを取得する
        /// </summary>
        /// <param name="idx">スクエアタイルイメージ番号</param>
        /// <returns></returns>
        public ImageSquareTile FindSquareTileFromIndex(int idx)
        {
            return FindSquareTileFromFilePaht(m_squareTileIndexs[idx]);
        }

        #region アニメーションチップ情報取得
        public bool IsChipAnimation()
        {
            return m_animationTile.Count != 0;
        }
        public AnmSquareTile GetChipAnmReference(int idx)
        {
            return m_animationTile[idx];
        }
        public void ChipAnmAdd(AnmSquareTile rMCA)
        {
            m_animationTile.Add(rMCA);
        }
        public int GetChipAniationsCount()
        {
            return m_animationTile.Count;
        }
        #endregion


        //##############################################################################
        //##############################################################################
        //##
        //##  読み込み
        //##
        //##############################################################################
        //##############################################################################
        #region 読み込み
        /// <summary>
        /// スクエア・イメージファイルデータ
        /// </summary>
        /// <param name="lData"></param>
        internal bool SquareTileRead(LoadData lData)
        {
            FSquareTileInfo info = null;
            Chunk chank = new Chunk();
            ImageSquareTile inImgFile;
            LoadChank lChank, lChank02;
            LoadData lData02;
            int imgNo = 0;

            lData.GetChank(chank);
            inImgFile = new ImageSquareTile();
            while ((lChank = lData.FindData(inImgFile)) != null || !lData.GetFindEnd())
            {
                // ツリービューに反映
                //tNode = m_rTV.Nodes[0].Nodes[0].Nodes.Add(inImgFile.GetString());
                //tNode.ImageIndex = 2;
                //tNode.SelectedImageIndex = 3;
                //tNode.Name = TV_NAME_MAPCHIP_IMG;

                //-------------------------------------
                // マップチップ情報読み込み
                //-------------------------------------
                if (lChank != null)
                {
                    while ((lData02 = lChank.FindChank(chank)) != null)
                    {   // 通常1回のループのみ
                        //-------------------------------
                        // データが正しいかチェックする

                        info = new FSquareTileInfo();
                        while ((lChank02 = lData02.FindData(info)) != null || !lData02.GetFindEnd())
                        {
                            // データ内部部分書き込み
                            inImgFile.tileInfos.Add(info);
                            // これ以上入れ子は今のところない予定なので入れ子は無視
                            if (lChank02 != null)
                            {
                                Chunk chank2 = new Chunk();
                                lChank02.GetBlockChank(chank2);
                                // IDから処理選択
                                while ((lData02 = lChank02.FindChank(chank2)) != null)
                                {
                                    if (chank2.id == FAnmSquareTileHeader.ID)
                                    {
                                        ReadAnimationTile(lData02);
                                    }
                                    else
                                    {
                                        // AnmSquareTile.ID 以外のIDが今現在存在しないで
                                        // 怪しい値を見つけたらエラーをはき出すようにする
                                        throw new IOException("未対応のIDが呼ばれた。");
                                    }
                                }
                                lChank02 = null;
                            }
                            info = new FSquareTileInfo();
                        }
                        lData02 = null;
                    }
                }
                ReadedSquareImageEventCall(this, new ReadedSquareImageEvent(inImgFile));

                m_squareTileIndexs.Add(inImgFile.GetString());
                lChank = null;
                inImgFile = new ImageSquareTile();
                ++imgNo;
            }
            inImgFile = null;
            return true;
        }
        /// <summary>
        /// アニメーションチップ読み込み
        /// </summary>
        /// <param name="lData"></param>
        /// <returns></returns>
        private bool ReadAnimationTile(LoadData lData)
        {
            AnmSquareTileFrame frame = null;
            AnmSquareTile tlMChipAnm = new AnmSquareTile();
            LoadChank lChank, lChank02;
            LoadData lData02;
            Chunk chank = new Chunk();
            int anmTileNo;


            lData.GetChank(chank);
            anmTileNo = 0;
            while ((lChank = lData.FindData(tlMChipAnm)) != null || !lData.GetFindEnd())
            {
                if (lChank != null)
                {
                    // チャンクブロック
                    lChank.GetBlockChank(chank);
                    // IDから処理選択
                    while ((lData02 = lChank.FindChank(chank)) != null)
                    {
                        if (chank.id == FAnmSquareTileFrame.ID)
                        {
                            //----------------------------
                            // アニメーションチップフレーム読み込み
                            //----------------------------
                            frame = new AnmSquareTileFrame();
                            while ((lChank02 = lData02.FindData(frame)) != null || !lData02.GetFindEnd())
                            {
                                tlMChipAnm.Add(frame);
                                // これ以上入れ子は今のところない予定なので入れ子は無視
                                if (lChank02 != null)
                                {
                                    lChank02 = null;
                                    throw new IOException("存在してはいけない入れ子を発見");
                                }
                                frame = new AnmSquareTileFrame();
                            }
                            frame = null;
                            lData02 = null;
                            break;
                        }
                        else
                        {
                            // FFID_MAPCHIP_INFO以外のIDが今現在存在しないで
                            // 怪しい値を見つけたらエラーをはき出すようにする
                            throw new IOException("未対応のIDが呼ばれた。");
                        }
                    }
                    lChank = null;
                }
                m_animationTile.Add(tlMChipAnm);
                tlMChipAnm = new AnmSquareTile();
                ++anmTileNo;
            }
            return true;
        }
        #endregion





        //##############################################################################
        //##############################################################################
        //##
        //##  書き込み
        //##
        //##############################################################################
        //##############################################################################
        #region 書き込み
        /// <summary>
        /// 書き込み開始
        /// </summary>
        /// <param name="objSave"></param>
        internal void SquareTileWrite(SaveState objSave)
        {
            //-------------------------------------------
            // スクエアイメージファイルチャンクの書き込み
            //-------------------------------------------
            if (m_ptrSquareTileImageFileDic.Count != 0)
            {
                //-----------------------------
                // FImageSquareTile
                objSave.BlockStart(FImageSquareTile.ID);
                {
                    foreach (ImageSquareTile valu in m_ptrSquareTileImageFileDic.Values)
                    {
                        // イメージ番号[0]:マップチップ用のイメージ
                        objSave.Write(valu);
                        if (valu.GetID() == FImageSquareTile.ID && valu.tileInfos.Count != 0)
                        {
                            //-----------------------------
                            // FSquareTileInfo
                            objSave.BlockStart(FSquareTileInfo.ID);
                            {
                                for (int i = 0; i < valu.tileInfos.Count; ++i)
                                {
                                    objSave.Write(valu.tileInfos[i]);
                                }
                            }
                            objSave.BlockEnd();
                        }
                    }
                }
                objSave.BlockEnd();
            }
        }
        /// <summary>
        /// アニメーションチップを書き込む
        /// </summary>
        /// <param name="objSave"></param>
        private bool WriteAnimationChip(SaveState objSave)
        {
            int i, j;
            //----------------
            // アニメーションチップ
            //----------------
            objSave.BlockStart(FAnmSquareTileHeader.ID);
            {
                for (i = 0; i < m_animationTile.Count; ++i)
                {
                    objSave.Write(m_animationTile[i]);
                    objSave.BlockStart(FAnmSquareTileFrame.ID);
                    {
                        for (j = 0; j < m_animationTile[i].ChipCount; ++j)
                        {
                            objSave.Write(m_animationTile[i].aTile[j]);
                        }
                    }
                    objSave.BlockEnd();
                }
            }
            objSave.BlockEnd();
            return true;
        }
        #endregion
    }

}
