using EditorMC2D.Common;
using EditorMC2D.UtilForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TileStageFormat;
using TileStageFormat.Map.Square;

namespace EditorMC2D.FormMapChip
{
    public partial class MapChipEditForm : Form
    {
        public event EventHandler CloseFormEvent;
        class CMapChipStateSaved
        {
            public const int SAVE_COUNT = 5;
            int m_pos;
            int m_nowCnt;
            int m_blockX;
            int m_blockY;
            private List<D2ArrayObject<FSquareTileInfoMap>> m_aBackGoChips = null;		//
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CMapChipStateSaved()
            {
                m_pos = 0;
                m_nowCnt = 0;
                m_aBackGoChips = new List<D2ArrayObject<FSquareTileInfoMap>>();
            }
            public int Pos
            {
                get { return m_pos; }
            }
            public int NowCnt
            {
                get { return m_nowCnt; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="blockX"></param>
            /// <param name="blockY"></param>
            /// <param name="r"></param>
            public void Recreate(int blockX, int blockY, D2ArrayObject<FSquareTileInfoMap> r)
            {
                int n;
                m_blockX = blockX;
                m_blockY = blockY;
                m_pos = 0;
                m_nowCnt = 1;
                m_aBackGoChips.Clear();
                for (n = 0; n < SAVE_COUNT; ++n)
                    m_aBackGoChips.Add(new D2ArrayObject<FSquareTileInfoMap>(blockX, blockY));
                CopyDesFromSrc(r, 0);
            }
            /// <summary>
            /// 一つ前のデータをセットする
            /// </summary>
            /// <param name="r"></param>
            public void Back(D2ArrayObject<FSquareTileInfoMap> r)
            {
                if (m_pos == 0) return;
                CopySrcFromDes(r, m_pos - 1);
                --m_pos;
            }
            public void Go(D2ArrayObject<FSquareTileInfoMap> r)
            {
                if (m_pos == (m_nowCnt-1)) return;
                CopySrcFromDes(r, m_pos + 1);
                ++m_pos;
            }
            /// <summary>
            /// rをコピーしプッシュする
            /// </summary>
            /// <param name="r"></param>
            public void CopyPush(D2ArrayObject<FSquareTileInfoMap> r)
            {
                int i;
               
                if ( m_nowCnt != (m_pos+1) )
                {
                    for (i = m_pos+1; i < m_nowCnt; ++i)
                        Init(i);
                    m_nowCnt = m_pos+1;
                }
                ++m_pos;
                if (++m_nowCnt > SAVE_COUNT) m_nowCnt = SAVE_COUNT;
                if (m_pos >= SAVE_COUNT)
                {
                    m_pos = SAVE_COUNT - 1;
                    m_aBackGoChips.RemoveAt(0);
                    m_aBackGoChips.Add(new D2ArrayObject<FSquareTileInfoMap>(m_blockX, m_blockY));
                }
                CopyDesFromSrc(r, m_pos);
            }
            public void Init(int idx)
            {
                int x = m_aBackGoChips[idx].LengthX;
                int y = m_aBackGoChips[idx].LengthY;
                for (int i = 0; i < y; ++i)
                    for (int j = 0; j < x; ++j)
                        m_aBackGoChips[idx][i, j].Init();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="r"></param>
            /// <param name="idx"></param>
            public void CopyDesFromSrc(D2ArrayObject<FSquareTileInfoMap> r, int idx)
            {
                FSquareTileInfoMap rMIM0 = null, rMIM1 = null;
                int i, j;

                for (i = 0; i < r.LengthY; ++i)
                    for (j = 0; j < r.LengthX; ++j)
                    {
                        rMIM0 = m_aBackGoChips[idx][i, j];
                        rMIM1 = r[i, j];
                        rMIM0.L00._n = rMIM1.L00._n;
                        rMIM0.L01._n = rMIM1.L01._n;
                    }
            }
            /// <summary>
            /// 対象とする（SRC)へコピーする
            /// </summary>
            /// <param name="r"></param>
            /// <param name="idx"></param>
            public void CopySrcFromDes(D2ArrayObject<FSquareTileInfoMap> r, int idx)
            {
                FSquareTileInfoMap rMIM0 = null, rMIM1 = null;
                int i, j;

                for (i = 0; i < r.LengthY; ++i)
                    for (j = 0; j < r.LengthX; ++j)
                    {
                        rMIM0 = r[i, j];
                        rMIM1 = m_aBackGoChips[idx][i, j];
                        rMIM0.L00._n = rMIM1.L00._n;
                        rMIM0.L01._n = rMIM1.L01._n;
                    }
            }
        }
        private int m_imgWidthBlock;
        private int m_imgHeightBlock;
        private int m_imgWidth;
        private int m_imgHeight;
        private int m_currentBlockX = 0;
        private int m_currentBlockY = 0;
        private int m_chipWriteImageNo = 0;
        private int m_chipWriteChipNo = 0;
        private int m_chipWriteFlg = 0;
        private int m_chipWriteAnmNo = 0;
        private int m_chipWriteGroupNo = 0;
        private int m_chipWriteAnmFlg = 0;
        private MainWindow m_rMainWindow = null;
        private SquareTilesMap m_rMap = null;
        private ReplaceSquareTiles m_objCurrentRCI = new ReplaceSquareTiles();
        private RangeSquareTiles m_objRMapchip = new RangeSquareTiles();
        private CMapChipStateSaved m_objBackGoChips = new CMapChipStateSaved();
        private CommonMC2D m_com = null;
        /// <summary>
        /// 範囲内のコピー
        /// </summary>
        private CatAndPasteSquareTiles m_objCAndPChip = new CatAndPasteSquareTiles();

        // フォームたち
        private ChipSelectForm m_objChipSF = null;
        private AnimationChipSelectForm m_objACSF = null;
        public NewReplaceChipsForm m_objNRCF = null;
        private RangeEditForm m_objREF = null;

        public MapChipEditForm(MainWindow rMainWindow)
        {
            InitializeComponent();
            m_com = CommonMC2D.Instance;
            vScrollBar.Minimum = 0;  //最小値の設定
            vScrollBar.LargeChange = 40; //バーと左右端の矢印の間をクリックした場合の移動量
            vScrollBar.SmallChange = 10; //左右端の矢印をクリックした場合の移動量
            hScrollBar.Minimum = 0;  //最小値の設定
            hScrollBar.LargeChange = 40; //バーと左右端の矢印の間をクリックした場合の移動量
            hScrollBar.SmallChange = 10; //左右端の矢印をクリックした場合の移動量
            objHSBxRPxFram.LargeChange = 1;
            objHSBxRPxFram.SmallChange = 1;
            m_rMainWindow = rMainWindow;
            
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  
        //##
        //##############################################################################
        //##############################################################################
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapName"></param>
        public void SetMap(string mapName)
        {
            m_rMap = m_com.D2Stage.FindSquareTileMapFromName(mapName);
            m_objCurrentRCI.Init(null);
            m_objRMapchip.Init();
            this.ResetMap();
            //
            //objXNAScreen.BasicInit(m_rMap, m_imgWidthBlock, m_imgHeightBlock, this);
            // 置換用の初期化
            m_rMap.SetTransPoseListViewItems(objLWxRPx);
            objLBxRpxCount.Text = ""+m_rMap.TransPoseCount;
            // マウスを押したことにする
            this.objXNAScreen_MouseDown(null, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
        }
        /// <summary>
        /// 現在登録中のマップのデータをリセットする
        /// </summary>
        private void ResetMap()
        {
            m_imgWidth = m_rMap.TileNumX * 40;
            m_imgHeight = m_rMap.TileNumY * 40;
            m_imgWidthBlock = m_rMap.TileNumX;
            m_imgHeightBlock = m_rMap.TileNumY;
            //objXNAScreen.BasicInit(m_imgWidthBlock, m_imgHeightBlock);
            m_currentBlockX = 0;
            m_currentBlockY = 0;
            objTABSelect.SelectedIndex = 0;
            objRBChipEdit.Checked = true;
            // ブロック数の表示
            objLBBlockNum.Text = string.Format("{0:D}", m_imgWidthBlock * m_imgHeightBlock);
            objLBBlockWH.Text = string.Format("{0:D}×{1:D}", m_imgWidthBlock, m_imgHeightBlock);
            //
            m_objBackGoChips.Recreate(m_rMap.TileNumX, m_rMap.TileNumY,m_rMap.GetMapchipAryReference());
            //
            // マウスを押したことにする
            this.objRBChipWork_Click(null, new EventArgs());
            //
            this.BackGoStateButtonEnableChange();
            //
            this.ResizePictureboxInImage();
        }
        /// <summary>
        /// 静的チップ
        /// </summary>
        /// <param name="imageNo"></param>
        /// <param name="tileNo"></param>
        /// <param name="flags"></param>
        public void SetStaticMapChip(int imageNo, int tileNo, int flags)
        {
            if ((objTABSelect.SelectedIndex == 0 && objRBChipWrite.Checked) || 
                 (objTABSelect.SelectedIndex == 1 && objRBxRPxChipWrite.Checked && m_objCurrentRCI.rTC != null)
            )
            {
                m_chipWriteImageNo = imageNo;
                m_chipWriteChipNo = tileNo;
                m_chipWriteFlg = flags;
                //objXNAScreen.SetSelectStaticChip(imageNo, tileNo, flags);
            }
        }
        /// <summary>
        /// アニメーションチップ
        /// </summary>
        /// <param name="imageNo"></param>
        /// <param name="tileNo"></param>
        /// <param name="flags"></param>
        public void SetStaticAnmMapChip(int nAnmNo, int nGroupNo, int flags)
        {
            if ( (objTABSelect.SelectedIndex == 0 && objRBChipWrite.Checked) ||
                 (objTABSelect.SelectedIndex == 1 && objRBxRPxChipWrite.Checked && m_objCurrentRCI.rTC != null)
                )
            {
                m_chipWriteAnmNo = nAnmNo;
                m_chipWriteGroupNo = nGroupNo;
                m_chipWriteAnmFlg = flags;
                //objXNAScreen.SetSelectAnmChip(nAnmNo, nGroupNo, flags);
            }
        }
        public void ClosedChipSelectForm()
        {
            if (m_objChipSF != null)
                m_objChipSF = null;
        }
        public void ClosedAnimationChipSelectForm()
        {
            if (m_objACSF != null)
                m_objACSF = null;
        }
        /// <summary>
        /// RangeEditForm側で閉じたら呼びだす
        /// </summary>
        public void ClosedRangeEditForm()
        {
            if (m_objREF != null)
                m_objREF = null;
            m_objCAndPChip.Init();
        }
        /// <summary>
        /// RangeEditFormFromからペーストの対象レイヤー値が変わった
        /// </summary>
        /// <param name="nPasteFlg"></param>
        public void SetRangeEditFormFromPasteFlg(int nPasteFlg)
        {
            m_objCAndPChip.PasteFlg = nPasteFlg;
        }
        public void SetDrawFPS(float fFPS)
        {
            //objTSSL_CopyRange.Text = string.Format("FPS:{0:R}", fFPS);
        }
        /// <summary>
        /// 
        /// </summary>
        private void BackGoStateButtonEnableChange()
        {
            if (m_objBackGoChips.Pos == 0 && m_objBackGoChips.NowCnt == 1 )
            {
                objTSBStateBack.Enabled = false;
                objTSBStateGo.Enabled = false;
            }
            else if (m_objBackGoChips.Pos == (m_objBackGoChips.NowCnt - 1))
            {
                objTSBStateBack.Enabled = true;
                objTSBStateGo.Enabled = false;
            }
            else if (m_objBackGoChips.Pos == 0 && m_objBackGoChips.NowCnt > 1)
            {
                objTSBStateBack.Enabled = false;
                objTSBStateGo.Enabled = true;
            }
            else
            {
                objTSBStateBack.Enabled = true;
                objTSBStateGo.Enabled = true;
            }
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  
        //##
        //##############################################################################
        //##############################################################################
        /// <summary>
        /// 主にフォーム（ピクチャーボックス）のサイズがリサイズされたときに使用する
        /// </summary>
        private void ResizePictureboxInImage()
        {
            ////objXNAScreen.Width = splitContainer1.Panel2.Width - vScrollBar.Width;
            ////objXNAScreen.Height = splitContainer1.Panel2.Height - hScrollBar.Height;
            //// スクロールバーなど
            //vScrollBar.Location = new Point(//objXNAScreen.Width, 0);
            //vScrollBar.Height = splitContainer1.Panel2.Height - vScrollBar.Width;
            //hScrollBar.Location = new Point(0, //objXNAScreen.Height);
            //hScrollBar.Width = splitContainer1.Panel2.Width - hScrollBar.Height;

            //if (m_rMap == null) return;
            //int nW = m_imgWidth - objXNAScreen.Width;
            //if (nW > 0)
            //{
            //    hScrollBar.Maximum = nW + 40;  //最大値の設定
            //    hScrollBar.Enabled = true;
            //}
            //else
            //{
            //    hScrollBar.Maximum = 0;  //最大値の設定
            //    hScrollBar.Enabled = false;
            //}
            //hScrollBar.Value = 0;

            //int nH = m_imgHeight - objXNAScreen.Height;
            //if (nH > 0)
            //{
            //    vScrollBar.Maximum = nH;  //最大値の設定
            //    vScrollBar.Enabled = true;
            //}
            //else
            //{
            //    vScrollBar.Enabled = false;
            //}
            //vScrollBar.Value = 0;

        }
        //##############################################################################
        //##############################################################################
        //##
        //##  Form
        //##
        //##############################################################################
        //##############################################################################
        /// <summary>
        /// 初期読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapChipEditForm_Load(object sender, EventArgs e)
        {
            // 表示位置の調整
            this.Location = new Point(m_rMainWindow.Size.Width, 0);
            // 進むボタンを無効
            this.BackGoStateButtonEnableChange();
            // アニメーションチップが存在してない
            //if (!m_com.IsChipAnimation())
            //{
            //    objRBChipAnm.Enabled = false;
            //    objRBChipAnm.BackgroundImage = Resources.movie_blue_film_strip_g;
            //}
        }
        /// <summary>
        /// リサイズされた
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainer1_Resize(object sender, EventArgs e)
        {
            this.ResizePictureboxInImage();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapChipEditForm_Shown(object sender, EventArgs e)
        {
            // 描画タイミング
            objTimer.Interval = 33;
            objTimer.Enabled = true;
            this.ResizePictureboxInImage();
        }
        /// <summary>
        /// フォームが閉じられた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapChipEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            objTimer.Enabled = false;
            if (CloseFormEvent != null)
                CloseFormEvent(sender, e);

            if (m_objChipSF != null)
                m_objChipSF.Close();
            if( m_objACSF != null )
                m_objACSF.Close();
            if (m_objNRCF != null)
                m_objNRCF.Close();
            if (m_objREF != null)
                m_objREF.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTimer_Tick(object sender, EventArgs e)
        {
            ////objXNAScreen.SetScrollBar(vScrollBar.Value, hScrollBar.Value);
        }
        /// <summary>
        /// メインタブが変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTABSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objTABSelect.SelectedIndex == 0)
            {
                // 通常マップエディタ
                ////objXNAScreen.SetmRPAnimationPlay(false);
                ////objXNAScreen.SetMode(XNAMapchipEdit.MODE_MAPEDIT);
                
                // 置き換え側の処理をしたことにする
                objRBxRPxChipEdit.Checked = true;
                //objRBxRPxChipWork_Click(null, null);
                //
                if( objRBChipWrite.Checked )
                    this.checkBoxChipType_Click(null, null);
            }
            else if (objTABSelect.SelectedIndex == 1)
            {
                // 置き換えタブになった
                objBTxRPxDeleteReplaseChip.Enabled = false;
                objTPAnmEdit.Enabled = false;
                objBTxRPxDeleteReplaseChip.Enabled = false;
                objRBxRPxChipEdit.Checked = true;
                ////objXNAScreen.SetMode(XNAMapchipEdit.MODE_REPLACE);
                //
                if (m_objACSF != null)
                    m_objACSF.Close();
                if (m_objChipSF != null)
                    m_objChipSF.Close();
                if (m_objREF != null)
                    m_objREF.Close();
            }
        }
        /// <summary>
        /// キーが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapChipEditForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.C:
                    if (objTABSelect.SelectedIndex == 0 && objRBChipRange.Checked && m_objRMapchip.DecisionFlg)
                    {
                        //----------------------------------------
                        // 範囲指定しコピー＆ペーストモード
                        //----------------------------------------
                        // コピー
                        if (m_objREF != null && ((Control.ModifierKeys & Keys.Control) == Keys.Control))
                        {
                            m_objREF.SetCopyCRangeMapchipe(m_objRMapchip);
                            m_objCAndPChip.Cat(m_objRMapchip, m_rMap, m_objREF.CatFlg);
                        }
                    }
                    break;
                case Keys.V:
                    if (objTABSelect.SelectedIndex == 0 && objRBChipRange.Checked && m_objRMapchip.DecisionFlg)
                    {
                        //----------------------------------------
                        // 範囲指定しコピー＆ペーストモード
                        //----------------------------------------
                        // 貼り付け
                        if (m_objREF != null && ((Control.ModifierKeys & Keys.Control) == Keys.Control))
                        {
                            m_objCAndPChip.Paste(m_objRMapchip.startPosBlockX, m_objRMapchip.startPosBlockY, m_rMap, m_objREF.PasteFlg);
                            // 現在の内容をプッシュし、戻る進み右ボタンの更新
                            m_objBackGoChips.CopyPush(m_rMap.GetMapchipAryReference());
                            this.BackGoStateButtonEnableChange();
                        }
                    }
                    break;
                case Keys.Z:
                    if (objTABSelect.SelectedIndex == 0 && objTSBStateBack.Enabled)
                    {
                        objTSBStateBacke_Click(null, new EventArgs());
                    }
                    break;
                case Keys.Y:
                    if (objTABSelect.SelectedIndex == 0 && objTSBStateGo.Enabled)
                    {
                        objTSBStateGo_Click(null, new EventArgs());
                    }
                    break;
            }
        }
        /// <summary>
        /// ツールバーの状態を進めるボタンを押した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTSBStateGo_Click(object sender, EventArgs e)
        {
            m_objBackGoChips.Go(m_rMap.GetMapchipAryReference());
            this.BackGoStateButtonEnableChange();
        }
        /// <summary>
        /// ツールバーの状態を戻るボタンを押した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTSBStateBacke_Click(object sender, EventArgs e)
        {
            m_objBackGoChips.Back(m_rMap.GetMapchipAryReference());
            this.BackGoStateButtonEnableChange();
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  objXNAScreen
        //##
        //##############################################################################
        //##############################################################################
        /// <summary>
        /// XNAスクリーン内でマウスが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objXNAScreen_MouseDown(object sender, MouseEventArgs e)
        {
            FSquareTileInfoMap ffTmp;
            int nV = vScrollBar.Value;
            int nH = hScrollBar.Value;

            bool bLeft = (e.Button & MouseButtons.Left) == MouseButtons.Left;
            bool bRight = (e.Button & MouseButtons.Right) == MouseButtons.Right;

            if (m_objNRCF != null)
            {
                //----------------------------------------
                // 新規置き換えフレーム作成モード
                //----------------------------------------
                this.CurrentBlocksReset(e.X, e.Y);

                if (bLeft)
                {
                    //objXNAScreen.SetChipBlockPos(m_currentBlockX, m_currentBlockY);
                    m_objNRCF.SetPos(m_currentBlockX, m_currentBlockY);
                    //objXNAScreen.SetNewReplaceRect(
                    //    m_currentBlockX, m_currentBlockY,
                    //    m_objNRCF.GetWidthBlock(), m_objNRCF.GetHeightBlock()
                    //);
                }
            }
            else if (objTABSelect.SelectedIndex == 1 && m_objCurrentRCI.rTC != null)
            {
                //----------------------------------------
                // 置き換えモード
                //----------------------------------------
                int nBX = (e.X + nH) / 40;
                int nBY = (e.Y + nV) / 40;
                if (m_objCurrentRCI.rTC.CheckDrawRange(nBX, nBY) && bLeft)
                {
                    m_currentBlockX = nBX;
                    m_currentBlockY = nBY;
                    //objXNAScreen.SetChipBlockPos(nBX, nBY);
                    ffTmp = m_objCurrentRCI.rTC.GetMapchipInMapReference(nBX,nBY, objHSBxRPxFram.Value);

                    if (objRBxRPxChipWrite.Checked)
                    {
                        // チップを書き込む
                        if (objRBxRPxTargetL00.Checked)
                        {
                            // レイヤー０
                            if (objRBxRPxChip.Checked)
                            {
                                ffTmp.L00.imageFileNo = (short)m_chipWriteImageNo;
                                ffTmp.L00.tileNo = (short)m_chipWriteChipNo;
                                ffTmp.L00.flags = (ushort)m_chipWriteFlg;
                            }
                            else
                            {   // アニメーション
                                ffTmp.L00.anmTileNo = (short)m_chipWriteAnmNo;
                                ffTmp.L00.anmGroupNo = (short)m_chipWriteGroupNo;
                                ffTmp.L00.flags = (ushort)(m_chipWriteAnmFlg | FSquareTileInfoMap.ANMCHIP);
                            }
                        }
                        else
                        {
                            // レイヤー１
                            if (objRBxRPxChip.Checked)
                            {
                                ffTmp.L01.imageFileNo = (short)m_chipWriteImageNo;
                                ffTmp.L01.tileNo = (short)m_chipWriteChipNo;
                                ffTmp.L01.flags = (ushort)m_chipWriteFlg;
                            }
                            else
                            {   // アニメーション
                                ffTmp.L01.anmTileNo = (short)m_chipWriteAnmNo;
                                ffTmp.L01.anmGroupNo = (short)m_chipWriteGroupNo;
                                ffTmp.L01.flags = (ushort)(m_chipWriteAnmFlg | FSquareTileInfoMap.ANMCHIP);
                            }
                        }

                    }
                    else if (objRBxRPxChipDelete.Checked)
                    {
                        // 削除ボタンが押された
                        if (objRBxRPxTargetL00.Checked)
                        {
                            ffTmp.L00.imageFileNo = -1;
                            ffTmp.L00.tileNo = -1;
                            ffTmp.L00.flags = 0;
                        }
                        else
                        {
                            ffTmp.L01.imageFileNo = -1;
                            ffTmp.L01.tileNo = -1;
                            ffTmp.L01.flags = 0;
                        }
                    }
                    else if (objRBxRPxChipEdit.Checked)
                    {
                        // 編集
                        if (objRBxRPxTargetL00.Checked && (ffTmp.L00.flags & FSquareTileInfoMap.ANMCHIP) != 0)
                        {
                            objGBxRPxAnm.Enabled = true;
                            obuUDxRPxGroupNo.Value = ffTmp.L00.anmGroupNo;
                        }
                        else if (objRBxRPxTargetL01.Checked && (ffTmp.L00.flags & FSquareTileInfoMap.ANMCHIP) != 0)
                        {
                            objGBxRPxAnm.Enabled = true;
                            obuUDxRPxGroupNo.Value = ffTmp.L01.anmGroupNo;
                        }
                        else
                            objGBxRPxAnm.Enabled = false;
                    }
                }
            }
            else if (objTABSelect.SelectedIndex == 0 )
            {
                //----------------------------------------
                // 通常書き込みモード
                //----------------------------------------
                this.CurrentBlocksReset(e.X, e.Y);

                ffTmp = m_rMap.GetMapchipReference(m_currentBlockX, m_currentBlockY);
                if (objRBChipWrite.Checked)
                {
                    if (bLeft)
                    {
                        //objXNAScreen.SetChipBlockPos(m_currentBlockX, m_currentBlockY);
                        // チップを書き込む
                        if (objRBTargetL00.Checked)
                        {
                            // レイヤー０
                            if (objRBChip.Checked)
                            {
                                ffTmp.L00.imageFileNo = (short)m_chipWriteImageNo;
                                ffTmp.L00.tileNo = (short)m_chipWriteChipNo;
                                ffTmp.L00.flags = (ushort)m_chipWriteFlg;
                            }
                            else
                            {   // アニメーション
                                ffTmp.L00.anmTileNo = (short)m_chipWriteAnmNo;
                                ffTmp.L00.anmGroupNo = (short)m_chipWriteGroupNo;
                                ffTmp.L00.flags = (ushort)(m_chipWriteAnmFlg | FSquareTileInfoMap.ANMCHIP);
                            }
                        }
                        else
                        {
                            // レイヤー１
                            if (objRBChip.Checked)
                            {
                                ffTmp.L01.imageFileNo = (short)m_chipWriteImageNo;
                                ffTmp.L01.tileNo = (short)m_chipWriteChipNo;
                                ffTmp.L01.flags = (ushort)m_chipWriteFlg;
                            }
                            else
                            {   // アニメーション
                                ffTmp.L01.anmTileNo = (short)m_chipWriteAnmNo;
                                ffTmp.L01.anmGroupNo = (short)m_chipWriteGroupNo;
                                ffTmp.L01.flags = (ushort)(m_chipWriteAnmFlg | FSquareTileInfoMap.ANMCHIP);
                            }
                        }
                        // 現在の内容をプッシュし、戻る進み右ボタンの更新
                        m_objBackGoChips.CopyPush(m_rMap.GetMapchipAryReference());
                        this.BackGoStateButtonEnableChange();
                    }

                }
                else if (objRBChipDelete.Checked)
                {
                    // 削除ボタンが押された
                    if (bLeft)
                    {
                        //objXNAScreen.SetChipBlockPos(m_currentBlockX, m_currentBlockY);
                        if (objRBTargetL00.Checked)
                        {
                            ffTmp.L00.imageFileNo = -1;
                            ffTmp.L00.tileNo = -1;
                            ffTmp.L00.flags = 0;
                        }
                        else
                        {
                            ffTmp.L01.imageFileNo = -1;
                            ffTmp.L01.tileNo = -1;
                            ffTmp.L01.flags = 0;
                        }
                        // 現在の内容をプッシュし、戻る進み右ボタンの更新
                        m_objBackGoChips.CopyPush(m_rMap.GetMapchipAryReference());
                        this.BackGoStateButtonEnableChange();
                    }
                }
                else if (objRBChipEdit.Checked)
                {
                    // 編集
                    bool bEditFlg = false;
                    if (bLeft)
                    {
                        //objXNAScreen.SetChipBlockPos(m_currentBlockX, m_currentBlockY);
                        if (objRBTargetL00.Checked && (ffTmp.L00.flags & FSquareTileInfoMap.ANMCHIP) != 0)
                        {
                            bEditFlg = true;
                            objGBAnm.Enabled = true;
                            obuUDGroupNo.Value = ffTmp.L00.anmGroupNo;
                        }
                        else if (objRBTargetL01.Checked && (ffTmp.L00.flags & FSquareTileInfoMap.ANMCHIP) != 0)
                        {
                            bEditFlg = true;
                            objGBAnm.Enabled = true;
                            obuUDGroupNo.Value = ffTmp.L01.anmGroupNo;
                        }
                        else
                            objGBAnm.Enabled = false;
                        // 現在の内容をプッシュし、戻る進み右ボタンの更新
                        if (bEditFlg)
                        {
                            m_objBackGoChips.CopyPush(m_rMap.GetMapchipAryReference());
                            this.BackGoStateButtonEnableChange();
                        }
                    }
                }
                else if (objRBChipRange.Checked && m_objREF != null)
                {
                    //----------------------------------------
                    // 範囲指定しコピー＆ペーストモード
                    //----------------------------------------
                    this.CurrentBlocksReset(e.X, e.Y);
                    if (bLeft)
                    {
                        if (((Control.ModifierKeys & Keys.Shift) == Keys.Shift) && m_objRMapchip.DecisionFlg)
                        {
                            // Shftを押しながらなので拡張
                            m_objRMapchip.Expansion(m_currentBlockX, m_currentBlockY);
                            //objXNAScreen.SetRangeMapchip(m_objRMapchip);
                            m_objREF.SetCRangeMapchipe(m_objRMapchip);
                        }
                        else
                        {
                            //objXNAScreen.SetChipBlockPos(m_currentBlockX, m_currentBlockY);
                            m_objRMapchip.Set(m_currentBlockX, m_currentBlockY, 1, 1);
                            //objXNAScreen.SetRangeMapchip(m_objRMapchip);
                        }
                    }
                    else if (bRight)
                    {
                        //objXNAScreen.SetCatAndPasteChips(m_objCAndPChip);
                    }
                }

            }
            
        }
        private void CurrentBlocksReset(int x, int y)
        {
            int nV = vScrollBar.Value;
            int nH = hScrollBar.Value;

            m_currentBlockX = (x + nH) / 40;
            if (m_imgWidthBlock <= m_currentBlockX)
                m_currentBlockX = m_imgWidthBlock - 1;
            else if( m_currentBlockX < 0 )
                m_currentBlockX = 0;

            m_currentBlockY = (y + nV) / 40;
            if (m_imgHeightBlock <= m_currentBlockY)
                m_currentBlockY = m_imgHeightBlock - 1;
            else if( m_currentBlockY < 0 )
                m_currentBlockY = 0;
        }
        /// <summary>
        /// XNAスクリーンでマウスがアップされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objXNAScreen_MouseUp(object sender, MouseEventArgs e)
        {
            bool bLeft = (e.Button & MouseButtons.Left) == MouseButtons.Left;
            bool bRight = (e.Button & MouseButtons.Right) == MouseButtons.Right;

            if (objTABSelect.SelectedIndex == 0 && objRBChipRange.Checked && m_objREF != null)
            {
                //----------------------------------------
                // 範囲指定しコピー＆ペーストモード
                //----------------------------------------
                if (bLeft && !m_objRMapchip.DecisionFlg)
                {
                    m_objRMapchip.Decision();
                    m_objREF.SetCRangeMapchipe(m_objRMapchip);
                }
                else if (bRight)
                {
                    //objXNAScreen.SetCatAndPasteChips(null);
                }
            }
        }
        /// <summary>
        /// XNAスクリーン内でマウスが動いた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objXNAScreen_MouseMove(object sender, MouseEventArgs e)
        {
            int nV = vScrollBar.Value;
            int nH = hScrollBar.Value;
            int x = (e.X + hScrollBar.Value) / 40;
            int y = (e.Y + vScrollBar.Value) / 40;
            toolStripStatusLabel1.Text = string.Format("ブロック位置：X={0:D}, Y={1:D}", x, y);
            //objXNAScreen.SetSelectBlockPosition(x, y);

            if (objTABSelect.SelectedIndex == 0 && objRBChipRange.Checked)
            {
                //----------------------------------------
                // 範囲指定しコピー＆ペーストモード
                //----------------------------------------
                this.CurrentBlocksReset(e.X, e.Y);
                if ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left && !m_objRMapchip.DecisionFlg)
                {
                    x = m_currentBlockX-m_objRMapchip.startPosBlockX;
                    y = m_currentBlockY-m_objRMapchip.startPosBlockY;
                    m_objRMapchip.Set(x >= 0 ? x+1 : x, y >= 0 ? y+1 : y);
                    //objXNAScreen.SetRangeMapchip(m_objRMapchip);
                    if( m_objREF != null )
                        m_objREF.SetCRangeMapchipe(m_objRMapchip);
                }
            }            

        }
        /// <summary>
        /// XNAスクリーンがリサイズされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objXNAScreen_Resize(object sender, EventArgs e)
        {
            this.ResizePictureboxInImage();
            //objXNAScreen.OnResize();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objScrollValueChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// チェックボックスから描画するやつを選択した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxXX_Click(object sender, EventArgs e)
        {
            //objXNAScreen.SetDrawTypeFlg(
            //    objCKL0.Checked, 
            //    objCKL1.Checked, 
            //    objCKColli.Checked, 
            //    objCKGrid.Checked,
            //    objCKAnm.Checked);
        }
        /// <summary>
        /// マップエディタ側
        /// チップタイプのラジオボタンのどちらかがクリックされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxChipType_Click(object sender, EventArgs e)
        {
            try
            {
                if (objRBChip.Checked)
                {
                    // アニメーション
                    if (m_objACSF != null)
                        m_objACSF.Close();

                    // 静止チップ
                    if (m_objChipSF == null)
                    {
                        m_objChipSF = new ChipSelectForm(this);
                        m_objChipSF.Show();
                    }
                    objGBAnm.Enabled = false;
                }
                else if (objRBChipAnm.Checked)
                {
                    // アニメーション
                    if (m_objChipSF != null)
                        m_objChipSF.Close();

                    if (m_objACSF == null)
                    {
                        m_objACSF = new AnimationChipSelectForm(this);
                        m_objACSF.Show();
                    }
                    objGBAnm.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 描く、削除ラジオボタンがおされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objRBChipWork_Click(object sender, EventArgs e)
        {
            if (objRBChipWrite.Checked)
            {
                // 描くボタン
                if (m_objACSF != null)
                    m_objACSF.Close();
                if (m_objREF != null)
                    m_objREF.Close();
                checkBoxChipType_Click(null, null);
                //objXNAScreen.SetSelectStaticChip(m_chipWriteImageNo, m_chipWriteChipNo, m_chipWriteFlg);
                objGBChipType.Enabled = true;
            }
            else if (objRBChipDelete.Checked )
            {
                // 削除ボタン
                if (m_objACSF != null)
                    m_objACSF.Close();
                if (m_objChipSF != null)
                    m_objChipSF.Close();
                if (m_objREF != null)
                    m_objREF.Close();
                //objXNAScreen.SetSelectDeleteChip();
                objGBChipType.Enabled = false;
            }
            else if (objRBChipEdit.Checked)
            {
                // 編集ボタン
                if (m_objACSF != null)
                    m_objACSF.Close();
                if (m_objChipSF != null)
                    m_objChipSF.Close();
                if (m_objREF != null)
                    m_objREF.Close();
                objGBAnm.Enabled = true;
                //objXNAScreen.SetSelectEditType();
                // 
                FSquareTileInfoMap ffTmp = m_rMap.GetMapchipReference(m_currentBlockX, m_currentBlockY);
                if (objRBTargetL00.Checked && (ffTmp.L00.flags & FSquareTileInfoMap.ANMCHIP) != 0)
                    objGBAnm.Enabled = true;
                else if (objRBTargetL01.Checked && (ffTmp.L00.flags & FSquareTileInfoMap.ANMCHIP) != 0)
                    objGBAnm.Enabled = true;
                else
                    objGBAnm.Enabled = false;
                objGBChipType.Enabled = false;
            }
            else if (objRBChipRange.Checked)
            {
                // 範囲
                if (m_objACSF != null)
                    m_objACSF.Close();
                if (m_objChipSF != null)
                    m_objChipSF.Close();
                if (m_objREF == null)
                {
                    m_objREF = new RangeEditForm(this);
                    m_objREF.Show();
                }
                //objXNAScreen.SetSelectRange();
            }
            else
            {
                if (m_objACSF != null)
                    m_objACSF.Close();
                if (m_objChipSF != null)
                    m_objChipSF.Close();
                if (m_objREF != null)
                    m_objREF.Close();
                objGBChipType.Enabled = false;
            }
        }
        /// <summary>
        /// アニメーショングループの値が変化した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void obuUDGroupNo_ValueChanged(object sender, EventArgs e)
        {
            FSquareTileInfoMap ffTmp = m_rMap.GetMapchipReference(m_currentBlockX, m_currentBlockY);
            if (objRBTargetL00.Checked && (ffTmp.L00.flags & FSquareTileInfoMap.ANMCHIP) != 0)
                ffTmp.L00.anmGroupNo = (short)obuUDGroupNo.Value;
            else if (objRBTargetL01.Checked && (ffTmp.L00.flags & FSquareTileInfoMap.ANMCHIP) != 0)
                ffTmp.L01.anmGroupNo = (short)obuUDGroupNo.Value;

            objGBChipType.Enabled = false;
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  置き換えで主に使う
        //##
        //##############################################################################
        //##############################################################################
        /// <summary>
        /// 置き換えのタブが変化した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTCxRPxAnm_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 0)
            {
                objLWxRPx.Focus();
                objLWxRPx.Select();
            }

        }
        /// <summary>
        /// 置き換え名一覧のリストがダブルクリックされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objLWxRPx_DoubleClick(object sender, EventArgs e)
        {
            // アニメション編集他部へ移動
            objTCxRPxAnm.SelectedIndex = 1;
            objTPAnmEdit.Enabled = true;
        }
        /// <summary>
        /// 置き換えリスト名の選択が変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objLWxRPx_SelectedIndexChanged(object sender, EventArgs e)
        {
            objBTxRPxDeleteReplaseChip.Enabled = true;
            if (objLWxRPx.SelectedItems.Count == 0) return;
            // 現行のものをセット
            m_objCurrentRCI.Init(m_rMap.FindSquareTilesTransposeFromName(objLWxRPx.SelectedItems[0].Text));
            objBTxRPxDeleteReplaseChip.Enabled = true;
            objTPAnmEdit.Enabled = true;
            this.ReplaseResetting();
        }
        private void ReplaseResetting()
        {
            // ()情報を表示
            objGBxRPxInfoName.Text = m_objCurrentRCI.rTC.GetString() + "の情報";
            objLBxRPxWH.Text = m_objCurrentRCI.rTC.tileNumX + " × " + m_objCurrentRCI.rTC.tileNumY;
            objLBxRPxPos.Text = m_objCurrentRCI.rTC.tilePosX + ", " + m_objCurrentRCI.rTC.tilePosY;
            objLBxRPxFrame.Text = m_objCurrentRCI.rTC.frameNum + "";
            //objXNAScreen.SetReplaceChip(m_objCurrentRCI.rTC);
            // 使用しないレイヤーのチェック
            if ((m_objCurrentRCI.rTC.flags & FSquareTilesTransposeHeader.INVALID_L00) != 0)
                objCKxRPxUnUseL00.Checked = true;
            else
                objCKxRPxUnUseL00.Checked = false;
            if ((m_objCurrentRCI.rTC.flags & FSquareTilesTransposeHeader.INVALID_L01) != 0)
                objCKxRPxUnUseL01.Checked = true;
            else
                objCKxRPxUnUseL01.Checked = false;
            //
            objHSBxRPxFram.Minimum = 0;
            objHSBxRPxFram.Maximum = m_objCurrentRCI.rTC.frameNum - 1;
            objUDxRPxNowFrame.Minimum = 0;
            objUDxRPxNowFrame.Maximum = m_objCurrentRCI.rTC.frameNum - 1;
            objLBxRPxName.Text = m_objCurrentRCI.rTC.GetString();
            objLBxRPxFramePos.Text = string.Format("{0:D}", m_objCurrentRCI.rTC.frameNum - 1);
            objUDxRPxWait.Value = m_objCurrentRCI.rTC.aTCF[0].wait;
            objCKxRPxPaste.Enabled = false;
            // セレクト位置を置き換えの初期位置に合わせる
            m_currentBlockX = m_objCurrentRCI.rTC.tilePosX;
            m_currentBlockY = m_objCurrentRCI.rTC.tilePosY;
            //objXNAScreen.SetChipBlockPos(m_currentBlockX, m_currentBlockY);
            // クリックしたことにする
            this.objRBxRPxChipWork_Click(null, null);
            this.objCKxRPxUnUseL00_Click(null, null);
            this.objCKxRPxUnUseL01_Click(null, null);

        }
        /// <summary>
        /// チップタイプのラジオボタンのどちらかがクリックされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxChipType_xRPxClick(object sender, EventArgs e)
        {
            if (objRBxRPxChip.Checked)
            {
                // アニメーション
                if (m_objACSF != null)
                    m_objACSF.Close();

                // 静止チップ
                if (m_objChipSF == null)
                {
                    m_objChipSF = new ChipSelectForm(this);
                    m_objChipSF.Show();
                }
                objGBxRPxAnm.Enabled = false;
            }
            else if (objRBxRPxChipAnm.Checked)
            {
                // アニメーション
                if (m_objChipSF != null)
                    m_objChipSF.Close();

                if (m_objACSF == null)
                {
                    m_objACSF = new AnimationChipSelectForm(this);
                    m_objACSF.Show();
                }
                objGBxRPxAnm.Enabled = false;
            }
        }
        /// <summary>
        /// 描く、削除ラジオボタンがおされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objRBxRPxChipWork_Click(object sender, EventArgs e)
        {
            if (objRBxRPxChipWrite.Checked)
            {
                // 描くボタン
                if (m_objACSF != null)
                    m_objACSF.Close();
                this.checkBoxChipType_xRPxClick(null, null);
                //objXNAScreen.SetSelectStaticChip(m_chipWriteImageNo, m_chipWriteChipNo, m_chipWriteFlg);
                objGBxRPxChipType.Enabled = true;
            }
            else if (objRBxRPxChipDelete.Checked)
            {
                // 削除ボタン
                if (m_objACSF != null)
                    m_objACSF.Close();
                if (m_objChipSF != null)
                    m_objChipSF.Close();
                //objXNAScreen.SetSelectDeleteChip();
                objGBxRPxChipType.Enabled = false;
            }
            else if (objRBxRPxChipEdit.Checked)
            {
                // 編集ボタン
                if (m_objACSF != null)
                    m_objACSF.Close();
                if (m_objChipSF != null)
                    m_objChipSF.Close();
                objGBxRPxAnm.Enabled = true;
                //objXNAScreen.SetSelectEditType();
                // 
                FSquareTileInfoMap ffTmp = m_objCurrentRCI.rTC.GetMapchipInMapReference(m_currentBlockX, m_currentBlockY, objHSBxRPxFram.Value);
                if (objRBxRPxTargetL00.Checked && (ffTmp.L00.flags & FSquareTileInfoMap.ANMCHIP) != 0)
                    objGBxRPxAnm.Enabled = true;
                else if (objRBTargetL01.Checked && (ffTmp.L00.flags & FSquareTileInfoMap.ANMCHIP) != 0)
                    objGBxRPxAnm.Enabled = true;
                else
                    objGBxRPxAnm.Enabled = false;
                objGBxRPxChipType.Enabled = false;
            }
            else
            {
                if (m_objACSF != null)
                    m_objACSF.Close();
                if (m_objChipSF != null)
                    m_objChipSF.Close();
                objGBxRPxChipType.Enabled = false;
            }
        }
        private void objTPxRPxList_Enter(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 使用しないレイヤーチェックぼっくすのどちらかがおされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCKxRPxUnUseL00_Click(object sender, EventArgs e)
        {
            if (objCKxRPxUnUseL00.Checked)
            {
                if (objCKxRPxUnUseL01.Checked)
                {
                    objCKxRPxUnUseL01.Checked = false;
                    m_objCurrentRCI.rTC.flags &= 0xFFFC;
                }
                objRBxRPxTargetL01.Checked = true;
                objRBxRPxTargetL00.Enabled = false;
                objRBxRPxTargetL01.Enabled = true;
                m_objCurrentRCI.rTC.flags |= FSquareTilesTransposeHeader.INVALID_L00;
            }
            else
            {
                objRBxRPxTargetL00.Enabled = true;
                m_objCurrentRCI.rTC.flags &= 0xFFFE;
            }
            
        }
        private void objCKxRPxUnUseL01_Click(object sender, EventArgs e)
        {
            if (objCKxRPxUnUseL01.Checked)
            {
                if (objCKxRPxUnUseL00.Checked)
                {
                    objCKxRPxUnUseL00.Checked = false;
                    m_objCurrentRCI.rTC.flags &= 0xFFFE;
                }
                objRBxRPxTargetL00.Checked = true;
                objRBxRPxTargetL00.Enabled = true;
                objRBxRPxTargetL01.Enabled = false;
                m_objCurrentRCI.rTC.flags |= FSquareTilesTransposeHeader.INVALID_L01;
            }
            else
            {
                objRBxRPxTargetL01.Enabled = true;
                m_objCurrentRCI.rTC.flags &= 0xFFFC;
            }
        }
        /// <summary>
        /// フレームの位置バーが変化した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objHSBxRPxFram_ValueChanged(object sender, EventArgs e)
        {
            if (objHSBxRPxFram.Value != objUDxRPxNowFrame.Value)
            {
                objUDxRPxNowFrame.Value = objHSBxRPxFram.Value;
                return;
            }
            //objXNAScreen.SetReplaceFramePos(objHSBxRPxFram.Value);
            objUDxRPxWait.Value = m_objCurrentRCI.rTC.aTCF[objHSBxRPxFram.Value].wait;
        }
        /// <summary>
        /// アップダウンのフレーム位置の値が変化した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objUDxRPxNowFrame_ValueChanged(object sender, EventArgs e)
        {
            if (objHSBxRPxFram.Value != objUDxRPxNowFrame.Value)
            {
                objHSBxRPxFram.Value = (int)objUDxRPxNowFrame.Value;
                return;
            }
            //objXNAScreen.SetReplaceFramePos(objHSBxRPxFram.Value);
            objUDxRPxWait.Value = m_objCurrentRCI.rTC.aTCF[objHSBxRPxFram.Value].wait;
        }
        /// <summary>
        /// ウエイト値が変化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objUDxRPxWait_ValueChanged(object sender, EventArgs e)
        {
            m_objCurrentRCI.rTC.aTCF[objHSBxRPxFram.Value].wait = (short)objUDxRPxWait.Value;
            //
            m_objCurrentRCI.rTC.TimeReset();
        }
        /// <summary>
        /// 右側へフレームを追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBTxRPxRightAdd_Click(object sender, EventArgs e)
        {
            int idx = objHSBxRPxFram.Value+1;

            if (objCKxRPxPaste.Checked && m_objCurrentRCI.nCopyFrameNo != -1)
            {
                m_objCurrentRCI.CreateInsertCopy(idx, m_objCurrentRCI.nCopyFrameNo);
                if (objHSBxRPxFram.Value <= m_objCurrentRCI.nCopyFrameNo)
                    m_objCurrentRCI.nCopyFrameNo++;
            }
            else
                m_objCurrentRCI.CreateInsertNew(idx, (int)objUDxRPxWait.Value);

            objHSBxRPxFram.Maximum = m_objCurrentRCI.rTC.FrameCount - 1;
            objUDxRPxNowFrame.Maximum = m_objCurrentRCI.rTC.FrameCount - 1;
            objHSBxRPxFram.Value += 1;
            objLBxRPxFramePos.Text = "" + (m_objCurrentRCI.rTC.FrameCount - 1);
            if (m_objCurrentRCI.rTC.FrameCount > 1)
            {
                objBTxRPxDelete.Enabled = true;
                objLBxRPxDelete.Enabled = true;
            }
        }
        /// <summary>
        /// 左側へフレームを追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBTxRPxLeftAdd_Click(object sender, EventArgs e)
        {
            int idx = objHSBxRPxFram.Value;

            if (objCKxRPxPaste.Checked && m_objCurrentRCI.nCopyFrameNo != -1)
            {
                m_objCurrentRCI.CreateInsertCopy(idx, m_objCurrentRCI.nCopyFrameNo);
                if (objHSBxRPxFram.Value <= m_objCurrentRCI.nCopyFrameNo)
                    m_objCurrentRCI.nCopyFrameNo++;
            }
            else
                m_objCurrentRCI.CreateInsertNew(idx, (int)objUDxRPxWait.Value);

            objHSBxRPxFram.Maximum = m_objCurrentRCI.rTC.FrameCount - 1;
            objUDxRPxNowFrame.Maximum = m_objCurrentRCI.rTC.FrameCount - 1;
            objHSBxRPxFram.Value += 1;
            objLBxRPxFramePos.Text = "" + (m_objCurrentRCI.rTC.FrameCount - 1);
            if (m_objCurrentRCI.rTC.FrameCount > 1)
            {
                objBTxRPxDelete.Enabled = true;
                objLBxRPxDelete.Enabled = true;
            }
        }
        /// <summary>
        /// 現在のフレームを削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBTxRPxDelete_Click(object sender, EventArgs e)
        {
            if (m_objCurrentRCI.rTC.FrameCount <= 1)
            {
                objBTxRPxDelete.Enabled = false;
                objLBxRPxDelete.Enabled = false;
                return;
            }

            if (m_objCurrentRCI.nCopyFrameNo == objHSBxRPxFram.Value)
            {
                m_objCurrentRCI.nCopyFrameNo = -1;
                objCKxRPxPaste.Enabled = false;
            }
            m_objCurrentRCI.rTC.Del(objHSBxRPxFram.Value);
            objHSBxRPxFram.Maximum = m_objCurrentRCI.rTC.FrameCount - 1;
            objUDxRPxNowFrame.Maximum = m_objCurrentRCI.rTC.FrameCount - 1;
            objLBxRPxFramePos.Text = "" + (m_objCurrentRCI.rTC.FrameCount - 1);
        }
        /// <summary>
        /// フレーム単位でのコピーボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBTxRPxCopy_Click(object sender, EventArgs e)
        {
            m_objCurrentRCI.nCopyFrameNo = objHSBxRPxFram.Value;
            objCKxRPxPaste.Enabled = true;
        }
        /// <summary>
        /// 置き換えチップの作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBTxRPxNewRP_Click(object sender, EventArgs e)
        {
            if (m_objNRCF == null)
            {
                m_objNRCF = new NewReplaceChipsForm(this);
                m_objNRCF.BasicInit(m_rMap);
                m_objNRCF.Show();
                m_objNRCF.SetPos(m_currentBlockX, m_currentBlockY);
                //objXNAScreen.SetmReplaceChipFrameMode(true);
                objTABSelect.Enabled = false;
            }
        }
        /// <summary>
        /// NewReplaceChipsForm側で閉じられた　メンバ関数objBTxRPxNewRP_Clickと関連
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClosedNewReplaceChipsForm(bool bOK)
        {
            if (m_objNRCF != null)
            {
                objTABSelect.Enabled = true;
                //objXNAScreen.SetmReplaceChipFrameMode(false);
                if (bOK)
                {
                    m_rMap.SetTransPoseListViewItems(objLWxRPx);
                    objLBxRpxCount.Text = "" + m_rMap.ReplaseChipsCount;
                    objLWxRPx.Focus();
                    int idx = objLWxRPx.Items.IndexOfKey(m_objNRCF.GetName());
                    if (idx != -1)
                    {
                        objLWxRPx.Items[idx].Selected = true;
                    }
                }
                m_objNRCF = null;
            }
        }
        /// <summary>
        /// 新しく置き換え短径をobjXNAScreen側へセットする
        /// </summary>
        /// <param name="x">ブロック単位でのX位置</param>
        /// <param name="y">ブロック単位でのY位置</param>
        /// <param name="w">ブロック単位での幅</param>
        /// <param name="h">ブロック単位での高さ</param>
        public void SetNewReplaceRect(int x, int y, int w, int h)
        {
            m_currentBlockX = x;
            m_currentBlockY = y;

            //objXNAScreen.SetNewReplaceRect(
            //    x, y,
            //    w, h
            //);
        }
        /// <summary>
        /// 現在選択されている置き換えチップ削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBTxRPxDeleteReplaseChip_Click(object sender, EventArgs e)
        {
            if (objLWxRPx.SelectedItems.Count == 0) return;

            DialogResult nRet = MessageBox.Show(
                "置き換え名(" + objLWxRPx.SelectedItems[0].Text + ")を本当に削除しますか？", 
                "質問", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
            if (nRet == DialogResult.No)
                return;

            if (m_rMap.DeleteTransPose(objLWxRPx.SelectedItems[0].Text))
            {
                objLBxRpxCount.Text = "" + m_rMap.ReplaseChipsCount;
                m_rMap.SetTransPoseListViewItems(objLWxRPx);
                if (m_rMap.ReplaseChipsCount == 0)
                {
                    objBTxRPxDeleteReplaseChip.Enabled = false;
                    objGBxRPxInfoName.Text = "xxxxの情報";
                    objLBxRPxWH.Text = "----";
                    objLBxRPxPos.Text = "----";
                    objLBxRPxFrame.Text = "----";
                    // 選択中の置き換えチップの解除
                    m_objCurrentRCI.Init(null);
                    //objXNAScreen.SetReplaceChip(null);
                }
                else
                {
                    objLWxRPx.Focus();
                    objLWxRPx.Items[0].Selected = true;
                }
                // 選択状態のものを解除
                objLBxRPxName.Text = "";
                objLBxRPxFramePos.Text = "--";
                objTPAnmEdit.Enabled = false;
            }
        }
        /// <summary>
        /// 選択された置き換えマップチップのアニメーションの再生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBTxRPxPlay_Click(object sender, EventArgs e)
        {
            //objXNAScreen.SetmRPAnimationPlay(true);
        }
        /// <summary>
        /// 置き換えアニメーションの変種タブが変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTBxRPxEdit_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex != 2)
            {
                //objXNAScreen.SetmRPAnimationPlay(false);
                //objHSBxRPxFram.Value = objXNAScreen.GetSetmReplaceChipFramePos();
            }
        }
        /// <summary>
        /// マップサイズ変更する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objMIMapSize_Click(object sender, EventArgs e)
        {
            BlockCanvasForm objF = new BlockCanvasForm(m_rMap);
            objF.ShowDialog();
            if (objF.GetOK())
            {
                string name="";
                int cnt=0;
                if ( m_rMap.CheckReplaceLap(objF.GetBlockW(), objF.GetBlockH(), objF.GetCheckedNo(), out cnt, out name))
                {
                    DialogResult ret = MessageBox.Show(
                        name + "ですがよろしいですか？\n変更した場合元に戻せません。",
                        "結果",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (ret == DialogResult.No)
                    {
                        return;
                    }
                }
                m_rMap.Resize(objF.GetBlockW(), objF.GetBlockH(), objF.GetCheckedNo());
                this.ResetMap();
                // 進むボタンを無効?
                this.BackGoStateButtonEnableChange();
                this.ResizePictureboxInImage();
            }
        }

    };
}
