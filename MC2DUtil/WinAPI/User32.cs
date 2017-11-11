using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MC2DUtil.WinAPI
{
    /// <summary>
    /// The initial state of the frame control. 
    /// </summary>
    [Flags()]
    public enum DrawFrameControlStates : uint
    {
        // =====================================================================================
        // If uType is DFC_BUTTON, uState can be one of the following values.
        // =====================================================================================
        /// <summary>
        /// Check box
        /// </summary>
        DFCS_BUTTONCHECK = 0,
        /// <summary>
        /// Image for radio button (nonsquare needs image)
        /// </summary>
        DFCS_BUTTONRADIOIMAGE = 1,
        /// <summary>
        /// Mask for radio button (nonsquare needs mask)
        /// </summary>
        DFCS_BUTTONRADIOMASK = 2,
        /// <summary>
        /// Radio button
        /// </summary>
        DFCS_BUTTONRADIO = 4,
        /// <summary>
        /// Three-state button
        /// </summary>
        DFCS_BUTTON3STATE = 8,
        /// <summary>
        /// Push button
        /// </summary>
        DFCS_BUTTONPUSH = 0x10,
        // =====================================================================================
        // If uType is DFC_CAPTION, uState can be one of the following values.
        // =====================================================================================
        /// <summary>
        /// Close button
        /// </summary>
        DFCS_CAPTIONCLOSE = 0,
        /// <summary>
        /// Minimize button
        /// </summary>
        DFCS_CAPTIONMIN = 1,
        /// <summary>
        /// Maximize button
        /// </summary>
        DFCS_CAPTIONMAX = 2,
        /// <summary>
        /// Restore button
        /// </summary>
        DFCS_CAPTIONRESTORE = 3,
        /// <summary>
        /// Help button
        /// </summary>
        DFCS_CAPTIONHELP = 4,
        // =====================================================================================
        // If uType is DFC_MENU, uState can be one of the following values.
        // =====================================================================================
        /// <summary>
        /// Submenu arrow
        /// </summary>
        DFCS_MENUARROW = 0,
        /// <summary>
        /// Check mark
        /// </summary>
        DFCS_MENUCHECK = 1,
        /// <summary>
        /// Bullet
        /// </summary>
        DFCS_MENUBULLET = 2,
        /// <summary>
        /// Submenu arrow pointing left. This is used for the right-to-left cascading menus used with right-to-left languages such as Arabic or Hebrew.
        /// </summary>
        DFCS_MENUARROWRIGHT = 4,
        // =====================================================================================
        // If uType is DFC_SCROLL, uState can be one of the following values.
        // =====================================================================================
        /// <summary>
        /// Up arrow of scroll bar
        /// </summary>
        DFCS_SCROLLUP = 0,
        /// <summary>
        /// Down arrow of scroll bar
        /// </summary>
        DFCS_SCROLLDOWN = 1,
        /// <summary>
        /// Left arrow of scroll bar
        /// </summary>
        DFCS_SCROLLLEFT = 2,
        /// <summary>
        /// Right arrow of scroll bar
        /// </summary>
        DFCS_SCROLLRIGHT = 3,
        /// <summary>
        /// Combo box scroll bar
        /// </summary>
        DFCS_SCROLLCOMBOBOX = 5,
        /// <summary>
        /// Size grip in lower-right corner of window
        /// </summary>
        DFCS_SCROLLSIZEGRIP = 8,
        /// <summary>
        /// Size grip in lower-left corner of window. This is used with right-to-left languages such as Arabic or Hebrew.
        /// </summary>
        DFCS_SCROLLSIZEGRIPRIGHT = 0x10,
        // =====================================================================================
        // The following style can be used to adjust the bounding rectangle of the push button.
        // =====================================================================================
        /// <summary>
        /// Bounding rectangle is adjusted to exclude the surrounding edge of the push button.
        /// </summary>
        DFCS_ADJUSTRECT = 0x2000,
        // =====================================================================================
        // One or more of the following values can be used to set the state of the control to be drawn.
        // =====================================================================================
        /// <summary>
        /// Button is inactive (grayed).
        /// </summary>
        DFCS_INACTIVE = 0x100,
        /// <summary>
        /// Button is pushed.
        /// </summary>
        DFCS_PUSHED = 0x200,
        /// <summary>
        /// Button is checked.
        /// </summary>
        DFCS_CHECKED = 0x400,
        /// <summary>
        /// The background remains untouched. This flag can only be combined with DFCS_MENUARROWUP or DFCS_MENUARROWDOWN.
        /// </summary>
        DFCS_TRANSPARENT = 0x800,
        /// <summary>
        /// Button is hot-tracked.
        /// </summary>
        DFCS_HOT = 0x1000,
        /// <summary>
        /// Button has a flat border.
        /// </summary>
        DFCS_FLAT = 0x4000,
        /// <summary>
        /// Button has a monochrome border.
        /// </summary>
        DFCS_MONO = 0x8000
    }


    public class User32
    {
        // リストビュー·ヘッダのパラメータ
        public const UInt32 HDI_FORMAT = 0x0004;
        public const UInt32 HDF_LEFT = 0x0000;
        public const UInt32 HDF_STRING = 0x4000;
        public const UInt32 HDF_SORTUP = 0x0400;
        public const UInt32 HDF_SORTDOWN = 0x0200;
        public const UInt32 LVM_GETHEADER = 0x1000 + 31;  // LVM_FIRST + 31
        public const UInt32 HDM_GETITEM = 0x1200 + 11;  // HDM_FIRST + 11
        public const UInt32 HDM_SETITEM = 0x1200 + 12;  // HDM_FIRST + 12

        //ActivateKeyboardLayout
        //AddClipboardFormatListener
        //AdjustWindowRect
        //AdjustWindowRectEx
        //AlignRects
        //AllowForegroundActivation
        //AllowSetForegroundWindow
        //AnimateWindow
        //AnyPopup
        //AppendMenuA
        //AppendMenuW
        //ArrangeIconicWindows
        //AttachThreadInput
        //BeginDeferWindowPos
        //BeginPaint
        //BlockInput
        //BringWindowToTop
        //BroadcastSystemMessage
        //BroadcastSystemMessageA
        //BroadcastSystemMessageExA
        //BroadcastSystemMessageExW
        //BroadcastSystemMessageW
        //BuildReasonArray
        //CalcMenuBar
        //CalculatePopupWindowPosition
        //CallMsgFilter
        //CallMsgFilterA
        //CallMsgFilterW
        //CallNextHookEx
        //CallWindowProcA
        //CallWindowProcW
        //CancelShutdown
        //CascadeChildWindows
        //CascadeWindows
        //ChangeClipboardChain
        //ChangeDisplaySettingsA
        //ChangeDisplaySettingsExA
        //ChangeDisplaySettingsExW
        //ChangeDisplaySettingsW
        //ChangeMenuA
        //ChangeMenuW
        //ChangeWindowMessageFilter
        //ChangeWindowMessageFilterEx
        //CharLowerA
        //CharLowerBuffA
        //CharLowerBuffW
        //CharLowerW
        //CharNextA
        //CharNextExA
        //CharNextW
        //CharPrevA
        //CharPrevExA
        //CharPrevW
        //CharToOemA
        //CharToOemBuffA
        //CharToOemBuffW
        //CharToOemW
        //CharUpperA
        //CharUpperBuffA
        //CharUpperBuffW
        //CharUpperW
        //CheckDBCSEnabledExt
        //CheckDlgButton
        //CheckMenuItem
        //CheckMenuRadioItem
        //CheckProcessForClipboardAccess
        //CheckProcessSession
        //CheckRadioButton
        //CheckWindowThreadDesktop
        //ChildWindowFromPoint
        //ChildWindowFromPointEx
        //CliImmSetHotKey
        //ClientThreadSetup

        /// <summary>
        /// 指定された点を、クライアント座標からスクリーン座標へ変換します。
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル</param>
        /// <param name="lpPoint">クライアント座標</param>
        /// <returns>関数が成功すると、0 以外の値が返ります。</returns>
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref System.Windows.Point lpPoint);
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref System.Drawing.Point lpPoint);

        /// <summary>
        /// マウスカーソル（ マウスポインタ）の移動可能な範囲を、指定された長方形の内側に制限します。
        /// 以後、マウスを動かしたり、SetCursorPos 関数を呼び出した結果、マウスカーソルが長方形より外側になると、
        /// システムは座標を自動的に調整して、この長方形の中にとどまらせます。
        /// </summary>
        /// <param name="lpRect">
        /// マウスの移動可能な範囲を表す左上隅と右下隅のスクリーン座標を保持する 構造体へのポインタを指定します。
        /// NULL を指定すると、マウスカーソルを画面上の自由な位置に動かせるようになります。
        /// </param>
        /// <returns>関数が成功すると、0 以外の値が返ります。</returns>
        [DllImport("user32.dll")]
        public static extern bool ClipCursor(ref RECT lpRect);

        /// <summary>
        /// クリップボードを閉じます。
        /// </summary>
        /// <returns>関数が成功すると、0 以外の値が返ります。</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool CloseClipboard();



        //CloseDesktop
        //CloseGestureInfoHandle
        //CloseTouchInputHandle
        //CloseWindow
        //CloseWindowStation
        //ConsoleControl
        //ControlMagnification
        //CopyAcceleratorTableA
        //CopyAcceleratorTableW
        //CopyIcon
        //CopyImage
        //CopyRect
        //CountClipboardFormats
        //CreateAcceleratorTableA
        //CreateAcceleratorTableW
        //CreateCaret
        //CreateCursor
        //CreateDCompositionHwndTarget
        //CreateDesktopA
        //CreateDesktopExA
        //CreateDesktopExW
        //CreateDesktopW
        //CreateDialogIndirectParamA
        //CreateDialogIndirectParamAorW
        //CreateDialogIndirectParamW
        //CreateDialogParamA
        //CreateDialogParamW
        //CreateIcon
        //CreateIconFromResource
        //CreateIconFromResourceEx
        //CreateIconIndirect
        //CreateMDIWindowA
        //CreateMDIWindowW
        //CreateMenu
        //CreatePopupMenu
        //CreateSystemThreads


        /// <summary>
        /// オーバーラップウィンドウ、ポップアップウィンドウ、子ウィンドウのいずれかを拡張スタイル付きで作成します。
        /// 拡張スタイルが指定できること以外は CreateWindow 関数と同じです。ウィンドウの作成方法、
        /// および CreateWindowEx 関数のその他のパラメータの詳しい説明については、CreateWindow の説明を参照してください。
        /// </summary>
        /// <param name="dwExStyle">作成するウィンドウの拡張ウィンドウスタイルを指定します。</param>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <param name="dwStyle">作成するウィンドウのスタイルを指定します。任意の組み合わせの に加えて、解説に示すコントロールスタイルも指定できます。</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <param name="hWndParent"></param>
        /// <param name="hMenu"></param>
        /// <param name="hInstance"></param>
        /// <param name="lpParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
           UInt32 dwExStyle,
           string lpClassName,
           string lpWindowName,
           UInt32 dwStyle,
           int x,
           int y,
           int nWidth,
           int nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam);

        //CreateWindowExA
        //CreateWindowExW
        //CreateWindowInBand
        //CreateWindowIndirect
        //CreateWindowStationA
        //CreateWindowStationW
        //CsrBroadcastSystemMessageExW
        //CtxInitUser32
        //DdeAbandonTransaction
        //DdeAccessData
        //DdeAddData
        //DdeClientTransaction
        //DdeCmpStringHandles
        //DdeConnect
        //DdeConnectList
        //DdeCreateDataHandle
        //DdeCreateStringHandleA
        //DdeCreateStringHandleW
        //DdeDisconnect
        //DdeDisconnectList
        //DdeEnableCallback
        //DdeFreeDataHandle
        //DdeFreeStringHandle
        //DdeGetData
        //DdeGetLastError
        //DdeGetQualityOfService
        //DdeImpersonateClient
        //DdeInitializeA
        //DdeInitializeW
        //DdeKeepStringHandle
        //DdeNameService
        //DdePostAdvise
        //DdeQueryConvInfo
        //DdeQueryNextServer
        //DdeQueryStringA
        //DdeQueryStringW
        //DdeReconnect
        //DdeSetQualityOfService
        //DdeSetUserHandle
        //DdeUnaccessData
        //DdeUninitialize
        //DefDlgProcA(forwardedtoNTDLL.NtdllDialogWndProc_A)
        //DefDlgProcW(forwardedtoNTDLL.NtdllDialogWndProc_W)
        //DefFrameProcA
        //DefFrameProcW
        //DefMDIChildProcA
        //DefMDIChildProcW
        //DefRawInputProc
        //DefWindowProcA(forwardedtoNTDLL.NtdllDefWindowProc_A)
        //DefWindowProcW(forwardedtoNTDLL.NtdllDefWindowProc_W)
        //DeferWindowPos
        //DeferWindowPosAndBand
        //DeleteMenu
        //DeregisterShellHookWindow
        //DestroyAcceleratorTable
        //DestroyCaret
        //DestroyCursor
        //DestroyDCompositionHwndTarget


        /// <summary>
        /// 1 個のアイコンを破棄し、そのアイコンに割り当てられていたメモリを解放します。
        /// </summary>
        /// <param name="hIcon">破棄対象のアイコンのハンドルを指定します。使用中のアイコンを指定してはなりません。</param>
        /// <returns>関数が成功すると、0 以外の値が返ります。</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);


        //DestroyMenu
        //DestroyReasons


        /// <summary>
        /// 指定されたウィンドウを破棄します。この関数は、 メッセージと メッセージをウィンドウに送って、ウィンドウを非アクティブ化し、
        /// キーボードの入力フォーカスをウィンドウから取り除きます。また、ウィンドウメニューの破棄、
        /// スレッドのメッセージキューのフラッシュ、タイマーの破棄、クリップボードの所有権の解放、
        /// クリップボードビューアチェインの切断も行います（ ウィンドウがビューアチェインの先頭にあった場合）。
        /// </summary>
        /// <param name="hwnd">破棄するウィンドウのハンドルを指定します。</param>
        /// <returns>関数が成功すると、0 以外の値が返ります。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hwnd);


        //DialogBoxIndirectParamA
        //DialogBoxIndirectParamAorW
        //DialogBoxIndirectParamW
        //DialogBoxParamA
        //DialogBoxParamW
        //DisableProcessWindowsGhosting
        //DispatchMessageA
        //DispatchMessageW
        //DisplayConfigGetDeviceInfo
        //DisplayConfigSetDeviceInfo
        //DisplayExitWindowsWarnings
        //DlgDirListA
        //DlgDirListComboBoxA
        //DlgDirListComboBoxW
        //DlgDirListW
        //DlgDirSelectComboBoxExA
        //DlgDirSelectComboBoxExW
        //DlgDirSelectExA
        //DlgDirSelectExW
        //DoSoundConnect
        //DoSoundDisconnect
        //DragDetect
        //DragObject
        //DrawAnimatedRects
        //DrawCaption
        //DrawCaptionTempA
        //DrawCaptionTempW
        //DrawEdge
        //DrawFocusRect
        //DrawFrame


        /// <summary>
        /// 指定されたタイプとスタイルを備える、ボタンやスクロールバーなどのフレームコントロールを描画します。
        /// </summary>
        /// <param name="hdc">コントロールの描画に使いたいデバイスコンテキストのハンドルを指定します。</param>
        /// <param name="lprc">長方形の論理座標を保持する 1 個の 構造体へのポインタを指定します。</param>
        /// <param name="uType">フレームコントロールのタイプを指定します。次の値のいずれかを指定します。
        ///   DFC_BUTTON	ボタンコントロールを描画します。
        ///   DFC_CAPTION	タイトルバーを描画します。
        ///   DCF_MENU	メニューを描画します。
        ///   DFC_POPUPMENU	Windows 98/Windows 2000：ポップアップメニューの項目を描画します。
        ///   DFC_SCROLL	スクロールバーを描画します。
        /// </param>
        /// <param name="uState">フレームコントロールの初期状態を指定します。uType パラメータが DFC_BUTTON の場合、uState パラメータに次のいずれかの値を指定します。</param>
        /// <returns>関数が成功すると、0 以外の値が返ります。</returns>
        [DllImport("user32.dll")]
        public static extern bool DrawFrameControl(IntPtr hdc, ref RECT lprc, uint uType, uint uState);

        /// <summary>
        /// 指定されたデバイスコンテキスト内で、1 個のアイコンまたはカーソル（マウスカーソル）を描画します。
        /// </summary>
        /// <param name="hDC">アイコンまたはカーソルの描画先となるデバイスコンテキストのハンドルを指定します。</param>
        /// <param name="X">アイコンの左上隅の x 座標を論理単位で指定します。</param>
        /// <param name="Y">アイコンの左上隅の y 座標を論理単位で指定します。</param>
        /// <param name="hIcon">描画対象のアイコンのハンドルを指定します。</param>
        /// <returns>関数が成功すると、0 以外の値が返ります。</returns>
        [DllImport("user32.dll")]
        public static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);

        /// <summary>
        /// 指定されたデバイスコンテキストで 1 個のアイコンまたはカーソル（マウスカーソル）を描画し、
        /// 指定されたラスタオペレーションを実行し、指定に応じてアイコンまたはカーソルを拡大または縮小します。
        /// </summary>
        /// <param name="hdc">アイコンまたはカーソルの描画先となるデバイスコンテキストのハンドルを指定します。</param>
        /// <param name="xLeft">アイコンまたはカーソルの左上隅の x 座標を論理単位で指定します。</param>
        /// <param name="yTop">アイコンまたはカーソルの左上隅の y 座標を論理単位で指定します。</param>
        /// <param name="hIcon">描画対象のアイコンまたはカーソルのハンドルを指定します。アニメーションカーソルのハンドルも指定できます。</param>
        /// <param name="cxWidth">アイコンまたはカーソルの幅を論理単位で指定します。このパラメータが 0 で、diFlags パラメータで DI_DEFAULTSIZE を指定した場合、この関数は SM_CXICON または SM_CXCURSOR のシステムメトリック値を使って幅を設定します。このパラメータが 0 で、DI_DEFAULTSIZE が指定されていない場合、リソースの実際の幅を使います。</param>
        /// <param name="cyHeight">アイコンまたはカーソルの高さを論理単位で指定します。このパラメータが 0 で、diFlags パラメータで DI_DEFAULTSIZE を指定した場合、この関数は SM_CYICON または SM_CYCURSOR のシステムメトリック値を使って高さを設定します。このパラメータが 0 で、DI_DEFAULTSIZE が指定されていない場合、リソースの実際の高さを使います。</param>
        /// <param name="istepIfAniCur">hIcon パラメータでアニメーションカーソルを指定した場合、描画対象のフレームのインデックス番号を指定します。hIcon パラメータがアニメーションカーソルを指定していない場合は、無視されます。アニメーションカーソル（.ani ファイル）は複数のビットマップで作成されていますが、各ビットマップをフレームと呼びます。そして、各フレームを識別するために割り当てられている番号（序数）がインデックス番号です。</param>
        /// <param name="hbrFlickerFreeDraw">1 個のブラシのハンドルを指定します。システムはこのブラシを使って、ちらつきのない描画を実現します。有効なブラシのハンドルを指定すると、システムは、指定されたブラシと背景色を使ってオフスクリーンビットマップを作成し、アイコンまたはカーソルをそのビットマップ内に描き、hdc パラメータで指定されたデバイスコンテキストへそのビットマップをコピーします。hbrFlickerFreeDraw パラメータで NULL を指定すると、システムは、アイコンまたはカーソルをデバイスコンテキストに直接描画します。</param>
        /// <param name="diFlags">描画フラグを指定します。次の値のいずれかを指定します。</param>
        /// <returns>関数が成功すると、0 以外の値が返ります。</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon,
           int cxWidth, int cyHeight, int istepIfAniCur, IntPtr hbrFlickerFreeDraw,
           int diFlags);

        //DrawMenuBar
        //DrawMenuBarTemp
        //DrawStateA
        //DrawStateW
        //DrawTextA
        //DrawTextExA
        //DrawTextExW
        //DrawTextW
        //DwmGetDxSharedSurface
        //DwmGetRemoteSessionOcclusionEvent
        //DwmGetRemoteSessionOcclusionState
        //DwmLockScreenUpdates
        //DwmStartRedirection
        //DwmStopRedirection
        //DwmValidateWindow
        //EditWndProc
        //EmptyClipboard
        //EnableMenuItem
        //EnableMouseInPointer
        //EnableScrollBar
        //EnableSessionForMMCSS


        /// <summary>
        /// 指定されたウィンドウまたはコントロールで、マウス入力とキーボード入力を有効または無効にします。入力を無効にすると、そのウィンドウはマウス入力やキーボード入力を受け付けません。入力を有効にすると、そのウィンドウはマウス入力やキーボード入力を受け付けます。
        /// </summary>
        /// <param name="hWnd">有効または無効にしたいウィンドウのハンドルを指定します。</param>
        /// <param name="bEnable">ウィンドウを有効にするか無効にするかを指定します。TRUE を指定すると有効に、FALSE を指定すると無効になります。</param>
        /// <returns>ウィンドウが既に無効になっている場合、0 以外の値が返ります。</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        //EndDeferWindowPos
        //EndDeferWindowPosEx
        //EndDialog
        //EndMenu
        //EndPaint
        //EndTask
        //EnterReaderModeHelper
        //EnumChildWindows
        //EnumClipboardFormats
        //EnumDesktopWindows
        //EnumDesktopsA
        //EnumDesktopsW
        //EnumDisplayDevicesA
        //EnumDisplayDevicesW
        //EnumDisplayMonitors
        //EnumDisplaySettingsA
        //EnumDisplaySettingsExA
        //EnumDisplaySettingsExW
        //EnumDisplaySettingsW
        //EnumPropsA
        //EnumPropsExA
        //EnumPropsExW
        //EnumPropsW
        //EnumThreadWindows
        //EnumWindowStationsA
        //EnumWindowStationsW
        //EnumWindows
        //EqualRect
        //EvaluateProximityToPolygon
        //EvaluateProximityToRect
        //ExcludeUpdateRgn


        /// <summary>
        /// 現在のユーザーをログオフさせるか、システムをシャットダウンさせるか、システムをシャットダウンさせて再起動させるか、いずれかを行います。この関数は、すべてのアプリケーションへ メッセージを送信して、それらのアプリケーションを終了できるかどうかを判断します。
        /// </summary>
        /// <param name="uFlags">シャットダウンのタイプを指定します。次の値のいずれかを指定します。</param>
        /// <param name="dwReason">予約されています。このパラメータは無視されます。</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ExitWindowsEx(uint uFlags, UInt32 dwReason);

        //FillRect
        //FindWindowA
        //FindWindowExA
        //FindWindowExW
        //FindWindowW

        /// <summary>
        /// 指定されたウィンドウのタイトルバーを 1 回点滅させます。ウィンドウのアクティブ状態を変更することはありません。
        /// </summary>
        /// <param name="hwnd">点滅対象のウィンドウのハンドルを指定します。このウィンドウは、開いていても最小化されていてもかまいません。</param>
        /// <param name="bInvert">ウィンドウの点滅状態を変化させるか、元の状態に戻すかを指定します。TRUE を指定すると、タイトルバーの点滅状態が変化します（アクティブのときは非アクティブになり、非アクティブのときはアクティブになります）。FALSE を指定すると、元の状態（アクティブか非アクティブ）に戻ります。アプリケーションが最小化されている場合、TRUE を指定すると、タスクバーのウィンドウボタンの状態が変化します（アクティブ/非アクティブのどちらか）。FALSE を指定すると、タスクバーのウィンドウボタンは非アクティブへ変化しますが、これは色が変わらないことを意味します。再描画する場合と同様に点滅状態が変化しますが、ユーザーにとっては、視覚的な反転による手がかりが何も得られません。</param>
        /// <returns>FlashWindow 関数を呼び出す前のウィンドウの点滅状態が返ります。呼び出しを行う前に、アクティブなウィンドウと同じ表示状態になっていた場合は、0 以外の値が返ります。非アクティブなウィンドウと同じ表示状態になっていた場合は、0 が返ります。</returns>
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        //FlashWindowEx
        //FrameRect
        //FreeDDElParam
        //FrostCrashedWindow
        //GetActiveWindow
        //GetAltTabInfo
        //GetAltTabInfoA
        //GetAltTabInfoW
        //GetAncestor
        //GetAppCompatFlags
        //GetAppCompatFlags2
        //GetAsyncKeyState
        //GetAutoRotationState
        //GetCIMSSM
        //GetCapture
        //GetCaretBlinkTime
        //GetCaretPos
        //GetClassInfoA
        //GetClassInfoExA
        //GetClassInfoExW
        //GetClassInfoW
        //GetClassLongA
        //GetClassLongPtrA
        //GetClassLongPtrW
        //GetClassLongW

        /// <summary>
        /// 指定されたウィンドウが属するクラスの名前を取得します。
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドルを指定します。クラスも間接的に指定したことになります（指定したウィンドウの属するクラスが使われます）。</param>
        /// <param name="lpClassName">バッファへのポインタを指定します。このバッファに、クラスの名前が文字列で格納されます。</param>
        /// <param name="nMaxCount">lpClassName パラメータがポイントするバッファの長さを TCHAR 単位で指定します。バッファに入り切らない部分は、切り捨てられます。</param>
        /// <returns>関数が成功すると、バッファにコピーされた TCHAR 値の数が返ります。</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        //GetClassWord
        //GetClientRect
        //GetClipCursor
        //GetClipboardAccessToken

        /// <summary>
        /// クリップボードから、指定された形式のデータを取得します。クリップボードは、あらかじめ開いておく必要があります。
        /// </summary>
        /// <param name="uFormat">クリップボードのデータ形式を指定します。詳細については、「」を参照してください。</param>
        /// <returns>データ形式のクリップボードオブジェクトのハンドルが返ります。</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetClipboardData(uint uFormat);

        //GetClipboardFormatNameA
        //GetClipboardFormatNameW
        //GetClipboardOwner
        //GetClipboardSequenceNumber
        //GetClipboardViewer
        //GetComboBoxInfo
        //GetCurrentInputMessageSource
        //GetCursor
        //GetCursorFrameInfo
        //GetCursorInfo
        //GetCursorPos

        /// <summary>
        /// 指定されたウィンドウのクライアント領域または画面全体を表すディスプレイデバイスコンテキストのハンドルを取得します。その後、GDI 関数を使って、返されたデバイスコンテキスト内で描画を行えます。
        /// </summary>
        /// <param name="hwnd">デバイスコンテキストの取得対象となるウィンドウのハンドルを指定します。NULL を指定すると、GetDC は画面全体を表すデバイスコンテキストを取得します。</param>
        /// <returns>関数が成功すると、指定したウィンドウのクライアント領域を表すデバイスコンテキストのハンドルが返ります。</returns>
        [DllImport("user32")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        /// <summary>
        /// 指定されたウィンドウのクライアント領域または画面全体を表すデバイスコンテキストのハンドルを取得します。その後、GDI 関数を使って、返されたデバイスコンテキスト内で描画を行えます。
        /// </summary>
        /// <param name="hWnd">デバイスコンテキストの取得対象となるウィンドウのハンドルを指定します。</param>
        /// <param name="hrgnClip">クリッピングリージョンを指定します。このクリッピングリージョンを、デバイスコンテキストの可視リージョンと組み合わせることもできます。</param>
        /// <param name="flags">デバイスコンテキストの作成方法を指定します。次の値の任意の組み合わせを指定します。</param>
        /// <returns>関数が成功すると、指定されたウィンドウに関連するデバイスコンテキストのハンドルが返ります。</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, UInt32 flags);

        //GetDesktopID
        //GetDesktopWindow
        //GetDialogBaseUnits
        //GetDisplayAutoRotationPreferences
        //GetDisplayConfigBufferSizes
        //GetDlgCtrlID
        //GetDlgItem
        //GetDlgItemInt
        //GetDlgItemTextA
        //GetDlgItemTextW
        //GetDoubleClickTime
        //GetDpiForMonitorInternal
        //GetFocus
        //GetForegroundWindow
        //GetGUIThreadInfo
        //GetGestureConfig
        //GetGestureExtraArgs
        //GetGestureInfo
        //GetGuiResources
        //GetIconInfo
        //GetIconInfoExA
        //GetIconInfoExW
        //GetInputDesktop
        //GetInputLocaleInfo
        //GetInputState
        //GetInternalWindowPos
        //GetKBCodePage
        //GetKeyNameTextA
        //GetKeyNameTextW
        //GetKeyState
        //GetKeyboardLayout
        //GetKeyboardLayoutList
        //GetKeyboardLayoutNameA
        //GetKeyboardLayoutNameW
        //GetKeyboardState
        //GetKeyboardType
        //GetLastActivePopup
        //GetLastInputInfo
        //GetLayeredWindowAttributes
        //GetListBoxInfo
        //GetMagnificationDesktopColorEffect
        //GetMagnificationDesktopMagnification
        //GetMagnificationLensCtxInformation
        //GetMenu
        //GetMenuBarInfo
        //GetMenuCheckMarkDimensions
        //GetMenuContextHelpId
        //GetMenuDefaultItem
        //GetMenuInfo
        //GetMenuItemCount
        //GetMenuItemID
        //GetMenuItemInfoA
        //GetMenuItemInfoW
        //GetMenuItemRect
        //GetMenuState
        //GetMenuStringA
        //GetMenuStringW
        //GetMessageA
        //GetMessageExtraInfo
        //GetMessagePos
        //GetMessageTime
        //GetMessageW
        //GetMonitorInfoA
        //GetMonitorInfoW
        //GetMouseMovePointsEx
        //GetNextDlgGroupItem
        //GetNextDlgTabItem
        //GetOpenClipboardWindow
        //GetParent
        //GetPhysicalCursorPos
        //GetPointerCursorId
        //GetPointerDevice
        //GetPointerDeviceCursors
        //GetPointerDeviceProperties
        //GetPointerDeviceRects
        //GetPointerDevices
        //GetPointerFrameInfo
        //GetPointerFrameInfoHistory
        //GetPointerFramePenInfo
        //GetPointerFramePenInfoHistory
        //GetPointerFrameTouchInfo
        //GetPointerFrameTouchInfoHistory
        //GetPointerInfo
        //GetPointerInfoHistory
        //GetPointerInputTransform
        //GetPointerPenInfo
        //GetPointerPenInfoHistory
        //GetPointerTouchInfo
        //GetPointerTouchInfoHistory
        //GetPointerType
        //GetPriorityClipboardFormat
        //GetProcessDefaultLayout
        //GetProcessDpiAwarenessInternal
        //GetProcessWindowStation
        //GetProgmanWindow
        //GetPropA
        //GetPropW
        //GetQueueStatus
        //GetRawInputBuffer
        //GetRawInputData
        //GetRawInputDeviceInfoA
        //GetRawInputDeviceInfoW
        //GetRawInputDeviceList
        //GetRawPointerDeviceData
        //GetReasonTitleFromReasonCode
        //GetRegisteredRawInputDevices
        //GetScrollBarInfo
        //GetScrollInfo
        //GetScrollPos
        //GetScrollRange
        //GetSendMessageReceiver
        //GetShellWindow
        //GetSubMenu
        //GetSysColor
        //GetSysColorBrush
        //GetSystemMenu
        //GetSystemMetrics
        //GetTabbedTextExtentA
        //GetTabbedTextExtentW
        //GetTaskmanWindow
        //GetThreadDesktop
        //GetTitleBarInfo
        //GetTopLevelWindow
        //GetTopWindow
        //GetTouchInputInfo
        //GetUnpredictedMessagePos
        //GetUpdateRect
        //GetUpdateRgn
        //GetUpdatedClipboardFormats
        //GetUserObjectInformationA
        //GetUserObjectInformationW
        //GetUserObjectSecurity
        //GetWinStationInfo
        //GetWindow
        //GetWindowBand
        //GetWindowCompositionAttribute
        //GetWindowCompositionInfo
        //GetWindowContextHelpId
        //GetWindowDC
        //GetWindowDisplayAffinity
        //GetWindowFeedbackSetting
        //GetWindowInfo
        //GetWindowLongA
        //GetWindowLongPtrA
        //GetWindowLongPtrW
        //GetWindowLongW
        //GetWindowMinimizeRect
        //GetWindowModuleFileName
        //GetWindowModuleFileNameA
        //GetWindowModuleFileNameW
        //GetWindowPlacement
        //GetWindowRect
        //GetWindowRgn
        //GetWindowRgnBox
        //GetWindowRgnEx
        //GetWindowTextA
        //GetWindowTextLengthA
        //GetWindowTextLengthW
        //GetWindowTextW
        //GetWindowThreadProcessId
        //GetWindowWord
        //GhostWindowFromHungWindow
        //GrayStringA
        //GrayStringW
        //HideCaret
        //HiliteMenuItem
        //HungWindowFromGhostWindow
        //IMPGetIMEA
        //IMPGetIMEW
        //IMPQueryIMEA
        //IMPQueryIMEW
        //IMPSetIMEA
        //IMPSetIMEW
        //ImpersonateDdeClientWindow
        //InSendMessage
        //InSendMessageEx
        //InflateRect
        //InitializeLpkHooks
        //InitializeTouchInjection
        //InjectTouchInput
        //InsertMenuA
        //InsertMenuItemA
        //InsertMenuItemW
        //InsertMenuW
        //InternalGetWindowIcon
        //InternalGetWindowText
        //IntersectRect
        //InvalidateRect
        //InvalidateRgn
        //InvertRect
        //IsCharAlphaA
        //IsCharAlphaNumericA
        //IsCharAlphaNumericW
        //IsCharAlphaW
        //IsCharLowerA
        //IsCharLowerW
        //IsCharUpperA
        //IsCharUpperW
        //IsChild
        //IsClipboardFormatAvailable
        //IsDialogMessage
        //IsDialogMessageA
        //IsDialogMessageW
        //IsDlgButtonChecked
        //IsGUIThread
        //IsHungAppWindow
        //IsIconic
        //IsImmersiveProcess
        //IsInDesktopWindowBand
        //IsMenu
        //IsMouseInPointerEnabled
        //IsProcessDPIAware
        //IsQueueAttached
        //IsRectEmpty
        //IsSETEnabled
        //IsServerSideWindow
        //IsThreadDesktopComposited
        //IsThreadMessageQueueAttached
        //IsTopLevelWindow
        //IsTouchWindow
        //IsWinEventHookInstalled
        //IsWindow
        //IsWindowEnabled
        //IsWindowInDestroy
        //IsWindowRedirectedForPrint
        //IsWindowUnicode
        //IsWindowVisible
        //IsWow64Message
        //IsZoomed
        //KillTimer
        //LoadAcceleratorsA
        //LoadAcceleratorsW
        //LoadBitmapA
        //LoadBitmapW
        //LoadCursorA
        //LoadCursorFromFileA
        //LoadCursorFromFileW
        //LoadCursorW
        //LoadIconA
        //LoadIconW
        //LoadImageA
        //LoadImageW
        //LoadKeyboardLayoutA
        //LoadKeyboardLayoutEx
        //LoadKeyboardLayoutW
        //LoadLocalFonts
        //LoadMenuA
        //LoadMenuIndirectA
        //LoadMenuIndirectW
        //LoadMenuW
        //LoadRemoteFonts
        //LoadStringA
        //LoadStringW
        //LockSetForegroundWindow
        //LockWindowStation
        //LockWindowUpdate
        //LockWorkStation
        //LogicalToPhysicalPoint
        //LogicalToPhysicalPointForPerMonitorDPI
        //LookupIconIdFromDirectory
        //LookupIconIdFromDirectoryEx
        //MBToWCSEx
        //MBToWCSExt
        //MB_GetString
        //MapDialogRect
        //MapVirtualKeyA
        //MapVirtualKeyExA
        //MapVirtualKeyExW
        //MapVirtualKeyW
        //MapWindowPoints
        //MenuItemFromPoint
        //MenuWindowProcA
        //MenuWindowProcW
        //MessageBeep
        //MessageBoxA
        //MessageBoxExA
        //MessageBoxExW
        //MessageBoxIndirectA
        //MessageBoxIndirectW
        //MessageBoxTimeoutA
        //MessageBoxTimeoutW
        //MessageBoxW
        //ModifyMenuA
        //ModifyMenuW
        //MonitorFromPoint
        //MonitorFromRect
        //MonitorFromWindow
        //MoveWindow
        //MsgWaitForMultipleObjects
        //MsgWaitForMultipleObjectsEx
        //NotifyOverlayWindow
        //NotifyWinEvent
        //OemKeyScan
        //OemToCharA
        //OemToCharBuffA
        //OemToCharBuffW
        //OemToCharW
        //OffsetRect
        //OpenClipboard
        //OpenDesktopA
        //OpenDesktopW
        //OpenIcon
        //OpenInputDesktop
        //OpenThreadDesktop
        //OpenWindowStationA
        //OpenWindowStationW
        //PackDDElParam
        //PackTouchHitTestingProximityEvaluation
        //PaintDesktop
        //PaintMenuBar
        //PaintMonitor
        //PeekMessageA
        //PeekMessageW
        //PhysicalToLogicalPoint
        //PhysicalToLogicalPointForPerMonitorDPI
        //PostMessageA
        //PostMessageW
        //PostQuitMessage
        //PostThreadMessageA
        //PostThreadMessageW
        //PrintWindow
        //PrivateExtractIconExA
        //PrivateExtractIconExW
        //PrivateExtractIconsA
        //PrivateExtractIconsW
        //PrivateRegisterICSProc
        //PtInRect
        //QueryBSDRWindow
        //QueryDisplayConfig
        //QuerySendMessage
        //RealChildWindowFromPoint
        //RealGetWindowClass
        //RealGetWindowClassA
        //RealGetWindowClassW
        //ReasonCodeNeedsBugID
        //ReasonCodeNeedsComment
        //RecordShutdownReason
        //RedrawWindow
        //RegisterBSDRWindow
        //RegisterClassA
        //RegisterClassExA
        //RegisterClassExW
        //RegisterClassW
        //RegisterClipboardFormatA
        //RegisterClipboardFormatW
        //RegisterDeviceNotificationA
        //RegisterDeviceNotificationW
        //RegisterErrorReportingDialog
        //RegisterFrostWindow
        //RegisterGhostWindow
        //RegisterHotKey
        //RegisterLogonProcess
        //RegisterMessagePumpHook
        //RegisterPointerDeviceNotifications
        //RegisterPointerInputTarget
        //RegisterPowerSettingNotification
        //RegisterRawInputDevices
        //RegisterServicesProcess
        //RegisterSessionPort
        //RegisterShellHookWindow
        //RegisterSuspendResumeNotification
        //RegisterSystemThread
        //RegisterTasklist
        //RegisterTouchHitTestingWindow
        //RegisterTouchWindow
        //RegisterUserApiHook
        //RegisterWindowMessageA
        //RegisterWindowMessageW
        //ReleaseCapture

        /// <summary>
        /// デバイスコンテキストを解放し、他のアプリケーションからつかえるようにします。ReleaseDC 関数の効果は、デバイスコンテキストのタイプによって異なります。この関数は、共通デバイスコンテキストとウィンドウデバイスコンテキストだけを解放します。クラスデバイスコンテキストやプライベートデバイスコンテキストには効果がありません。
        /// </summary>
        /// <param name="hWnd">解放対象のデバイスコンテキストに対応するウィンドウのハンドルを指定します。</param>
        /// <param name="hDC">解放対象のデバイスコンテキストのハンドルを指定します。</param>
        /// <returns>戻り値は、デバイスコンテキストを解放したかどうかを示します。デバイスコンテキストが解放された場合、1 が返ります。</returns>
        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        //RemoveClipboardFormatListener
        //RemoveMenu
        //RemovePropA
        //RemovePropW
        //ReplyMessage
        //ResolveDesktopForWOW
        //ReuseDDElParam
        //ScreenToClient
        //ScrollChildren
        //ScrollDC
        //ScrollWindow
        //ScrollWindowEx
        //SendDlgItemMessageA
        //SendDlgItemMessageW
        //SendIMEMessageExA
        //SendIMEMessageExW
        //SendInput

        /// <summary>
        /// 1 つまたは複数のウィンドウへ、指定されたメッセージを送信します。この関数は、指定されたウィンドウのウィンドウプロシージャを呼び出し、そのウィンドウプロシージャがメッセージを処理し終わった後で、制御を返します。
        /// </summary>
        /// <param name="hWnd">1 つのウィンドウのハンドルを指定します。このウィンドウのウィンドウプロシージャがメッセージを受信します。HWND_BROADCAST を指定すると、この関数は、システム内のすべてのトップレベルウィンドウ（親を持たないウィンドウ）へメッセージを送信します。無効になっている所有されていないウィンドウ、不可視の所有されていないウィンドウ、オーバーラップされた（手前にほかのウィンドウがあって覆い隠されている）ウィンドウ、ポップアップウィンドウも送信先になります。子ウィンドウへはメッセージを送信しません。</param>
        /// <param name="Msg">送信するべきメッセージを指定します。</param>
        /// <param name="wParam">メッセージ特有の追加情報を指定します。</param>
        /// <param name="lParam">メッセージ特有の追加情報を指定します。</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, UInt32 wParam, UInt32 lParam);

        //SendMessageCallbackA
        //SendMessageCallbackW
        //SendMessageTimeoutA
        //SendMessageTimeoutW

        //SendNotifyMessageA
        //SendNotifyMessageW
        //SetActiveWindow
        //SetCapture
        //SetCaretBlinkTime
        //SetCaretPos
        //SetClassLongA
        //SetClassLongPtrA
        //SetClassLongPtrW
        //SetClassLongW
        //SetClassWord
        //SetClipboardData
        //SetClipboardViewer
        //SetCoalescableTimer
        //SetCursor
        //SetCursorContents
        //SetCursorPos
        //SetDebugErrorLevel
        //SetDeskWallpaper
        //SetDisplayAutoRotationPreferences
        //SetDisplayConfig
        //SetDlgItemInt
        //SetDlgItemTextA
        //SetDlgItemTextW
        //SetDoubleClickTime
        //SetFocus
        //SetForegroundWindow
        //SetGestureConfig
        //SetImmersiveBackgroundWindow
        //SetInternalWindowPos
        //SetKeyboardState
        //SetLastErrorEx
        //SetLayeredWindowAttributes
        //SetMagnificationDesktopColorEffect
        //SetMagnificationDesktopMagnification
        //SetMagnificationLensCtxInformation
        //SetMenu
        //SetMenuContextHelpId
        //SetMenuDefaultItem
        //SetMenuInfo
        //SetMenuItemBitmaps
        //SetMenuItemInfoA
        //SetMenuItemInfoW
        //SetMessageExtraInfo
        //SetMessageQueue
        //SetMirrorRendering
        //SetParent
        //SetPhysicalCursorPos
        //SetProcessDPIAware
        //SetProcessDefaultLayout
        //SetProcessDpiAwarenessInternal
        //SetProcessRestrictionExemption
        //SetProcessWindowStation
        //SetProgmanWindow
        //SetPropA
        //SetPropW
        //SetRect
        //SetRectEmpty
        //SetScrollInfo
        //SetScrollPos
        //SetScrollRange
        //SetShellWindow
        //SetShellWindowEx
        //SetSysColors
        //SetSysColorsTemp
        //SetSystemCursor
        //SetSystemMenu
        //SetTaskmanWindow
        //SetThreadDesktop
        //SetThreadInputBlocked
        //SetTimer
        //SetUserObjectInformationA
        //SetUserObjectInformationW
        //SetUserObjectSecurity
        //SetWinEventHook
        //SetWindowBand
        //SetWindowCompositionAttribute
        //SetWindowCompositionTransition
        //SetWindowContextHelpId
        //SetWindowDisplayAffinity
        //SetWindowFeedbackSetting
        //SetWindowLongA
        //SetWindowLongPtrA
        //SetWindowLongPtrW
        //SetWindowLongW
        //SetWindowPlacement
        //SetWindowPos
        //SetWindowRgn
        //SetWindowRgnEx
        //SetWindowStationUser
        //SetWindowTextA
        //SetWindowTextW
        //SetWindowWord
        //SetWindowsHookA
        //SetWindowsHookExA
        //SetWindowsHookExW
        //SetWindowsHookW
        //ShowCaret
        //ShowCursor
        //ShowOwnedPopups
        //ShowScrollBar
        //ShowStartGlass
        //ShowSystemCursor
        //ShowWindow
        //ShowWindowAsync
        //ShutdownBlockReasonCreate
        //ShutdownBlockReasonDestroy
        //ShutdownBlockReasonQuery
        //SignalRedirectionStartComplete
        //SkipPointerFrameMessages
        //SoftModalMessageBox
        //SoundSentry
        //SubtractRect
        //SwapMouseButton
        //SwitchDesktop
        //SwitchDesktopWithFade
        //SwitchToThisWindow
        //SystemParametersInfoA
        //SystemParametersInfoW
        //TabbedTextOutA
        //TabbedTextOutW
        //TileChildWindows
        //TileWindows
        //ToAscii
        //ToAsciiEx
        //ToUnicode
        //ToUnicodeEx
        //TrackMouseEvent
        //TrackPopupMenu
        //TrackPopupMenuEx
        //TranslateAccelerator
        //TranslateAcceleratorA
        //TranslateAcceleratorW
        //TranslateMDISysAccel
        //TranslateMessage
        //TranslateMessageEx
        //UnhookWinEvent
        //UnhookWindowsHook
        //UnhookWindowsHookEx
        //UnionRect
        //UnloadKeyboardLayout
        //UnlockWindowStation
        //UnpackDDElParam
        //UnregisterClassA
        //UnregisterClassW
        //UnregisterDeviceNotification
        //UnregisterHotKey
        //UnregisterMessagePumpHook
        //UnregisterPointerInputTarget
        //UnregisterPowerSettingNotification
        //UnregisterSessionPort
        //UnregisterSuspendResumeNotification
        //UnregisterTouchWindow
        //UnregisterUserApiHook
        //UpdateDefaultDesktopThumbnail
        //UpdateLayeredWindow
        //UpdateLayeredWindowIndirect
        //UpdatePerUserSystemParameters
        //UpdateWindow
        //UpdateWindowInputSinkHints
        //UpdateWindowTransform
        //User32InitializeImmEntryTable
        //UserClientDllInitialize
        //UserHandleGrantAccess
        //UserLpkPSMTextOut
        //UserLpkTabbedTextOut
        //UserRealizePalette
        //UserRegisterWowHandlers
        //VRipOutput
        //VTagOutput
        //ValidateRect
        //ValidateRgn
        //VkKeyScanA
        //VkKeyScanExA
        //VkKeyScanExW
        //VkKeyScanW
        //WCSToMBEx
        //WINNLSEnableIME
        //WINNLSGetEnableStatus
        //WINNLSGetIMEHotkey
        //WaitForInputIdle
        //WaitForRedirectionStartComplete
        //WaitMessage
        //WinHelpA
        //WinHelpW
        //WindowFromDC
        //WindowFromPhysicalPoint
        //WindowFromPoint
        //_UserTestTokenForInteractive
        //gSharedInfo
        //gapfnScSendMessage
        //keybd_event
        //mouse_event
        //wsprintfA
        //wsprintfW
        //wvsprintfA
        //wvsprintfW

    }
}
