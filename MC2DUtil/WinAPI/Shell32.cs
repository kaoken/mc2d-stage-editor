using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MC2DUtil.WinAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };


    public class Shell32
    {
        [Flags]
        public enum SHGFI
        {
            ICON = 0x000000100,
            DISPLAYNAME = 0x000000200,
            TYPENAME = 0x000000400,
            ATTRIBUTES = 0x000000800,
            ICONLOCATION = 0x000001000,
            EXETYPE = 0x000002000,
            SYSICONINDEX = 0x000004000,
            LINKOVERLAY = 0x000008000,
            SELECTED = 0x000010000,
            ATTR_SPECIFIED = 0x000020000,
            LARGEICON = 0x000000000,
            SMALLICON = 0x000000001,
            OPENICON = 0x000000002,
            SHELLICONSIZE = 0x000000004,
            PIDL = 0x000000008,
            USEFILEATTRIBUTES = 0x000000010
        }
        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum FILE
        {
            ATTRIBUTE_ARCHIVE = 0x00000020,
            ATTRIBUTE_ENCRYPTED = 0x00004000,
            ATTRIBUTE_HIDDEN = 0x00000002,
            ATTRIBUTE_NORMAL = 0x00000080,
            ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,
            ATTRIBUTE_OFFLINE = 0x00001000,
            ATTRIBUTE_READONLY = 0x00000001,
            ATTRIBUTE_SYSTEM = 0x00000004,
            ATTRIBUTE_TEMPORARY = 0x00000100
        }
        /// <summary>
        /// 
        /// </summary>
        public class CItem
        {
            public BitmapSource image { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">パスとファイル名を含む最大長MAX_PATHの文字列。 絶対パスと相対パスの両方が有効です。</param>
        /// <param name="dwFileAttributes">1つ以上のファイル属性フラグ（Winnt.hで定義されているFILE_ATTRIBUTE_値）の組み合わせ。 
        /// uFlagsにSHGFI_USEFILEATTRIBUTESフラグが含まれていない場合、このパラメータは無視されます。</param>
        /// <param name="psfi"></param>
        /// <param name="cbSizeFileInfo"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("shell32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr SHGetFileInfo(
            string path,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbSizeFileInfo,
            uint uFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool GetFileInfo(string fileName, ref CItem item)
        {
            if (!System.IO.File.Exists(fileName))
                return false;
            string fullPath = System.IO.Path.GetFullPath(fileName);
            SHFILEINFO shinfo = new SHFILEINFO();
            SHGetFileInfo(
                fullPath,
                (uint)FILE.ATTRIBUTE_NORMAL,
                ref shinfo,
                (uint)Marshal.SizeOf(shinfo),
                (uint)(SHGFI.ICON |
                SHGFI.LARGEICON |
                SHGFI.TYPENAME |
                SHGFI.DISPLAYNAME)
            );
            item.image = Imaging.CreateBitmapSourceFromHIcon(
                    shinfo.hIcon,
                    new Int32Rect(0, 0, 32, 32),
                    BitmapSizeOptions.FromEmptyOptions()
                );
            item.type = shinfo.szTypeName;
            item.name = shinfo.szDisplayName;
            return true;
        }

    }
}
