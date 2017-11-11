namespace EditorMC2D.UtilForm
{
    partial class CollisionChipSelectForm
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
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.objRB270 = new System.Windows.Forms.RadioButton();
            this.objRB0 = new System.Windows.Forms.RadioButton();
            this.objRB90 = new System.Windows.Forms.RadioButton();
            this.objRB180 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.objRBLRNo = new System.Windows.Forms.RadioButton();
            this.objRBLR_LR = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.objTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.vScrollBar1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.groupBox2);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.groupBox1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.btnOK);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.pictureBox1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(198, 303);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(198, 328);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.LargeChange = 40;
            this.vScrollBar1.Location = new System.Drawing.Point(169, 95);
            this.vScrollBar1.Maximum = 239;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 164);
            this.vScrollBar1.TabIndex = 11;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.objRB270);
            this.groupBox2.Controls.Add(this.objRB0);
            this.groupBox2.Controls.Add(this.objRB90);
            this.groupBox2.Controls.Add(this.objRB180);
            this.groupBox2.Location = new System.Drawing.Point(65, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(121, 81);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "反時計回り";
            // 
            // objRB270
            // 
            this.objRB270.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRB270.AutoSize = true;
            this.objRB270.BackColor = System.Drawing.SystemColors.MenuBar;
            this.objRB270.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.objRB270.Location = new System.Drawing.Point(64, 47);
            this.objRB270.Name = "objRB270";
            this.objRB270.Size = new System.Drawing.Size(45, 22);
            this.objRB270.TabIndex = 8;
            this.objRB270.Text = "270°";
            this.objRB270.UseVisualStyleBackColor = false;
            this.objRB270.CheckedChanged += new System.EventHandler(this.objRB270_CheckedChanged);
            // 
            // objRB0
            // 
            this.objRB0.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRB0.AutoSize = true;
            this.objRB0.BackColor = System.Drawing.SystemColors.MenuBar;
            this.objRB0.Checked = true;
            this.objRB0.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.objRB0.Location = new System.Drawing.Point(13, 19);
            this.objRB0.Name = "objRB0";
            this.objRB0.Size = new System.Drawing.Size(45, 22);
            this.objRB0.TabIndex = 5;
            this.objRB0.TabStop = true;
            this.objRB0.Text = "  0°";
            this.objRB0.UseVisualStyleBackColor = false;
            this.objRB0.CheckedChanged += new System.EventHandler(this.objRB0_CheckedChanged);
            // 
            // objRB90
            // 
            this.objRB90.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRB90.AutoSize = true;
            this.objRB90.BackColor = System.Drawing.SystemColors.MenuBar;
            this.objRB90.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.objRB90.Location = new System.Drawing.Point(64, 19);
            this.objRB90.Name = "objRB90";
            this.objRB90.Size = new System.Drawing.Size(45, 22);
            this.objRB90.TabIndex = 6;
            this.objRB90.Text = " 90°";
            this.objRB90.UseVisualStyleBackColor = false;
            this.objRB90.CheckedChanged += new System.EventHandler(this.objRB90_CheckedChanged);
            // 
            // objRB180
            // 
            this.objRB180.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRB180.AutoSize = true;
            this.objRB180.BackColor = System.Drawing.SystemColors.MenuBar;
            this.objRB180.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.objRB180.Location = new System.Drawing.Point(13, 47);
            this.objRB180.Name = "objRB180";
            this.objRB180.Size = new System.Drawing.Size(45, 22);
            this.objRB180.TabIndex = 7;
            this.objRB180.Text = "180°";
            this.objRB180.UseVisualStyleBackColor = false;
            this.objRB180.CheckedChanged += new System.EventHandler(this.objRB180_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.objRBLRNo);
            this.groupBox1.Controls.Add(this.objRBLR_LR);
            this.groupBox1.Location = new System.Drawing.Point(9, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(50, 81);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "反転";
            // 
            // objRBLRNo
            // 
            this.objRBLRNo.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRBLRNo.AutoSize = true;
            this.objRBLRNo.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.objRBLRNo.Checked = true;
            this.objRBLRNo.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.objRBLRNo.Location = new System.Drawing.Point(6, 18);
            this.objRBLRNo.Name = "objRBLRNo";
            this.objRBLRNo.Size = new System.Drawing.Size(39, 22);
            this.objRBLRNo.TabIndex = 3;
            this.objRBLRNo.TabStop = true;
            this.objRBLRNo.Text = "なし";
            this.objRBLRNo.UseVisualStyleBackColor = false;
            this.objRBLRNo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.objRBLRNo_MouseClick);
            // 
            // objRBLR_LR
            // 
            this.objRBLR_LR.Appearance = System.Windows.Forms.Appearance.Button;
            this.objRBLR_LR.AutoSize = true;
            this.objRBLR_LR.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.objRBLR_LR.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.objRBLR_LR.Location = new System.Drawing.Point(6, 46);
            this.objRBLR_LR.Name = "objRBLR_LR";
            this.objRBLR_LR.Size = new System.Drawing.Size(39, 22);
            this.objRBLR_LR.TabIndex = 4;
            this.objRBLR_LR.Text = "左右";
            this.objRBLR_LR.UseVisualStyleBackColor = false;
            this.objRBLR_LR.MouseClick += new System.Windows.Forms.MouseEventHandler(this.objRBLR_LR_MouseClick);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(9, 265);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(177, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "決定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnOK_MouseClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(9, 95);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(164, 164);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // objTimer
            // 
            this.objTimer.Tick += new System.EventHandler(this.objTimer_Tick);
            // 
            // CollisionChipSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 328);
            this.ControlBox = false;
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "CollisionChipSelectForm";
            this.Text = "衝突判定チップ";
            this.Load += new System.EventHandler(this.CollisionChipSelectForm_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton objRBLRNo;
        private System.Windows.Forms.RadioButton objRBLR_LR;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton objRB270;
        private System.Windows.Forms.RadioButton objRB180;
        private System.Windows.Forms.RadioButton objRB90;
        private System.Windows.Forms.RadioButton objRB0;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Timer objTimer;
        private System.Windows.Forms.VScrollBar vScrollBar1;


    }
}