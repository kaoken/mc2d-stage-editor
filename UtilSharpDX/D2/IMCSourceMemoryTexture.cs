using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.D2
{
    public interface IMCSourceMemoryTexture
    {
        int GetSize();
        void Lock();
        void Unlock();
    };
}
