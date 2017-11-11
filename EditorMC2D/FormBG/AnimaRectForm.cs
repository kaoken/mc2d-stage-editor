using Common;
using EditorMC2D.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TileStageFormat.Animation.Rect;
using TileStageFormat.Tile;
using TileStageFormat.Tile.Rect;
using TileStageFormat.Tile.Square;

namespace EditorMC2D.FormBG
{
    public partial class AnimaRectForm : Form
    {
        public EventHandler CloseFormEvent;
        private List<AnimationRect> m_raAnimaRect = null;
        private ImageRect m_rIF = null;
        private static int MAX_SCREEN_W = 4096;
        private static int MAX_SCREEN_H = 4096;
        private int m_SelectAnimaRectNo = -1;
        private int m_imageFileNo = -1;
        private int m_imageRectNo = -1;
        private bool m_bInitializeing = false;
        private CommonMC2D m_com;

        private AnimationRect m_rAR = null;

        public AnimaRectForm()
        {
            InitializeComponent();
            m_com = CommonMC2D.Instance;
            vScrollBar.Minimum = 0;         //最小値の設定
            vScrollBar.LargeChange = 40;    //バーと左右端の矢印の間をクリックした場合の移動量
            vScrollBar.SmallChange = 10;    //左右端の矢印をクリックした場合の移動量
            hScrollBar.Minimum = 0;         //最小値の設定
            hScrollBar.LargeChange = 40;    //バーと左右端の矢印の間をクリックした場合の移動量
            hScrollBar.SmallChange = 10;    //左右端の矢印をクリックした場合の移動量
            mObjHSFrame.LargeChange = 1;    //バーと左右端の矢印の間をクリックした場合の移動量
            mObjHSFrame.SmallChange = 1;    //左右端の矢印をクリックした場合の移動量
            //
            BasicFormInit();
        }
        /// <summary>
        /// 
        /// </summary>
        private void BasicFormInit()
        {
            //
            mObjUDFrameNo.Value = 0;
            mObjUDFrameNo.Minimum = 0;
            mObjUDFrameNo.Maximum = 0;
            //
            mObjHSFrame.Value = 0;
            mObjHSFrame.Minimum = 0;
            mObjHSFrame.Maximum = 0;
        }
        private void AnimaRectForm_Load(object sender, EventArgs e)
        {
            // 表示位置の調整
            //this.Location = new Point(m_rMainWindow.Size.Width, 0);
            this.ResizePictureboxInImage();
            //-----------------------
            // リストビューに追加
           // m_com.GetRectImageNames(ref objCBoxImageFile);
            //-----------------------
            // アニメーションRECTの取得
            m_raAnimaRect = m_com.D2Stage.GetAnimationRects();
            if (m_raAnimaRect.Count == 0)
                mObjGBoxAEdit.Enabled = false;
            else
            {
                mObjGBoxAEdit.Enabled = true;
                int i;
                mObjLVRECT.Items.Clear();
                for (i = 0; i < m_raAnimaRect.Count; ++i)
                {
                    string[] item1 = { i + "", m_raAnimaRect[i].num+"" };
                    ListViewItem lv = new ListViewItem(item1);
                    mObjLVRECT.Items.Add(lv);
                    mObjLVRECT.Items[mObjLVRECT.Items.Count - 1].Selected = true;
                }
            }
        }
        /// <summary>
        /// このフォームを終了する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimaRectForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseFormEvent(this, e);
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
            ////mObjXNAScreen.Width = splitContainer1.Panel2.Width - vScrollBar.Width;
            ////mObjXNAScreen.Height = splitContainer1.Panel2.Height - hScrollBar.Height;
            //// スクロールバーなど
            //vScrollBar.Location = new Point(//mObjXNAScreen.Width, 0);
            //vScrollBar.Height = splitContainer1.Panel2.Height - vScrollBar.Width;
            //hScrollBar.Location = new Point(0, //mObjXNAScreen.Height);
            hScrollBar.Width = splitContainer1.Panel2.Width - hScrollBar.Height;

