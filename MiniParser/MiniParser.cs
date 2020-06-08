using System;

namespace MiniParser
{
    public static class MiniParser
    {
        public static Parsed After(this string input, string prefix, out string result) => Parsed.From(input).After(prefix, out result);

        public static Parsed After(this Parsed input, string prefix, out string result)
        {
            if (!input.Success) return input.Stop(out result);

            if (input.Rest.StartsWith(prefix))
            {
                result = input.Rest.Remove(0, prefix.Length);
                return new Parsed(true, "");
            }

            result = "";
            return new Parsed(false, input.Rest);
        }

        public static Parsed Until(this string input, string delimiter, out string result) => Parsed.From(input).Until(delimiter, out result);

        public static Parsed Until(this Parsed input, string delimiter, out string result)
        {
            if (!input.Success) return input.Stop(out result);

            var strings = input.Rest.Split(new[] {delimiter}, 2, StringSplitOptions.None);

            if (strings.Length == 1)
            {
                result = "";
                return new Parsed(false, input.Rest);
            }

            result = strings[0];
            return new Parsed(true, strings[1]);
        }

        public static Parsed End(this string input, out string result) => Parsed.From(input).End(out result);

        public static Parsed End(this Parsed input, out string result)
        {
            if (!input.Success) return input.Stop(out result);

            result = input.Rest;
            return new Parsed(true, "");
        }

        static Parsed Stop(Parsed input, out string result)
        {
            result = "";
            return input;
        }
    }
}