using EditorMC2D.Common;
using System.Drawing;
using EditorMC2D.Events;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MC2DUtil.WinAPI;
using System;
using MC2DUtil.graphics;
using System.Drawing.Imaging;
using TileStageFormat;
using EditorMC2D.Forms;

namespace EditorMC2D.Docking
{
    public partial class SolutionExplorerDoc : DockContent
    {
        /// <summary>
        /// ツリービューのイメージインデックス
        /// </summary>
        private enum TreeImageIndex
        {
            /// <summary> ソリューション </summary>
            Solution,
            /// <summary> フォルダー </summary>
            FolderApp,
            /// <summary> タイル群 APP </summary>
            TileApp,
            /// <summary> フォルダー閉じた状態 </summary>
            FolderClose,
            /// <summary> フォルダー 開いた状態 </summary>
            FolderOpen,
            ImageFile,
            ModuleFile,
            SoundFile,
            TextFile,
            CGFile,
            GLSLFile,
            GLSESFile,
            HLSLFile,
            ScriptFile,
            FontFile,
            TileRoot,
            Rect,
            Isometric,
            IsometricSelect,
            Hexagon,
            HexagonSelect,
            Square,
            SquareSelect,
            RectAnm,
            TileAnm,
            MapRoot,
            Map
        }
        /// <summary>
        /// 項目の種類
        /// </summary>
        private enum ItemKind
        {
            Solution,
            FolderApp,
            TileApp,
            FolderImage,
            FolderModule,
            FolderSound,
            FolderScript,
            FolderEffect,
            FolderFont,
            Folder,
            ImageFile,
            ModuleFile,
            SoundFile,
            TextFile,
            CGFile,
            GLSLFile,
            GLSESFile,
            /// <summary> HLSLファイル </summary>
            HLSLFile,
            /// <summary> AngelScriptファイル </summary>
            ScriptFile,
            /// <summary> フォントファイル </summary>
            FontFile,
            /// <summary> タイル群開始 </summary>
            TileRoot,
            /// <summary> RECTタイル </summary>
            Rect,
            /// <summary> アイソメトリック・タイル </summary>
            Isometric,
            /// <summary> ヘキサゴン・タイル </summary>
            Hexagon,
            /// <summary> スクエア・タイル </summary>
            Square,
            /// <summary> 短径アニメーション </summary>
            RectAnm,
            TileAnm,
            MapRoot,
            Map
        }
        private bool m_isChangeTheme = false;

        /// <summary>
        /// プロジェクトステートファイルからのツリー情報を保存する
        /// </summary>
        private class TreeProjectState
        {
            /// <summary>ファイルパスやディレクトリパス</summary>
            public string filePaht;
            /// <summary>展開済みか</summary>
            public bool isExpand;
            /// <summary>種類</summary>
            public ItemKind kind;
            /// <summary>種類</summary>
            public List<TreeProjectState> nodes;
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="path">ファイルパス</param>
            /// <param name="isEx">展開済みか</param>
            /// <param name="k"></param>
            public TreeProjectState(string path, bool isEx, ItemKind k = ItemKind.Solution)
            {
                filePaht = path;
                isExpand = isEx;
                kind = k;
                nodes = new List<TreeProjectState>();
            }
        };
        /// <summary>
        /// ノードに関連されたノード情報
        /// </summary>
        private class NodeInfo
        {
            /// <summary>ロックされているか</summary>
            public bool isLock;
            /// <summary>項目種類</summary>
            public ItemKind kind;
            /// <summary>項目のイメージ</summary>
            public TreeImageIndex itemImgIndex;
            public bool isChildNode;
            /// <summary>相対パス</summary>
            public string filePath;
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="k"></param>
            public NodeInfo(ItemKind k)
            {
                kind = k;
                isChildNode = false;
                filePath = "";
                isLock = false;
                itemImgIndex = 0;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
            /// <param name="isDir"></param>
            public void LockCheck(string path, bool isDir)
            {
                if (isDir)
                {
                    switch (path.ToLower())
                    {
                        case "core":
                        case "share":
                            isLock = true;
                            break;
                        default:
                            isLock = false;
                            break;
                    }
                }
                else
                {
                    switch (path.ToLower())
                    {
                        case "config.ans":
                        case "main.ans":
                            isLock = true;
                            break;
                        default:
                            isLock = false;
                            break;
                    }
                }
            }
        };

        /// <summary>
        /// ツリービューのカラー
        /// </summary>
        private class TreeViewColors
        {
            public Color Text;
            public Color ActiveText;
            public Color Background;
            public Color SelectActive;
            public Color SelectNonActive;
        }

        #region 変数
        List<TreeProjectState> m_TPSNode = new List<TreeProjectState>();

        private Bitmap m_arrowOpenImg = null;
        private Bitmap m_arrowCloseImg = null;
        /// <summary>TreeViewのカラー</summary>
        private TreeViewColors m_colors = new TreeViewColors();
        /// <summary>TreeViewので使用する項目イメージ</summary>
        private List<Bitmap> m_itemImages = new List<Bitmap>();