            //if (m_rMap == null) return;
            hScrollBar.Maximum = MAX_SCREEN_W;  //最大値の設定
            hScrollBar.Enabled = true;
            hScrollBar.Value = (MAX_SCREEN_W/2)-1;

            vScrollBar.Maximum = MAX_SCREEN_H;  //最大値の設定
            vScrollBar.Enabled = true;
            vScrollBar.Value = (MAX_SCREEN_H/2)-1;

        }        /// <summary>
        /// パネルサイズが変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainer1_Resize(object sender, EventArgs e)
        {
            mObjTab01.Height = splitContainer1.Panel1.Height;
            this.ResizePictureboxInImage();
        }
        /// <summary>
        /// TABメニューがリサイズされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjTab01_Resize(object sender, EventArgs e)
        {
            mObjGBoxAEdit.Height = mObjTab01.Height-26;
            mObjLVImageRectInfo.Height = mObjTab01.Height - 346;

            mObjLVRECT.Height = mObjTab01.SelectedTab.Size.Height-26;

            //if (mObjTab01.SelectedTab.Name == "tabPage1")
            //{

            //}
            //else if (mObjTab01.SelectedTab.Name == "tabPage2")
            //{

            //}
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
        private void mObjXNAScreen_MouseDown(object sender, MouseEventArgs e)
        {

        }
        /// <summary>
        /// XNAスクリーンでマウスがアップされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjXNAScreen_MouseUp(object sender, MouseEventArgs e)
        {

        }
        /// <summary>
        /// XNAスクリーン内でマウスが動いた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjXNAScreen_MouseMove(object sender, MouseEventArgs e)
        {

        }
        /// <summary>
        /// XNAスクリーンがリサイズされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjXNAScreen_Resize(object sender, EventArgs e)
        {
            ////mObjXNAScreen.OnResize();

        }
        /// <summary>
        /// 新規作成をクリックした
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            m_raAnimaRect.Add(new AnimationRect());
            //
            string[] item1 = { (m_raAnimaRect.Count-1) + "", "0"};
            ListViewItem lv = new ListViewItem(item1);
            mObjLVRECT.Items.Add(lv);
            mObjLVRECT.Items[mObjLVRECT.Items.Count - 1].Selected = true;
            //mObjLVRECT.Sel
        }
        /// <summary>
        /// アニメーションリストをダブルクリックした
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjLVRECT_DoubleClick(object sender, EventArgs e)
        {
            Point eM = mObjLVRECT.PointToClient(MousePosition);

            ListViewItem item = mObjLVRECT.GetItemAt(eM.X, eM.Y);
            if (item != null)
            {
                m_bInitializeing = true;

                this.BasicFormInit();
                m_SelectAnimaRectNo = int.Parse(item.Text);
                // TABメニュー移動
                mObjTab01.SelectedIndex = 1;
                m_rAR = m_raAnimaRect[m_SelectAnimaRectNo];
                mObjGBoxAEdit.Enabled = true;
                //
                mObjLFramePos.Text = m_rAR.FrameCount+"";

                // フレームが存在しないか
                if (m_rAR.FrameCount == 0)
                {
                    //
                    mObjBtnRightAdd.Enabled = false;
                    objLBRightAdd.Enabled = false;
                    mObjBtnLeftAdd.Enabled = false;
                    objLBLeftAdd.Enabled = false;
                    mObjBtnDelete.Enabled = false;
                    objLBDelete.Enabled = false;

                    objLBWait.Enabled = false;
                    mObjUDWait.Enabled = false;
                    //
                    mObjLBFileNameOut.Text = "----";
                    mObjLBFileName.Enabled = false;
                    mObjLBFileNameOut.Enabled = false;
                    mObjLBRectNoOut.Text = "--";
                    mObjLBRectNo.Enabled = false;
                    mObjLBRectNoOut.Enabled = false;


                    mObjUDFrameNo.Enabled = false;
                    mObjHSFrame.Enabled = false;
                    ////mObjXNAScreen.SetImgRectEmpty();
                }
                else
                {
                    // 0フレーム選択状態
                    mObjBtnRightAdd.Enabled = false;
                    objLBRightAdd.Enabled = false;
                    mObjBtnLeftAdd.Enabled = false;
                    objLBLeftAdd.Enabled = false;

                    objLBWait.Enabled = true;
                    mObjUDWait.Enabled = true;
                    //
                    mObjLBFileName.Enabled = true;
                    mObjLBFileNameOut.Enabled = true;
                    mObjLBRectNo.Enabled = true;
                    mObjLBRectNoOut.Enabled = true;

                    mObjUDFrameNo.Enabled = true;
                    mObjHSFrame.Enabled = true;

                    mObjUDFrameNo.Maximum = m_rAR.FrameCount - 1;
                    mObjHSFrame.Maximum = m_rAR.FrameCount - 1;
                    AnimationRectFrame rTmp = m_rAR.aARF[0];
                    m_rIF = m_com.D2Stage.FindRectFromIndex(rTmp.imageFileNo);
                    //mObjXNAScreen.SetImgRect(m_rIF, rTmp.imageRectNo);
                    mObjLBFileNameOut.Text = m_com.D2Stage.RectFilePathFromIndex((int)rTmp.imageFileNo);
                    mObjLBRectNoOut.Text = rTmp.imageRectNo+"";
               }

                m_bInitializeing = false;
            }
        }
        /// <summary>
        /// イメージRECTの選択が変わった
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCBoxImageFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            // リストビューのリストを作る
            m_rIF = m_com.D2Stage.FindRectFromFilePaht(objCBoxImageFile.Text);
            if (m_rIF == null)
            {
                return;
            }
            else if (m_rIF.GetID() != FImageRect.ID)
            {
                return;
            }
            int i;
            FImageRectInfo ffIRc = null;
            for (i = 0; i < m_rIF.aSprite.Count; ++i)
            {
                ffIRc = m_rIF.aSprite[i];
                string[] item1 = { i + "", ffIRc.x + "", ffIRc.y + "", ffIRc.width + "", ffIRc.height + "" };
                ListViewItem lvTmp = new ListViewItem(item1);
                mObjLVImageRectInfo.Items.Add(lvTmp);
            }
            //mObjXNAScreen.SetBackGroudColorChange(false);

