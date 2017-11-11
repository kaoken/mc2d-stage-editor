namespace MapEdit
{
    partial class FileSelectForm
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.fileName = new System.Windows.Forms.ColumnHeader();
            this.columnDate = new System.Windows.Forms.ColumnHeader();
            this.objBtnOK = new System.Windows.Forms.Button();
            this.objBtnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.objLBFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileName,
            this.columnDate});
            this.listView1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(1, 2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(504, 200);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // fileName
            // 
            this.fileName.Text = "ファイル名";
            this.fileName.Width = 389;
            // 
            // columnDate
            // 
            this.columnDate.Text = "日付";
            this.columnDate.Width = 110;
            // 
            // objBtnOK
            // 
            this.objBtnOK.Location = new System.Drawing.Point(349, 238);
            this.objBtnOK.Name = "objBtnOK";
            this.objBtnOK.Size = new System.Drawing.Size(75, 23);
            this.objBtnOK.TabIndex = 1;
            this.objBtnOK.Text = "OK";
            this.objBtnOK.UseVisualStyleBackColor = true;
            this.objBtnOK.MouseClick += new System.Windows.Forms.MouseEventHandler(this.objBtnOK_MouseClick);
            // 
            // objBtnCancel
            // 
            this.objBtnCancel.Location = new System.Drawing.Point(430, 238);
            this.objBtnCancel.Name = "objBtnCancel";
            this.objBtnCancel.Size = new System.Drawing.Size(75, 23);
            this.objBtnCancel.TabIndex = 2;
            this.objBtnCancel.Text = "キャンセル";
            this.objBtnCancel.UseVisualStyleBackColor = true;
            this.objBtnCancel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.objBtnCancel_MouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "ファイル名：";
            // 
            // objLBFile
            // 
            this.objLBFile.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.objLBFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.objLBFile.Location = new System.Drawing.Point(65, 209);
            this.objLBFile.Name = "objLBFile";
            this.objLBFile.Size = new System.Drawing.Size(440, 16);
            this.objLBFile.TabIndex = 4;
            this.objLBFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FileSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 264);
            this.Controls.Add(this.objLBFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.objBtnCancel);
            this.Controls.Add(this.objBtnOK);
            this.Controls.Add(this.listView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileSelectForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ファイル選択";
            this.Shown += new System.EventHandler(this.FileSelectForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button objBtnOK;
        private System.Windows.Forms.Button objBtnCancel;
        private System.Windows.Forms.ColumnHeader fileName;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label objLBFile;
    }
}