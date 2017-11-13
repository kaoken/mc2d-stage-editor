using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace UtilSharpDX.HLSL
{
    /// <summary>
    /// HLSLのRSCソースの管理方法の列挙対
    /// </summary>
    public enum HLSLRSC_SOURCELOCATION
    {
        /// <summary>
        /// 通常
        /// </summary>
        DEFAULT,
        /// <summary>
        /// ファイルから
        /// </summary>
        FILE,
        /// <summary>
        /// メモリーから
        /// </summary>
        MEMORY,
        /// <summary>
        /// リソース
        /// </summary>
        RESOURCE
    };



    public sealed class DXHLSLMgr : IApp
    {
        /// <summary>
        /// DirectX11デバイス
        /// </summary>
        public Application App { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, WeakReference<IMCEffect>> m_nameHLSLIndex = new Dictionary<string, WeakReference<IMCEffect>>();

        /// <summary>
        /// 
        /// </summary>
        private void Create()
        {

        }
        /// <summary>
        /// 終了時の処理
        /// </summary>
        private void Destroy()
        {
            int n = 0;
            string strDebugTmp="";

            //==== HLSL

            foreach (var val in m_nameHLSLIndex)
            {
                IMCEffect target;
                if (val.Value.TryGetTarget(out target))
			    {
                    ++n;
                    strDebugTmp += target.GetName() + "\n";
                }
            }
            m_nameHLSLIndex.Clear();
		    if (n != 0){
                MessageBox.Show(strDebugTmp,"HLSLが解放されてない。",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
		    }

        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        public DXHLSLMgr(Application app)
        {
            App = app;
        }
        ~DXHLSLMgr()
        {
            Destroy();
        }

        /// <summary>
        /// 作成時の処理
        /// </summary>
        /// <param name="srcFile">エフェクトファイル名</param>
        /// <param name="effect">MCEffectが格納される</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool GetEffect(string srcFile, out IMCEffect effect)
        {
            effect = null;
            if (m_nameHLSLIndex.ContainsKey(srcFile))
            {
                IMCEffect target;
                if (!m_nameHLSLIndex[srcFile].TryGetTarget(out target))
                {
                    m_nameHLSLIndex.Remove(srcFile);
                    return false;
                }
                effect = target;
                return true;
            }
            return false;
        }

        /// <summary>
        /// ファイルを指定してエフェクトを作成する。
        /// </summary>
        /// <param name="srcFile">エフェクトファイル名</param>
        /// <param name="effect"></param>
        /// <param name="errMsg">エラーメッセージを出力する。</param>
        /// <returns>成功した場合は、0 を返す。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetCreateEffectFromFile(string srcFile, out IMCEffect effect, out string errMsg)
        {
            errMsg = "";
            effect = null;
            // 既に存在するか？
            if (GetEffect(srcFile, out effect)) return 0;


            Effect effectTmp;
            if (Path.GetExtension(srcFile) == ".cfx")
            {
                byte[] effectByteCode;
                var fs = new System.IO.FileStream(
                    srcFile,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);
                    effectByteCode = new byte[fs.Length];
                    fs.Read(effectByteCode, 0, (int)fs.Length);
                fs.Close();
                effectTmp = new Effect(App.DXDevice, effectByteCode);
            }
            else
            {
                var effectByteCode = ShaderBytecode.CompileFromFile("HLSL\\" + srcFile, "fx_5_0", ShaderFlags.None, EffectFlags.None, null, new HLSLIncludeFile());
                if (effectByteCode.HasErrors)
                {
                    Debug.WriteLine(errMsg);
                    errMsg = effectByteCode.Message;
                    return -1;
                }
                Debug.WriteLine(errMsg);
                effectTmp = new Effect(App.DXDevice, effectByteCode);
            }


            effect = new MCEffect(effectTmp, HLSLRSC_SOURCELOCATION.FILE, srcFile);
            RegisterHLSL(srcFile, effect);

            return 0;
        }


        /// <summary>
        /// リソース内のファイルを指定してエフェクトを作成する。
        /// </summary>
        /// <param name="resourcePrefix">埋め込みリソースの場所を表す接頭辞</param>
        /// <param name="srcFile">エフェクトファイル名</param>
        /// <param name="effect"></param>
        /// <param name="errMsg">エラーメッセージを出力する。</param>
        /// <returns>成功した場合は、0 を返す。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetCreateEffectFromResource(string resourcePrefix, string srcFile, out IMCEffect effect, out string errMsg)
        {
            errMsg = "";
            effect = null;
            // 既に存在するか？
            if (GetEffect(srcFile, out effect)) return 0;



            //System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            //string[] names = myAssembly.GetManifestResourceNames(); // it is really there by its name "shader.tkb"
            //Stream myStream = myAssembly.GetManifestResourceStream(names[0]);


            Effect effectTmp;
            if (Path.GetExtension(srcFile) == ".cfx")
            {
                byte[] effectByteCode;
                using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePrefix + "." + srcFile))
                {
                    effectByteCode = new byte[s.Length];
                    s.Read(effectByteCode, 0, (int)s.Length);
                }
                effectTmp = new Effect(App.DXDevice, effectByteCode);
            }
            else
            {
                string hlslText;
                using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePrefix + "." + srcFile))
                {
                    using (StreamReader r = new StreamReader(s))
                    {
                        hlslText = r.ReadToEnd();
                    }
                }
                var effectByteCode = ShaderBytecode.Compile(hlslText, "fx_5_0", ShaderFlags.None, EffectFlags.None, null, new HLSLIncludeResource(resourcePrefix));
                if (effectByteCode.HasErrors)
                {
                    Debug.WriteLine(errMsg);
                    errMsg = effectByteCode.Message;
                    return -1;
                }
                Debug.WriteLine(errMsg);
                effectTmp = new Effect(App.DXDevice, effectByteCode.Bytecode);
            }

            effect = new MCEffect(effectTmp, HLSLRSC_SOURCELOCATION.RESOURCE, srcFile);
            RegisterHLSL(srcFile, effect);

            return 0;
        }



        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        internal int OnCreateDevice(Device device)
        {
            foreach (var val in m_nameHLSLIndex)
            {
                IMCEffect effect;
                if (val.Value.TryGetTarget(out effect))
                    effect.OnCreateDevice(device);
            }

            //==========================
            // Blend State 
            //==========================
            App.BlendStateMgr.OnCreateDevice(device);
		    //==========================
		    // レイアウト
		    //==========================
		    App.LayoutMgr.OnCreateDevice(device);

		    return 0;
	    }

        /// <summary>
        /// OnD3D11CreateDeviceで作成されたすべてのD3D11のリソースを解放
        /// </summary>
        internal void OnDestroyDevice()
        {
            foreach (var val in m_nameHLSLIndex)
            {
                IMCEffect effect;
                if (val.Value.TryGetTarget(out effect))
                    effect.OnDestroyDevice();
            }
            //==========================
            // Blend State 
            //==========================
            App.BlendStateMgr.OnDestroyDevice();
            //==========================
            // レイアウト
            //==========================
            App.LayoutMgr.OnDestroyDevice();
	    }

        /// <summary>
        /// MainLoopを抜け出し、終了後に呼ばれる
        /// </summary>
        internal void OnEndDevice()
        {
            //==========================
            // Blend State 
            //==========================
            App.BlendStateMgr.OnEndDevice();


            Destroy();
        }

        /// <summary>
        /// 指定した名前と関連づけてrSPを登録する
        /// </summary>
        /// <param name="name">エフェクトファイル名</param>
        /// <param name="effect"></param>
        /// <returns>既に存在していた場合、登録しないでfalseを返す。登録されていない場合、trueを返す</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private bool RegisterHLSL(string name, IMCEffect effect)
	    {
            if (!m_nameHLSLIndex.ContainsKey(name))
            {
                m_nameHLSLIndex[name] = new WeakReference<IMCEffect>(effect);
                return true;
            }

            return true;
	    }
    }
}
