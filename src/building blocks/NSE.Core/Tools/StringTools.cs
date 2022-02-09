using System.Linq;

namespace NSE.Core.Tools
{
    public static class StringTools
    {
        public static string NumberOnly(this string str, string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }
    }
}
