using System.Text;

namespace F23.StringSimilarity.Tests.TestUtil;

internal class EncodingUtil
{
    public static Encoding Latin1 =>
#if NET5_0_OR_GREATER
        Encoding.Latin1;
#else
        Encoding.GetEncoding("ISO-8859-1");
#endif
}
