﻿using System;

namespace MiniParser
{
    public class Parsed : Tuple<bool, string>
    {
        public Parsed(bool success, string rest) : base(success, rest) { }

        public bool Success => Item1;
        public string Rest => Item2;

        public Parsed Stop<T>(out T result)
        {
            result = default;
            return this;
        }

        public Parsed Stop<T1, T2>(out T1 result1, out T2 result2)
        {
            result1 = default;
            result2 = default;
            return this;
        }

        public void OrThrow()
        {
            if (!Success) throw new ArgumentException($"Could not parse {Rest}.");
        }

        public static Parsed From(string input) => new Parsed(true, input);

        public static implicit operator Parsed(string value) => new Parsed(true, value);
        public static implicit operator bool(Parsed value) => value.Success;
    }
}
