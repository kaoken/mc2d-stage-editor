namespace EditorMC2D.Option
{
    partial class FormOption
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("全般", 2, 2);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("フォントと色", 2, 2);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("環境", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("全般", 2, 2);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("全般", 2, 2);
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Angel Script", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("テキストエディタ", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode6});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOption));
            this.objTree = new System.Windows.Forms.TreeView();
            this.objImgList = new System.Windows.Forms.ImageList(this.components);
            this.objOK = new System.Windows.Forms.Button();
            this.objCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.objPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // objTree
            // 
            this.objTree.ImageIndex = 0;
            this.objTree.ImageList = this.objImgList;
            this.objTree.Location = new System.Drawing.Point(12, 11);
            this.objTree.Name = "objTree";
            treeNode1.ImageIndex = 2;
            treeNode1.Name = "FormWhole";
            treeNode1.SelectedImageIndex = 2;
            treeNode1.Text = "全般";
            treeNode2.ImageIndex = 2;
            treeNode2.Name = "FormFontAndColor";
            treeNode2.SelectedImageIndex = 2;
            treeNode2.Text = "フォントと色";
            treeNode3.Name = "environment";
            treeNode3.SelectedImageKey = "(既定値)";
            treeNode3.StateImageKey = "(なし)";
            treeNode3.Text = "環境";
            treeNode4.ImageIndex = 2;
            treeNode4.Name = "TXT_W";
            treeNode4.SelectedImageIndex = 2;
            treeNode4.Text = "全般";
            treeNode5.ImageIndex = 2;
            treeNode5.Name = "TXT_AS_W";
            treeNode5.SelectedImageIndex = 2;
            treeNode5.Text = "全般";
            treeNode6.Name = "FormASTextEditorConfig";
            treeNode6.Text = "Angel Script";
            treeNode7.Name = "ノード0";
            treeNode7.Text = "テキストエディタ";
            this.objTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode7});
            this.objTree.SelectedImageIndex = 0;
            this.objTree.ShowLines = false;
            this.objTree.ShowPlusMinus = false;
            this.objTree.Size = new System.Drawing.Size(241, 336);
            this.objTree.TabIndex = 0;
            this.objTree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.objTree_BeforeCollapse);
            this.objTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.objTree_BeforeExpand);
            this.objTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.objTree_MouseDown);
            // 
            // objImgList
            // 
            this.objImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("objImgList.ImageStream")));
            this.objImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.objImgList.Images.SetKeyName(0, "arrow_0.png");
            this.objImgList.Images.SetKeyName(1, "arrow_1.png");
            this.objImgList.Images.SetKeyName(2, "void16x16.png");
            // 
            // objOK
            // 
            this.objOK.Location = new System.Drawing.Point(574, 364);
            this.objOK.Name = "objOK";
            this.objOK.Size = new System.Drawing.Size(75, 21);
            this.objOK.TabIndex = 1;
            this.objOK.Text = "OK";
            this.objOK.UseVisualStyleBackColor = true;
            this.objOK.Click += new System.EventHandler(this.objOK_Click);
            // 
            // objCancel
            // 
            this.objCancel.Location = new System.Drawing.Point(655, 364);
            this.objCancel.Name = "objCancel";
            this.objCancel.Size = new System.Drawing.Size(75, 21);
            this.objCancel.TabIndex = 2;
            this.objCancel.Text = "キャンセル";
            this.objCancel.UseVisualStyleBackColor = true;
            this.objCancel.Click += new System.EventHandler(this.objCancel_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(271, 351);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(460, 2);
            this.label1.TabIndex = 3;
            // 
            // objPanel
            // 
            this.objPanel.AutoScroll = true;
            this.objPanel.Location = new System.Drawing.Point(271, 11);
            this.objPanel.Name = "objPanel";
            this.objPanel.Size = new System.Drawing.Size(460, 336);
            this.objPanel.TabIndex = 4;
            // 
            // FormOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 393);
            this.Controls.Add(this.objPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.objCancel);
            this.Controls.Add(this.objOK);
            this.Controls.Add(this.objTree);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOption";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "オプション";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormOption_FormClosed);
            this.Load += new System.EventHandler(this.FormOption_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView objTree;
        private System.Windows.Forms.Button objOK;
        private System.Windows.Forms.Button objCancel;
        private System.Windows.Forms.ImageList objImgList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel objPanel;
    }
}