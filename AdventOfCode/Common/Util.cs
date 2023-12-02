using System.Text;

namespace AdventOfCode.Common
{
    public static class Util
    {
        public static string ToText(char[,] data)
        {
            var sb = new StringBuilder();

            for (var y = 0; y < data.GetLength(1); y++)
            {
                for (var x = 0; x < data.GetLength(0); x++)
                {
                    sb.Append(data[x, y]);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static void FillMap(char[,] data, char input)
        {
            for (var y = 0; y < data.GetLength(1); y++)
            {
                for (var x = 0; x < data.GetLength(0); x++)
                {
                    data[x, y] = input;
                }
            }
        }
    }
}
