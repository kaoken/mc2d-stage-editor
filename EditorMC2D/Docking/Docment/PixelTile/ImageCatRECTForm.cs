using EditorMC2D.Common;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TileStageFormat.Tile;
using TileStageFormat.Tile.Rect;

namespace EditorMC2D.FormMapChip
{
    public partial class ImageCatRECTForm : Form
    {
        public EventHandler CloseFormEvent;
        private bool m_bSelectingRect = false;
        private int m_nCurrentScaleIdx = 0;
        private int m_selectRectIndex = 0;
        private MainWindow m_rMainWindow = null;
        private ImageRect m_rIF = null;
        private bool m_bMouseDown = false;
        private FImageRectInfo m_rFFIRect = null;
        private Rectangle m_SelectRc = new Rectangle();
        private Point m_SelectStartPoint = new Point();

        public ImageCatRECTForm(MainWindow rMainWindow)
        {
            InitializeComponent();
            vScrollBar.Minimum = 0;  //最小値の設定
            vScrollBar.LargeChange = 10; //バーと左右端の矢印の間をクリックした場合の移動量
            vScrollBar.SmallChange = 1; //左右端の矢印をクリックした場合の移動量
            hScrollBar.Minimum = 0;  //最小値の設定
            hScrollBar.LargeChange = 10; //バーと左右端の矢印の間をクリックした場合の移動量
            hScrollBar.SmallChange = 1; //左右端の矢印をクリックした場合の移動量
            m_rMainWindow = rMainWindow;
        }

        public void SetImageName(string strFileName)
        {
            m_rIF = CommonMC2D.Instance.D2Stage.FindRectFromFilePaht(strFileName);
            //objXNAScreen.SetImage(strFileName);
            //　イメージ情報
            objLBImageName.Text = ""+strFileName;
            objLBWidth.Text = "" + m_rIF.width;
            objLBHeight.Text = "" + m_rIF.height;
            objLBRectNum.Text = ""+m_rIF.aSprite.Count;

            objUDRectX.Minimum = objUDRectWidth.Minimum = objUDRectY.Minimum = objUDRectHeight.Minimum = 0;
            objUDRectX.Maximum = objUDRectWidth.Maximum = m_rIF.width;
            objUDRectY.Maximum = objUDRectHeight.Maximum = m_rIF.height;
            //-----------------------
            // リストビューに追加
            FImageRectInfo ffIRc = null;
            for (int i = 0; i < m_rIF.aSprite.Count; ++i )
            {
                ffIRc = m_rIF.aSprite[i];
                string[] item1 = { i + "", ffIRc.x + "", ffIRc.y + "", ffIRc.width + "", ffIRc.height + "" };
                ListViewItem lvTmp = new ListViewItem(item1);
                objLVRECT.Items.Add(lvTmp);
            }
            if (m_rIF.aSprite.Count != 0)
            {
                objLVRECT.Focus();
                objLVRECT.Items[0].Selected = true;
                // RECR num
                objLBRectNum.Text = "" + m_rIF.aSprite.Count;
            }
        }
        protected void ResetRectState()
        {
            if (m_rFFIRect == null) return;
            objUDRectX.Value = m_SelectRc.X = m_rFFIRect.x;
            objUDRectWidth.Value = m_SelectRc.Width = m_rFFIRect.width;
            objUDRectY.Value = m_SelectRc.Y = m_rFFIRect.y;
            objUDRectHeight.Value = m_SelectRc.Height = m_rFFIRect.height;
            // XNA側にRECTデータを渡す
            //objXNAScreen.SetSelectRECT(m_SelectRc);
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
            //objXNAScreen.Width = splitContainer1.Panel2.Width - vScrollBar.Width;
            //objXNAScreen.Height = splitContainer1.Panel2.Height - hScrollBar.Height;
            //// スクロールバーなど
            //vScrollBar.Location = new Point(objXNAScreen.Width, 0);
            //vScrollBar.Height = splitContainer1.Panel2.Height - vScrollBar.Width;
            //hScrollBar.Location = new Point(0, objXNAScreen.Height);
            //hScrollBar.Width = splitContainer1.Panel2.Width - hScrollBar.Height;

            //int nS = (int)Math.Pow(2, m_nCurrentScaleIdx);
            //if (m_rIF == null) return;
            //int nW = m_rIF.width * nS - objXNAScreen.Width;
            //if (nW > 0)
            //{
            //    hScrollBar.Maximum = nW + hScrollBar.LargeChange - hScrollBar.SmallChange;  //最大値の設定
            //    hScrollBar.Enabled = true;
            //}
            //else
            //{
            //    hScrollBar.Maximum = 0;  //最大値の設定
            //    hScrollBar.Enabled = false;
            //}
            //hScrollBar.Value = 0;

            //int nH = m_rIF.height * nS - objXNAScreen.Height;
            //if (nH > 0)
            //{
            //    vScrollBar.Maximum = nH + vScrollBar.LargeChange - vScrollBar.SmallChange;  //最大値の設定
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
        private void ImageCatRECTForm_Load(object sender, EventArgs e)
        {
            // 表示位置の調整
            this.Location = new Point(m_rMainWindow.Size.Width, 0);

        }

        private void ImageCatRECTForm_Shown(object sender, EventArgs e)
        {
            // 描画タイミング
            objTimer.Interval = 33;
            objTimer.Enabled = true;
            this.ResizePictureboxInImage();
            objCBScale.SelectedIndex = 0;
            //
            objGBRectInfo.Enabled = false;
        }

        private void ImageCatRECTForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseFormEvent(this, e);
        }

