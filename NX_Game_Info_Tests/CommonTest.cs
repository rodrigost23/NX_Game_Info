using System;
using Xunit;
using NX_Game_Info;
using System.Globalization;

namespace NX_Game_Info_Tests
{
    public class CommonTest
    {
        [Theory]
        [InlineData(0, "0 B")]
        [InlineData(512, "512 B")]
        [InlineData(1024, "1 KB")]
        [InlineData(1536, "1.5 KB")]
        [InlineData(1048576, "1 MB")]
        [InlineData(1384120, "1.32 MB")]
        [InlineData(1572864, "1.5 MB")]
        [InlineData(1073741824, "1 GB")]
        [InlineData(1610612736, "1.5 GB")]
        [InlineData(1099511627776, "1 TB")]
        [InlineData(1649267441664, "1.5 TB")]
        [InlineData(1125899906842624, "1 PB")]
        [InlineData(1486187877032264, "1.32 PB")]
        [InlineData(1688849860263936, "1.5 PB")]
        [InlineData(1152921504606846976, "1 EB")]
        public void GetBytesReadable_ShouldReturnCorrectReadableString(long bytes, string expected)
        {
            // Force decimal separator to be a dot
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            string result = Common.GetBytesReadable(bytes);

            Assert.Equal(expected, result);
        }
    }
}