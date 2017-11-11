using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;
using Common;
using TileStageFormat.Tile.Square;

namespace EditorMC2D.FormMapChip
{
    public partial class ChipSelectForm : Form
    {
        private int m_currentChipNo = 0;
        private int m_selectImageIndex = 0;
        private int m_selectChipNo = 0;
        private int m_selectFlg = 0;
        private ImageSquareTile m_rCurrentImgaeFile = null;
        private List<ImageSquareTile> m_rImageFile = null;
        private MapChipEditForm m_rMCE = null;
        private AnimationChipEditForm m_rACE = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChipSelectForm(AnimationChipEditForm rACE)
        {
            this.UseConstructor();
            m_rACE = rACE;
            objGBChips.Enabled = true;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChipSelectForm(MapChipEditForm rMCE)
        {
            this.UseConstructor();
            m_rMCE = rMCE;
        }
        private void UseConstructor()
        {
            InitializeComponent();
            vScrollBar.Minimum = 0;  //最小値の設定
            vScrollBar.LargeChange = 40; //バーと左右端の矢印の間をクリックした場合の移動量
            vScrollBar.SmallChange = 10; //左右端の矢印をクリックした場合の移動量
            hScrollBar.Minimum = 0;  //最小値の設定
            hScrollBar.LargeChange = 40; //バーと左右端の矢印の間をクリックした場合の移動量
            hScrollBar.SmallChange = 10; //左右端の矢印をクリックした場合の移動量
        }
        public int GetSelectImageIndex()
        {
            return m_selectImageIndex;
        }
        public int GetSelectChipNo()
        {
            return m_selectChipNo;
        }
        public int GetSelectFlg()
        {
            return m_selectFlg;
        }
        /// <summary>
        /// 主にフォーム（ピクチャーボックス）のサイズがリサイズされたときに使用する
        /// </summary>
        private void ResizePictureboxInImage()
        {
            //m_xnaScreen.Top = 0;
            //m_xnaScreen.Left = 0;
            //m_xnaScreen.Width = splitContainer1.Panel2.Width - vScrollBar.Width;
            //m_xnaScreen.Height = splitContainer1.Panel2.Height - hScrollBar.Height;
            //// スクロールバーなど
            //vScrollBar.Location = new Point(m_xnaScreen.Width, 0);
            //vScrollBar.Height = splitContainer1.Panel2.Height - vScrollBar.Width;
            //hScrollBar.Location = new Point(0, m_xnaScreen.Height);
            //hScrollBar.Width = splitContainer1.Panel2.Width - hScrollBar.Height;

            //if (!m_xnaScreen.IsLoad()) return;
            //int nW = m_xnaScreen.ImgWidth - m_xnaScreen.Width;
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

            //int nH = m_xnaScreen.ImgHeight - m_xnaScreen.Height;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChipSelectForm_Load(object sender, EventArgs e)
        {
            //int i;
            //m_rImageFile = Common.Common.Instance.D2Stage.();
            //objCBImg.Items.Clear();

            //for (i = 0; i < m_rImageFile.Count; ++i)
            //{
            //    if (m_rImageFile[i].GetType() == ImageSquareTile.FILETYPE_CHIP)
            //    {
            //        ComboboxItem item = new ComboboxItem(i, m_rImageFile[i].GetString());
            //        objCBImg.Items.Add(item);
            //    }
            //}
            //objCBImg.SelectedIndex = 0;

            //this.DrawRadioBottonChips();

            //if (m_rMCE != null)
            //{
            //    m_rMCE.SetStaticMapChip(m_selectImageIndex,m_selectChipNo,m_selectFlg);
            //}
            //else if (m_rACE != null)
            //{
            //    m_rACE.SetStaticMapChip(m_selectImageIndex, m_selectChipNo, m_selectFlg);
            //}
            //// 表示位置の調整
            //if (m_rMCE != null)
            //{
            //    this.Location = new Point((m_rMCE.Size.Width / 2) + m_rMCE.Location.X, 0);
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChipSelectForm_Shown(object sender, EventArgs e)
        {
            this.ResizePictureboxInImage();
            // 描画タイミング
            objTimer.Interval = 66;
            objTimer.Enabled = true;
        }
        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChipSelectForm_Resize(object sender, EventArgs e)
        {
            this.ResizePictureboxInImage();
        }
        /// <summary>
        /// フォームを閉じたことを対象フォームに伝える
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChipSelectForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_rMCE != null)
            {
                m_rMCE.ClosedChipSelectForm();
            }
            else if (m_rACE != null)
            {
                m_rACE.ClosedChipSelectForm();
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
        /// ピクチャーボックス内でマウスダウン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_xnaScreen_MouseDown(object sender, MouseEventArgs e)
        {
            //int nV = vScrollBar.Value;
            //int nH = hScrollBar.Value;
            //int x, y;

            ////m_xnaScreen.Activate();

            //x = (e.X + nH) / 40;
            //if (m_xnaScreen.WidthBlock <= x)
            //    x = m_xnaScreen.WidthBlock - 1;
            //y = (e.Y + nV) / 40;
            //if (m_xnaScreen.HeightBlock <= y)
            //    y = m_xnaScreen.HeightBlock - 1;

            //m_xnaScreen.SetCurrentBlock(x, y);
            //// 現在のチップ番号
            //m_currentChipNo = x + y * m_xnaScreen.WidthBlock;
            //m_selectChipNo = m_currentChipNo;
            ////
            //objLBChipNo.Text = "No. " + m_currentChipNo;

            ////
            //this.DrawRadioBottonChips();
            //if (m_rMCE != null)
            //{
            //    m_rMCE.SetStaticMapChip(m_selectImageIndex, m_selectChipNo, m_selectFlg);
            //}
            //else if (m_rACE != null)
            //{
            //    m_rACE.SetStaticMapChip(m_selectImageIndex, m_selectChipNo, m_selectFlg);
            //}
        }
        /// <summary>
        /// 衝突選択ボタンのイメージ描画
        /// </summary>
        private void DrawRadioBottonChips()
        {
            //if (m_rCurrentImgaeFile == null) return;
            //// 
            //Rectangle rc = new Rectangle(0, 0, 40, 40);

            //Bitmap bmp = m_xnaScreen.GetCurrentBitmapChip();

            //objRBChip00.BackgroundImage = bmp;
            //objRBChip01.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    bmp, rc, FSquareTileInfo.COLLI_ROT_L90
            //);
            //objRBChip02.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    bmp, rc, FSquareTileInfo.COLLI_ROT_L180
            //);
            //objRBChip03.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    bmp, rc, FSquareTileInfo.COLLI_ROT_L270
            //);
            //objRBChip04.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    bmp, rc, FSquareTileInfo.COLLI_FLIPHORIZONTAL
            //);
            //objRBChip05.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    bmp, rc, FSquareTileInfo.COLLI_FLIPHORIZONTAL | FSquareTileInfo.COLLI_ROT_L270
            //);
            //objRBChip06.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    bmp, rc, FSquareTileInfo.COLLI_FLIPHORIZONTAL | FSquareTileInfo.COLLI_ROT_L180
            //);
            //objRBChip07.BackgroundImage = Common.Common.GetImageChipBitmap(
            //    bmp, rc, FSquareTileInfo.COLLI_FLIPHORIZONTAL | FSquareTileInfo.COLLI_ROT_L90
            //);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTimer_Tick(object sender, EventArgs e)
        {
            //m_xnaScreen.SetScrollBar(vScrollBar.Value, hScrollBar.Value);
        }
        /// <summary>
        /// イメージの選択が変わった
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCBImg_SelectedIndexChanged(object sender, EventArgs e)
        {
            //objRBChip00.Checked = true;
            //m_selectFlg = 0;
            //m_selectChipNo = 0;
            //ComboboxItem item = (ComboboxItem)objCBImg.Items[objCBImg.SelectedIndex];
            //m_selectImageIndex = item.Id;
            //m_rCurrentImgaeFile = Common.Common.Instance.D2Stage.SquareTiles.FindSquareTileFromIndex(m_selectImageIndex);

            //string strFullPas = @".\Media\materials\texture\";
            //strFullPas += m_rCurrentImgaeFile.GetString();
            //m_xnaScreen.Load(strFullPas, ref m_rCurrentImgaeFile);
            ////---------------------------------------
            ////---- 
            ////---------------------------------------
            //if (m_rMCE != null)
            //{
            //    m_rMCE.SetStaticMapChip(m_selectImageIndex, m_selectChipNo, m_selectFlg);
            //}
            //else if (m_rACE != null)
            //{
            //    m_rACE.SetStaticMapChip(m_selectImageIndex, m_selectChipNo, m_selectFlg);
            //}
        }
        private void SendXXXForm()
        {
            if (m_rMCE != null)
            {

            }
            else
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objRB_Click(object sender, EventArgs e)
        {
            if( objRBChip00.Checked )
            {
                m_selectFlg = FSquareTileInfo.COLLI_ROT_0;
            }
            else if( objRBChip01.Checked )
            {
                m_selectFlg = FSquareTileInfo.COLLI_ROT_L90;
            }
            else if( objRBChip02.Checked )
            {
                m_selectFlg = FSquareTileInfo.COLLI_ROT_L180;
            }
            else if( objRBChip03.Checked )
            {
                m_selectFlg = FSquareTileInfo.COLLI_ROT_L270;
            }
            else if( objRBChip04.Checked )
            {
                m_selectFlg = FSquareTileInfo.COLLI_FLIPHORIZONTAL;
            }
            else if( objRBChip05.Checked )
            {
                m_selectFlg = FSquareTileInfo.COLLI_FLIPHORIZONTAL;
                m_selectFlg |= FSquareTileInfo.COLLI_ROT_L90;
            }
            else if( objRBChip06.Checked )
            {
                m_selectFlg = FSquareTileInfo.COLLI_FLIPHORIZONTAL;
                m_selectFlg |= FSquareTileInfo.COLLI_ROT_L180;
            }
            else if( objRBChip07.Checked )
            {
                m_selectFlg = FSquareTileInfo.COLLI_FLIPHORIZONTAL;
                m_selectFlg |= FSquareTileInfo.COLLI_ROT_L270;
            }
            //
            if (m_rMCE != null)
            {
                m_rMCE.SetStaticMapChip(m_selectImageIndex, m_selectChipNo, m_selectFlg);
            }
            else if (m_rACE != null)
            {
                m_rACE.SetStaticMapChip(m_selectImageIndex, m_selectChipNo, m_selectFlg);
            }
        }

        private void ChipSelectForm_Activated(object sender, EventArgs e)
        {
            objTimer.Enabled = true;
        }

        private void ChipSelectForm_Deactivate(object sender, EventArgs e)
        {
            objTimer.Enabled = false;
        }

        private void checkBoxXX_Click(object sender, EventArgs e)
        {
            //m_xnaScreen.DrawColliChip = objCKColli.Checked;
            //m_xnaScreen.DrawGrid = objCKGrid.Checked;
        }

    }
}
