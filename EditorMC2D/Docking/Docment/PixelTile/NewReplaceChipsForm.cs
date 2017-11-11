using System;
using System.Windows.Forms;
using TileStageFormat.Map.Square;

namespace EditorMC2D.FormMapChip
{
    /// <summary>
    /// 置き換えチップを作成する為に使用するダイアログ
    /// </summary>
    public partial class NewReplaceChipsForm : Form
    {
        private MapChipEditForm m_rMCE = null;
        private SquareTilesMap m_rMap = null;
        private bool m_bOK = false;


        public NewReplaceChipsForm()
        {
            InitializeComponent();
        }
        public NewReplaceChipsForm(MapChipEditForm rMCE)
        {
            InitializeComponent();
            m_rMCE = rMCE;
        }
        public void BasicInit(SquareTilesMap rMap)
        {
            m_rMap = rMap;
            objUDX.Maximum = m_rMap.TileNumX - 1;
            objUDY.Maximum = m_rMap.TileNumY - 1;
            objUDW.Maximum = m_rMap.TileNumX;
            objUDH.Maximum = m_rMap.TileNumY;
        }
        public void SetPos(int x, int y)
        {
            objUDX.Value = x;
            objUDY.Value = y;
            m_rMCE.SetNewReplaceRect((int)objUDX.Value, (int)objUDY.Value, (int)objUDW.Value, (int)objUDH.Value);
        }
        public string GetName()
        {
            return objTBName.Text;
        }
        public int GetWidthBlock()
        {
            return (int)objUDW.Value;
        }
        public int GetHeightBlock()
        {
            return (int)objUDH.Value;
        }
        private void NewReplaceChipsForm_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBTOK_Click(object sender, EventArgs e)
        {
            objTBName.Text.Replace(" ", "");
            if (objTBName.Text.Length == 0)
            {
                MessageBox.Show("名前が未入力です。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //
            bool bRet = m_rMap.TransPoseTilesAdd(
                objTBName.Text, 
                (int)objUDX.Value, (int)objUDY.Value, 
                (int)objUDW.Value, (int)objUDH.Value,
                objRBFrameCopy.Checked
            );
            if (!bRet)
            {
                MessageBox.Show("すでに同じ名前が存在します。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            m_bOK = true;
            Close();
        }
        /// <summary>
        /// キャンセル
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objBTCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void objRBFrameXX_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 閉じた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewReplaceChipsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_rMCE.ClosedNewReplaceChipsForm(m_bOK);
        }
        /// <summary>
        /// 値が変わった
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objUDX_ValueChanged(object sender, EventArgs e)
        {
            objUDW.Maximum = m_rMap.TileNumX - ((int)objUDX.Value);
            m_rMCE.SetNewReplaceRect((int)objUDX.Value, (int)objUDY.Value, (int)objUDW.Value, (int)objUDH.Value);
        }

        private void objUDY_ValueChanged(object sender, EventArgs e)
        {
            objUDH.Maximum = m_rMap.TileNumY - ((int)objUDY.Value );
            m_rMCE.SetNewReplaceRect((int)objUDX.Value, (int)objUDY.Value, (int)objUDW.Value, (int)objUDH.Value);
        }

        private void objUDW_ValueChanged(object sender, EventArgs e)
        {
            m_rMCE.SetNewReplaceRect((int)objUDX.Value, (int)objUDY.Value, (int)objUDW.Value, (int)objUDH.Value);
        }

        private void objUDH_ValueChanged(object sender, EventArgs e)
        {
            m_rMCE.SetNewReplaceRect((int)objUDX.Value, (int)objUDY.Value, (int)objUDW.Value, (int)objUDH.Value);
        }
    }
}
