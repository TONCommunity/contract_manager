﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLib
{
    public class DerTaggedObject
        : Asn1TaggedObject
    {
        /**
		 * @param tagNo the tag number for this object.
		 * @param obj the tagged object.
		 */
        public DerTaggedObject(
            int tagNo,
            Asn1Encodable obj)
            : base(tagNo, obj)
        {
        }

        /**
		 * @param explicitly true if an explicitly tagged object.
		 * @param tagNo the tag number for this object.
		 * @param obj the tagged object.
		 */
        public DerTaggedObject(
            bool explicitly,
            int tagNo,
            Asn1Encodable obj)
            : base(explicitly, tagNo, obj)
        {
        }

        /**
		 * create an implicitly tagged object that contains a zero
		 * length sequence.
		 */
        public DerTaggedObject(
            int tagNo)
            : base(false, tagNo, DerSequence.Empty)
        {
        }

        internal override void Encode(
            DerOutputStream derOut)
        {
            if (!IsEmpty())
            {
                byte[] bytes = obj.GetDerEncoded();

                if (explicitly)
                {
                    derOut.WriteEncoded(Asn1Tags.Constructed | Asn1Tags.Tagged, tagNo, bytes);
                }
                else
                {
                    //
                    // need to mark constructed types... (preserve Constructed tag)
                    //
                    int flags = (bytes[0] & Asn1Tags.Constructed) | Asn1Tags.Tagged;
                    derOut.WriteTag(flags, tagNo);
                    derOut.Write(bytes, 1, bytes.Length - 1);
                }
            }
            else
            {
                derOut.WriteEncoded(Asn1Tags.Constructed | Asn1Tags.Tagged, tagNo, new byte[0]);
            }
        }
    }
}