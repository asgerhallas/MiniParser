using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static System.Text.RegularExpressions.Regex;

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

        public static Parsed Regex(this string input, string pattern) => Parsed.From(input).Regex<string>(pattern, out _);
        public static Parsed Regex(this Parsed input, string pattern) => input.Regex<string>(pattern, out _);

        public static Parsed Regex(this string input, string pattern, out string capture1) => Parsed.From(input).RegexInternal(pattern, out capture1, out NoCapture _);
        public static Parsed Regex(this Parsed input, string pattern, out string capture1) => input.RegexInternal(pattern, out capture1, out NoCapture _);

        public static Parsed Regex<T>(this string input, string pattern, out T capture1) => Parsed.From(input).RegexInternal(pattern, out capture1, out NoCapture _);
        public static Parsed Regex<T>(this Parsed input, string pattern, out T capture1) => input.RegexInternal(pattern, out capture1, out NoCapture _);

        public static Parsed Regex(this string input, string pattern, out string capture1, out string capture2) => Parsed.From(input).RegexInternal(pattern, out capture1, out capture2);
        public static Parsed Regex(this Parsed input, string pattern, out string capture1, out string capture2) => input.RegexInternal(pattern, out capture1, out capture2);

        public static Parsed Regex<T1, T2>(this string input, string pattern, out T1 capture1, out T2 capture2) => Parsed.From(input).RegexInternal(pattern, out capture1, out capture2);
        public static Parsed Regex<T1, T2>(this Parsed input, string pattern, out T1 capture1, out T2 capture2) => input.RegexInternal(pattern, out capture1, out capture2);

        static Parsed RegexInternal<T1, T2>(this Parsed input, string pattern, out T1 capture1, out T2 capture2)
        {
            capture1 = default;
            capture2 = default;

            if (!input.Success) return input;

            var explicitEot = pattern.EndsWith("$");

            var match = Match(input.Rest, !explicitEot ? $"{pattern}(.*)$" : pattern, RegexOptions.Compiled);

            if (match.Success)
            {
                capture1 = Convert<T1>(match.Groups[1].Value);
                
                if (typeof(T2) != typeof(NoCapture))
                    capture2 = Convert<T2>(match.Groups[2].Value);

                return new Parsed(true, !explicitEot ? match.Groups[match.Groups.Count-1].Value : "");
            }

            return new Parsed(false, input.Rest);
        }

        public static T Convert<T>(string value) => (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);

        class NoCapture { }
    }
}