using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilSharpDX.Mesh
{
    public sealed class MCMeshMgr : IApp
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, WeakReference<MCBaseMesh>> m_nameMeshIndex = new Dictionary<string, WeakReference<MCBaseMesh>>();
        /// <summary>
        /// 
        /// </summary>
        private int m_meshCounterID = 1;

        /// <summary>
        /// DirectX11デバイス
        /// </summary>
        public Application App { get; private set; }


        /// <summary>
        /// 作成時の処理
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
            string output="";

            foreach (var val in m_nameMeshIndex)
            {
                ++n;
                output += val.Key + "\n";
            }
            m_nameMeshIndex.Clear();

            if (n != 0)
            {
                output += "\n合計 " + n + "\n";
                MessageBox.Show(output,
                    "メッシュが解放されませんでした。",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        public MCMeshMgr(Application app)
        {
            App = app;
            m_meshCounterID = 1;
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MCMeshMgr()
        {
            Destroy();
        }
        //========================
        //==== デバイス
        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="device">新しく作成された Device デバイス</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返すようにプログラムする。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal int OnCreateDevice(Device device)
        {
            foreach (var val in m_nameMeshIndex)
            {
                MCBaseMesh mesh;
                if (val.Value.TryGetTarget(out mesh))
                    mesh.OnCreateDevice(device);
            }

            //#######################################################
            //# CDXObjectMeshを登録
            //#######################################################
            /*CDXObjectMesh* pObjMesh = new CDXObjectMesh(this);
		    MC_ASSERT(pObjMesh!=nullptr);

		    this->InsertNewObjMeshCOM((ISGMesh*)(pObjMesh)();*/

            //#######################################################
            //# 補助メッシュ作成
            //#######################################################
            //	LPOBJECT_MESH_INFO_MGR pOMIM;
            bool bRet;
            //=============================
            // 境界ボックス
            //=============================
            bRet = true;//this->GetHandle(MC_T("BoundingBoxMesh"), &HMCMesh );
		    if (bRet){
			    // 存在する
		    }
		    else{
			    // 存在しない
			    /*
			    CBoundingBoxMesh* pBBM;
			    pBBM = new CBoundingBoxMesh(this);
			    if( pBBM == nullptr ){
			    DEBUG_ERR_ICONMSG(MC_T("予期せぬ処理を検出(MCMeshMgr)"), MC_T("BoundingBoxMesh作成失敗。");
			    return MC_E_OUTOFMEMORY;
			    }
			    bRet = this->RegisterMesh(MC_T("BoundingBoxMesh"), &m_hBoundingBox, &pOMIM();
			    if( !bRet ) {
			    MC_ASSERT(0);
			    this->MeshRelease(m_hBoundingBox);
			    }
			    MC_ASSERT(pOMIM!=nullptr);

			    pBBM->SetName(MC_T("BoundingBoxMesh");
			    pBBM->OnCreateDevice(pD3DDevice);
			    pOMIM->pISGMesh = dynamic_cast<ISGMesh*>(pBBM);

			    if( pOMIM->pISGMesh == nullptr ){
			    DEBUG_ERR_ICONMSG(MC_T("不正な処理を検出"), MC_T("キャスト失敗");
			    return MC_E_FAIL;
			    }

			    pOMIM = nullptr;*/
		    }
		    //=============================
		    // 境界球
		    //=============================
		    //=============================
		    // ワイヤーフレームカメラ
		    //=============================


		    return 0;
	    }
        /// <summary>
        ///  MainLoopを抜け出し、終了後に呼ばれる
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnDestroyDevice()
        {
            foreach (var val in m_nameMeshIndex)
            {
                MCBaseMesh mesh;
                if (val.Value.TryGetTarget(out mesh))
                    mesh.OnDestroyDevice();
            }

            Destroy();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetMeshCounterID() { return m_meshCounterID; }


        /// <summary>
        /// 任意のメッシュ名で、メッシュを登録する
        /// </summary>
        /// <param name="name">メッシュ名</param>
        /// <param name="mesh">登録するMCBaseMeshSP</param>
        /// <returns>登録できた場合、trueを返し、既に登録済みのメッシュ名があった場合、falseを返す。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool RegisterMesh(string name, MCBaseMesh mesh)
        {
            if (m_nameMeshIndex.ContainsKey(name))
            {
                return false;
		    }
		    m_nameMeshIndex.Add(name, new WeakReference<MCBaseMesh>(mesh));
		    return true;
	    }

        /// <summary>
        /// 任意のメッシュ名で、メッシュを取得する
        /// </summary>
        /// <param name="name">メッシュ名</param>
        /// <param name="mesh">取得するMCBaseMesh</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool GetMesh(string name, out MCBaseMesh mesh)
        {
            mesh = null;
            if (!m_nameMeshIndex.ContainsKey(name)) return false;


            if (m_nameMeshIndex[name].TryGetTarget(out mesh))
		    {
                m_nameMeshIndex.Remove(name);
                return false;
            }
            return true;
        }

    }
}