        private void ImageCatRECTForm_Resize(object sender, EventArgs e)
        {

        }

        private void objTimer_Tick(object sender, EventArgs e)
        {
            //objXNAScreen.SetScrollBar(vScrollBar.Value, hScrollBar.Value);
        }

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            this.ResizePictureboxInImage();
        }
        /// <summary>
        /// スケール値が変わった
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCBScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_nCurrentScaleIdx != objCBScale.SelectedIndex)
            {
                m_nCurrentScaleIdx = objCBScale.SelectedIndex;
                //objXNAScreen.SetCurrentScale(m_nCurrentScaleIdx);
                this.ResizePictureboxInImage();
            }
        }
        /// <summary>
        /// １つRECTを作るメニューボタンを押した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objMOneNewRECT_Click(object sender, EventArgs e)
        {
            FImageRectInfo ffIRc = new FImageRectInfo();
            m_rIF.aSprite.Add(ffIRc);

            string[] item1 = { m_rIF.aSprite.Count + "", ffIRc.x + "", ffIRc.y + "", ffIRc.width + "", ffIRc.height + "" };
            ListViewItem lvTmp = new ListViewItem(item1);
            objLVRECT.Items.Add(lvTmp);
            objLVRECT.Focus();
            objLVRECT.Items[objLVRECT.Items.Count-1].Selected = true;
            // RECR num
            objLBRectNum.Text = "" + m_rIF.aSprite.Count;
            this.ResetRectState();
        }
        /// <summary>
        /// リストビューの選択が変わった
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objLVRECT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objLVRECT.Items.Count == 0) return;
            if (objLVRECT.SelectedItems.Count == 0) return;
            m_bSelectingRect = true;
            objGBRectInfo.Enabled = true;

            m_selectRectIndex = int.Parse(objLVRECT.SelectedItems[0].SubItems[0].Text);
            m_rFFIRect = m_rIF.aSprite[m_selectRectIndex];

            this.ResetRectState();
        }

        private void objCKRectOnlyDraw_CheckedChanged(object sender, EventArgs e)
        {
            //objXNAScreen.SetRectOnlyDraw(objCKRectOnlyDraw.Checked);
        }
        /// <summary>
        /// XのUpDown値が変化した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objUDRectX_ValueChanged(object sender, EventArgs e)
        {
            m_SelectRc.X = m_rFFIRect.x = (short)objUDRectX.Value;
            objLVRECT.SelectedItems[0].SubItems[1].Text = m_rFFIRect.x + "";
            // XNA側にRECTデータを渡す
            //objXNAScreen.SetSelectRECT(m_SelectRc);
        }

        private void objUDRectY_ValueChanged(object sender, EventArgs e)
        {
            m_SelectRc.Y = m_rFFIRect.y = (short)objUDRectY.Value;
            objLVRECT.SelectedItems[0].SubItems[2].Text = m_rFFIRect.y + "";
            // XNA側にRECTデータを渡す
            //objXNAScreen.SetSelectRECT(m_SelectRc);
        }

        private void objUDRectW_ValueChanged(object sender, EventArgs e)
        {
            m_SelectRc.Width = m_rFFIRect.width = (short)objUDRectWidth.Value;
            objLVRECT.SelectedItems[0].SubItems[3].Text = m_rFFIRect.width + "";
            // XNA側にRECTデータを渡す
            //objXNAScreen.SetSelectRECT(m_SelectRc);
        }

        private void objUDRectH_ValueChanged(object sender, EventArgs e)
        {
            m_SelectRc.Height = m_rFFIRect.height = (short)objUDRectHeight.Value;
            objLVRECT.SelectedItems[0].SubItems[4].Text = m_rFFIRect.height + "";
            // XNA側にRECTデータを渡す
            //objXNAScreen.SetSelectRECT(m_SelectRc);
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  objXNAScreen
        //##
        //##############################################################################
        //##############################################################################

        private void objXNAScreen_Resize(object sender, EventArgs e)
        {
            //objXNAScreen.OnResize();
        }

        private void objXNAScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (objCKRectOnlyDraw.Checked) return;
            int nS = (int)Math.Pow(2, m_nCurrentScaleIdx);
            int x = ((e.X + hScrollBar.Value) / nS);
            int y = ((e.Y + vScrollBar.Value) / nS);
            x = x < 0 ? 0 : x;
            y = y < 0 ? 0 : y;
            x = x >= m_rIF.width ? m_rIF.width-1 : x;
            y = y >= m_rIF.height ? m_rIF.width-1 : y;
            objLBMouseX.Text = "" + x;
            objLBMouseY.Text = "" + y;

            if (m_bMouseDown && m_bSelectingRect && m_rFFIRect != null)
            {
                // X
                if (m_SelectStartPoint.X < x)
                {
                    m_rFFIRect.x = (short)m_SelectStartPoint.X;
                    m_rFFIRect.width = (short)(x - m_SelectStartPoint.X+1);
                }
                else
                {
                    m_rFFIRect.x = (short)x;
                    m_rFFIRect.width = (short)(m_SelectStartPoint.X - x+1);
                }
                // Y
                if (m_SelectStartPoint.Y < y)
                {
                    m_rFFIRect.y = (short)m_SelectStartPoint.Y;
                    m_rFFIRect.height = (short)(y - m_SelectStartPoint.Y+1);
                }
                else
                {
                    m_rFFIRect.y = (short)y;
                    m_rFFIRect.height = (short)(m_SelectStartPoint.Y - y+1);
                }
                if (m_rFFIRect.x < 0) m_rFFIRect.x = 0;
                if (m_rFFIRect.y < 0) m_rFFIRect.y = 0;
                if (m_rFFIRect.width > m_rIF.width) m_rFFIRect.x = (short)m_rIF.width;
                if (m_rFFIRect.height > m_rIF.height) m_rFFIRect.y = (short)m_rIF.height;
                //
                objLVRECT.SelectedItems[0].SubItems[1].Text = m_rFFIRect.x + "";
                objLVRECT.SelectedItems[0].SubItems[2].Text = m_rFFIRect.y + "";
                objLVRECT.SelectedItems[0].SubItems[3].Text = m_rFFIRect.width + "";
                objLVRECT.SelectedItems[0].SubItems[4].Text = m_rFFIRect.height + "";
                //
                this.ResetRectState();
            }
        }
        private void objXNAScreen_MouseDown(object sender, MouseEventArgs e)
        {
            //objXNAScreen.Focus();
            if (objCKRectOnlyDraw.Checked) return;
            if (m_bSelectingRect)
            {
                m_bMouseDown = true;
                int nS = (int)Math.Pow(2, m_nCurrentScaleIdx);
                int x = ((e.X + hScrollBar.Value) / nS);
                int y = ((e.Y + vScrollBar.Value) / nS);
                m_SelectStartPoint.X = x;
                m_SelectStartPoint.Y = y;
            }
        }

        private void objXNAScreen_MouseUp(object sender, MouseEventArgs e)
        {
            if (objCKRectOnlyDraw.Checked) return;
            m_bMouseDown = false;
        }

        private void objXNAScreen_MouseLeave(object sender, EventArgs e)
        {

        }
    }
}
