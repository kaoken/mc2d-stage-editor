namespace EditorMC2D.FormMapChip
{
    partial class ChipSelectForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.objCBImg = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.objCKGrid = new System.Windows.Forms.CheckBox();
            this.objCKColli = new System.Windows.Forms.CheckBox();
            this.objLBChipNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.objGBChips = new System.Windows.Forms.GroupBox();
            this.objRBChip07 = new System.Windows.Forms.RadioButton();
            this.objRBChip06 = new System.Windows.Forms.RadioButton();
            this.objRBChip05 = new System.Windows.Forms.RadioButton();
            this.objRBChip04 = new System.Windows.Forms.RadioButton();
            this.objRBChip03 = new System.Windows.Forms.RadioButton();
            this.objRBChip02 = new System.Windows.Forms.RadioButton();
            this.objRBChip01 = new System.Windows.Forms.RadioButton();
            this.objRBChip00 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.objTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.objGBChips.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(590, 500);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(590, 525);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitContainer1.Panel1.Controls.Add(this.objCBImg);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.objLBChipNo);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.objGBChips);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.hScrollBar);
            this.splitContainer1.Panel2.Controls.Add(this.vScrollBar);
            this.splitContainer1.Size = new System.Drawing.Size(590, 500);
            this.splitContainer1.SplitterDistance = 188;
            this.splitContainer1.TabIndex = 0;
            // 
            // objCBImg
            // 
            this.objCBImg.FormattingEnabled = true;
            this.objCBImg.Location = new System.Drawing.Point(7, 20);
            this.objCBImg.Name = "objCBImg";
            this.objCBImg.Size = new System.Drawing.Size(170, 20);
            this.objCBImg.TabIndex = 7;
            this.objCBImg.SelectedIndexChanged += new System.EventHandler(this.objCBImg_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.objCKGrid);
            this.groupBox2.Controls.Add(this.objCKColli);
            this.groupBox2.Location = new System.Drawing.Point(3, 174);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(174, 55);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "表示";
            // 
            // objCKGrid
            // 
            this.objCKGrid.Appearance = System.Windows.Forms.Appearance.Button;
            this.objCKGrid.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.objCKGrid.Checked = true;
            this.objCKGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.objCKGrid.Location = new System.Drawing.Point(88, 18);
            this.objCKGrid.Name = "objCKGrid";
            this.objCKGrid.Size = new System.Drawing.Size(70, 25);
            this.objCKGrid.TabIndex = 6;
            this.objCKGrid.Text = "グリッド";
            this.objCKGrid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.objCKGrid.UseVisualStyleBackColor = false;
            this.objCKGrid.Click += new System.EventHandler(this.checkBoxXX_Click);
            // 
            // objCKColli
            // 
            this.objCKColli.Appearance = System.Windows.Forms.Appearance.Button;
            this.objCKColli.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.objCKColli.Location = new System.Drawing.Point(9, 18);
            this.objCKColli.Name = "objCKColli";
            this.objCKColli.Size = new System.Drawing.Size(70, 25);
            this.objCKColli.TabIndex = 5;
            this.objCKColli.Text = "衝突判定";
            this.objCKColli.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.objCKColli.UseVisualStyleBackColor = false;
            this.objCKColli.Click += new System.EventHandler(this.checkBoxXX_Click);
            // 
            // objLBChipNo
            // 
            this.objLBChipNo.AutoSize = true;
            this.objLBChipNo.Location = new System.Drawing.Point(79, 244);
            this.objLBChipNo.Name = "objLBChipNo";
            this.objLBChipNo.Size = new System.Drawing.Size(11, 12);
            this.objLBChipNo.TabIndex = 5;
            this.objLBChipNo.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 244);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "チップ番号";
            // 
            // objGBChips
            // 
            this.objGBChips.Controls.Add(this.objRBChip07);
            this.objGBChips.Controls.Add(this.objRBChip06);
            this.objGBChips.Controls.Add(this.objRBChip05);
            this.objGBChips.Controls.Add(this.objRBChip04);
            this.objGBChips.Controls.Add(this.objRBChip03);
            this.objGBChips.Controls.Add(this.objRBChip02);
            this.objGBChips.Controls.Add(this.objRBChip01);
            this.objGBChips.Controls.Add(this.objRBChip00);
            this.objGBChips.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.objGBChips.Location = new System.Drawing.Point(3, 57);
            this.objGBChips.Name = "objGBChips";
            this.objGBChips.Size = new System.Drawing.Size(182, 110);
            this.objGBChips.TabIndex = 2;
            this.objGBChips.TabStop = false;
            this.objGBChips.Text = "チップ";
            // 
            // objRBChip07
            // 
            this.objRBChip07.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRBChip07.BackColor = System.Drawing.SystemColors.Desktop;
            this.objRBChip07.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.objRBChip07.Location = new System.Drawing.Point(133, 61);
            this.objRBChip07.Name = "objRBChip07";
            this.objRBChip07.Size = new System.Drawing.Size(44, 44);
            this.objRBChip07.TabIndex = 7;
            this.objRBChip07.UseVisualStyleBackColor = false;
            this.objRBChip07.Click += new System.EventHandler(this.objRB_Click);
            // 
            // objRBChip06
            // 
            this.objRBChip06.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRBChip06.BackColor = System.Drawing.SystemColors.Desktop;
            this.objRBChip06.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.objRBChip06.Location = new System.Drawing.Point(90, 61);
            this.objRBChip06.Name = "objRBChip06";
            this.objRBChip06.Size = new System.Drawing.Size(44, 44);
            this.objRBChip06.TabIndex = 6;
            this.objRBChip06.UseVisualStyleBackColor = false;
            this.objRBChip06.Click += new System.EventHandler(this.objRB_Click);
            // 
            // objRBChip05
            // 
            this.objRBChip05.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRBChip05.BackColor = System.Drawing.SystemColors.Desktop;
            this.objRBChip05.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.objRBChip05.Location = new System.Drawing.Point(47, 61);
            this.objRBChip05.Name = "objRBChip05";
            this.objRBChip05.Size = new System.Drawing.Size(44, 44);
            this.objRBChip05.TabIndex = 5;
            this.objRBChip05.UseVisualStyleBackColor = false;
            this.objRBChip05.Click += new System.EventHandler(this.objRB_Click);
            // 
            // objRBChip04
            // 
            this.objRBChip04.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRBChip04.BackColor = System.Drawing.SystemColors.Desktop;
            this.objRBChip04.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.objRBChip04.Location = new System.Drawing.Point(4, 61);
            this.objRBChip04.Name = "objRBChip04";
            this.objRBChip04.Size = new System.Drawing.Size(44, 44);
            this.objRBChip04.TabIndex = 4;
            this.objRBChip04.UseVisualStyleBackColor = false;
            this.objRBChip04.Click += new System.EventHandler(this.objRB_Click);
            // 
            // objRBChip03
            // 
            this.objRBChip03.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRBChip03.BackColor = System.Drawing.SystemColors.Desktop;
            this.objRBChip03.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.objRBChip03.Location = new System.Drawing.Point(133, 18);
            this.objRBChip03.Name = "objRBChip03";
            this.objRBChip03.Size = new System.Drawing.Size(44, 44);
            this.objRBChip03.TabIndex = 3;
            this.objRBChip03.UseVisualStyleBackColor = false;
            this.objRBChip03.Click += new System.EventHandler(this.objRB_Click);
            // 
            // objRBChip02
            // 
            this.objRBChip02.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRBChip02.BackColor = System.Drawing.SystemColors.Desktop;
            this.objRBChip02.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.objRBChip02.Location = new System.Drawing.Point(90, 18);
            this.objRBChip02.Name = "objRBChip02";
            this.objRBChip02.Size = new System.Drawing.Size(44, 44);
            this.objRBChip02.TabIndex = 2;
            this.objRBChip02.UseVisualStyleBackColor = false;
            this.objRBChip02.Click += new System.EventHandler(this.objRB_Click);
            // 
            // objRBChip01
            // 
            this.objRBChip01.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRBChip01.BackColor = System.Drawing.SystemColors.Desktop;
            this.objRBChip01.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.objRBChip01.Location = new System.Drawing.Point(47, 18);
            this.objRBChip01.Name = "objRBChip01";
            this.objRBChip01.Size = new System.Drawing.Size(44, 44);
            this.objRBChip01.TabIndex = 1;
            this.objRBChip01.UseVisualStyleBackColor = false;
            this.objRBChip01.Click += new System.EventHandler(this.objRB_Click);
            // 
            // objRBChip00
            // 
            this.objRBChip00.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRBChip00.BackColor = System.Drawing.SystemColors.Desktop;
            this.objRBChip00.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.objRBChip00.Checked = true;
            this.objRBChip00.Location = new System.Drawing.Point(4, 18);
            this.objRBChip00.Name = "objRBChip00";
            this.objRBChip00.Size = new System.Drawing.Size(44, 44);
            this.objRBChip00.TabIndex = 0;
            this.objRBChip00.TabStop = true;
            this.objRBChip00.UseVisualStyleBackColor = false;
            this.objRBChip00.Click += new System.EventHandler(this.objRB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "対象イメージ：";
            // 
            // hScrollBar
            // 
            this.hScrollBar.Location = new System.Drawing.Point(4, 484);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(383, 16);
            this.hScrollBar.TabIndex = 2;
            // 
            // vScrollBar
            // 
            this.vScrollBar.Location = new System.Drawing.Point(390, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(16, 481);
            this.vScrollBar.TabIndex = 1;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // objTimer
            // 
            this.objTimer.Tick += new System.EventHandler(this.objTimer_Tick);
            // 
            // ChipSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 525);
            this.ControlBox = false;
            this.Controls.Add(this.toolStripContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(598, 533);
            this.Name = "ChipSelectForm";
            this.Text = "ChipSelectForm";
            this.Activated += new System.EventHandler(this.ChipSelectForm_Activated);
            this.Deactivate += new System.EventHandler(this.ChipSelectForm_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChipSelectForm_FormClosed);
            this.Load += new System.EventHandler(this.ChipSelectForm_Load);
            this.Shown += new System.EventHandler(this.ChipSelectForm_Shown);
            this.Resize += new System.EventHandler(this.ChipSelectForm_Resize);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.objGBChips.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.Timer objTimer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox objGBChips;
        private System.Windows.Forms.RadioButton objRBChip07;
        private System.Windows.Forms.RadioButton objRBChip06;
        private System.Windows.Forms.RadioButton objRBChip05;
        private System.Windows.Forms.RadioButton objRBChip04;
        private System.Windows.Forms.RadioButton objRBChip03;
        private System.Windows.Forms.RadioButton objRBChip02;
        private System.Windows.Forms.RadioButton objRBChip01;
        private System.Windows.Forms.RadioButton objRBChip00;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.HScrollBar hScrollBar;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label objLBChipNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox objCKGrid;
        private System.Windows.Forms.CheckBox objCKColli;
        private System.Windows.Forms.ComboBox objCBImg;
    }
}