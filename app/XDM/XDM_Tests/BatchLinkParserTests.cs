using NUnit.Framework;
using XDM.Core.UI;

namespace XDM.Tests
{
    public class BatchLinkParserTests
    {
        [Test]
        public void ParsesMultipleLinksSeparatedByNewLines()
        {
            var text = "https://example.com/one.zip\r\n\r\nhttps://example.com/two.zip\n";

            var result = BatchLinkParser.TryParse(text, out var links, out var invalidLink);

            Assert.That(result, Is.True);
            Assert.That(invalidLink, Is.Empty);
            Assert.That(links, Has.Count.EqualTo(2));
            Assert.That(links[0].AbsoluteUri, Is.EqualTo("https://example.com/one.zip"));
            Assert.That(links[1].AbsoluteUri, Is.EqualTo("https://example.com/two.zip"));
        }

        [Test]
        public void ParsesLinksSeparatedByWhitespace()
        {
            var text = "https://example.com/one.zip https://example.com/two.zip\thttp://example.com/three.zip";

            var result = BatchLinkParser.TryParse(text, out var links, out _);

            Assert.That(result, Is.True);
            Assert.That(links, Has.Count.EqualTo(3));
        }

        [Test]
        public void EmptyInputReturnsNoLinks()
        {
            var result = BatchLinkParser.TryParse("  \r\n ", out var links, out var invalidLink);

            Assert.That(result, Is.True);
            Assert.That(links, Is.Empty);
            Assert.That(invalidLink, Is.Empty);
        }

        [TestCase("not-a-url")]
        [TestCase("ftp://example.com/file.zip")]
        public void RejectsInvalidOrUnsupportedLinks(string text)
        {
            var result = BatchLinkParser.TryParse(text, out var links, out var invalidLink);

            Assert.That(result, Is.False);
            Assert.That(links, Is.Empty);
            Assert.That(invalidLink, Is.EqualTo(text));
        }
    }
}
