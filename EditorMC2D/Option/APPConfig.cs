using EditorMC2D.Common;
using EditorMC2D.Option.Environment;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using MC2DUtil;
using System;
using EditorMC2D.Option.TextEditer;

namespace EditorMC2D.Option
{

    public class APPConfig
    {
        public const string ColorThemeBlue = "blue";
        public const string ColorThemeLightColor = "lightColor";
        public const string ColorThemeDarkColor = "darkColor";

        /// <summary>
        /// 保存先のファイル名
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        private static string m_fileName = @"config.xml";
        [System.Xml.Serialization.XmlIgnore]
        private CommonMC2D m_comCore = null;

        /// <summary>
        /// コンフィグファイルのバージョン
        /// </summary>
        public string Version = "0.0.1";
        /// <summary>
        /// 最近使用したプロジェクト
        /// </summary>
        public SerializableList<RecentProject> RecentProjects = new SerializableList<RecentProject>();

        #region 環境
        /// <summary>
        /// 全般
        /// </summary>
        public Whole Whole = new Whole();
        /// <summary>
        /// 配色テーマごとのフォントカラー群
        /// </summary>
        public SerializableDictionary<string, FontAndColor> FontAndColors = new SerializableDictionary<string, FontAndColor>();
        #endregion



        #region テキストエディター
        /// <summary>
        /// テキストエディター全般
        /// </summary>
        public TextConfig TextConfig = new TextConfig();
        #endregion

        /// <summary>
        /// ネットワーク
        /// </summary>
        public class ST_Network
        {
            public string IP = "127.0.0.1";
            public int Port = 20034;
        }
        public ST_Network Network = new ST_Network();



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public APPConfig()
        {
            m_comCore = CommonMC2D.Instance;
            FontAndColors.Add(ColorThemeBlue, new FontAndColor(ColorThemeBlue));
            FontAndColors.Add(ColorThemeLightColor, new FontAndColor(ColorThemeLightColor));
            FontAndColors.Add(ColorThemeDarkColor, new FontAndColor(ColorThemeDarkColor));
        }

        /// <summary>
        /// 現行テーマのドキュメントごとのカラーテーブルを取得する
        /// </summary>
        /// <param name="editorName"></param>
        /// <returns></returns>
        public PrintSetting GetTextEditorTheme(string editorName)
        {
            return FontAndColors[Whole.ColorTheme].PrintSettings[editorName];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void AddRecentProject(string filePath)
        {
            for(int i=0; i< RecentProjects.Count; ++i)
            {
                if(RecentProjects[i].FilePath == filePath)
                {
                    RecentProjects.Remove(RecentProjects[i]);
                    break;
                }
            }
            var m = new RecentProject();
            var a = filePath.Split('\\');
            m.Title = a[0];
            string t = "";
            for(int i=a.Length-1; i>0; --i)
            {
                t = @"\" + a[i] + t;
                if( t.Length > 64)
                {
                    t = @"\..." + t;
                    break;
                }
            }
            m.Title += t;
            m.FilePath = filePath;
            RecentProjects.Add(m);
            if (RecentProjects.Count > 10)
            {
                int i = RecentProjects.Count - 10;
                for (int j = 0; j<i; ++j)
                {
                    RecentProjects.Remove(RecentProjects[0]);
                }
            }
        }


        /// <summary>
        /// コンフィグファイルの保存
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(APPConfig));
            using (var sw = new StreamWriter(m_fileName, false, Encoding.UTF8))
            {
                serializer.Serialize(sw, this);
                sw.Flush();
                sw.Close();
            }
            return true;
        }
        /// <summary>
        /// コンフィグファイルの読み込み
        /// </summary>
        /// <returns>読み込んだファイルのインスタンスを返す</returns>
        public static APPConfig Read()
        {
            APPConfig loadAry;
            if (!System.IO.File.Exists(m_fileName))
            {
                loadAry = new APPConfig();
                loadAry.Save();
            }
            else
            {
                try
                {
                    // デシリアライズする
                    var serializer = new XmlSerializer(typeof(APPConfig));
                    var xmlSettings = new System.Xml.XmlReaderSettings()
                    {
                        CheckCharacters = false,
                    };
                    using (var sr = new StreamReader(m_fileName, Encoding.UTF8))
                    using (var xmlReader = XmlReader.Create(sr, xmlSettings))
                    {
                        loadAry = (APPConfig)serializer.Deserialize(xmlReader);
                        xmlReader.Close();
                    }
                    // エディター関連の
                    foreach(var v in loadAry.FontAndColors)
                    {
                        foreach(var w in v.Value.PrintSettings)
                        {
                            if( w.Key == "TextEditor")
                            {
                                w.Value.ReadXmlTextEditorAfterCommit(v.Value.Theme);
                            }else if (w.Key == "OutputWindow")
                            {
                                w.Value.ReadXmlOutputWindowAfterCommit(v.Value.Theme);
                            }
                            else if (w.Key == "SearchResultWindow")
                            {
                                w.Value.ReadXmlSearchResultWindowAfterCommit(v.Value.Theme);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    loadAry = new APPConfig();
                    loadAry.Save();
                    Console.WriteLine(e.Message);
                }
            }


            return loadAry;
        }
    }
}
