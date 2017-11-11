using MC2DUtil.ChunkFormat;
using System;
using System.Collections.Generic;
using TileStageFormat.Animation.Rect;
using TileStageFormat.Events;
using TileStageFormat.Tile;
using TileStageFormat.Tile.Rect;

namespace TileStageFormat
{
    public partial class D2StageFile
    {
        /// <summary>
        /// RECT画像を呼び終えた後、呼び出すイベント
        /// </summary>
        public event ReadedRectImageHandler ReadedRectImageEventCall;





        /// <summary>イメージファイル名からRECTを取得する辞書</summary>
        protected SortedDictionary<string, ImageRect> m_rectFileDic = new SortedDictionary<string, ImageRect>(StringComparer.CurrentCultureIgnoreCase);
        /// <summary>整数ポインタからイメージファイル名からRECTを取得する辞書を取得する辞書</summary>
        protected Dictionary<IntPtr, ImageRect> m_ptrRectImageFileDic = new Dictionary<IntPtr, ImageRect>();


        /// <summary>イメージNoからRECT画像ファイル名でのインデックス</summary>
        private List<string> m_rectIndexs = new List<string>();
        private List<AnimationRect> m_vAnimaRECT = new List<AnimationRect>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public AnimationRect GetAnimationRect(int idx)
        {
            return m_vAnimaRECT[idx];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<AnimationRect> GetAnimationRects()
        {
            return m_vAnimaRECT;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="imgRectNo"></param>
        ///// <param name="rectNo"></param>
        ///// <returns></returns>
        //public Microsoft.Xna.Framework.Rectangle GetImgRect(int imgRectNo, int rectNo)
        //{
        //    return FindRectFromIndex(imgRectNo).aSprite[rectNo].GetXRect();
        //}

        /// <summary>
        /// クリア
        /// </summary>
        internal void RectClear()
        {
            m_rectFileDic.Clear();
            m_ptrRectImageFileDic.Clear();
            m_rectIndexs.Clear();
        }



        /// <summary>
        /// ファイル名からRECTデータを取得する
        /// </summary>
        /// <param name="filePath">相対ファイルパス名</param>
        /// <returns></returns>
        public ImageRect FindRectFromFilePaht(string filePath)
        {
            if (m_rectFileDic.ContainsKey(filePath)) return m_rectFileDic[filePath];
            return null;
        }
        /// <summary>
        /// インデックスからRECTデータを取得する
        /// </summary>
        /// <param name="idx">RECTイメージ番号</param>
        /// <returns></returns>
        public ImageRect FindRectFromIndex(int idx)
        {
            return FindRectFromFilePaht(m_rectIndexs[idx]);
        }
        /// <summary>
        /// Rectイメージ番号から相対ファイルパス名を取得する。
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public string RectFilePathFromIndex(int idx)
        {
            return m_rectIndexs[idx];
        }
        /// <summary>
        /// ファイルパス名からイメージ番号を取得する
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int RectImageNoFromFilePath(string filePath)
        {
            for(int i=0;i< m_rectIndexs.Count;++i)
            {
                if (m_rectIndexs[i] == filePath) return i;
            }
            return -1;
        }


        //##############################################################################
        //##############################################################################
        //##
        //##  読み込み
        //##
        //##############################################################################
        //##############################################################################
        #region 読み込み
        /// <summary>
        /// Rectイメージを読み込む
        /// </summary>
        /// <param name="lData"></param>
        /// <returns></returns>
        internal bool RectRead(LoadData lData)
        {
            FImageRectInfo fImgRc = null;
            Chunk chank = new Chunk();
            ImageRect inImgFile;
            LoadChank lChank, lChank02;
            LoadData lData02;
            int nImgNo = 0;

            lData.GetChank(chank);
            inImgFile = new ImageRect();
            while ((lChank = lData.FindData(inImgFile)) != null || !lData.GetFindEnd())
            {
                // ツリービューに反映
                //tNode = m_rTV.Nodes[0].Nodes[1].Nodes.Add(inImgFile.GetString());
                //tNode.ImageIndex = 2;
                //tNode.SelectedImageIndex = 3;
                //tNode.Name = TV_NAME_RECT_IMG;

                //-------------------------------------
                // イメージRECT情報読み込み
                //-------------------------------------
                if (lChank != null)
                {
                    while ((lData02 = lChank.FindChank(chank)) != null)
                    {   // 通常1回のループのみ
                        //-------------------------------
                        // データが正しいかチェックする
                        fImgRc = new FImageRectInfo();
                        while ((lChank02 = lData02.FindData(fImgRc)) != null || !lData02.GetFindEnd())
                        {
                            // データ内部部分書き込み
                            inImgFile.aSprite.Add(fImgRc);
                            // これ以上入れ子は今のところない予定なので入れ子は無視
                            if (lChank02 != null)
                            {
                                lChank02 = null;
                            }
                            fImgRc = new FImageRectInfo();
                        }
                        lData02 = null;
                    }
                }
                ReadedRectImageEventCall(this,new ReadedRectImageEvent(inImgFile));
                lChank = null;
                inImgFile = new ImageRect();
                ++nImgNo;
            }
            inImgFile = null;
            return true;
        }
        /// <summary>
        ///  イメージRECTマップ
        /// </summary>
        /// <param name="lData"></param>
        private bool ReadImageRECT_MAPData(LoadData lData)
        {
            //LoadChank lChank;
            //Chunk chank = new Chunk();
            //CStageImageRectMap objIRM = null;
            //FImageRectMap ffIRM = new FImageRectMap();


            //lData.GetChank(chank);
            //while ((lChank = lData.FindData(ffIRM)) != null || !lData.GetFindEnd())
            //{
            //    if (lChank != null)
            //    {
            //        objIRM = new CStageImageRectMap("");
            //        objIRM.FF_LoadStageFile(ffIRM.GetString(), m_vImageSquareTile, m_vAnimaRECT, lChank);
            //        m_mapImgRectMap.Add(ffIRM.GetString(), objIRM);
            //    }
            //}
            return true;
        }

        ///// <summary>
        ///// アニメーションRECTの読み込み
        ///// </summary>
        ///// <param name="lData"></param>
        ///// <returns></returns>
        //private bool ReadAnimationRect(LoadData lData)
        //{
        //    LoadData lData02;
        //    LoadChank lChank, lChank02;
        //    Chunk chank = new Chunk();
        //    AnimationRect tlAR = new AnimationRect();

        //    // チャンクブロックチェックしない
        //    // IDから処理選択
        //    lData.GetChank(chank);
        //    while ((lChank = lData.FindData(tlAR)) != null || !lData.GetFindEnd())
        //    {
        //        if (lChank != null)
        //        {
        //            lChank.GetBlockChank(chank);
        //            // IDから処理選択
        //            while ((lData02 = lChank.FindChank(chank)) != null)
        //            {
        //                if (chank.id == FAnimationRectFrame.ID)
        //                {
        //                    //---------------------------------
        //                    // RECTアニメーション
        //                    //---------------------------------
        //                    AnimationRectFrame tlBRD = new AnimationRectFrame();
        //                    // チェック
        //                    if (chank.nVersion != tlBRD.GetVersion() || chank.num == 0)
        //                    {
        //                        throw new IOException("FAnimationRectFrame バージョンが異常です。");
        //                    }
        //                    tlAR.num = 0;
        //                    tlAR.period = 0.0f;
        //                    while ((lChank02 = lData02.FindData(tlBRD)) != null || !lData02.GetFindEnd())
        //                    {
        //                        // 合計時間
        //                        tlAR.period += (float)(tlBRD.wait) * FRAME_RATE;
        //                        // 現在これ以上入れ子はない
        //                        if (lChank02 != null)
        //                        {
        //                            throw new IOException("FAnimationRectFrame 予期せぬ入れ子。");
        //                        }
        //                        ++tlAR.num;
        //                        tlAR.Add(tlBRD);
        //                        tlBRD = new AnimationRectFrame();
        //                    }
        //                    if (tlAR.num <= 1)
        //                    {
        //                        tlAR.period = 0.0f;
        //                    }
        //                }
        //                else
        //                {
        //                    // FFID_MAPCHIP_INFO以外のIDが今現在存在しないで
        //                    // 怪しい値を見つけたらエラーをはき出すようにする
        //                    throw new IOException("未対応のIDが呼ばれた。");
        //                }
        //            }
        //        }
        //        m_vAnimaRECT.Add(tlAR);
        //        tlAR = new AnimationRect();
        //    }
        //    return true;
        //}
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
        internal void RectWrite(SaveState objSave)
        {
            //-------------------------------------------
            // スクエアイメージファイルチャンクの書き込み
            //-------------------------------------------
            if (m_ptrRectImageFileDic.Count != 0)
            {
                //-----------------------------
                // FImageSquareTile
                objSave.BlockStart(FImageRect.ID);
                {
                    foreach (ImageRect valu in m_ptrRectImageFileDic.Values)
                    {
                        // イメージ番号[0]:マップチップ用のイメージ
                        objSave.Write(valu);
                        if (valu.GetID() == FImageRect.ID && valu.aSprite.Count != 0)
                        {
                            //-----------------------------
                            // FImageRect
                            objSave.BlockStart(FImageRect.ID);
                            {
                                for (int i = 0; i < valu.aSprite.Count; ++i)
                                {
                                    objSave.Write(valu.aSprite[i]);
                                }
                            }
                            objSave.BlockEnd();
                        }

                    }
                }
                objSave.BlockEnd();
            }
        }

        #endregion
    }
}
