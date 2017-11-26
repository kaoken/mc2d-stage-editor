using Common;
using KaokenFileFormat.Format;
using EditorMC2D.Common;
using EditorMC2D.UtilForm;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TileStageFormat.Tile.Square;
using WeifenLuo.WinFormsUI.Docking;
using UtilSharpDX.Math;

namespace EditorMC2D.Document.PixelTile
{

    /// <summary>
    /// </summary>
    /// <remarks>タイル一つ一つの情報を入力するためのもの</remarks>
    public partial class SquareDoc : DockContent
    {
        public EventHandler CloseFormEvent;
        private string m_fileName = "";  // ファイル名
        private ImageAttributes m_IA = null;
        private int m_currentChipNo = 0;
        private ImageSquareTile m_rImgaeFile = null;
        private int m_currentTileFlgNo = 0;
        /// <summary> 
        /// コンストラクタ
        /// </summary>
        public SquareDoc()
        {
            InitializeComponent();
            vScrollBar.Minimum = 0;  //最小値の設定
            vScrollBar.LargeChange = 40; //バーと左右端の矢印の間をクリックした場合の移動量
            vScrollBar.SmallChange = 10; //左右端の矢印をクリックした場合の移動量
            hScrollBar.Minimum = 0;  //最小値の設定
            hScrollBar.LargeChange = 40; //バーと左右端の矢印の間をクリックした場合の移動量
            hScrollBar.SmallChange = 10; //左右端の矢印をクリックした場合の移動量
            //lvTileFlags.SelectedIndex = m_currentTileFlgNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        public void SetImageFile(string fileName)
        {
            m_fileName = fileName;
            //m_rImgaeFile = Common.Common.Instance.Get_TL_IMAGE_FILE(m_fileName);
            //m_xnaScreen.Load(@"./data/img/" + m_fileName, ref m_rImgaeFile);

            //ListViewItem listVI;
            //lvTileFlags.CheckBoxes = true;
            //for (int i = 0; i < 16; ++i)
            //{
            //    listVI = new ListViewItem(new string[] { "タイルフラグ" + i});
            //    listVI.Checked = false;
            //    lvTileFlags.Items.Add(listVI);
            //}
            //---------------------------------------
            //---- ListView
            //---------------------------------------
            // ListViewItem listVI;
            // SGTextureInformation txInfo;
            //// Common.SHFILEINFO shinfo = new Common.SHFILEINFO();
            // string strTmp;
            // objLVInfo.Items.Clear();

            // txInfo = SGTexture2D.GetTextureInformation(@"./data/img/" + m_fileName);
            // //Win32.SHGetFileInfo(@"./data/img/" + m_strFileName, Win32.FILE_ATTRIBUTE_ARCHIVE, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_EXETYPE | Win32.SHGFI_TYPENAME);
            // Image img = Image.FromFile(@"./data/img/" + m_fileName);

            // listVI = new ListViewItem(new string[] { "画像の種類", img.RawFormat.ToString() });
            // objLVInfo.Items.Add(listVI);

            // // Property　幅高さ
            // strTmp = string.Format("{0:D} × {1:D}", txInfo.Width, txInfo.Height);
            // listVI = new ListViewItem(new string[] { "幅×高", strTmp });
            // objLVInfo.Items.Add(listVI);

            // // Property　フォーマット
            // strTmp = txInfo.Format.ToString();
            // listVI = new ListViewItem(new string[] { "フォーマット", strTmp });
            // objLVInfo.Items.Add(listVI);

            // // Property　ビットの深さ
            // strTmp = string.Format("{0:D}", txInfo.Depth);
            // listVI = new ListViewItem(new string[] { "深度", strTmp });
            // objLVInfo.Items.Add(listVI);

            // // Property　ミップレベル
            // strTmp = string.Format("{0:D}", txInfo.MipLevels);
            // listVI = new ListViewItem(new string[] { "ミップレベル", strTmp });
            // objLVInfo.Items.Add(listVI);

            // // Property　リソースタイプ
            // strTmp = txInfo.ResourceTypeName;
            // listVI = new ListViewItem(new string[] { "リソースタイプ", strTmp });
            // objLVInfo.Items.Add(listVI);

            // // Property　ブロック数
            // strTmp = string.Format("{0:D} × {1:D}", m_xnaScreen.WidthBlock, m_xnaScreen.HeightBlock);
            // listVI = new ListViewItem(new string[] { "ブロック(縦×横)", strTmp });
            // objLVInfo.Items.Add(listVI);

            // // Property　ブロック数
            // strTmp = string.Format("{0:D}", m_xnaScreen.WidthBlock * m_xnaScreen.HeightBlock);
            // listVI = new ListViewItem(new string[] { "ブロック合計", strTmp });
            // objLVInfo.Items.Add(listVI);
            //--------------------------------------
            // チェックボタン
            this.SetStartCheckBoxs();
            // 衝突選択ボタンのイメージ描画
            objBtnCollision.BackgroundImage = CommonMC2D.Instance.GetCollisionChipBitmap(
                m_rImgaeFile.tileInfos[m_currentChipNo].collisionChipNo,
                m_rImgaeFile.tileInfos[m_currentChipNo].collisionChipFlgs
            );
            //
            this.ResizePictureboxInImage();
        }

        //##############################################################################
        //##############################################################################
        //##
        //##  Form
        //##
        //##############################################################################
        //##############################################################################
        /// <summary>
        /// 読み込まれたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapChipInfoForm_Load(object sender, EventArgs e)
        {
            //ColorMatrixオブジェクトの作成
            ColorMatrix cm = new ColorMatrix();
            //ColorMatrixの行列の値を変更して、アルファ値が0.5に変更されるようにする
            cm.Matrix00 = 1;
            cm.Matrix11 = 1;
            cm.Matrix22 = 1;
            cm.Matrix33 = 1;
            cm.Matrix44 = 1;
            //ImageAttributesオブジェクトの作成
            m_IA = new ImageAttributes();
            //ColorMatrixを設定する
            m_IA.SetColorKey(
                System.Drawing.Color.FromArgb(255, 0, 255),
                System.Drawing.Color.FromArgb(255, 0, 255), 
                ColorAdjustType.Default
            );
            m_IA.SetColorMatrix(cm);
        }
        /// <summary>
        /// 表示されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapChipInfoForm_Shown(object sender, EventArgs e)
        {
            this.ResizePictureboxInImage();
        }
        /// <summary>
        /// ウィンドを閉じたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapChipInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            lvColliFlag.Items.Clear();
            lvTileFlags.Items.Clear();
            CloseFormEvent(this,e);
        }
        /// <summary>
        /// 現在選択されている状態のチップ情報をチェックボックスに反映させる
        /// </summary>
        private void SetStartCheckBoxs()
        {
            ushort collisionTargetFlgs = m_rImgaeFile.tileInfos[m_currentChipNo].collisionTargetFlgs;
            uint chipFlg = m_rImgaeFile.tileInfos[m_currentChipNo].tileFlg;

            if (m_currentTileFlgNo == 0)
            {
                for(int i = 0; i < lvColliFlag.Items.Count; ++i)
                {
                    if( lvColliFlag.Items[i].Checked)
                    {
                        collisionTargetFlgs += (ushort)(1 << i);
                    }
                }
            }

        }
        /// <summary>
        /// リサイズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            this.ResizePictureboxInImage();
        }
        /// <summary>
        /// ピクチャーボックス内で動いた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objPBoxImg_MouseMove(object sender, MouseEventArgs e)
        {

        }
        /// <summary>
        /// m_xnaScreen内でマウスを押した
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
            //objPBSelectChip.Refresh();
            ////
            //objLBL_No.Text = "No. " + m_currentChipNo;
            ////--------------------------------------
            //// チェックボタン
            //this.SetStartCheckBoxs();
            //// 衝突選択ボタンのイメージ描画
            //objBtnCollision.BackgroundImage = Common.Common.Instance.GetCollisionChipBitmap(
            //    m_rImgaeFile.tileInfos[m_currentChipNo].collisionChipNo,
            //    m_rImgaeFile.tileInfos[m_currentChipNo].collisionChipFlgs
            //);
        }
        /// <summary>
        /// キーを押した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapChipInfoForm_KeyDown(object sender, KeyEventArgs e)
        {
            //int x = m_xnaScreen.CurrentBlockX, y = m_xnaScreen.CurrentBlockY;

            //if (e.KeyCode == Keys.Down)
            //{
            //    ++y;
            //}
            //else if (e.KeyCode == Keys.Up)
            //{
            //    --y;
            //}
            //else if (e.KeyCode == Keys.Left)
            //{
            //    ++x;
            //}
            //else if (e.KeyCode == Keys.Right)
            //{
            //    --x;
            //}
            //if (m_xnaScreen.WidthBlock <= m_xnaScreen.CurrentBlockX)
            //    x = m_xnaScreen.WidthBlock - 1;
            //else if( m_xnaScreen.CurrentBlockX < 0 )
            //    x = 0;
            //if (m_xnaScreen.HeightBlock <= m_xnaScreen.CurrentBlockY)
            //    y = m_xnaScreen.HeightBlock - 1;
            //else if( m_xnaScreen.CurrentBlockY < 0 )
            //    y = 0;
            //m_xnaScreen.SetCurrentBlock(x, y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objPBSelectChip_Paint(object sender, PaintEventArgs e)
        {
            //if ( !m_xnaScreen.IsLoad()) return;
            //e.Graphics.DrawImage(m_xnaScreen.GetCurrentBitmapChip(), new Rectangle(0, 0, 40, 40), new Rectangle(0, 0, 40, 40), GraphicsUnit.Pixel);
        }
        /// <summary>
        /// チェックボタンのどれかが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCB_XXX_CheckStateChanged(object sender, EventArgs e)
        {
            ushort collisionTargetFlgs = m_rImgaeFile.tileInfos[m_currentChipNo].collisionTargetFlgs;

            if (m_currentTileFlgNo == 0)
            {
                collisionTargetFlgs &= 0xFF00;

                //if (objCB_Hero.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_0;
                //if (objCB_Enemy.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_1;
                //if (objCB_HeroBullet.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_2;
                //if (objCB_EnemyBullet.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_3;
                //if (objCB_Item.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_4;
                //if (objCB_Object.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_STAGE_OBJECT_0;
            }
            else
            {
                collisionTargetFlgs &= 0x00FF;
                //if (objCB_Hero.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_6;
                //if (objCB_Enemy.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_7;
                //if (objCB_HeroBullet.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_8;
                //if (objCB_EnemyBullet.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_9;
                //if (objCB_Item.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_10;
                //if (objCB_Object.Checked)
                //    collisionTargetFlgs |= FSquareTileInfo.TARGET_COLLI_11;
            }
            ushort chipFlg = 0;
            //if (objCB_Hasigo.Checked)
            //    chipFlg |= FSquareTileInfo.TILE_FLG_0;
            //if (objCB_HeroDamage.Checked)
            //    chipFlg |= FSquareTileInfo.TILE_FLG_1;

            m_rImgaeFile.tileInfos[m_currentChipNo].collisionTargetFlgs = collisionTargetFlgs;
            m_rImgaeFile.tileInfos[m_currentChipNo].tileFlg = chipFlg;

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

        /// <summary>
        /// マウスのホイル
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            //
            int v = vScrollBar.Value;
            v -= (int)(e.Delta / 120F * 20);
            if (v >= vScrollBar.Maximum)
                v = vScrollBar.Maximum;
            else if (v <= vScrollBar.Minimum)
                v = vScrollBar.Minimum;
            vScrollBar.Value = v;
        }
        /// <summary>
        /// 衝突チップ選択ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBtnCollision_Click(object sender, EventArgs e)
        {
            CollisionChipSelectForm f = new CollisionChipSelectForm();
            f.SetParameters(
                m_rImgaeFile.tileInfos[m_currentChipNo].collisionChipNo,
                m_rImgaeFile.tileInfos[m_currentChipNo].collisionChipFlgs
            );
            f.ShowDialog();
            m_rImgaeFile.tileInfos[m_currentChipNo].collisionChipNo = f.GetCollisionChipNo();
            m_rImgaeFile.tileInfos[m_currentChipNo].collisionChipFlgs = f.GetCollisionChipFlgs();
            // 衝突選択ボタンのイメージ描画
            objBtnCollision.BackgroundImage = CommonMC2D.Instance.GetCollisionChipBitmap(
                m_rImgaeFile.tileInfos[m_currentChipNo].collisionChipNo,
                m_rImgaeFile.tileInfos[m_currentChipNo].collisionChipFlgs
            );
        }

        /// <summary>
        /// タイマー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_Timer_Tick(object sender, EventArgs e)
        {
            //m_xnaScreen.SetScrollBar(vScrollBar.Value, hScrollBar.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCKColli_Click(object sender, EventArgs e)
        {
            //m_xnaScreen.DrawColliChip = tsbCollision.Checked;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCKGrid_Click(object sender, EventArgs e)
        {
            //m_xnaScreen.DrawGrid = tsbGird.Checked;
        }
        private void SetMapchipFlgUtil(CheckBox obj, short collisionTargetFlgs, short flg)
        {
            if ((collisionTargetFlgs & flg) != 0)
                obj.Checked = true;
            else
                obj.Checked = false;
        }
        /// <summary>
        /// タイルフラグの切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvTileFlags_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_rImgaeFile == null) return;
            //if( m_currentTileFlgNo != lvTileFlags.SelectedIndex )
            //    m_currentTileFlgNo = lvTileFlags.SelectedIndex;


            ushort collisionTargetFlgs = m_rImgaeFile.tileInfos[m_currentChipNo].collisionTargetFlgs;

            //if (m_currentTileFlgNo == 0)
            //{
            //    SetMapchipFlgUtil(objCB_Hero, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_0);
            //    SetMapchipFlgUtil(objCB_Enemy, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_1);
            //    SetMapchipFlgUtil(objCB_HeroBullet, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_2);
            //    SetMapchipFlgUtil(objCB_EnemyBullet, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_3);
            //    SetMapchipFlgUtil(objCB_Item, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_4);
            //    SetMapchipFlgUtil(objCB_Object, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_STAGE_OBJECT_0);
            //}
            //else
            //{
            //    SetMapchipFlgUtil(objCB_Hero, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_6);
            //    SetMapchipFlgUtil(objCB_Enemy, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_7);
            //    SetMapchipFlgUtil(objCB_HeroBullet, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_8);
            //    SetMapchipFlgUtil(objCB_EnemyBullet, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_9);
            //    SetMapchipFlgUtil(objCB_Item, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_10);
            //    SetMapchipFlgUtil(objCB_Object, collisionTargetFlgs, FSquareTileInfo.TARGET_COLLI_11);
            //}
        }
        /// <summary>
        /// 衝突ターゲット全てのチェックを外す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_btnColliAllUncheck_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lvColliFlag.Items.Count; ++i)
            {
                lvTileFlags.Items[i].Checked = false;
            }
        }
        /// <summary>
        /// 衝突ターゲット全てのチェックをつける
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_btnColliAllCheck_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lvColliFlag.Items.Count; ++i)
            {
                lvTileFlags.Items[i].Checked = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void GridToggle_Click(object sender, EventArgs e)
        {

        }

        private void CollisionToggle_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// スクロールバーの変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (squareTilesImgCtrl == null) return;
            squareTilesImgCtrl.CameraRangePosition = new MCVector2(
                hScrollBar.Value / (float)(1 + hScrollBar.Maximum - hScrollBar.LargeChange),
                vScrollBar.Value / (float)(1 + vScrollBar.Maximum - vScrollBar.LargeChange));
#if DEBUG
            var p = squareTilesImgCtrl.CameraPosition;
            //LBScreenSize.Text = ClientSize.Width + "×" + ClientSize.Height + "--" + p.X + "×" + p.Y;

#endif
        }

        /// <summary>
        /// 読み込み画像のα値
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlpha_Click(object sender, EventArgs e)
        {
            //squareTilesImgCtrl.IsAlpha = BtnAlpha.Checked;
            //BtnAlpha.Checked = !BtnAlpha.Checked;
            //if (BtnAlpha.Checked)
            //    BtnAlpha.BackColor = SystemColors.ActiveCaption;
            //else
            //    BtnAlpha.BackColor = SystemColors.Control;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DocumentFocus(Object sender, EventArgs e)
        {
            //if (DocumentFocusEvent != null)
            //    DocumentFocusEvent(this, new DocumentFocusEventArgs(this));
        }

        /// <summary>
        /// アクティブで無い
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SquareDocDeactivate(object sender, EventArgs e)
        {
            squareTilesImgCtrl.IsActive = false;
        }

        /// <summary>
        /// アクティブになった
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SquareDocActivated(object sender, EventArgs e)
        {
            squareTilesImgCtrl.IsActive = true;
        }

        private void squareTilesImgCtrl_Click(object sender, EventArgs e)
        {

        }
    }
}
