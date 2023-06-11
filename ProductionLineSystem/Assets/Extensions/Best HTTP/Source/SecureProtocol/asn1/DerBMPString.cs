#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
    /**
     * Der BMPString object.
     */
    public class DerBmpString
		: DerStringBase
    {
        private readonly string str;

		/**
         * return a BMP string from the given object.
         *
         * @param obj the object we want converted.
         * @exception ArgumentException if the object cannot be converted.
         */
        public static DerBmpString GetInstance(
            object obj)
        {
            if (obj == null || obj is DerBmpString)
            {
                return (DerBmpString)obj;
            }

            throw new ArgumentException("illegal object in GetInstance: " + BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Platform.GetTypeName(obj));
        }

        /**
         * return a BMP string from a tagged object.
         *
         * @param obj the tagged object holding the object we want
         * @param explicitly true if the object is meant to be explicitly
         *              tagged false otherwise.
         * @exception ArgumentException if the tagged object cannot
         *              be converted.
         */
        public static DerBmpString GetInstance(
            Asn1TaggedObject	obj,
            bool				isExplicit)
        {
			Asn1Object o = obj.GetObject();

			if (isExplicit || o is DerBmpString)
			{
				return GetInstance(o);
			}

			return new DerBmpString(Asn1OctetString.GetInstance(o).GetOctets());
        }

		/**
         * basic constructor - byte encoded string.
         */

        public DerBmpString(byte[] str)
        {
			if (str == null)
				throw new ArgumentNullException("str");

            int byteLen = str.Length;
            if (0 != (byteLen & 1))
                throw new ArgumentException("malformed BMPString encoding encountered", "str");

            int charLen = byteLen / 2;
            char[] cs = new char[charLen];

            for (int i = 0; i != charLen; i++)
            {
                cs[i] = (char)((str[2 * i] << 8) | (str[2 * i + 1] & 0xff));
            }

            this.str = new string(cs);
        }

        internal DerBmpString(char[] str)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            this.str = new string(str);
        }

        /**
         * basic constructor
         */
        public DerBmpString(string str)
        {
			if (str == null)
				throw new ArgumentNullException("str");

            this.str = str;
        }

        public override string GetString()
        {
            return str;
        }

		protected override bool Asn1Equals(
			Asn1Object asn1Object)
        {
			DerBmpString other = asn1Object as DerBmpString;

			if (other == null)
				return false;

			return this.str.Equals(other.str);
        }

        internal override int EncodedLength(bool withID)
        {
            return Asn1OutputStream.GetLengthOfEncodingDL(withID, str.Length * 2);
        }

		internal override void Encode(Asn1OutputStream asn1Out, bool withID)
        {
            char[] c = str.ToCharArray();
            byte[] b = new byte[c.Length * 2];

			for (int i = 0; i != c.Length; i++)
            {
                b[2 * i] = (byte)(c[i] >> 8);
                b[2 * i + 1] = (byte)c[i];
            }

            asn1Out.WriteEncodingDL(withID, Asn1Tags.BmpString, b);
        }
    }
}
#pragma warning restore
#endif
