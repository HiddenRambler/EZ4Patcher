using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace EZ4Patcher
{
    public class EZBridge
    {
        [DllImport("ezbridge.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern void PatchRom(
            [Out] byte[] data,
            uint datasize,
            ref uint truncatedSize,
            ref ushort saveType,
            ref uint saveSize);
    }
}
