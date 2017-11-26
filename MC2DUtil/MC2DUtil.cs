using System;
using System.IO;
using System.Reflection;

namespace MC2DUtil
{

    public static class MC2DUtil
    {
        /// <summary>
        /// 
        /// </summary>
        static MC2DUtil()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }

        /// <summary>
        /// カスタムアセンブリリゾルバは、アーキテクチャ固有の実装アセンブリを検索します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("MC2DUtil", StringComparison.OrdinalIgnoreCase))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var path = GetAssemblyPath(Path.GetDirectoryName(assembly.Location));

                if (!File.Exists(path))
                {
                    // ファイルが見つからない場合は、代わりにCodeBaseを使用してみてください
                    //（シャドウコピーを使用する場合は異なる場合があります）。
                    // CodeBaseはURIなので、ファイル名を解析する必要があります。
                    path = GetAssemblyPath(Path.GetDirectoryName(new Uri(assembly.CodeBase).AbsolutePath));
                }

                return Assembly.LoadFile(path);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static string GetAssemblyPath(string dir) => Path.Combine(dir, ArchitectureMoniker, "MC2DUtil.dll");

        // 注：現在、ARMはサポートされていません。
        // x64 'AMD64'を呼び出すので、％PROCESSOR_ARCHITECTURE％を使用しないでください。
        private static string ArchitectureMoniker => Environment.Is64BitProcess ? "x64" : "x86";

    }
}
