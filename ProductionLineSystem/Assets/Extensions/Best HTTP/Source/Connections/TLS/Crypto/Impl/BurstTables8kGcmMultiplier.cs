#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR) && BESTHTTP_WITH_BURST
#pragma warning disable
using System;

using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;

namespace BestHTTP.Connections.TLS.Crypto.Impl
{
    [BestHTTP.PlatformSupport.IL2CPP.Il2CppEagerStaticClassConstructionAttribute]
    [BurstCompile]
    public sealed class BurstTables8kGcmMultiplier
    {
        private byte[] H;
        private ulong[] T;

        public void Init(byte[] H)
        {
            if (T == null)
            {
                T = new ulong[32 * 32];
            }
            else if (Arrays.AreEqual(this.H, H))
            {
                return;
            }

            this.H = Arrays.Clone(H);

            for (int i = 0; i < 32; ++i)
            {
                //ulong[] t = T[i] = new ulong[32];

                // t[0] = 0

                if (i == 0)
                {
                    // t[1] = H.p^3
                    //GcmUtilities.AsUlongs(this.H, t, 2);
                    Pack.BE_To_UInt64(this.H, 0, this.T, (i * 32) + 2, 2);

                    // GcmUtilities.MultiplyP3(t, 2, t, 2);
                    GcmUtilities.MultiplyP3(this.T, (i * 32) + 2, this.T, (i * 32) + 2);
                }
                else
                {
                    // t[1] = T[i-1][1].p^4
                    //GcmUtilities.MultiplyP4(T[i - 1], 2, t, 2);
                    GcmUtilities.MultiplyP4(this.T, ((i - 1) * 32) + 2, this.T, (i * 32) + 2);
                }

                for (int n = 2; n < 16; n += 2)
                {
                    // t[2.n] = t[n].p^-1
                    //GcmUtilities.DivideP(t, n, t, n << 1);
                    GcmUtilities.DivideP(this.T, (i * 32) + n, this.T, (i * 32) + (n << 1));

                    // t[2.n + 1] = t[2.n] + t[1]
                    //GcmUtilities.Xor(t, n << 1, t, 2, t, (n + 1) << 1);

                    GcmUtilities.Xor(this.T, (i * 32) + (n << 1), this.T, (i * 32) + 2, this.T, (i * 32) + ((n + 1) << 1));
                }
            }
        }

        public unsafe void MultiplyH(byte[] x)
        {
            //ulong[] z = new ulong[2];
            //for (int i = 15; i >= 0; --i)
            //{
            //    GcmUtilities.Xor(z, 0, T[i + i + 1], (x[i] & 0x0F) << 1);
            //    GcmUtilities.Xor(z, 0, T[i + i], (x[i] & 0xF0) >> 3);
            //}
            //Pack.UInt64_To_BE(z, x, 0);

            fixed (byte* px = x)
            fixed (ulong* pt = this.T)
                MultiplyHImpl(px, pt);
        }

        [BurstCompile]
        private unsafe void MultiplyHImpl(byte* x, ulong* t)
        {
            //ulong z0 = 0, z1 = 0;
            ulong* z = stackalloc ulong[2];
            z[0] = 0;
            z[1] = 0;

            for (int i = 15; i >= 0; --i)
            {
                //ulong[] tu = T[i + i + 1];
                //ulong[] tv = T[i + i];
                int tuIdx = (i + i + 1) * 32;
                int tvIdx = (i + i) * 32;

                int uPos = (x[i] & 0x0F) << 1, vPos = (x[i] & 0xF0) >> 3;

                //z0 ^= tu[uPos + 0] ^ tv[vPos + 0];
                //z1 ^= tu[uPos + 1] ^ tv[vPos + 1];

                z[0] ^= t[tuIdx + uPos + 0] ^ t[tvIdx + vPos + 0];
                z[1] ^= t[tuIdx + uPos + 1] ^ t[tvIdx + vPos + 1];
            }

            //UInt32_To_BE((uint)(n >> 32), bs, off);
            uint n = (uint)(z[0] >> 32);
            x[0] = (byte)(n >> 24);
            x[1] = (byte)(n >> 16);
            x[2] = (byte)(n >> 8);
            x[3] = (byte)(n);
            //UInt32_To_BE((uint)(n), bs, off + 4);
            n = (uint)(z[0]);
            x[4] = (byte)(n >> 24);
            x[5] = (byte)(n >> 16);
            x[6] = (byte)(n >> 8);
            x[7] = (byte)(n);
            
            n = (uint)(z[1] >> 32);
            x[8] = (byte)(n >> 24);
            x[9] = (byte)(n >> 16);
            x[10] = (byte)(n >> 8);
            x[11] = (byte)(n);
            //UInt32_To_BE((uint)(n), bs, off + 4);
            n = (uint)(z[1]);
            x[12] = (byte)(n >> 24);
            x[13] = (byte)(n >> 16);
            x[14] = (byte)(n >> 8);
            x[15] = (byte)(n);
        }
    }
}
#pragma warning restore
#endif
