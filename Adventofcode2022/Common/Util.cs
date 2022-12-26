using System;
using System.Text;

namespace Adventofcode2022.Common
{
    public static class Util
    {
        public static void PrintMap(char[,] data)
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
            
            Console.WriteLine(sb.ToString());
        }
    }
}
