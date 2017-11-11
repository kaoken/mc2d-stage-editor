using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TileStageFormat.Tile.Square;

namespace EditorMC2D.FormMapChip
{
    public partial class AnimationChipEditForm : Form
    {
        public EventHandler CloseFormEvent;
        private const int WIDTH_BLOCK_X = 8;
        private int m_currentBlockX = 0;
        private int m_currentBlockY = 0;
        private int m_currentChipNo = 0;
        private int m_heightBlock = 0;
        private int m_chipWriteImageNo = 0;
        private int m_chipWriteChipNo = 0;
        private int m_chipWriteFlg = 0;
        private bool m_bCreateAnm = false;
        private bool m_bEditChipAnm = false;
        private bool m_anmChipData = false;
        private List<ImageSquareTile> m_rImageFile = null;
        private MainWindow m_rMainWindow = null;
        private ChipSelectForm m_rChipSF = null;


        public AnimationChipEditForm(MainWindow rMainWindow)
        {
            InitializeComponent();
            m_rMainWindow = rMainWindow;
            //m_xnaScreen.SetAnimationChipEditForm(this);
            //
            obhHSFrame.Minimum = 0;  //最小値の設定
            obhHSFrame.LargeChange = 1; //バーと左右端の矢印の間をクリックした場合の移動量
            obhHSFrame.SmallChange = 1; //左右端の矢印をクリックした場合の移動量
            vScrollBar.Minimum = 0;  //最小値の設定
            vScrollBar.LargeChange = 40; //バーと左右端の矢印の間をクリックした場合の移動量
            vScrollBar.SmallChange = 10; //左右端の矢印をクリックした場合の移動量

        }
        /// <summary>
        /// 
        /// </summary>
        private void SetAnmChipFromChange()
        {
            //m_bEditChipAnm = true;
            //AnmSquareTile tlMCA;
            //int n;
            //if (m_anmChipData == true)
            //{
            //    tlMCA = Common.Common.Instance.GetChipAnmReference(m_currentChipNo);
            //    objLBCHipNo.Text = "" + m_currentChipNo;
            //    objLBFrameNum.Text = "" + tlMCA.ChipCount;
            //    obhHSFrame.Maximum = tlMCA.ChipCount-1;
            //    obhUDFrameNo.Maximum = tlMCA.ChipCount - 1;
            //}
            //else
            //{
            //    objLBCHipNo.Text = "0";
            //    objLBFrameNum.Text = "0";
            //    obhHSFrame.Maximum = 0;
            //    obhUDFrameNo.Maximum = 0;
            //}
            //objGBFrame.Enabled = true;
            //this.ChengeAnmFrams();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ClosedChipSelectForm()
        {
            if (m_rChipSF != null)
                m_rChipSF = null;
        }
        /// <summary>
        /// ChipSelectFormより　選択されたチップ情報が送られてくる
        /// </summary>
        /// <param name="imageNo"></param>
        /// <param name="tileNo"></param>
        /// <param name="flag"></param>
        public void SetStaticMapChip(int imageNo, int tileNo, int flag)
        {
            m_chipWriteImageNo = imageNo;
            m_chipWriteChipNo = tileNo;
            m_chipWriteFlg = flag;
            //objXNAScreen.SetSelectStaticChip(imageNo, tileNo, flag);
            //objPBTargetChip.Image = m_xnaScreen.GetImageChipBitmap(imageNo, tileNo, flag);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ChengeAnmFrams()
        {
            //if (!m_bEditChipAnm || !m_anmChipData) return;
            //m_heightBlock = (int)Math.Ceiling(Common.Common.Instance.GetChipAniationsCount() / (double)WIDTH_BLOCK_X);
            //AnmSquareTile tlMCA = Common.Common.Instance.GetChipAnmReference(m_currentChipNo);
            //AnmSquareTileFrame rMCAF = tlMCA.aTile[obhHSFrame.Value];
            //if (0 == obhHSFrame.Value)
            //{
            //    objLBF00.Text = "X";
            //    objPBF00.Image = Common.Common.Instance.GetCollisionChipBitmap(0, 0);
            //}
            //else
            //{
            //    objLBF00.Text = "" + (obhHSFrame.Value - 1);
            //    rMCAF = tlMCA.aTile[obhHSFrame.Value-1];

            //    objPBF00.Image = m_xnaScreen.GetImageChipBitmap(rMCAF.imageNo, rMCAF.tileNo, rMCAF.transformFlg);
            //}

            //if ((tlMCA.ChipCount - 1) == obhHSFrame.Value)
            //{
            //    objLBF02.Text = "X";
            //    objPBF02.Image = Common.Common.Instance.GetCollisionChipBitmap(0, 0);

            //}
            //else
            //{
            //    objLBF02.Text = "" + (obhHSFrame.Value + 1);
            //    rMCAF = tlMCA.aTile[obhHSFrame.Value + 1];
            //    objPBF02.Image = m_xnaScreen.GetImageChipBitmap(rMCAF.imageNo, rMCAF.tileNo, rMCAF.transformFlg);
            //}

            //objLBF01.Text = "" + obhHSFrame.Value;
            //objUDWait.Value = tlMCA.aTile[obhHSFrame.Value].wait;
            //rMCAF = tlMCA.aTile[obhHSFrame.Value];
            //objPBF01.Image = m_xnaScreen.GetImageChipBitmap(rMCAF.imageNo, rMCAF.tileNo, rMCAF.transformFlg);
        }
        /// <summary>
        /// 主にフォーム（ピクチャーボックス）のサイズがリサイズされたときに使用する
        /// </summary>
        private void ResizePictureboxInImage()
        {
            //m_xnaScreen.Width = splitContainer1.Panel2.Width - vScrollBar.Width;
            //m_xnaScreen.Height = splitContainer1.Panel2.Height;
            //// スクロールバーなど
            //vScrollBar.Location = new Point(m_xnaScreen.Width, 0);
            //vScrollBar.Height = splitContainer1.Panel2.Height;

            //int nH = (m_heightBlock*40) - m_xnaScreen.Height;
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
        /// 
        /// </summary>
        /// <param name="time"></param>
        public void objXNAScreen_Call(float time)
        {
            //objLBCHipNo.Text = objXNAScreen.GetFPS()+ " s";
        }
        private void AnimationChipEditForm_Load(object sender, EventArgs e)
        {
            //m_rImageFile = Common.Common.Instance.GetImageListReference();
            //// 表示位置の調整
            //this.Location = new Point(m_rMainWindow.Size.Width, 0);
            ////
            //m_heightBlock = (int)Math.Ceiling(Common.Common.Instance.GetChipAniationsCount() / (double)WIDTH_BLOCK_X);
            //objLBChipNum.Text = "" + Common.Common.Instance.GetChipAniationsCount();
            //if (m_heightBlock != 0)
            //{
            //    m_anmChipData = true;
            //    this.SetAnmChipFromChange();
            //}
            //this.ResizePictureboxInImage();
        }
        private void AnimationChipEditForm_Shown(object sender, EventArgs e)
        {
            m_rChipSF = new ChipSelectForm(this);
            m_rChipSF.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimationChipEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseFormEvent(this, e);
            if (m_rChipSF != null)
                m_rChipSF.Close();
        }

        private void AnimationChipEditForm_Resize(object sender, EventArgs e)
        {
            this.ResizePictureboxInImage();
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  XNA
        //##
        //##############################################################################
        //##############################################################################
        /// <summary>
        /// XNAスクリーン内でマウスダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objXNAScreen_MouseDown(object sender, MouseEventArgs e)
        {
            //if (!m_bEditChipAnm || !m_anmChipData) return;
            //int nV = vScrollBar.Value;
            //int nTmpX, nTmpY;

            //nTmpX = (e.X) / 40;
            //if (WIDTH_BLOCK_X <= m_currentBlockX)
            //    nTmpX = WIDTH_BLOCK_X - 1;
            //nTmpY = (e.Y + nV) / 40;
            //if (m_heightBlock <= m_currentBlockY)
            //    nTmpY = m_heightBlock - 1;

            //if (Common.Common.Instance.GetChipAniationsCount() > ((nTmpY * WIDTH_BLOCK_X) + nTmpX))
            //{
            //    objMNCreateChip.Enabled = true;
            //    //
            //    m_currentBlockX = nTmpX;
            //    m_currentBlockY = nTmpY;
            //    m_currentChipNo = ((nTmpY * WIDTH_BLOCK_X) + nTmpX);
            //    m_xnaScreen.SetChipBlockPos(m_currentBlockX, m_currentBlockY);

            //    AnmSquareTile tlMCA = Common.Common.Instance.GetChipAnmReference(m_currentChipNo);
            //    obhHSFrame.Maximum = tlMCA.ChipCount-1;
            //    obhUDFrameNo.Maximum = tlMCA.ChipCount-1;
            //    obhHSFrame.Value = 0;
            //    obhUDFrameNo.Value = 0;
            //    // 現在選択されているマップチップ番号
            //    objLBCHipNo.Text = "" + m_currentChipNo;
            //    this.ChengeAnmFrams();
            //    //
            //    bool bDel = true;
            //    if (tlMCA.ChipCount == 1) bDel = false;

            //    objBtnDelete.Enabled = bDel;
            //    objLBDelete.Enabled = bDel;
            //    objBtnLeftAdd.Enabled = true;
            //    objLBLeftAdd.Enabled = true;
            //    obhUDFrameNo.Enabled = true;
            //    objUDWait.Enabled = true;
            //}
        }
        /// <summary>
        /// リサイズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objXNAScreen_Resize(object sender, EventArgs e)
        {
            //m_xnaScreen.OnResize();
        }

        //##############################################################################
        //##############################################################################
        //##
        //##  
        //##
        //##############################################################################
        //##############################################################################
        /// <summary>
        /// アニメーションチップ新規作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objMNCreateChip_Click(object sender, EventArgs e)
        {
            //if (m_anmChipData == false)
            //{
            //    this.SetAnmChipFromChange();
            //}
            //objMNCreateChip.Enabled = false;
            //m_bCreateAnm = true;
            //if (!m_bEditChipAnm) return;

            //objLBF00.Text = "X";
            //objPBF00.Image = Common.Common.Instance.GetCollisionChipBitmap(0, 0);
            //objLBF01.Text = "X";
            //objPBF01.Image = Common.Common.Instance.GetCollisionChipBitmap(0, 0);
            //objLBF02.Text = "X";
            //objPBF02.Image = Common.Common.Instance.GetCollisionChipBitmap(0, 0);
            ////
            //objBtnDelete.Enabled = false;
            //objLBDelete.Enabled = false;
            //objBtnLeftAdd.Enabled = false;
            //objLBLeftAdd.Enabled = false;
            //obhUDFrameNo.Enabled = false;
            ////objUDWait.Enabled = false;
            ////
            //m_currentChipNo = Common.Common.Instance.GetChipAniationsCount();
            //m_currentBlockX = m_currentChipNo % WIDTH_BLOCK_X;
            //m_currentBlockY = m_currentChipNo / WIDTH_BLOCK_X;
            //m_xnaScreen.SetChipBlockPos(m_currentBlockX, m_currentBlockY);
        }
        /// <summary>
        /// フレームスクロールの値が変化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void obhHSFrame_ValueChanged(object sender, EventArgs e)
        {
            if (!m_bEditChipAnm) return;
            if (obhUDFrameNo.Value != obhHSFrame.Value)
            {
                obhUDFrameNo.Value = obhHSFrame.Value;
                this.ChengeAnmFrams();
            }
        }
        /// <summary>
        /// フレームスクロールの値が変化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void obhUDFrameNo_ValueChanged(object sender, EventArgs e)
        {
            if (!m_bEditChipAnm || !m_anmChipData) return;
            if (obhUDFrameNo.Value != obhHSFrame.Value)
            {
                obhHSFrame.Value = (int)obhUDFrameNo.Value;
                this.ChengeAnmFrams();
            }
        }
        /// <summary>
        /// 1フレームのウエイト値が変化した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objUDWait_ValueChanged(object sender, EventArgs e)
        {
            //if (!m_bEditChipAnm || !m_anmChipData) return;
            //AnmSquareTile tlMCA = Common.Common.Instance.GetChipAnmReference(m_currentChipNo);
            //AnmSquareTileFrame rMCAF = tlMCA.aTile[obhHSFrame.Value];
            //rMCAF.wait = (short)objUDWait.Value;
            //tlMCA.TimeReset();
            //m_xnaScreen.AnimationReset();
        }
        /// <summary>
        /// 右側へチップを追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBtnRightAdd_Click(object sender, EventArgs e)
        {
            //bool bCreateAnm = false;
            //AnmSquareTile tlMCA;
            //AnmSquareTileFrame tlMCAF = new AnmSquareTileFrame();

            //tlMCAF.imageNo = (short)m_chipWriteImageNo;
            //tlMCAF.tileNo = (short)m_chipWriteChipNo;
            //tlMCAF.transformFlg = (byte)m_chipWriteFlg;

            //if (m_bCreateAnm)
            //{
            //    m_anmChipData = true;
            //    bCreateAnm = true;
            //    objMNCreateChip.Enabled = true;
            //    m_bCreateAnm = false;
            //    objBtnLeftAdd.Enabled = true;
            //    objLBLeftAdd.Enabled = true;
            //    obhUDFrameNo.Enabled = true;
            //    objUDWait.Enabled = true;
            //    tlMCA = new AnmSquareTile();

            //    tlMCAF.wait = (short)objUDWait.Value;
            //    tlMCA.Add(tlMCAF);
            //    tlMCA.period = tlMCAF.time;
            //    Common.Common.Instance.ChipAnmAdd(tlMCA);
            //    // 現在選択されているマップチップ番号
            //    objLBCHipNo.Text = "" + m_currentChipNo;
            //    // チップアニメーションの総数更新
            //    objLBChipNum.Text = ""+ Common.Common.Instance.GetChipAniationsCount();
            //}
            //else
            //{
            //    tlMCA = Common.Common.Instance.GetChipAnmReference(m_currentChipNo);
            //    tlMCAF.wait = (short)objUDWait.Value;
            //    int nR = obhHSFrame.Value + 1;
            //    if (tlMCA.ChipCount == nR)
            //        tlMCA.Add(tlMCAF);
            //    else
            //        tlMCA.Insert(nR, tlMCAF);
            //}
            //// 削除ボタンの有効
            //if (tlMCA.ChipCount > 1)
            //{
            //    objBtnDelete.Enabled = true;
            //    objLBDelete.Enabled = true;
            //}
            //// 最大値の変更
            //obhHSFrame.Maximum = tlMCA.ChipCount - 1;
            //obhUDFrameNo.Maximum = tlMCA.ChipCount - 1;
            //// フレーム数の表示再設定
            //objLBFrameNum.Text = "" + tlMCA.ChipCount;
            ////
            //if (!bCreateAnm)
            //    obhHSFrame.Value += 1;
            

            //tlMCA.TimeReset();
            //this.ChengeAnmFrams();
            //m_xnaScreen.AnimationResize();
        }
        /// <summary>
        /// 左側へチップを追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBtnLeftAdd_Click(object sender, EventArgs e)
        {
            //AnmSquareTile tlMCA;
            //AnmSquareTileFrame tlMCAF = new AnmSquareTileFrame();

            //tlMCAF.imageNo = (short)m_chipWriteImageNo;
            //tlMCAF.tileNo = (short)m_chipWriteChipNo;
            //tlMCAF.transformFlg = (byte)m_chipWriteFlg;

            //tlMCA = Common.Common.Instance.GetChipAnmReference(m_currentChipNo);
            //tlMCAF.wait = (short)objUDWait.Value;
            //int nL = obhHSFrame.Value;
            //tlMCA.Insert(nL, tlMCAF);

            //if (tlMCA.ChipCount > 1)
            //{
            //    objBtnDelete.Enabled = true;
            //    objLBDelete.Enabled = true;
            //}
            //// 最大値の変更
            //obhHSFrame.Maximum = tlMCA.ChipCount - 1;
            //obhUDFrameNo.Maximum = tlMCA.ChipCount - 1;
            //// フレーム数の表示再設定
            //objLBFrameNum.Text = "" + tlMCA.ChipCount;

            //tlMCA.TimeReset();
            //this.ChengeAnmFrams();
            //m_xnaScreen.AnimationResize();
        }
        /// <summary>
        /// 現在のマップチップ削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBtnDelete_Click(object sender, EventArgs e)
        {
            //AnmSquareTile tlMCA;
            //int nPos;

            //tlMCA = Common.Common.Instance.GetChipAnmReference(m_currentChipNo);
            //tlMCA.Del(obhHSFrame.Value);

            //tlMCA.TimeReset();
            //// フレーム数のリセット
            //nPos = obhHSFrame.Value-1;
            //if (nPos < 0) nPos = 0;

            //// 最大値の変更
            //obhHSFrame.Maximum = tlMCA.ChipCount - 1;
            //obhUDFrameNo.Maximum = tlMCA.ChipCount - 1;
            //// 位置の変更
            //obhHSFrame.Value = nPos;
            //obhUDFrameNo.Value = nPos;
            //// 削除ボタン無効
            //if (tlMCA.ChipCount == 1)
            //{
            //    objBtnDelete.Enabled = false;
            //    objLBDelete.Enabled = false;
            //}
            //// フレーム数の表示再設定
            //objLBFrameNum.Text = "" + tlMCA.ChipCount;

            //this.ChengeAnmFrams();
            //m_xnaScreen.AnimationResize();
        }

    }
}