        /// <summary>ツリービューのノードに関連されたノード情報</summary>
        private Dictionary<int, NodeInfo> m_nodeInfos = null;
        /// <summary>ツリービューのノードを、名前から検索するために使用する</summary>
        private Dictionary<string, TreeNode> m_nodeDic = null;
        /// <summary>ノードの加算値</summary>
        private int m_nodeInfoKey = int.MinValue;
        /// <summary>ツリービューの項目のインデント</summary>
        private const int m_tvIndent = 18;
        /// <summary>CommonMC2D</summary>
        private CommonMC2D m_com;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SolutionExplorerDoc()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            m_com = CommonMC2D.Instance;
            m_nodeInfos = new Dictionary<int, NodeInfo>();
            m_nodeDic = new Dictionary<string, TreeNode>();

            #region アイコン登録
            //Solution,
            m_itemImages.Add(Properties.Resources.solution_16x);
            //FolderApp,
            m_itemImages.Add(Properties.Resources.FolderBrowserDialogControl_16x);
            //TileApp,
            m_itemImages.Add(Properties.Resources.TileApplication_16x);
            //FolderClose,
            m_itemImages.Add(Properties.Resources.folder_Open_16xLG);
            //FolderOpen,
            m_itemImages.Add(Properties.Resources.folder_Open_16xLG);
            //ImageFile,
            m_itemImages.Add(Properties.Resources.Image_16x);
            //ModuleFile
            m_itemImages.Add(Properties.Resources.Graphics3D_16x);
            //SoundFile,
            m_itemImages.Add(Properties.Resources.SoundFile_16x);
            //TextFile,
            m_itemImages.Add(Properties.Resources.Text_16x);
            //CGFile,
            m_itemImages.Add(Properties.Resources.cgFile_16x);
            //GLSLFile,
            m_itemImages.Add(Properties.Resources.glsesFile_16x);
            //GLSESFile,
            m_itemImages.Add(Properties.Resources.glesFile_16x);
            //HLSLFile,
            m_itemImages.Add(Properties.Resources.hlslFile_16x);
            //ScriptFile,
            m_itemImages.Add(Properties.Resources.Mc2d_16x);
            //FontFile,
            m_itemImages.Add(Properties.Resources.Font_16x);
            //TileRoot,
            m_itemImages.Add(Properties.Resources.grid_Data_16xLG);
            //Rect,
            m_itemImages.Add(Properties.Resources.Rectangle_16x);
            //Isometric,
            m_itemImages.Add(Properties.Resources.Isometric_16x);
            //IsometricSelect,
            m_itemImages.Add(Properties.Resources.openIsometric_16x);
            //Hexagon,
            m_itemImages.Add(Properties.Resources.hexagon_16x);
            //HexagonSelect,
            m_itemImages.Add(Properties.Resources.openHexagon_16x);
            //Squaret,
            m_itemImages.Add(Properties.Resources.square_16x);
            //SquaretSelect,
            m_itemImages.Add(Properties.Resources.openSquare_16x);
            //RectAnm,
            m_itemImages.Add(Properties.Resources.AnmRectangle_16x);
            //TileAnm,
            m_itemImages.Add(Properties.Resources.AnmTile_16xLG);
            //MapRoot
            m_itemImages.Add(Properties.Resources.MapTileLayer_16x);
            //Map
            m_itemImages.Add(Properties.Resources.ImageMapFile_16x);
#endregion

            m_com.MainWindow.ClosedProjectEvent += new ClosedSolutionHandler(this.ClosedSolution);
            m_com.MainWindow.D2StageFileOpenedEvent += new D2StageFileOpenedHandler(this.D2StageFileOpened);
        }
        //########################################################################
        //# ツリービュー
        //########################################################################
        #region ツリービュー
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="ni"></param>
        /// <returns></returns>
        private string AddNodeInfo(TreeNode n, NodeInfo ni)
        {
            string ret = m_nodeInfoKey + "";
            m_nodeInfos.Add(m_nodeInfoKey, ni);
            n.Name = m_nodeInfoKey + "";
            m_nodeInfoKey++;
            return ret;
        }
        /// <summary>
        /// 指定ノードを削除する
        /// </summary>
        /// <param name="n"></param>
        private void DelNodeInfo(TreeNode n)
        { m_nodeInfos.Remove(int.Parse(n.Name)); }
        /// <summary>
        /// 指定ノードから情報を取得する
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private NodeInfo GetNodeInfo(TreeNode n)
        { return m_nodeInfos[int.Parse(n.Name)]; }


        private const string ProjectNode = "ProjectNode";
        private const string MedhiaFolder = "MedhiaFolder";
        private const string EffectFolder = "EffectFolder";
        private const string ImageFolder = "ImageFolder";
        private const string ModuleFolder = "ModuleFolder";
        private const string ScriptFolder = "ScriptFolder";
        private const string SoundFolder = "SoundFolder";
        private const string FontFolder = "FontFolder";

