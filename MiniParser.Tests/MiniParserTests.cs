using System;
using Shouldly;
using Xunit;

namespace MiniParser.Tests
{
    public class MiniParserTests
    {
        [Fact]
        public void ParsePrefix()
        {
            var parsed = "fixed.Blah".After("fixed.", out var result);

            parsed.ShouldBe(new Parsed(true, ""));
            result.ShouldBe("Blah");
        }

        [Fact]
        public void ParsePrefix_Stops()
        {
            var parsed = new Parsed(false, "fixed.Blah").After("fixed.", out var result);

            parsed.ShouldBe(new Parsed(false, "fixed.Blah"));
            result.ShouldBe("");
        }

        [Fact]
        public void ParsePrefix_NotFound()
        {
            var parsed = "not.Blah".After("fixed.", out var result);

            parsed.ShouldBe(new Parsed(false, "not.Blah"));
            result.ShouldBe("");
        }

        [Fact]
        public void ParseUntil()
        {
            var parsed = "a/b".Until("/", out var result);

            parsed.ShouldBe(new Parsed(true, "b"));
            result.ShouldBe("a");
        }

        [Fact]
        public void ParseUntil_Stops()
        {
            var parsed = new Parsed(false, "a/b").Until("/", out var result);

            parsed.ShouldBe(new Parsed(false, "a/b"));
            result.ShouldBe("");
        }

        [Fact]
        public void ParseUntil_NotFound()
        {
            var parsed = "a-b".Until("/", out var result);

            parsed.ShouldBe(new Parsed(false, "a-b"));
            result.ShouldBe("");
        }

        [Fact]
        public void ParsePrefix_ThenUntil()
        {
            var parsed = "a/hest-test".Until("/", out var first).After("hest-", out var last);

            parsed.ShouldBe(new Parsed(true, ""));
            first.ShouldBe("a");
            last.ShouldBe("test");
        }

        [Fact]
        public void ParseEnd()
        {
            var parsed = "asger".End(out var first);

            parsed.ShouldBe(new Parsed(true, ""));
            first.ShouldBe("asger");
        }

        [Fact]
        public void ParseEnd_Stops()
        {
            var parsed = new Parsed(false, "asger").End(out var first);

            parsed.ShouldBe(new Parsed(false, "asger"));
            first.ShouldBe("");
        }
    }

}
