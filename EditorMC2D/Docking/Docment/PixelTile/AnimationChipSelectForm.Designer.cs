namespace EditorMC2D.FormMapChip
{
    partial class AnimationChipSelectForm
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.obuUDGroupNo = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.objCKGrid = new System.Windows.Forms.CheckBox();
            this.objLBChipNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.objRBChip07 = new System.Windows.Forms.RadioButton();
            this.objRBChip06 = new System.Windows.Forms.RadioButton();
            this.objRBChip05 = new System.Windows.Forms.RadioButton();
            this.objRBChip04 = new System.Windows.Forms.RadioButton();
            this.objRBChip03 = new System.Windows.Forms.RadioButton();
            this.objRBChip02 = new System.Windows.Forms.RadioButton();
            this.objRBChip01 = new System.Windows.Forms.RadioButton();
            this.objRBChip00 = new System.Windows.Forms.RadioButton();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.obuUDGroupNo)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(533, 484);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(533, 509);
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
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.obuUDGroupNo);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.objLBChipNo);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.vScrollBar);
            this.splitContainer1.Size = new System.Drawing.Size(533, 484);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 219);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "グループ番号";
            // 
            // obuUDGroupNo
            // 
            this.obuUDGroupNo.Location = new System.Drawing.Point(106, 216);
            this.obuUDGroupNo.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.obuUDGroupNo.Name = "obuUDGroupNo";
            this.obuUDGroupNo.Size = new System.Drawing.Size(65, 19);
            this.obuUDGroupNo.TabIndex = 11;
            this.obuUDGroupNo.ValueChanged += new System.EventHandler(this.obuUDGroupNo_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.objCKGrid);
            this.groupBox2.Location = new System.Drawing.Point(10, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(182, 55);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "表示";
            // 
            // objCKGrid
            // 
            this.objCKGrid.Appearance = System.Windows.Forms.Appearance.Button;
            this.objCKGrid.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.objCKGrid.Checked = true;
            this.objCKGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.objCKGrid.Location = new System.Drawing.Point(6, 18);
            this.objCKGrid.Name = "objCKGrid";
            this.objCKGrid.Size = new System.Drawing.Size(173, 25);
            this.objCKGrid.TabIndex = 6;
            this.objCKGrid.Text = "グリッド";
            this.objCKGrid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.objCKGrid.UseVisualStyleBackColor = false;
            this.objCKGrid.Click += new System.EventHandler(this.objCKGrid_Click);
            // 
            // objLBChipNo
            // 
            this.objLBChipNo.AutoSize = true;
            this.objLBChipNo.Location = new System.Drawing.Point(81, 10);
            this.objLBChipNo.Name = "objLBChipNo";
            this.objLBChipNo.Size = new System.Drawing.Size(11, 12);
            this.objLBChipNo.TabIndex = 9;
            this.objLBChipNo.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "チップ番号";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.objRBChip07);
            this.groupBox1.Controls.Add(this.objRBChip06);
            this.groupBox1.Controls.Add(this.objRBChip05);
            this.groupBox1.Controls.Add(this.objRBChip04);
            this.groupBox1.Controls.Add(this.objRBChip03);
            this.groupBox1.Controls.Add(this.objRBChip02);
            this.groupBox1.Controls.Add(this.objRBChip01);
            this.groupBox1.Controls.Add(this.objRBChip00);
            this.groupBox1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(10, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 106);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "チップ";
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
            // vScrollBar
            // 
            this.vScrollBar.Location = new System.Drawing.Point(309, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 484);
            this.vScrollBar.TabIndex = 3;
            // 
            // AnimationChipSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 509);
            this.ControlBox = false;
            this.Controls.Add(this.toolStripContainer1);
            this.MaximumSize = new System.Drawing.Size(549, 10000);
            this.MinimumSize = new System.Drawing.Size(549, 38);
            this.Name = "AnimationChipSelectForm";
            this.Text = "アニメーションチップ選択";
            this.Activated += new System.EventHandler(this.AnimationChipSelectForm_Activated);
            this.Deactivate += new System.EventHandler(this.AnimationChipSelectForm_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AnimationChipSelectForm_FormClosed);
            this.Load += new System.EventHandler(this.AnimationChipSelectForm_Load);
            this.Shown += new System.EventHandler(this.AnimationChipSelectForm_Shown);
            this.Resize += new System.EventHandler(this.AnimationChipSelectForm_Resize);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.obuUDGroupNo)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox objCKGrid;
        private System.Windows.Forms.Label objLBChipNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton objRBChip07;
        private System.Windows.Forms.RadioButton objRBChip06;
        private System.Windows.Forms.RadioButton objRBChip05;
        private System.Windows.Forms.RadioButton objRBChip04;
        private System.Windows.Forms.RadioButton objRBChip03;
        private System.Windows.Forms.RadioButton objRBChip02;
        private System.Windows.Forms.RadioButton objRBChip01;
        private System.Windows.Forms.RadioButton objRBChip00;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown obuUDGroupNo;
    }
}