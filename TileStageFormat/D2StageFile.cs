using MC2DUtil.ChunkFormat;
using System;
using TileStageFormat.Tile.Square;
using TileStageFormat.Tile;
using System.Windows.Forms;
using System.Windows;

namespace TileStageFormat
{

    /// <summary>
    /// 
    /// </summary>
    public partial class D2StageFile
    {
        private string m_stgPath = @"\"+ MediaDir.Media + @"\d2stg\2DStage.stg";
        /// <summary>ファイルオープンフラグ</summary>
        private bool m_isOpenFile = false;
        /// <summary>ファイルの相対パス</summary>
        private string m_filePath;

        /// <summary>
        /// ファイルが正常に開かれ、読み込まれたか？
        /// </summary>
        public bool IsOpenFile { get { return m_isOpenFile; } }
        /// <summary>ファイルの相対パス</summary>
        public string FilePath { get { return m_filePath; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D2StageFile()
        {

        }
        /// <summary>
        /// 内容をクリアする
        /// </summary>
        public void Clear()
        {
            SquareTilesClear();
            RectClear();
            m_isOpenFile = false;
        }


        //##########################################################################
        //##########################################################################
        //##
        //## 読み込み処理
        //##
        //##########################################################################
        //##########################################################################
        #region ファイルを読み込む
        /// <summary>
        /// ファイルを読み込む
        /// </summary>
        /// <param name="dirPath">MC2D実行ファイルがあるディレクトリパス</param>
        /// <returns>正常に読み込まれればtrueを返す</returns>
        public bool OpenFile(string dirPath)
        {
            LoadState objLoad = new LoadState();
            Header header = new Header();
            Chunk chank = new Chunk();
            LoadChank lChank;
            LoadData lData;
            // FImageSquareTile ImageFile;
            //FSquareTileInfo MapChipInfo;
            try
            {
                Clear();

                m_filePath = dirPath + m_stgPath;
                objLoad.InitSetting(dirPath);
                if ((lChank = objLoad.Start()) != null)
                {
                    // ヘッダー読み込み
                    objLoad.GetHeader(header);
                    // ファーストブロックチャンク
                    lChank.GetBlockChank(chank);
                    while ((lData = lChank.FindChank(chank)) != null)
                    {
                        if (chank.id == FImageSquareTile.ID)
                        {
                            //----------------------------
                            // スクエア・タイルイメージファイル読み込み
                            //----------------------------
                            SquareTileRead(lData);
                        }
                        else if (chank.id == FImageHexagonTile.ID)
                        {
                            //----------------------------
                            // ヘキサゴン・イメージファイル読み込み
                            //----------------------------
                        }
                        else if (chank.id == FImageIsometricTile.ID)
                        {
                            //----------------------------
                            // アイソメトリック・メージファイル読み込み
                            //----------------------------
                        }
                        else if (chank.id == FImageRect.ID)
                        {
                            //----------------------------
                            // RECTイメージファイル読み込み
                            //----------------------------
                            RectRead(lData);
                        }
                        //else if (chank.id == FAnmSquareTileHeader.ID)
                        //{
                        //    //----------------------------
                        //    // アニメーションチップ読み込み
                        //    //----------------------------
                        //    this.ReadMapChipAnim(lData);
                        //}
                        //else if (chank.id == FSquareTileMapHeader.ID)
                        //{
                        //    //----------------------------
                        //    // マップ
                        //    //----------------------------
                        //    this.ReadMapData(lData);
                        //}
                        //else if( chank.id == FAnimationRect.ID)
                        //{
                        //    //----------------------------
                        //    // アニメーションRECT
                        //    //----------------------------
                        //    this.ReadAnimationRect(lData);
                        //}
                        //else if (chank.id == FF_BACKGROUND.ID)
                        //{
                        //    //----------------------------
                        //    // 背景、全景など
                        //    //----------------------------
                        //    this.ReadBacground(lData);
                        //}
                        //else if (chank.id == FImageRectMap.ID)
                        //{
                        //    //----------------------------
                        //    // イメージRECTマップ
                        //    //----------------------------
                        //    this.ReadImageRECT_MAPData(lData);
                        //}
                        lData = null;
                    }
                    lChank = null;
                }
                objLoad.End();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "ファイル読み込みエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            m_isOpenFile = true;
            return true;
        }




        ///// <summary>
        ///// 　マップファイルデーター
        ///// </summary>
        ///// <param name="lData"></param>
        //private bool ReadMapData(LoadData lData)
        //{
        //    FSquareTileMapHeader map = new FSquareTileMapHeader();

        //    LoadChank lChank;
        //    Chunk chank = new Chunk();
        //    TreeNode tNode;

        //    lData.GetChank(chank);
        //    while ((lChank = lData.FindData(map)) != null || !lData.GetFindEnd())
        //    {
        //        tNode = m_rTV.Nodes[0].Nodes[4].Nodes.Add(map.GetString());
        //        tNode.ImageIndex = 8;
        //        tNode.SelectedImageIndex = 9;
        //        tNode.Name = TV_NAME_MAP;

        //        if (lChank != null)
        //        {
        //            lChank.GetBlockChank(chank);
        //            SquareTilesMap stgMap = new SquareTilesMap();
        //            stgMap.FF_LoadStageFile(ref map, lChank);
        //            m_mapMap.Add(map.GetString(), stgMap);
        //            lChank = null;
        //        }
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 背景読み込み
        ///// </summary>
        ///// <param name="lData"></param>
        ///// <returns></returns>
        //private bool ReadBacground(LoadData lData)
        //{
        //    LoadChank lChank;
        //    Chunk chank = new Chunk();
        //    CStageBackground objBG = null;
        //    FF_BACKGROUND bg = new FF_BACKGROUND();


        //    lData.GetChank(chank);
        //    while ((lChank = lData.FindData(bg)) != null || !lData.GetFindEnd())
        //    {
        //        if (lChank != null)
        //        {
        //            objBG = new CStageBackground();
        //            objBG.FF_LoadStageFile(bg.GetString(), m_imagesSquare, bg, lChank, 0);
        //            m_mapBG.Add(bg.GetString(), objBG);
        //        }
        //    }
        //    return true;
        //}


        #endregion
        //##########################################################################
        //##########################################################################
        //##
        //## 書き込み処理
        //##
        //##########################################################################
        //##########################################################################
        #region ファイルを書き込む
        /// <summary>
        /// ファイルを書き込む
        /// </summary>
        /// <param name="dirPath">MC2D実行ファイルがあるディレクトリパス</param>
        /// <returns>正常に書き込まれればtrueを返す</returns>
        public bool WriteFile(string dirPath)
        {
            SaveState objSave = new SaveState();
            Header header = new Header();
            FImageSquareTile ImageFile = new FImageSquareTile();
            //FSquareTileInfo MapChipInfo;
            try
            {
                m_filePath = dirPath + m_stgPath;
                objSave.InitSetting(m_filePath, 1000);
                if (objSave.Start())
                {
                    //-------------------------------------------
                    // スクエアイメージファイルチャンクの書き込み
                    //-------------------------------------------
                    SquareTileWrite(objSave);
                    //-------------------------------------------
                    // Rectイメージファイルチャンクの書き込み
                    //-------------------------------------------
                    RectWrite(objSave);

                    ////-------------------------------------------
                    //// アニメーションRECTの書き込み
                    ////-------------------------------------------
                    //if (m_vAnimaRECT.Count != 0)
                    //{
                    //    this.WriteAnimationRect(objSave);
                    //}
                    ////-------------------------------------------
                    //// マップの書き込み
                    ////-------------------------------------------
                    //if (m_mapMap.Count != 0)
                    //{
                    //    this.WriteMap(objSave);
                    //}
                    ////-------------------------------------------
                    //// 背景の書き込み
                    ////-------------------------------------------
                    ////if (m_mapBG.Count != 0)
                    ////{
                    ////    this.WriteBG(objSave);
                    ////}
                    ////-------------------------------------------
                    //// イメージRECTマップ書き込み
                    ////-------------------------------------------
                    //if (m_mapImgRectMap.Count != 0)
                    //{
                    //    this.WriteImageRECT_MAP(objSave);
                    //}
                }
                objSave.End();
                m_isOpenFile = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception caught",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button3
                );
                return false;
            }
            return true;
        }



        ///// <summary>
        ///// マップを書き込む
        ///// </summary>
        ///// <param name="objSave"></param>
        ///// <returns></returns>
        //private bool WriteMap(SaveState objSave)
        //{
        //    objSave.BlockStart(FSquareTileMapHeader.ID);
        //    {
        //        foreach (KeyValuePair<string, SquareTilesMap> kvp in m_mapMap)
        //        {
        //            kvp.Value.FF_WriteStageFile(objSave);
        //        }
        //    }
        //    objSave.BlockEnd();
        //    return true;
        //}

        ///// <summary>
        ///// アニメーションRECTを書き込む
        ///// </summary>
        ///// <param name="objSave"></param>
        ///// <returns></returns>
        //private bool WriteAnimationRect(SaveState objSave)
        //{
        //    int i, j;
        //    // 背景、全景で一枚絵から切り取るデータ
        //    objSave.BlockStart(FAnimationRect.ID);
        //    {
        //        for (i = 0; i < m_vAnimaRECT.Count; ++i)
        //        {
        //            objSave.Write(m_vAnimaRECT[i]);
        //            for (j = 0; j < m_vAnimaRECT[i].aARF.Count; ++j)
        //            {
        //                objSave.BlockStart(FAnimationRectFrame.ID, FAnimationRectFrame.VERSION);
        //                {
        //                    objSave.Write(m_vAnimaRECT[i].aARF[j]);
        //                }
        //                objSave.BlockEnd();
        //            }
        //        }
        //    }
        //    objSave.BlockEnd();
        //    return true;
        //}

        ///// <summary>
        ///// 背景を書き込む
        ///// </summary>
        ///// <param name="objSave"></param>
        ///// <returns></returns>
        //private bool WriteBG(SaveState objSave)
        //{
        //    objSave.BlockStart(FF_BACKGROUND.ID);
        //    {
        //        foreach (KeyValuePair<string, CStageBackground> kvp in m_mapBG)
        //        {
        //            kvp.Value.FF_WriteStageFile(objSave);
        //        }
        //    }
        //    objSave.BlockEnd();

        //    return true;
        //} 
        #endregion
    }
}