        /// <summary>
        /// ツリービューの初期化
        /// </summary>
        private void TreeViewInit()
        {
            this.treeView.Nodes.Clear();
            m_nodeInfos.Clear();
            m_nodeDic.Clear();

            TreeNode rootTn = new TreeNode("プロジェクト", (int)TreeImageIndex.Solution, (int)TreeImageIndex.Solution);
            rootTn.Name = ProjectNode;
            AddNodeInfo(rootTn, new NodeInfo(ItemKind.Solution));
            treeView.Nodes.Add(rootTn);
            m_nodeDic.Add(ProjectNode, rootTn);

            //----------------------------
            // フォルダー
            //----------------------------
            TreeNode FolderTn = new TreeNode("メディアディレクトリ", (int)TreeImageIndex.FolderApp, (int)TreeImageIndex.FolderApp);
            FolderTn.Name = MedhiaFolder;
            AddNodeInfo(FolderTn, new NodeInfo(ItemKind.FolderApp));
            rootTn.Nodes.Add(FolderTn);
            m_nodeDic.Add(MedhiaFolder, FolderTn);
                //
                TreeNode tn = new TreeNode("エフェクト", (int)TreeImageIndex.FolderClose, (int)TreeImageIndex.FolderOpen);
                tn.Name = EffectFolder;
                AddNodeInfo(tn, new NodeInfo(ItemKind.FolderEffect));
                FolderTn.Nodes.Add(tn);
                m_nodeDic.Add(EffectFolder, tn);
                //
                tn = new TreeNode("テクスチャー", (int)TreeImageIndex.FolderClose, (int)TreeImageIndex.FolderOpen);
                tn.Name = ImageFolder;
                AddNodeInfo(tn, new NodeInfo(ItemKind.FolderImage));
                FolderTn.Nodes.Add(tn);
                m_nodeDic.Add(ImageFolder, tn);
                //
                tn = new TreeNode("オブジェクト", (int)TreeImageIndex.FolderClose, (int)TreeImageIndex.FolderOpen);
                tn.Name = ModuleFolder;
                AddNodeInfo(tn, new NodeInfo(ItemKind.FolderModule));
                FolderTn.Nodes.Add(tn);
                m_nodeDic.Add(ModuleFolder, tn);
                //
                tn = new TreeNode("スクリプト", (int)TreeImageIndex.FolderClose, (int)TreeImageIndex.FolderOpen);
                tn.Name = ScriptFolder;
                AddNodeInfo(tn, new NodeInfo(ItemKind.FolderScript));
                FolderTn.Nodes.Add(tn);
                m_nodeDic.Add(ScriptFolder, tn);
                //
                tn = new TreeNode("サウンド", (int)TreeImageIndex.FolderClose, (int)TreeImageIndex.FolderOpen);
                tn.Name = SoundFolder;
                AddNodeInfo(tn, new NodeInfo(ItemKind.FolderSound));
                FolderTn.Nodes.Add(tn);
                m_nodeDic.Add(SoundFolder, tn);
                //
                tn = new TreeNode("フォント", (int)TreeImageIndex.FolderClose, (int)TreeImageIndex.FolderOpen);
                tn.Name = FontFolder;
                AddNodeInfo(tn, new NodeInfo(ItemKind.FolderFont));
                FolderTn.Nodes.Add(tn);
                m_nodeDic.Add(FontFolder, tn);

            //----------------------------
            // タイル群
            //----------------------------
            TreeNode tileApp = new TreeNode("タイル等", (int)TreeImageIndex.TileApp, (int)TreeImageIndex.TileApp);
            AddNodeInfo(tileApp, new NodeInfo(ItemKind.TileApp));
            rootTn.Nodes.Add(tileApp);
                //
                TreeNode tileRoot = new TreeNode("タイル群", (int)TreeImageIndex.TileRoot, (int)TreeImageIndex.TileRoot);
                AddNodeInfo(tileRoot, new NodeInfo(ItemKind.TileRoot));
                tileApp.Nodes.Add(tileRoot);
                    //
                    tn = new TreeNode("アイソメトリック", (int)TreeImageIndex.Isometric, (int)TreeImageIndex.IsometricSelect);
                    AddNodeInfo(tn, new NodeInfo(ItemKind.Isometric));
                    tileRoot.Nodes.Add(tn);
                    //
                    tn = new TreeNode("ヘキサゴン", (int)TreeImageIndex.Hexagon, (int)TreeImageIndex.HexagonSelect);
                    AddNodeInfo(tn, new NodeInfo(ItemKind.Hexagon));
                    tileRoot.Nodes.Add(tn);
                    //
                    tn = new TreeNode("スクウェア", (int)TreeImageIndex.Square, (int)TreeImageIndex.SquareSelect);
                    AddNodeInfo(tn, new NodeInfo(ItemKind.Square));
                    tileRoot.Nodes.Add(tn);
            //
            tn = new TreeNode("短径", (int)TreeImageIndex.Rect, (int)TreeImageIndex.Rect);
            AddNodeInfo(tn, new NodeInfo(ItemKind.Rect));
            tileApp.Nodes.Add(tn);
            //
            tn = new TreeNode("短径アニメーション", (int)TreeImageIndex.RectAnm, (int)TreeImageIndex.RectAnm);
            AddNodeInfo(tn, new NodeInfo(ItemKind.RectAnm));
            tileApp.Nodes.Add(tn);

            treeView.Nodes[0].Expand();

        }
        /// <summary>
        /// 親ノードか？
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        private bool IsRootNode(TreeNode Node)
        {
            return (Node.Level == 0 && Node.PrevNode == null);
        }
        /// <summary>
        /// パスからツリーノードを取得する
        /// </summary>
        /// <param name="path">ファイルパス、またはディレクトリパス</param>
        /// <returns>存在するパス名ならツリーノードが返される。内場合nullを返す</returns>
        private TreeNode GetNodeFromPath(string path)
        {
            if (treeView.Nodes == null) return null;
            foreach (TreeNode n in treeView.Nodes[0].Nodes)
            {
                int idx = int.Parse(n.Name);
                if (!m_nodeInfos.ContainsKey(idx)) continue;

                if (m_nodeInfos[idx].filePath == path)
                    return n;
            }
            return null;
        }
        /// <summary>
        /// 項目ごとにアイコン、テキストなどを描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (!m_isChangeTheme) return;
            e.DrawDefault = false;
            if (e.Node.Bounds.IsEmpty) return;

