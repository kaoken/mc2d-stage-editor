using System;
using System.Windows.Forms;
using TileStageFormat.Map.Square;

namespace EditorMC2D.FormMapChip
{
    /// <summary>
    /// 選択範囲表示をするためのもの
    /// </summary>
    public partial class RangeEditForm : Form
    {
        private RangeSquareTiles m_objRM = new RangeSquareTiles();
        private RangeSquareTiles m_objCopyRM = new RangeSquareTiles();
        private MapChipEditForm m_rMCEF = null;
        private int m_nCopyFlg = 0;


        public RangeEditForm(MapChipEditForm rMCEF)
        {
            InitializeComponent();
            m_rMCEF = rMCEF;
        }
        public void SetCRangeMapchipe(RangeSquareTiles r)
        {
            m_objRM.Set(r);
            m_objRM.Decision();

            objGBCopyLayer.Enabled = true;
            objLX.Text = "" + m_objRM.startPosBlockX;
            objLY.Text = "" + m_objRM.startPosBlockY;
            objLW.Text = "" + m_objRM.widthBlock;
            objLH.Text = "" + m_objRM.hightBlock;
        }
        public void SetCopyCRangeMapchipe(RangeSquareTiles r)
        {
            m_objCopyRM.Set(r);
            m_objCopyRM.Decision();

            objGBCopyRange.Enabled = true;
            objGBPasteLayer.Enabled = true;
            objLCopyX.Text = "" + r.startPosBlockX;
            objLCopyY.Text = "" + r.startPosBlockY;
            objLCopyW.Text = "" + r.widthBlock;
            objLCopyH.Text = "" + r.hightBlock;

            if (objCKAllLayer.Checked)
            {
                objLTargetLayer.Text = "全て";
                objGBPasteLayer.Enabled = false;
                m_nCopyFlg = CatAndPasteSquareTiles.TARGET_LAYER_ALL;
            }
            else if (objRBCopyL00.Checked)
            {
                objLTargetLayer.Text = "0";
                m_nCopyFlg = CatAndPasteSquareTiles.TARGET_LAYER00;
            }
            else if (objRBCopyL01.Checked)
            {
                objLTargetLayer.Text = "1";
                m_nCopyFlg = CatAndPasteSquareTiles.TARGET_LAYER01;
            }

        }
        public int CatFlg
        {
            get
            {
                return m_nCopyFlg;
            }
        }
        public int PasteFlg
        {
            get
            {
                int i=0;
                if (objRBPasteL00.Checked)
                    i |= CatAndPasteSquareTiles.TARGET_LAYER00;
                if (objRBPasteL01.Checked)
                    i |= CatAndPasteSquareTiles.TARGET_LAYER01;
                return i;
            }
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  Form
        //##
        //##############################################################################
        //##############################################################################
        private void RangeEditForm_Load(object sender, EventArgs e)
        {
            objGBCopyRange.Enabled = false;
            objGBPasteLayer.Enabled = false;
        }

        private void RangeEditForm_Shown(object sender, EventArgs e)
        {
            this.objRBPasteLxx_CheckedChanged(null, null);
        }

        private void RangeEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if( m_rMCEF != null )
                m_rMCEF.ClosedRangeEditForm();
        }
        /// <summary>
        /// 全レイヤーチェックされたかも
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objCBAllLayer_CheckedChanged(object sender, EventArgs e)
        {
            if (objCKAllLayer.Checked)
                objGBCopyLayer.Enabled = false;
            else
                objGBCopyLayer.Enabled = true;
        }
        /// <summary>
        /// 貼り付けラジオボタンの値が変化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objRBPasteLxx_CheckedChanged(object sender, EventArgs e)
        {
            if (objRBPasteL00.Checked)
            {
                if (m_rMCEF != null)
                    m_rMCEF.SetRangeEditFormFromPasteFlg(CatAndPasteSquareTiles.TARGET_LAYER00);
            }
            else
            {
                if (m_rMCEF != null)
                    m_rMCEF.SetRangeEditFormFromPasteFlg(CatAndPasteSquareTiles.TARGET_LAYER01);
            }
        }
    }
}
