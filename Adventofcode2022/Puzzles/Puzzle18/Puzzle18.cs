using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public class Puzzle18 : Puzzle
    {
        private struct Vec3
        {
            public static readonly Vec3 Up    = new Vec3(0,+1,0);
            public static readonly Vec3 Down  = new Vec3(0,-1,0);
            public static readonly Vec3 Left  = new Vec3(-1,0,0);
            public static readonly Vec3 Right = new Vec3(+1,0,0);
            public static readonly Vec3 Front = new Vec3(0,0,+1);
            public static readonly Vec3 Back  = new Vec3(0,0,-1);

            public static readonly Vec3[] All = { Up, Down, Left, Right, Front, Back };
            
            public int X;
            public int Y;
            public int Z;

            public Vec3(int x, int y, int z)
            {
                (X,Y,Z) = (x,y,z);
            }
        }
        
        public override string[] GetResults(IReadOnlyList<string> input)
        {
            var data = input
                    .Select(x => x.Split(','))
                    .Select(x => new Vec3(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])))
                    .ToList();

            var min = data
                .Aggregate((a, b) => new Vec3(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z)));
            
            var max = data
                .Aggregate((a, b) => new Vec3(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z)));

            var model = new char[max.X+1, max.Y+1, max.Z+1];
            
            for (var z = 0; z < model.GetLength(2); z++)
            {
                for (var y = 0; y < model.GetLength(1); y++)
                {
                    for (var x = 0; x < model.GetLength(0); x++)
                    {
                        model[x, y, z] = '.';
                    }
                }
            }
            
            foreach (var vec3 in data)
            {
                model[vec3.X, vec3.Y, vec3.Z] = '#';
            }

            Print(model);
            
            return new[]
            {
                DoTask1(model).ToString(),
                ""
            };
        }

        private int DoTask1(char[,,] model)
        {
            var result = 0;
            
            for (var z = 0; z < model.GetLength(2); z++)
            {
                for (var y = 0; y < model.GetLength(1); y++)
                {
                    for (var x = 0; x < model.GetLength(0); x++)
                    {
                        result += TestPosition(model, x, y, z);
                    }
                }
            }

            return result;
        }

        private int TestPosition(char[,,] model, int x, int y, int z)
        {
            if (model[x,y,z] != '#')
            {
                return 0;
            }

            var lengthX = model.GetLength(0);
            var lengthY = model.GetLength(1);
            var lengthZ = model.GetLength(2);

            var result = 0;
            
            foreach (var dir in Vec3.All)
            {
                var pos = new Vec3(dir.X + x, dir.Y + y, dir.Z + z);

                if (pos.X < 0 || pos.Y < 0 || pos.Z < 0)
                {
                    result++;
                    continue;
                }
                
                if (pos.X >= lengthX || pos.Y >= lengthY || pos.Z >= lengthZ)
                {
                    result++;
                    continue;
                }

                if (model[pos.X, pos.Y, pos.Z] != '#')
                {
                    result++;
                    continue;
                }
            }

            return result;
        }

        private void Print(char[,,] model)
        {
            var builder = new StringBuilder();
            
            for (var z = 0; z < model.GetLength(2); z++)
            {
                for (var y = 0; y < model.GetLength(1); y++)
                {
                    for (var x = 0; x < model.GetLength(0); x++)
                    {
                        builder.Append(model[x, y, z]);
                    }
                    builder.AppendLine();
                }

                builder.AppendLine();
            }
            
            File.WriteAllText("DSFGDFG.txt", builder.ToString());
        }
    }
}
