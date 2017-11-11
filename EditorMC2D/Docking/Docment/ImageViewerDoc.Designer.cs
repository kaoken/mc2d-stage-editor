namespace EditorMC2D.Docking.Docment
{
    partial class ImageViewerDoc
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.LBImageTyep = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelImageWH = new System.Windows.Forms.ToolStripStatusLabel();
            this.LBScreenSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.topToolStrip = new System.Windows.Forms.ToolStrip();
            this.CBScaleList = new System.Windows.Forms.ToolStripComboBox();
            this.BtnAlpha = new System.Windows.Forms.ToolStripButton();
            this.imageViwerContrl = new UtilSharpDX.Controls.ImageViwerContrl();
            this.statusStrip.SuspendLayout();
            this.topToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // vScrollBar
            // 
            this.vScrollBar.Location = new System.Drawing.Point(486, 25);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 305);
            this.vScrollBar.TabIndex = 0;
            this.vScrollBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // hScrollBar
            // 
            this.hScrollBar.Location = new System.Drawing.Point(0, 330);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(486, 17);
            this.hScrollBar.TabIndex = 1;
            this.hScrollBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LBImageTyep,
            this.labelImageWH,
            this.LBScreenSize});
            this.statusStrip.Location = new System.Drawing.Point(0, 347);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(503, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // LBImageTyep
            // 
            this.LBImageTyep.Name = "LBImageTyep";
            this.LBImageTyep.Size = new System.Drawing.Size(29, 17);
            this.LBImageTyep.Text = "DDS";
            // 
            // labelImageWH
            // 
            this.labelImageWH.Name = "labelImageWH";
            this.labelImageWH.Size = new System.Drawing.Size(51, 17);
            this.labelImageWH.Text = "800×600";
            // 
            // LBScreenSize
            // 
            this.LBScreenSize.Name = "LBScreenSize";
            this.LBScreenSize.Size = new System.Drawing.Size(0, 17);
            // 
            // topToolStrip
            // 
            this.topToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CBScaleList,
            this.BtnAlpha});
            this.topToolStrip.Location = new System.Drawing.Point(0, 0);
            this.topToolStrip.Name = "topToolStrip";
            this.topToolStrip.Size = new System.Drawing.Size(503, 25);
            this.topToolStrip.TabIndex = 3;
            this.topToolStrip.Text = "toolStrip1";
            // 
            // CBScaleList
            // 
            this.CBScaleList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBScaleList.Name = "CBScaleList";
            this.CBScaleList.Size = new System.Drawing.Size(121, 25);
            this.CBScaleList.SelectedIndexChanged += new System.EventHandler(this.CBScaleList_SelectedIndexChanged);
            // 
            // BtnAlpha
            // 
            this.BtnAlpha.BackColor = System.Drawing.SystemColors.Control;
            this.BtnAlpha.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnAlpha.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnAlpha.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnAlpha.Name = "BtnAlpha";
            this.BtnAlpha.Size = new System.Drawing.Size(45, 22);
            this.BtnAlpha.Text = "α 表示";
            this.BtnAlpha.ToolTipText = "Alpha";
            this.BtnAlpha.Click += new System.EventHandler(this.BtnAlpha_Click);
            // 
            // imageViwerContrl
            // 
            this.imageViwerContrl.BackColor = System.Drawing.Color.White;
            this.imageViwerContrl.CameraPosition = new System.Drawing.Point(0, 0);
            this.imageViwerContrl.DesignModeTitle = "デザインモード";
            this.imageViwerContrl.ImageScale = 1F;
            this.imageViwerContrl.IsActive = true;
            this.imageViwerContrl.IsAlpha = false;
            this.imageViwerContrl.IsRenderPause = false;
            this.imageViwerContrl.Location = new System.Drawing.Point(0, 25);
            this.imageViwerContrl.Name = "imageViwerContrl";
            this.imageViwerContrl.Size = new System.Drawing.Size(321, 271);
            this.imageViwerContrl.TabIndex = 4;
            this.imageViwerContrl.Text = "imageViwerContrl1";
            this.imageViwerContrl.UserResized = false;
            // 
            // ImageViewerDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 369);
            this.Controls.Add(this.imageViwerContrl);
            this.Controls.Add(this.topToolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.hScrollBar);
            this.Controls.Add(this.vScrollBar);
            this.Name = "ImageViewerDoc";
            this.Text = "Form1";
            this.Activated += new System.EventHandler(this.ImageViewerDoc_Activated);
            this.Deactivate += new System.EventHandler(this.ImageViewerDoc_Deactivate);
            this.SizeChanged += new System.EventHandler(this.ImageViewerDoc_SizeChanged);
            this.Resize += new System.EventHandler(this.ImageViewerDoc_Resize);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.topToolStrip.ResumeLayout(false);
            this.topToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.HScrollBar hScrollBar;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip topToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel LBImageTyep;
        private System.Windows.Forms.ToolStripStatusLabel labelImageWH;
        private System.Windows.Forms.ToolStripButton BtnAlpha;
        private System.Windows.Forms.ToolStripComboBox CBScaleList;
        private System.Windows.Forms.ToolStripStatusLabel LBScreenSize;
        private UtilSharpDX.Controls.ImageViwerContrl imageViwerContrl;
    }
}

