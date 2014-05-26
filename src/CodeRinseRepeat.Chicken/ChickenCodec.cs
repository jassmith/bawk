using System;
using System.Text;

namespace CodeRinseRepeat.Chicken
{
    public class ChickenCodec
    {
        static readonly Char [] FeedStock = { 'C', 'H', 'I', 'C', 'K', 'E', 'N', '.' };
        public static Lazy<byte []> ChickenFeed = new Lazy<byte []> (SpreadFeed);

        static unsafe void Encode (byte* input, int length, byte* output)
        {
            if (length == 0)
                return;

            var inend = input + length;
            var outptr = output;
            var inptr = input;
            var chickens = ChickenFeed.Value;
            fixed (byte* coop = chickens) {
                while (inptr < inend) {
                    var c = *inptr++;
                    var chick = coop + ( c * 8 );

                    for (int i = 0; i < 7; i++) {
                        *outptr++ = *( chick + i );
                    }

                    if (*( chick + 7 ) != 0)
                        *outptr++ = *( chick + 7 );

                    *outptr++ = (byte)' ';
                }
            }
        }

        static byte [] SpreadFeed ()
        {
            var feed = new byte [256 * 8]; // Wouldn't want bounds checking to slow down our encoder
            unsafe {
                fixed (byte* feedptr = feed) {
                    for (int i = 0; i < 256; i++) {
                        SeedFeed (feedptr + i * 8, (byte)i);
                    }
                }
            }
            return feed;
        }

        static unsafe void SeedFeed (byte* feed, byte seed)
        {
            for (byte i = 0; i < 8; i++) {
                var mask = (byte)( 1 << i );
                *feed++ = (byte)( FeedStock [i] + ( ( ( seed & mask ) > 0 ) ? 0 : 0x20 ) );
            }

            if (*( feed - 1 ) != (byte)'.')
                *( feed - 1 ) = 0;
        }

        static unsafe void Decode (byte* input, int length, byte* output)
        {
            var position = 0;
            byte saved = 0;
            var inend = input + length;
            var outptr = output;
            var inptr = input;

            while (inptr < inend) {
                var c = *inptr++;

                if (c == ' ' || position >= 8) {
                    // Only consume the chicken if it is reasonably well formed
                    if (position > 6)
                        *outptr++ = saved;

                    position = 0;
                    saved = 0;
                } else {
                    saved |= (byte)( ( ( c >= 'A' ) && ( c <= 'Z' ) || c == '.' ) ? ( 1 << position ) : 0 );
                    position++;
                }
            }

            // Consume the last chicken if it was reasonably well-formed.
            if (position > 6)
// ReSharper disable once RedundantAssignment because this is a pointer, silly R# -- it's like a ref byte[]
// for big kids.
                *outptr++ = saved;
        }

        public static unsafe string DeChicken (string body)
        {
            var @in = Encoding.UTF8.GetBytes (body);
            var @out = new byte [(int)Math.Ceiling ((double)body.Length / 8)];
            fixed (byte* @inptr = @in)
            fixed (byte* @outptr = @out)
                Decode (@inptr, @in.Length, @outptr);
            return Encoding.UTF8.GetString (@out).TrimEnd ('\u0000').TrimEnd ();
        }

        public static unsafe string Chicken (string body)
        {
            var @in = Encoding.UTF8.GetBytes (body);
            var @out = new byte [body.Length * 9];
            fixed (byte* @inptr = @in)
            fixed (byte* @outptr = @out)
                Encode (@inptr, @in.Length, @outptr);
            return Encoding.UTF8.GetString (@out).TrimEnd ('\u0000').TrimEnd ();
        }
    }
}