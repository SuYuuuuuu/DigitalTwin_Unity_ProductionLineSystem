#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	public class Asn1StreamParser
	{
		private readonly Stream _in;
		private readonly int _limit;

        private readonly byte[][] tmpBuffers;

        public Asn1StreamParser(Stream input)
			: this(input, Asn1InputStream.FindLimit(input))
		{
		}

        public Asn1StreamParser(byte[] encoding)
            : this(new MemoryStream(encoding, false), encoding.Length)
        {
        }

        public Asn1StreamParser(Stream input, int limit)
            : this(input, limit, new byte[16][])
		{
        }

        internal Asn1StreamParser(Stream input, int limit, byte[][] tmpBuffers)
        {
            if (!input.CanRead)
                throw new ArgumentException("Expected stream to be readable", "input");

            this._in = input;
            this._limit = limit;
            this.tmpBuffers = tmpBuffers;
        }

        internal IAsn1Convertible ReadIndef(int tagValue)
		{
			// Note: INDEF => CONSTRUCTED

			// TODO There are other tags that may be constructed (e.g. BIT_STRING)
			switch (tagValue)
			{
				case Asn1Tags.External:
					return new DerExternalParser(this);
				case Asn1Tags.OctetString:
					return new BerOctetStringParser(this);
				case Asn1Tags.Sequence:
					return new BerSequenceParser(this);
				case Asn1Tags.Set:
					return new BerSetParser(this);
				default:
					throw new Asn1Exception("unknown BER object encountered: 0x" + tagValue.ToString("X"));
			}
		}

		internal IAsn1Convertible ReadImplicit(bool constructed, int tag)
		{
			if (_in is IndefiniteLengthInputStream)
			{
				if (!constructed)
					throw new IOException("indefinite-length primitive encoding encountered");

				return ReadIndef(tag);
			}

			if (constructed)
			{
				switch (tag)
				{
					case Asn1Tags.Set:
						return new DerSetParser(this);
					case Asn1Tags.Sequence:
						return new DerSequenceParser(this);
					case Asn1Tags.OctetString:
						return new BerOctetStringParser(this);
				}
			}
			else
			{
				switch (tag)
				{
					case Asn1Tags.Set:
						throw new Asn1Exception("sequences must use constructed encoding (see X.690 8.9.1/8.10.1)");
					case Asn1Tags.Sequence:
						throw new Asn1Exception("sets must use constructed encoding (see X.690 8.11.1/8.12.1)");
					case Asn1Tags.OctetString:
						return new DerOctetStringParser((DefiniteLengthInputStream)_in);
				}
			}

			throw new Asn1Exception("implicit tagging not implemented");
		}

		internal Asn1Object ReadTaggedObject(bool constructed, int tag)
		{
			if (!constructed)
			{
				// Note: !CONSTRUCTED => IMPLICIT
				DefiniteLengthInputStream defIn = (DefiniteLengthInputStream)_in;
				return new DerTaggedObject(false, tag, new DerOctetString(defIn.ToArray()));
			}

			Asn1EncodableVector v = ReadVector();

			if (_in is IndefiniteLengthInputStream)
			{
				return v.Count == 1
					?   new BerTaggedObject(true, tag, v[0])
					:   new BerTaggedObject(false, tag, BerSequence.FromVector(v));
			}

			return v.Count == 1
				?   new DerTaggedObject(true, tag, v[0])
				:   new DerTaggedObject(false, tag, DerSequence.FromVector(v));
		}

		public virtual IAsn1Convertible ReadObject()
		{
			int tag = _in.ReadByte();
			if (tag == -1)
				return null;

			// turn of looking for "00" while we resolve the tag
			Set00Check(false);

			//
			// calculate tag number
			//
			int tagNo = Asn1InputStream.ReadTagNumber(_in, tag);

			bool isConstructed = (tag & Asn1Tags.Constructed) != 0;

			//
			// calculate length
			//
			int length = Asn1InputStream.ReadLength(_in, _limit,
                tagNo == Asn1Tags.OctetString || tagNo == Asn1Tags.Sequence || tagNo == Asn1Tags.Set || tagNo == Asn1Tags.External);

			if (length < 0) // indefinite-length method
			{
				if (!isConstructed)
					throw new IOException("indefinite-length primitive encoding encountered");

                IndefiniteLengthInputStream indIn = new IndefiniteLengthInputStream(_in, _limit);
                Asn1StreamParser sp = new Asn1StreamParser(indIn, _limit, tmpBuffers);

                int tagClass = tag & Asn1Tags.Private;
                if (0 != tagClass)
                {
                    if ((tag & Asn1Tags.Application) != 0)
                        return new BerApplicationSpecificParser(tagNo, sp);

                    return new BerTaggedObjectParser(true, tagNo, sp);
                }

                return sp.ReadIndef(tagNo);
			}
			else
			{
				DefiniteLengthInputStream defIn = new DefiniteLengthInputStream(_in, length, _limit);

                int tagClass = tag & Asn1Tags.Private;
                if (0 != tagClass)
                {
                    if ((tag & Asn1Tags.Application) != 0)
                        return new DerApplicationSpecific(isConstructed, tagNo, defIn.ToArray());

                    return new BerTaggedObjectParser(isConstructed, tagNo,
                        new Asn1StreamParser(defIn, defIn.Remaining, tmpBuffers));
                }

                if (!isConstructed)
                {
                    // Some primitive encodings can be handled by parsers too...
                    switch (tagNo)
                    {
                        case Asn1Tags.OctetString:
                            return new DerOctetStringParser(defIn);
                    }

                    try
                    {
                        return Asn1InputStream.CreatePrimitiveDerObject(tagNo, defIn, tmpBuffers);
                    }
                    catch (ArgumentException e)
                    {
                        throw new Asn1Exception("corrupted stream detected", e);
                    }
                }

                Asn1StreamParser sp = new Asn1StreamParser(defIn, defIn.Remaining, tmpBuffers);

                // TODO There are other tags that may be constructed (e.g. BitString)
                switch (tagNo)
				{
					case Asn1Tags.OctetString:
						//
						// yes, people actually do this...
						//
						return new BerOctetStringParser(sp);
					case Asn1Tags.Sequence:
						return new DerSequenceParser(sp);
					case Asn1Tags.Set:
						return new DerSetParser(sp);
					case Asn1Tags.External:
						return new DerExternalParser(sp);
					default:
                        throw new IOException("unknown tag " + tagNo + " encountered");
                }
			}
		}

		private void Set00Check(
			bool enabled)
		{
			if (_in is IndefiniteLengthInputStream)
			{
				((IndefiniteLengthInputStream) _in).SetEofOn00(enabled);
			}
		}

        internal Asn1EncodableVector ReadVector()
        {
            IAsn1Convertible obj = ReadObject();
            if (null == obj)
                return new Asn1EncodableVector(0);

            Asn1EncodableVector v = new Asn1EncodableVector();
            do
            {
                v.Add(obj.ToAsn1Object());
            }
            while ((obj = ReadObject()) != null);
            return v;
        }
	}
}
#pragma warning restore
#endif
