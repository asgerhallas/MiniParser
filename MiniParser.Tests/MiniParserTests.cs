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
            result.ShouldBe(default);
        }

        [Fact]
        public void ParsePrefix_NotFound()
        {
            var parsed = "not.Blah".After("fixed.", out var result);

            parsed.ShouldBe(new Parsed(false, "not.Blah"));
            result.ShouldBe(default);
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
            result.ShouldBe(default);
        }

        [Fact]
        public void ParseUntil_Multiple()
        {
            var parsed = "a/b/c".Until("/", out var result);

            parsed.ShouldBe(new Parsed(true, "b/c"));
            result.ShouldBe("a");
        }

        [Fact]
        public void ParseUntil_NotFound()
        {
            var parsed = "a-b".Until("/", out var result);

            parsed.ShouldBe(new Parsed(false, "a-b"));
            result.ShouldBe(default);
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
            first.ShouldBe(default);
        }

        [Fact]
        public void ParseUntilLast()
        {
            var parsed = "a,b,c".UntilLast(",", out var result);

            result.ShouldBe("a,b");
            parsed.ShouldBe(new Parsed(true, ",c"));
        }

        [Fact]
        public void ParseUntilLast_Stops()
        {
            var parsed = new Parsed(false, "a,b,c").UntilLast(",", out var result);

            result.ShouldBe(default);
            parsed.ShouldBe(new Parsed(false, "a,b,c"));
        }

        [Fact]
        public void ParseUntilLast_NotFound()
        {
            var parsed = "a-b-c".UntilLast(",", out var result);

            result.ShouldBe(default);
            parsed.ShouldBe(new Parsed(false, "a-b-c"));
        }

        [Fact]
        public void ParseRegex()
        {
            var parsed = "asger".Regex("as");

            parsed.ShouldBe(new Parsed(true, "ger"));
        }

        [Fact]
        public void ParseRegex_NoMatch()
        {
            var parsed = "asger".Regex("kurt");

            parsed.ShouldBe(new Parsed(false, "asger"));
        }

        [Fact]
        public void ParseRegex_FromMiddle()
        {
            var parsed = "asger".Regex("sg");

            parsed.ShouldBe(new Parsed(true, "er"));
        }

        [Fact]
        public void ParseRegex_ExplicitBeginning()
        {
            var parsed = "asger".Regex("^sg");

            parsed.ShouldBe(new Parsed(false, "asger"));
        }

        [Fact]
        public void ParseRegex_MultipleMatches()
        {
            var parsed = "rokoko".Regex("ok");

            parsed.ShouldBe(new Parsed(true, "oko"));
        }

        [Fact]
        public void ParseRegex_UntilEnd()
        {
            var parsed = "asger".Regex(".*");

            parsed.ShouldBe(new Parsed(true, ""));
        }

        [Fact]
        public void ParseRegex_UntilEnd_Explicit()
        {
            var parsed = "asger".Regex(".*$");

            parsed.ShouldBe(new Parsed(true, ""));
        }

        [Fact]
        public void ParseRegex_Capture()
        {
            var parsed = "asger".Regex("a(.*)e", out var capture1);

            capture1.ShouldBe("sg");
            parsed.ShouldBe(new Parsed(true, "r"));
        }

        [Fact]
        public void ParseRegex_Capture_Convert()
        {
            var parsed = "a100r".Regex("a(.*)r", out int capture1);

            capture1.ShouldBe(100);
            parsed.ShouldBe(new Parsed(true, ""));
        }

        [Fact]
        public void ParseRegex_Capture2()
        {
            var parsed = "asger".Regex("a(.*)(.)r", out string capture1, out string capture2);

            capture1.ShouldBe("sg");
            capture2.ShouldBe("e");
            parsed.ShouldBe(new Parsed(true, ""));
        }

        [Fact]
        public void ParseRegex_Capture2_Convert()
        {
            var parsed = "a100r03C7F07D-BF1D-4E49-BFBE-5933AD081CFD".Regex("a(.*)r(.*)", out int capture1, out Guid capture2);

            capture1.ShouldBe(100);
            capture2.ShouldBe(Guid.Parse("03C7F07D-BF1D-4E49-BFBE-5933AD081CFD"));
            parsed.ShouldBe(new Parsed(true, ""));
        }

        [Fact(Skip = "Do we actually want to support this? You can just parentherize what you need...")]
        public void ParseRegex_Capture_ImplicitAll()
        {
            var parsed = "asger".Regex("a.*e", out var capture1);

            capture1.ShouldBe("asge");
            parsed.ShouldBe(new Parsed(true, "r"));
        }
    }
}
