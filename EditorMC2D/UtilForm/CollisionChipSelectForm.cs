using EditorMC2D.Common;
using System;
using System.Drawing;
using System.Windows.Forms;
using TileStageFormat.Tile.Square;

namespace EditorMC2D.UtilForm
{
    public partial class CollisionChipSelectForm : Form
    {
        private int m_framePos = 0;
        private int m_currentBlockX = 0;
        private int m_currentBlockY = 0;
        private byte m_collisionChipFlgs = 0;
        private byte m_collisionChipNo = 0;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CollisionChipSelectForm()
        {
            InitializeComponent();
        }
        public byte GetCollisionChipFlgs()
        {
            return m_collisionChipFlgs;
        }
        public byte GetCollisionChipNo()
        {
            return m_collisionChipNo;
        }
        /// <summary>
        /// 衝突チップフラグなどのパラメーターセット
        /// </summary>
        /// <param name="collisionChipNo">チップ番号</param>
        /// <param name="collisionChipFlgs">フラグ</param>
        public void SetParameters(byte collisionChipNo, byte collisionChipFlgs)
        {
            m_collisionChipFlgs = collisionChipFlgs;
            m_collisionChipNo = collisionChipNo;
            m_currentBlockX = collisionChipNo % 4;
            m_currentBlockY = collisionChipNo / 4;

            if ((m_collisionChipFlgs & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
            {
                objRBLR_LR.Checked = true;
            }
            if ((m_collisionChipFlgs & FSquareTileInfo.COLLI_ROT_L90) != 0)
            {
                objRB90.Checked = true;
            }
            else if ((m_collisionChipFlgs & FSquareTileInfo.COLLI_ROT_L180) != 0)
            {
                objRB180.Checked = true;
            }
            else if ((m_collisionChipFlgs & FSquareTileInfo.COLLI_ROT_L270) != 0)
            {
                objRB270.Checked = true;
            }
            else
            {
                objRB0.Checked = true;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollisionChipSelectForm_Load(object sender, EventArgs e)
        {
            //
            objTimer.Interval = 66;
            objTimer.Enabled = true;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCBRot_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 決定ボタンを押した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objTimer_Tick(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        /// <summary>
        ///　描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int x, y, n;
            for (y = 0; y < 9; ++y)
            {
                n = (vScrollBar1.Value+1) / 40;
                if (n > y || (n+4) < y) continue;
                for (x = 0; x < 4; ++x)
                {
                    Bitmap tmp = CommonMC2D.Instance.GetCollisionChipBitmap((byte)(x + y * 4), m_collisionChipFlgs);
                    e.Graphics.DrawImage(tmp, new Rectangle(x*40,y*40-vScrollBar1.Value,40,40));
                    tmp.Dispose();
                }
            }
            //---------------------------------------
            // 選択されているブロックを示す
            //---------------------------------------
            //m_framePos = Common.Common.Instance.DrawAnimationFrame(0, -vScrollBar1.Value, m_currentBlockX, m_currentBlockY, m_framePos, e.Graphics);
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  ラジオボタン
        //##
        //##############################################################################
        //##############################################################################
        /// <summary>
        /// 反転なしボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objRBLRNo_MouseClick(object sender, MouseEventArgs e)
        {
            if (objRBLRNo.Checked)
            {
                m_collisionChipFlgs = (byte)(m_collisionChipFlgs & ~FSquareTileInfo.COLLI_FLIPHORIZONTAL);
            }
        }
        /// <summary>
        /// 左右反転ボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objRBLR_LR_MouseClick(object sender, MouseEventArgs e)
        {
            if (objRBLR_LR.Checked)
            {
                m_collisionChipFlgs |= FSquareTileInfo.COLLI_FLIPHORIZONTAL;
            }
        }
        /// <summary>
        /// 0度回転が押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objRB0_CheckedChanged(object sender, EventArgs e)
        {
            if (objRB0.Checked)
            {
                m_collisionChipFlgs &= FSquareTileInfo.COLLI_FLIPHORIZONTAL;
            }
        }
        /// <summary>
        /// 90度回転が押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objRB90_CheckedChanged(object sender, EventArgs e)
        {
            if (objRB90.Checked)
            {
                m_collisionChipFlgs &= FSquareTileInfo.COLLI_FLIPHORIZONTAL;
                m_collisionChipFlgs |= FSquareTileInfo.COLLI_ROT_L90;
            }
        }
        /// <summary>
        /// 180度回転が押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objRB180_CheckedChanged(object sender, EventArgs e)
        {
            if (objRB180.Checked)
            {
                m_collisionChipFlgs &= FSquareTileInfo.COLLI_FLIPHORIZONTAL;
                m_collisionChipFlgs |= FSquareTileInfo.COLLI_ROT_L180;
            }
        }
        /// <summary>
        /// 270度回転が押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objRB270_CheckedChanged(object sender, EventArgs e)
        {
            if (objRB270.Checked)
            {
                m_collisionChipFlgs &= FSquareTileInfo.COLLI_FLIPHORIZONTAL;
                m_collisionChipFlgs |= FSquareTileInfo.COLLI_ROT_L270;
            }
        }
        /// <summary>
        /// ピクチャーボックス内でマウスがおされた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int nTmpX, nTmpY;
            nTmpX = e.X / 40;
            if (4 <= m_currentBlockX)
                nTmpX = 3;
            nTmpY = (e.Y+vScrollBar1.Value) / 40;
            if (9 < m_currentBlockY)
                nTmpY = 9;

            if ((nTmpY*4 + nTmpX)<33)
            {
                m_currentBlockX = nTmpX;
                m_currentBlockY = nTmpY;
            }
            m_collisionChipNo = (byte)(m_currentBlockX + m_currentBlockY * 4);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
