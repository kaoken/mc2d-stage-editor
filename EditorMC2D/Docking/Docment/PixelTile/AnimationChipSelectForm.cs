using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Common;
using TileStageFormat.Tile.Square;

namespace EditorMC2D.FormMapChip
{
    public partial class AnimationChipSelectForm : Form
    {
        private const int WIDTH_BLOCK_X = 8;
        private int m_currentBlockX = 0;
        private int m_currentBlockY = 0;
        private int m_nCurrentChipNo = 0;
        private int m_nImageBlockX = 0;
        private int m_nImageBlockY = 0;
        private int m_heightBlock = 0;
        private int m_selectFlg = 0;
        private MapChipEditForm m_rMCE = null;
        private List<ImageSquareTile> m_rImageFile = null;
        private ImageSquareTile m_rCurrentImgaeFile = null;
        private Image m_CureentImg = null;

        
        public AnimationChipSelectForm(MapChipEditForm rMCE)
        {
            InitializeComponent();
            m_rMCE = rMCE;
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

            //int nH = (m_heightBlock * 40) - m_xnaScreen.Height;
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
        /// <summary>
        /// 衝突選択ボタンのイメージ描画
        /// </summary>
        private void DrawRadioBottonChips()
        {
            //if (m_rCurrentImgaeFile == null) return;
            //// 
            //Rectangle rc = new Rectangle(m_nImageBlockX * 40, m_nImageBlockY * 40, 40, 40);
            //objRBChip00.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    m_CureentImg, rc, 0
            //);
            //objRBChip01.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    m_CureentImg, rc, FSquareTileInfo.COLLI_ROT_R90
            //);
            //objRBChip02.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    m_CureentImg, rc, FSquareTileInfo.COLLI_ROT_R180
            //);
            //objRBChip03.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    m_CureentImg, rc, FSquareTileInfo.COLLI_ROT_R270
            //);
            //objRBChip04.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    m_CureentImg, rc, FSquareTileInfo.COLLI_FLIPHORIZONTAL
            //);
            //objRBChip05.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    m_CureentImg, rc, FSquareTileInfo.COLLI_FLIPHORIZONTAL | FSquareTileInfo.COLLI_ROT_R90
            //);
            //objRBChip06.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    m_CureentImg, rc, FSquareTileInfo.COLLI_FLIPHORIZONTAL | FSquareTileInfo.COLLI_ROT_R180
            //);
            //objRBChip07.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    m_CureentImg, rc, FSquareTileInfo.COLLI_FLIPHORIZONTAL | FSquareTileInfo.COLLI_ROT_R270
            //);
            ////
            //if (m_rMCE != null)
            //{
            //    m_rMCE.SetStaticAnmMapChip(m_nCurrentChipNo, (int)obuUDGroupNo.Value, m_selectFlg);
            //}
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  Form
        //##
        //##############################################################################
        //##############################################################################
        private void AnimationChipSelectForm_Load(object sender, EventArgs e)
        {
            //m_rImageFile = Common.Common.Instance.GetImageListReference();
            //// 表示位置の調整
            //this.Location = new Point((m_rMCE.Size.Width/2) + m_rMCE.Location.X, 0);
            ////
            //m_heightBlock = (int)Math.Ceiling(Common.Common.Instance.GetChipAniationsCount() / (double)WIDTH_BLOCK_X);
            ////objLBChipNum.Text = "" + CCommon.Instance.GetChipAniationsCount();

            ////
            //vScrollBar.Minimum = 0;  //最小値の設定
            //vScrollBar.LargeChange = 40; //バーと左右端の矢印の間をクリックした場合の移動量
            //vScrollBar.SmallChange = 10; //左右端の矢印をクリックした場合の移動量
        }

        private void AnimationChipSelectForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if( m_rMCE != null )
                m_rMCE.ClosedAnimationChipSelectForm();
        }

        private void AnimationChipSelectForm_Shown(object sender, EventArgs e)
        {
            MouseEventArgs eMEA = new MouseEventArgs(MouseButtons.Left, 1, 0, 0,0);
            this.objXNAScreen_MouseDown(null, eMEA);
            this.ResizePictureboxInImage();
        }

        private void AnimationChipSelectForm_Activated(object sender, EventArgs e)
        {

        }

        private void AnimationChipSelectForm_Deactivate(object sender, EventArgs e)
        {

        }

        private void AnimationChipSelectForm_Resize(object sender, EventArgs e)
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
        private void objXNAScreen_MouseDown(object sender, MouseEventArgs e)
        {
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
            //    //
            //    m_currentBlockX = nTmpX;
            //    m_currentBlockY = nTmpY;
            //    m_nCurrentChipNo = ((nTmpY * WIDTH_BLOCK_X) + nTmpX);
            //    m_xnaScreen.SetChipBlockPos(m_currentBlockX, m_currentBlockY);

            //    AnmSquareTile tlMCA = Common.Common.Instance.GetChipAnmReference(m_nCurrentChipNo);

            //    m_rCurrentImgaeFile = Common.Common.Instance.D2Stage.SquareTiles.FindSquareTileFromIndex(tlMCA.aTile[0].imageNo);
            //    string strFullPas = @".\Media\materials\texture\";
            //    strFullPas += m_rCurrentImgaeFile.GetString();
            //    m_CureentImg = Image.FromFile(strFullPas);
            //    m_nImageBlockX = tlMCA.aTile[0].tileNo % m_rCurrentImgaeFile.blockX;
            //    m_nImageBlockY = tlMCA.aTile[0].tileNo / m_rCurrentImgaeFile.blockX;
            //    this.DrawRadioBottonChips();
            //}
        }
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
        private void objRB_Click(object sender, EventArgs e)
        {
            if (objRBChip00.Checked)
            {
                m_selectFlg = FSquareTileInfo.COLLI_ROT_0;
            }
            else if (objRBChip01.Checked)
            {
                m_selectFlg = FSquareTileInfo.COLLI_ROT_R90;
            }
            else if (objRBChip02.Checked)
            {
                m_selectFlg = FSquareTileInfo.COLLI_ROT_R180;
            }
            else if (objRBChip03.Checked)
            {
                m_selectFlg = FSquareTileInfo.COLLI_ROT_R270;
            }
            else if (objRBChip04.Checked)
            {
                m_selectFlg = FSquareTileInfo.COLLI_FLIPHORIZONTAL;
            }
            else if (objRBChip05.Checked)
            {
                m_selectFlg = FSquareTileInfo.COLLI_FLIPHORIZONTAL;
                m_selectFlg |= FSquareTileInfo.COLLI_ROT_R90;
            }
            else if (objRBChip06.Checked)
            {
                m_selectFlg = FSquareTileInfo.COLLI_FLIPHORIZONTAL;
                m_selectFlg |= FSquareTileInfo.COLLI_ROT_R180;
            }
            else if (objRBChip07.Checked)
            {
                m_selectFlg = FSquareTileInfo.COLLI_FLIPHORIZONTAL;
                m_selectFlg |= FSquareTileInfo.COLLI_ROT_R270;
            }
            //
            if (m_rMCE != null)
            {
                m_rMCE.SetStaticAnmMapChip(m_nCurrentChipNo, (int)obuUDGroupNo.Value, m_selectFlg);
            }
        }
        /// <summary>
        /// グリッド表示非表示ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCKGrid_Click(object sender, EventArgs e)
        {
            //m_xnaScreen.SetDrawGrid(objCKGrid.Checked);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void obuUDGroupNo_ValueChanged(object sender, EventArgs e)
        {
            m_rMCE.SetStaticAnmMapChip(m_nCurrentChipNo, (int)obuUDGroupNo.Value, m_selectFlg);
        }
    }
}