            m_imageFileNo = m_com.D2Stage.RectImageNoFromFilePath(objCBoxImageFile.Text);
        }
        /// <summary>
        /// イメージRECTの切り出したRECTの選択が変わった
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjLVImageRectInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_rIF == null) return;
            ListViewItem item;
            ListView listView = sender as ListView;
            int idx = -1;

            if (listView.SelectedItems.Count > 0)
            {
                item = listView.SelectedItems[0];
                idx = int.Parse(item.Text);
            }
            else
            {

            }
            if (idx == -1) return;

            //mObjXNAScreen.SetImgRect(m_rIF, idx);
            //
            mObjBtnRightAdd.Enabled = true;
            objLBRightAdd.Enabled = true;
            mObjBtnLeftAdd.Enabled = true;
            objLBLeftAdd.Enabled = true;
            //
            //mObjBtnDelete.Enabled = false;
            //objLBDelete.Enabled = false;
            //
            //objLBWait.Enabled = false;
            //mObjUDWait.Enabled = false;

            //mObjXNAScreen.SetBackGroudColorChange(false);

            m_imageRectNo = idx;
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  アニメーション編集側
        //##
        //##############################################################################
        //##############################################################################
        /// <summary>
        /// フレーム番号が変化した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjUDFrameNo_ValueChanged(object sender, EventArgs e)
        {
            this.ChangedFrameNum();
        }
        /// <summary>
        /// フレーム番号の水平バーが変化した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjHSFrame_ValueChanged(object sender, EventArgs e)
        {
            this.ChangedFrameNum();
        }
        /// <summary>
        /// mObjUDFrameNo_ValueChanged(), mObjHSFrame_ValueChanged()から使用
        /// </summary>
        private void ChangedFrameNum()
        {
            if (m_bInitializeing || mObjUDFrameNo.Value == mObjHSFrame.Value) return;

            ImageRect rIF = null;
            //mObjBtnRightAdd.Enabled = false;
            //objLBRightAdd.Enabled = false;
            //mObjBtnLeftAdd.Enabled = false;
            //objLBLeftAdd.Enabled = false;
            //
            objLBWait.Enabled = true;
            mObjUDWait.Enabled = true;

            if (mObjUDFrameNo.Value != mObjHSFrame.Value)
            {
                mObjUDFrameNo.Value = mObjHSFrame.Value;
                //this.ChengeAnmFrams();
                mObjUDWait.Value = m_rAR.aARF[mObjHSFrame.Value].wait;
            }
            //mObjXNAScreen.SetBackGroudColorChange(true);
            //
            AnimationRectFrame rTmp = m_rAR.aARF[mObjHSFrame.Value];
            rIF = m_com.D2Stage.FindRectFromIndex(rTmp.imageFileNo);
            //mObjXNAScreen.SetImgRect(rIF, rTmp.imageRectNo);
            mObjLBFileNameOut.Text = m_com.D2Stage.RectFilePathFromIndex(rTmp.imageFileNo);
            mObjLBRectNoOut.Text = rTmp.imageRectNo + "";
        }



        /// <summary>
        /// mObjBtnRightAdd_Click(), mObjBtnLeftAdd_Click()
        /// 右または左へのフレーム追加
        /// </summary>
        /// <param name="nLR"></param>
        private bool LR_FrameAdd(int nLR)
        {
            mObjBtnRightAdd.Enabled = false;
            objLBRightAdd.Enabled = false;
            mObjBtnLeftAdd.Enabled = false;
            objLBLeftAdd.Enabled = false;

            mObjUDFrameNo.Enabled = true;
            mObjHSFrame.Enabled = true;
            //
            objLBWait.Enabled = true;
            mObjUDWait.Enabled = true;
            // 選択解除
            if (mObjLVImageRectInfo.SelectedItems.Count > 0)
            {
                mObjLVImageRectInfo.SelectedItems[0].Selected = false;
            }
            //mObjLVImageRectInfo.SelectedItems.Clear();



            //　アニメーション
            AnimationRectFrame tlARF = new AnimationRectFrame();
            tlARF.wait = 1;
            tlARF.imageFileNo = (short)m_imageFileNo;
            tlARF.imageRectNo = (short)m_imageRectNo;
            if (nLR == 1)
            {   // 左
                m_rAR.aARF.Insert(mObjHSFrame.Value, tlARF);
            }
            else
            {
                // 右
                int n = mObjHSFrame.Value + 1;
                if (m_rAR.FrameCount == n || m_rAR.FrameCount == 0)
                    m_rAR.aARF.Add(tlARF);
                else
                    m_rAR.aARF.Insert(n, tlARF);
            }
            int i = m_rAR.FrameCount;

            // 削除ボタンの有効
            if (i > 1)
            {
                mObjBtnDelete.Enabled = true;
                objLBDelete.Enabled = true;
            }
            // 最大値の変更
            mObjUDFrameNo.Maximum = i - 1;
            mObjHSFrame.Maximum = i - 1;
            // フレーム数の表示再設定
            mObjLFramePos.Text = "" + i;
            if (nLR == 0 && i > 1 )
            {
                // 右
                //mObjUDFrameNo.Value += 1;
                mObjHSFrame.Value += 1;
            }

            mObjUDWait.Value = m_rAR.aARF[mObjHSFrame.Value].wait;
            // リストビューに反映
            mObjLVRECT.Items[m_SelectAnimaRectNo].SubItems[1].Text = m_rAR.FrameCount + "";
            //
            m_rIF = m_com.D2Stage.FindRectFromIndex(tlARF.imageFileNo);
            //mObjXNAScreen.SetImgRect(m_rIF, tlARF.imageRectNo);
            mObjLBFileNameOut.Text = m_com.D2Stage.RectFilePathFromIndex(tlARF.imageFileNo);
            mObjLBRectNoOut.Text = tlARF.imageRectNo + "";
            return true;
        }
        /// <summary>
        /// 右側に追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjBtnRightAdd_Click(object sender, EventArgs e)
        {
            if( m_rAR == null )return;
            this.LR_FrameAdd(0);

        }
        /// <summary>
        /// 左側に追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjBtnLeftAdd_Click(object sender, EventArgs e)
        {
            if (m_rAR == null) return;
            this.LR_FrameAdd(1);

        }
        /// <summary>
        /// 現在のフレームを削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjBtnDelete_Click(object sender, EventArgs e)
        {
            if (m_rAR == null) return;
            if (m_rAR.FrameCount == 0) return;

            AnimationRectFrame rARF = null;
            ImageRect rIF = null;
            int target = mObjHSFrame.Value;

            // リストビューに反映
            if (m_rAR.FrameCount == 1)
            {
                mObjLFramePos.Text = "--";
                mObjHSFrame.Maximum = 0;
                mObjUDFrameNo.Maximum = 0;
                mObjHSFrame.Value = 0;
                mObjLVRECT.Items[m_SelectAnimaRectNo].SubItems[1].Text = "0";
                mObjLBFileNameOut.Text = "----";
                mObjLBRectNoOut.Text = "--";
                mObjBtnDelete.Enabled = false;
                objLBDelete.Enabled = false;
                mObjUDWait.Enabled = false;
                //mObjXNAScreen.SetImgRectEmpty();
            }
            else
            {
                if (target != 0)
                {
                    rARF = m_rAR.aARF[target - 1];
                    mObjHSFrame.Value -= 1;
                }
                else
                    rARF = m_rAR.aARF[target + 1];

                mObjLFramePos.Text = (m_rAR.FrameCount - 1) + "";
                mObjHSFrame.Maximum   -= 1;
                mObjUDFrameNo.Maximum -= 1;
                mObjLVRECT.Items[m_SelectAnimaRectNo].SubItems[1].Text = (m_rAR.FrameCount-1) + "";
                rIF = m_com.D2Stage.FindRectFromIndex(rARF.imageFileNo);
                //mObjXNAScreen.SetImgRect(rIF, rARF.imageRectNo);
                mObjLBFileNameOut.Text = m_com.D2Stage.RectFilePathFromIndex(rARF.imageFileNo);
                mObjLBRectNoOut.Text = rARF.imageRectNo + "";
            }

            // RECTイメージ削除
            m_rAR.Del(target);
 
       }
        /// <summary>
        /// 現在のフレームのウエイト値が変化した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mObjUDWait_ValueChanged(object sender, EventArgs e)
        {
            //
            if (m_rAR == null) return;
            m_rAR.aARF[mObjHSFrame.Value].wait = (short)mObjUDWait.Value;
            m_rAR.TimeReset();
        }
        /// <summary>
        /// イメージ表示の垂直バーが変化した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            //mObjXNAScreen.SetScrollBar(vScrollBar.Value - MAX_SCREEN_W / 2, hScrollBar.Value - MAX_SCREEN_H / 2);
        }
        /// <summary>
        /// イメージ表示の水平バーが変化した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            //mObjXNAScreen.SetScrollBar(vScrollBar.Value - MAX_SCREEN_W / 2, hScrollBar.Value - MAX_SCREEN_H / 2);
        }

        /// <summary>
        /// 再生ボタンを押した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mBtnPlay_Click(object sender, EventArgs e)
        {
            if (m_rIF == null) return;

            mObjTab01.Enabled = false;
            //mObjXNAScreen.SetBackGroudColorChange(true);
            //mObjXNAScreen.ImgRectAnmPlay(m_rAR);
        }
        /// <summary>
        /// 停止ボタンを押した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mBtnStop_Click(object sender, EventArgs e)
        {
            mObjTab01.Enabled = true;
            //mObjXNAScreen.SetBackGroudColorChange(true);
            //mObjXNAScreen.ImgRectAnmStop();
        }

    }
}
