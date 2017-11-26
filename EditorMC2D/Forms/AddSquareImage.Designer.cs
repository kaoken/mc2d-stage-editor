namespace EditorMC2D.Forms
{
    partial class AddSquareImage
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.edgeLenght = new System.Windows.Forms.NumericUpDown();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.btnImgSelect = new System.Windows.Forms.Button();
            this.tbImgFilePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.edgeLenght)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(12, 86);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "作成";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(239, 86);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // edgeLenght
            // 
            this.edgeLenght.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.edgeLenght.Location = new System.Drawing.Point(65, 46);
            this.edgeLenght.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.edgeLenght.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.edgeLenght.Name = "edgeLenght";
            this.edgeLenght.Size = new System.Drawing.Size(53, 19);
            this.edgeLenght.TabIndex = 2;
            this.edgeLenght.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.edgeLenght.ValueChanged += new System.EventHandler(this.EdgeLenghtt_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "辺の長さ";
            // 
            // btnImgSelect
            // 
            this.btnImgSelect.Location = new System.Drawing.Point(239, 12);
            this.btnImgSelect.Name = "btnImgSelect";
            this.btnImgSelect.Size = new System.Drawing.Size(75, 23);
            this.btnImgSelect.TabIndex = 6;
            this.btnImgSelect.Text = "画像ファイル";
            this.btnImgSelect.UseVisualStyleBackColor = true;
            this.btnImgSelect.Click += new System.EventHandler(this.ImgSelect_Click);
            // 
            // tbImgFilePath
            // 
            this.tbImgFilePath.Location = new System.Drawing.Point(12, 13);
            this.tbImgFilePath.Name = "tbImgFilePath";
            this.tbImgFilePath.ReadOnly = true;
            this.tbImgFilePath.Size = new System.Drawing.Size(221, 19);
            this.tbImgFilePath.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(122, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "px （長さは作成後、変更不可能）";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(12, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(302, 2);
            this.label3.TabIndex = 9;
            this.label3.Text = "\r\n";
            // 
            // AddSquareImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 115);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnImgSelect);
            this.Controls.Add(this.tbImgFilePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edgeLenght);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AddSquareImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "スクエア・イメージの作成";
            this.Load += new System.EventHandler(this.AddSquareImage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.edgeLenght)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown edgeLenght;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnImgSelect;
        private System.Windows.Forms.TextBox tbImgFilePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}