using System;
using System.Text;

namespace CodeRinseRepeat.Chicken
{
    public class ChickenCodec {
        const string ChickenBase = "chicken";

        static string Encode(char c)
        {
            var @out = new char[10];

            var i = 0;
            for (; i < 7; i++)
                @out[i] = (c & (1 << i)) != 0 ? Char.ToUpper(ChickenBase[i]) : ChickenBase[i];

            // Is the high bit set?
            if ((c & (1 << i)) == 1)
                @out[i] = '.';

            return new string(@out).TrimEnd();
        }

        static char Decode(string @in)
        {
            int i = 0, c = 0;

            for (; i < @in.Length; i++)
                if (Char.IsUpper(@in[i]) || @in[i] == '.')
                    c |= (1 << i);

            return (char) c;
        }

        public static string DeChicken (string body)
        {
            var chickens = body.Split(' ');
            var @out = new StringBuilder();

            foreach (var bawk in chickens) {
                if (bawk.Length > 8)
                    throw new ArgumentException(String.Format("Could not decode {0}.", bawk));
                @out.Append(Decode(bawk));
            }

            return @out.ToString().TrimEnd();
        }

        public static string Chicken (string body)
        {
            var output = new StringBuilder();
            foreach (var c in body)
                output.AppendFormat("{0} ", Encode(c));
            return output.ToString().TrimEnd();
        }
    }
}