            // ノードの以前の内容を消去します。これを行わない場合は、ときに、マウスオーバーフォントはわずかにより太字で得られます。
            Rectangle bounds = new Rectangle(0, e.Node.Bounds.Y, this.Width - 1, e.Node.Bounds.Height);
            if (e.Node.IsSelected)
            {
                Brush b;
                if (this.IsActivated)
                    b = new SolidBrush(m_colors.SelectActive);
                else
                    b = new SolidBrush(m_colors.SelectNonActive);

                e.Graphics.FillRectangle(b, bounds);
            }
            else
            {
                Brush b = new SolidBrush(m_colors.Background);
                e.Graphics.FillRectangle(b, bounds);
            }

            DrawNodeIcon(e);
            DrawNodeText(e);
        }
        /// <summary>
        /// ツリービューの項目にアイコンを描画
        /// </summary>
        /// <param name="e"></param>
        private void DrawNodeIcon(DrawTreeNodeEventArgs e)
        {
            Bitmap bmp = null;
            NodeInfo ni = GetNodeInfo(e.Node);
            switch (ni.kind)
            {
                case ItemKind.Solution:
                    ni.itemImgIndex = TreeImageIndex.Solution; break;
                case ItemKind.FolderApp:
                    ni.itemImgIndex = TreeImageIndex.FolderApp; break;
                case ItemKind.TileApp:
                    ni.itemImgIndex = TreeImageIndex.TileApp; break;
                case ItemKind.FolderImage:
                case ItemKind.FolderModule:
                case ItemKind.FolderSound:
                case ItemKind.FolderScript:
                case ItemKind.FolderEffect:
                case ItemKind.FolderFont:
                case ItemKind.Folder:
                    ni.itemImgIndex = e.Node.IsExpanded ? TreeImageIndex.FolderOpen : TreeImageIndex.FolderClose;
                    break;
                case ItemKind.ImageFile:
                    ni.itemImgIndex = TreeImageIndex.ImageFile; break;
                case ItemKind.ModuleFile:
                    ni.itemImgIndex = TreeImageIndex.ModuleFile; break;
                case ItemKind.SoundFile:
                    ni.itemImgIndex = TreeImageIndex.SoundFile; break;
                case ItemKind.TextFile:
                    ni.itemImgIndex = TreeImageIndex.TextFile; break;
                case ItemKind.CGFile:
                    ni.itemImgIndex = TreeImageIndex.CGFile; break;
                case ItemKind.GLSLFile:
                    ni.itemImgIndex = TreeImageIndex.GLSLFile; break;
                case ItemKind.GLSESFile:
                    ni.itemImgIndex = TreeImageIndex.GLSESFile; break;
                case ItemKind.HLSLFile:
                    ni.itemImgIndex = TreeImageIndex.HLSLFile; break;
                case ItemKind.ScriptFile:
                    ni.itemImgIndex = TreeImageIndex.ScriptFile; break;
                case ItemKind.FontFile:
                    ni.itemImgIndex = TreeImageIndex.FontFile; break;
                case ItemKind.TileRoot:
                    ni.itemImgIndex = TreeImageIndex.TileRoot; break;
                case ItemKind.Rect:
                    ni.itemImgIndex = TreeImageIndex.Rect; break;
                case ItemKind.Isometric:
                    ni.itemImgIndex = TreeImageIndex.Isometric; break;
                case ItemKind.Hexagon:
                    ni.itemImgIndex = TreeImageIndex.Hexagon; break;
                case ItemKind.Square:
                    ni.itemImgIndex = TreeImageIndex.Square; break;
                case ItemKind.RectAnm:
                    ni.itemImgIndex = TreeImageIndex.RectAnm; break;
                case ItemKind.TileAnm:
                    ni.itemImgIndex = TreeImageIndex.TileAnm; break;
                case ItemKind.MapRoot:
                    ni.itemImgIndex = TreeImageIndex.MapRoot; break;
                case ItemKind.Map:
                    ni.itemImgIndex = TreeImageIndex.Map; break;
            }

            bmp = m_itemImages[(int)ni.itemImgIndex];
            switch (ni.itemImgIndex)
            {
                case TreeImageIndex.FolderClose: bmp = ni.isLock ? Properties.Resources.folder_lock_Closed_16xLG : Properties.Resources.folder_Closed_16xLG; break;
                case TreeImageIndex.FolderOpen: bmp = ni.isLock ? Properties.Resources.folder_lock_Open_16xLG : Properties.Resources.folder_Open_16xLG; break;
            }

            int indent = (e.Node.Level * m_tvIndent) + this.Margin.Size.Width;
            int iconLeft = indent + m_tvIndent;


            int y = (e.Bounds.Y + e.Bounds.Height / 2) - (bmp.Height / 2) - 1;
            if (!IsRootNode(e.Node))
            {
                e.Graphics.DrawImage(bmp, new Rectangle(iconLeft, y, bmp.Width, bmp.Height));
                if (e.Node.Nodes.Count != 0)
                {
                    e.Graphics.DrawImage(e.Node.IsExpanded ? m_arrowCloseImg : m_arrowOpenImg, new Rectangle(iconLeft - m_tvIndent, y, bmp.Width, bmp.Height));
                }
            }
            else
            {
                e.Graphics.DrawImage(bmp, new Rectangle(indent, y, bmp.Width, bmp.Height));
            }
        }
        /// <summary>
        /// ツリービューの項目にテキストを描画
        /// </summary>
        /// <param name="e"></param>
        private void DrawNodeText(DrawTreeNodeEventArgs e)
        {
            if (e.Node.Bounds.IsEmpty) return;
            if (e.Node.IsEditing) return;

            NodeInfo ni = GetNodeInfo(e.Node);

            Rectangle bounds = e.Node.Bounds;
            using (Font font = e.Node.NodeFont)
            {
                bounds.Width = TextRenderer.MeasureText(e.Node.Text, font).Width;
                bounds.Y -= 1;
                bounds.X +=  1;
                if (IsRootNode(e.Node))
                {
                    bounds = new Rectangle(this.Margin.Size.Width + m_itemImages[(int)ni.itemImgIndex].Width+9, 0, bounds.Width, bounds.Height);
                }

                Color fontColor = m_colors.Text;
                if (e.Node.IsSelected && this.IsActivated)
                    fontColor = m_colors.ActiveText;

                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, bounds, fontColor);
            }
        }
        /// <summary>
        /// パス名から展開済みかチェックする
        /// </summary>
        /// <param name="path"></param>
        /// <returns>展開済みならtrueを返す</returns>
        private bool CheckExpandFromPath(string path)
        {
            foreach (TreeProjectState node in m_TPSNode)
            {
                if (node.filePaht.ToLower() == path.ToLower() && node.isExpand)
                {
                    return true;
                }
                if (CheckExpandFromPathSub(node.nodes, path))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// CheckExpandFromPathメソッド内でのみ使用する入れ子
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <see cref="CheckExpandFromPath"/>
        private bool CheckExpandFromPathSub(List<TreeProjectState> nodes, string path)
        {
            foreach (TreeProjectState node in nodes)
            {
                if (node.filePaht.ToLower() == path.ToLower() && node.isExpand)
                {
                    return true;
                }
                if (CheckExpandFromPathSub(node.nodes, path))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// メディアディレクトリにリソースを追加する
        /// </summary>
        /// <param name="parent">親ノード</param>
        /// <param name="dirPath">現在までの相対パス（ディレクトリ）</param>
        /// <param name="kind">現在までの相対パス（ディレクトリ）</param>
        private void AutoAddDirAndFile(TreeNode parent, string dirPath, ItemKind kind)
        {
            // フォルダ一覧を取得
            try
            {
                var rep = m_com.DirPathMC2D+@"\";

                DirectoryInfo dirs = new DirectoryInfo(dirPath);
                foreach (DirectoryInfo dir in dirs.GetDirectories())
                {
                    // フォルダを追加
                    if (Regex.IsMatch(dir.Name, @"^\.", RegexOptions.ECMAScript)) continue;

                    TreeNode nodeFolder = new TreeNode(dir.Name, (int)TreeImageIndex.FolderOpen, (int)TreeImageIndex.FolderClose);
                    NodeInfo n = new NodeInfo(ItemKind.Folder);
                    AddNodeInfo(nodeFolder, n);
                    parent.Nodes.Add(nodeFolder);
                    n.filePath = (dirPath + @"\" + dir.Name).Replace(rep, "");
                    n.LockCheck(n.filePath, true);
                    AutoAddDirAndFile(nodeFolder, dirPath + @"\" + dir.Name, kind);

                    // 展開済みか？
                    if (CheckExpandFromPath(n.filePath))
                    {
                        nodeFolder.Expand();
                    }
                }
                // Files
                FileInfo[] files = dirs.GetFiles();
                //
                RegexOptions op = RegexOptions.ECMAScript | RegexOptions.IgnoreCase;
                foreach (FileInfo file in files)
                {
                    if (Regex.IsMatch(file.Name, @"[^\.]+\.(?:as|mc2d)$", op) && kind == ItemKind.ScriptFile)
                    {
                        TreeNode nodeFile = new TreeNode(file.Name, 2, 2);
                        NodeInfo n = new NodeInfo(ItemKind.ScriptFile);
                        n.filePath = (dirPath + @"\" + file.Name).Replace(rep, "");
                        n.LockCheck(n.filePath, false);
                        AddNodeInfo(nodeFile, n);
                        parent.Nodes.Add(nodeFile);

                        // 展開済みか？
                        if (CheckExpandFromPath(n.filePath))
                        {
                            nodeFile.Expand();
                        }
                    }
                    else if (Regex.IsMatch(file.Name, @"[^\.]+\.(?:mp3|ogg|wav|wave|flac)$", op) && kind == ItemKind.SoundFile)
                    {
                        TreeNode nodeFile = new TreeNode(file.Name, 2, 2);
                        NodeInfo n = new NodeInfo(ItemKind.SoundFile);
                        n.filePath = (dirPath + @"\" + file.Name).Replace(rep, "");
                        n.LockCheck(n.filePath, false);
                        AddNodeInfo(nodeFile, n);
                        parent.Nodes.Add(nodeFile);

                        // 展開済みか？
                        if (CheckExpandFromPath(n.filePath))
                        {
                            nodeFile.Expand();
                        }
                    }
                    else if (Regex.IsMatch(file.Name, @"[^\.]+\.(?:otf|ttf|ttc)$", op) && kind == ItemKind.FontFile)
                    {
                        TreeNode nodeFile = new TreeNode(file.Name, 2, 2);
                        NodeInfo n = new NodeInfo(ItemKind.FontFile);
                        n.filePath = (dirPath + @"\" + file.Name).Replace(rep, "");
                        n.LockCheck(n.filePath, false);
                        AddNodeInfo(nodeFile, n);
                        parent.Nodes.Add(nodeFile);

                        // 展開済みか？
                        if (CheckExpandFromPath(n.filePath))
                        {
                            nodeFile.Expand();
                        }
                    }
                    else if (Regex.IsMatch(file.Name, @"[^\.]+\.(?:jpg|jpeg|png|bmp|dds)$", op) && kind == ItemKind.ImageFile)
                    {
                        TreeNode nodeFile = new TreeNode(file.Name, 2, 2);
                        NodeInfo n = new NodeInfo(ItemKind.ImageFile);
                        n.filePath = (dirPath + @"\" + file.Name).Replace(rep, "");
                        n.LockCheck(n.filePath, false);
                        AddNodeInfo(nodeFile, n);
                        parent.Nodes.Add(nodeFile);

                        // 展開済みか？
                        if (CheckExpandFromPath(n.filePath))
                        {
                            nodeFile.Expand();
                        }
                    }
                    else if (Regex.IsMatch(file.Name, @"[^\.]+\.(?:fx|hlsl)$", op) && kind == ItemKind.FolderEffect)
                    {
                        TreeNode nodeFile = new TreeNode(file.Name, 2, 2);
                        NodeInfo n = new NodeInfo(ItemKind.HLSLFile);
                        n.filePath = (dirPath + @"\" + file.Name).Replace(rep, "");
                        n.LockCheck(n.filePath, false);
                        AddNodeInfo(nodeFile, n);
                        parent.Nodes.Add(nodeFile);

                        // 展開済みか？
                        if (CheckExpandFromPath(n.filePath))
                        {
                            nodeFile.Expand();
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                m_com.AddOutputText(CommonMC2D.OutputTextType.System, e.Message + "\n");
            }

        }

        /// <summary>
        /// ツリービュー内でマウスダウンした
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_nodeInfos == null) return;
            var tmp = treeView.SelectedNode;
            treeView.SelectedNode = treeView.GetNodeAt(e.X, e.Y);　//マウス座標から選択位置のノードを取得
            if (treeView.SelectedNode != null)  //ノード上でクリックされたか？
            {
                int idx = int.Parse(treeView.SelectedNode.Name);
                if (!m_nodeInfos.ContainsKey(idx)) return;
                NodeInfo ni = m_nodeInfos[idx];

                if (e.Button == MouseButtons.Right)
                {
                    Point point = new Point(e.X, e.Y);
                    User32.ClientToScreen(treeView.Handle, ref point);

                    if (ni.isLock)
                    {
                        deleteMI.Enabled = false;
                        renameMI.Enabled = false;
                    }
                    else
                    {
                        deleteMI.Enabled = true;
                        renameMI.Enabled = true;
                    }
                    switch (ni.kind)
                    {
                        case ItemKind.Solution:
                            break;
                        case ItemKind.FolderImage:
                        case ItemKind.FolderModule:
                        case ItemKind.FolderSound:
                        case ItemKind.FolderScript:
                        case ItemKind.FolderEffect:
                        case ItemKind.FolderFont:
                        case ItemKind.Folder:
                            addMI.Visible = true;
                            contextMenu.Show(point);
                            break;
                        case ItemKind.ScriptFile:
                            addMI.Visible = false;
                            contextMenu.Show(point);
                            break;
                        // タイル
                        case ItemKind.Square:
                            addMI.Visible = false;
                            cmImgTileCreate.Show(point);
                            break;
                    }

                }
                else if (e.Button == MouseButtons.Left)
                {
                    switch (m_nodeInfos[idx].kind)
                    {
                        case ItemKind.Solution:
                            break;
                        case ItemKind.FolderImage:
                        case ItemKind.FolderModule:
                        case ItemKind.FolderSound:
                        case ItemKind.FolderScript:
                        case ItemKind.FolderEffect:
                        case ItemKind.Folder:
                            break;
                    }
                }
            } else if (tmp != null)
            {
                treeView.SelectedNode = tmp;
            }
        }
        /// <summary>
        /// ツリービュー内でダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_DoubleClick(object sender, EventArgs ea)
        {
            if (ea.GetType() != typeof(MouseEventArgs)) return;
            MouseEventArgs e = (MouseEventArgs)ea;
            if (m_nodeInfos == null) return;
            var tmp = treeView.SelectedNode;
            treeView.SelectedNode = treeView.GetNodeAt(e.X, e.Y);　//マウス座標から選択位置のノードを取得
            if (treeView.SelectedNode != null)  //ノード上でクリックされたか？
            {
                int idx = int.Parse(treeView.SelectedNode.Name);
                if (!m_nodeInfos.ContainsKey(idx)) return;
                NodeInfo ni = m_nodeInfos[idx];
                if (e.Button == MouseButtons.Left)
                {
                    switch (m_nodeInfos[idx].kind)
                    {
                        case ItemKind.ScriptFile:
                            m_com.MainWindow.AddAngelScriptEditor(ni.filePath);
                            break;
                        case ItemKind.SoundFile:
                            m_com.MainWindow.AddAudioDocument(ni.filePath);
                            break;
                        case ItemKind.FontFile:
                            m_com.MainWindow.AddFontDocument(ni.filePath);
                            break;
                        case ItemKind.ImageFile:
                            m_com.MainWindow.AddImageViewerDoc(ni.filePath);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// treeViewのラベルの編集が開始された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            //ルートのコードは編集できないようにする
            if (e.Node.Parent == null)
            {
                e.CancelEdit = true;
            }
            else if (e.Node.Name != null && e.Node.Name != "")
            {
                int idx = int.Parse(e.Node.Name);
                if (!m_nodeInfos.ContainsKey(idx)) return;
                NodeInfo ni = m_nodeInfos[idx];

                if (ni.isLock)
                    e.CancelEdit = true;
            }
        }
        /// <summary>
        /// treeViewのラベルの編集された時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            //ラベルが変更されたか調べる
            //e.Labelがnullならば、変更されていない
            if (e.Label != null)
            {
                if (e.Label == "")
                {
                    MessageBox.Show("名前が空です。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.CancelEdit = true;
                    return;
                }
                else if (e.Node.Parent != null)//同名のノードが同じ親ノード内にあるか調べる
                {
                    foreach (TreeNode n in e.Node.Parent.Nodes)
                    {
                        //同名のノードがあるときは編集をキャンセルする
                        if (n != e.Node && n.Text == e.Label)
                        {
                            MessageBox.Show("同名のノードがすでにあります。");
                            //編集をキャンセルして元に戻す
                            e.CancelEdit = true;
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// treeViewでキーが離れた時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_KeyUp(object sender, KeyEventArgs e)
        {
            TreeView tv = (TreeView)sender;
            //F2キーが離されたときは、フォーカスのあるアイテムの編集を開始
            if (e.KeyCode == Keys.F2 && tv.SelectedNode != null && tv.LabelEdit)
            {
                tv.SelectedNode.BeginEdit();
            }
        }
        /// <summary>
        /// ノードを展開した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                e.Cancel = true;
                return;
            }
            TreeNode node = e.Node;
            node.ImageIndex = 1;
            node.SelectedImageIndex = 1;
        }
        /// <summary>
        /// ノードを縮小した
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

            TreeNode node = e.Node;
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
        }
        #endregion


        /// <summary>
        /// ツリーノード内でメニュー項目をクリックした
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TNMenuClick(object sender, EventArgs e)
        {
            TreeNode n = treeView.SelectedNode;
            if (n == null) return;
            if (n.Parent == null) return;
            //
            int idx = int.Parse(n.Name);
            if (!m_nodeInfos.ContainsKey(idx)) return;

            string strKind = "";
            if (sender.GetType() == typeof(ToolStripMenuItem))
            {
                ToolStripMenuItem obj = (ToolStripMenuItem)sender;
                strKind = obj.Name;
            }
            else return;

            switch (strKind)
            {
                case "addFolderMI":
                    break;
                case "AddAngelScriptMI":
                    break;
                case "cutMI":
                    break;
                case "copyMI":
                    break;
                case "pasteMI":
                    break;
                case "objTSIMDelDirFile":
                    //if (m_comCore.DeleteDirOrFile(m_nodeInfo[idx].filePath) == CCommonCore.DELETE_DIR_FILE_RET.OK)
                    //{
                    //    n.Remove();
                    //}
                    break;
                case "renameMI":
                    treeView.SelectedNode.BeginEdit();
                    break;
                case "openExplorerMI":
                    break;
                case "createSquareImgTiles":
                    // スクエアタイルイメージの作成
                    var a = new AddSquareImage();
                    a.ShowDialog();
                    break;
            }
        }




        //##############################################################################
        //##############################################################################
        //##
        //##  イベント
        //##
        //##############################################################################
        //##############################################################################
        #region イベント
        /// <summary>
        /// ソリューションが閉じられた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClosedSolution(object sender, ClosedProjectEventArgs e)
        {
            topToolBar.Enabled = false;
        }
        /// <summary>
        /// ステージファイルが開かれた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void D2StageFileOpened(object sender, D2StageFileOpenedEventArgs e)
        {
            topToolBar.Enabled = true;
        }
        /// <summary>
        /// プロジェクトが開かれた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ProjectOpened(object sender, ProjectOpenedEventArgs e)
        {
            TreeViewInit();

            string path = m_com.DirPathMC2D + @"\" + MediaDir.Media + @"\";

            var n = m_nodeDic[EffectFolder];
            AutoAddDirAndFile(n, path + MediaDir.Effect, ItemKind.FolderEffect);
            n = m_nodeDic[FontFolder];
            AutoAddDirAndFile(n, path + MediaDir.Font, ItemKind.FontFile);
            n = m_nodeDic[ImageFolder];
            AutoAddDirAndFile(n, path + MediaDir.Image, ItemKind.ImageFile);
            n = m_nodeDic[SoundFolder];
            AutoAddDirAndFile(n, path + MediaDir.Sound, ItemKind.SoundFile);
            n = m_nodeDic[ScriptFolder];
            AutoAddDirAndFile(n, path + MediaDir.Script, ItemKind.ScriptFile);

        }
        /// <summary>
        /// テーマが切り替えられた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EnableVSRenderer(object sender, DockThemeChangeEventArgs e)
        {
            vsToolStripExtender.SetStyle(topToolBar, e.version, e.theme);
            vsToolStripExtender.SetStyle(contextMenu, e.version, e.theme);
            treeView.Font = e.theme.Skin.DockPaneStripSkin.TextFont;

            if (e.theme.ToString() == "WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme" || e.theme.ToString() == "WeifenLuo.WinFormsUI.Docking.VS2015LightTheme")
            {
                m_colors.Background = ColorTranslator.FromHtml("#ffffff");
                m_colors.SelectActive = ColorTranslator.FromHtml("#3399ff");
                m_colors.SelectNonActive = ColorTranslator.FromHtml("#cccedb");
                m_colors.Text = ColorTranslator.FromHtml("#000000");
                m_colors.ActiveText = ColorTranslator.FromHtml("#ffffff");
                m_arrowOpenImg = Properties.Resources.tree_open_arrow_dark;
                m_arrowCloseImg = Properties.Resources.tree_close_arrow_dark;
            }
            else
            {
                // ダーク 
                m_colors.Background = ColorTranslator.FromHtml("#252526");
                m_colors.SelectActive = ColorTranslator.FromHtml("#3399ff");
                m_colors.SelectNonActive = ColorTranslator.FromHtml("#3f3f46");
                m_colors.Text = ColorTranslator.FromHtml("#f1f1f1");
                m_colors.ActiveText = ColorTranslator.FromHtml("#ffffff");
                m_arrowOpenImg = Properties.Resources.tree_open_arrow;
                m_arrowCloseImg = Properties.Resources.tree_close_arrow;
            }

            treeView.BackColor = m_colors.Background; 
            treeView.ForeColor = m_colors.Text;
            m_isChangeTheme = true;
        }
        /// <summary>
        /// 全てが保存された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AllSaved(object sender, AllSavedEventArgs e)
        {

        }
        /// <summary>
        /// フォームが読み込まれた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SolutionExplorerDoc_Load(object sender, EventArgs e)
        {
            // ツリービュー
            treeView.Sorted = true;	// ※文字列順に自動Sortします。
            TreeViewInit();
        }
        #endregion

    }
}
