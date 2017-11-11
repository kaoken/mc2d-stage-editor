using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EditorMC2D.FormMapChip
{
    public partial class ImageRectMapForm : Form
    {
        public EventHandler CloseFormEvent;

        public ImageRectMapForm()
        {
            InitializeComponent();
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  
        //##
        //##############################################################################
        //##############################################################################
        /// <summary>
        /// 指定されたイメージRECTマップ名をセットし読み込む
        /// </summary>
        /// <param name="strMapName">イメージRECTマップ名</param>
        public void SetMap(string strMapName)
        {
            //m_rMap = CCommon.Instance.GetStageMapReference(strMapName);
            //m_objCurrentRCI.Init(null);
            //m_objRMapchip.Init();
            //this.ResetMap();
            ////
            //objXNAScreen.BasicInit(m_rMap, m_imgWidthBlock, m_imgHeightBlock, this);
            //// 置換用の初期化
            //m_rMap.SetTransPoseListViewItems(objLWxRPx);
            //objLBxRpxCount.Text = "" + m_rMap.TransPoseCount;
            //// マウスを押したことにする
            //this.objXNAScreen_MouseDown(null, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
        }
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

            ////if (m_rMap == null) return;
            ////int nW = m_imgWidth - objXNAScreen.Width;
            ////if (nW > 0)
            ////{
            ////    hScrollBar.Maximum = nW + 40;  //最大値の設定
            ////    hScrollBar.Enabled = true;
            ////}
            ////else
            ////{
            ////    hScrollBar.Maximum = 0;  //最大値の設定
            ////    hScrollBar.Enabled = false;
            ////}
            ////hScrollBar.Value = 0;

            ////int nH = m_imgHeight - objXNAScreen.Height;
            ////if (nH > 0)
            ////{
            ////    vScrollBar.Maximum = nH;  //最大値の設定
            ////    vScrollBar.Enabled = true;
            ////}
            ////else
            ////{
            ////    vScrollBar.Enabled = false;
            ////}
            ////vScrollBar.Value = 0;
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
        private void ImageRectMapForm_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// リサイズされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageRectMapForm_Resize(object sender, EventArgs e)
        {
            this.ResizePictureboxInImage();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageRectMapForm_Shown(object sender, EventArgs e)
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
        private void ImageRectMapForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            objTimer.Enabled = false;
            CloseFormEvent(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTimer_Tick(object sender, EventArgs e)
        {

        }

        private void objLVRECT_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        }

        private void objXNAScreen_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void objXNAScreen_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void objXNAScreen_MouseLeave(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// XNAスクリーンがリサイズされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objXNAScreen_Resize(object sender, EventArgs e)
        {
            this.ResizePictureboxInImage();
        }
        /// <summary>
        /// Rectイメージの追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRectImgAdd_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 指定されているRectイメージの削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRectImgDel_Click(object sender, EventArgs e)
        {

        }
    }
}
