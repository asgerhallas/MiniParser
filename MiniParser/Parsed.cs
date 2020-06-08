using System;

namespace MiniParser
{
    public class Parsed : Tuple<bool, string>
    {
        public Parsed(bool success, string rest) : base(success, rest) { }

        public bool Success => Item1;
        public string Rest => Item2;

        public Parsed Stop(out string result)
        {
            result = "";
            return this;
        }

        public static Parsed From(string input) => new Parsed(true, input);

        public static implicit operator Parsed(string value) => new Parsed(true, value);
        public static implicit operator bool(Parsed value) => value.Success;
    }
}
