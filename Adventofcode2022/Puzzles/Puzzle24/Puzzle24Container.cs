using System;
using System.Collections.Generic;
using Adventofcode2022.Common;

namespace Adventofcode2022.Puzzles
{
    public sealed class Puzzle24Container
    {
        private struct Blizzard
        {
            public Vec2 Pos;
            public Vec2 Dir;
        }

        private List<Blizzard> _data = new List<Blizzard>();
        private HashSet<Vec2> _hash = new HashSet<Vec2>(1000);

        private int _sizeX;
        private int _sizeY;

        private Vec2 _start;
        private Vec2 _finish;

        public Vec2 Start => _start;
        public Vec2 Finish => _finish;

        public Puzzle24Container(IReadOnlyList<string> input)
        {
            _sizeX = input[0].Length;
            _sizeY = input.Count;
            
            _start = new Vec2(1, 0);
            _finish = new Vec2(_sizeX - 2, _sizeY - 1);
            
            for (var y = 0; y < _sizeY; y++)
            {
                for (var x = 0; x < _sizeX; x++)
                {
                    var data = input[y][x];

                    if (data is '.' or '#')
                    {
                        continue;
                    }
                    
                    var dir = data switch
                    {
                        '^' => new Vec2(0, -1),
                        '>' => new Vec2(+1, 0),
                        'v' => new Vec2(0, +1),
                        '<' => new Vec2(-1, 0)
                    };
                    
                    _data.Add(new Blizzard
                    {
                        Pos = new Vec2(x, y),
                        Dir = dir
                    });
                }
            }

            UpdateHash();
        }

        private Puzzle24Container()
        {
            
        }

        public Puzzle24Container NextStep()
        {
            var result = new Puzzle24Container
            {
                _sizeX = _sizeX,
                _sizeY = _sizeY,
                _start = _start,
                _finish = _finish
            };

            for (var i = 0; i < _data.Count; i++)
            {
                var blizzard = _data[i];

                blizzard.Pos += blizzard.Dir;

                if (blizzard.Pos.x == 0)
                    blizzard.Pos = new Vec2(_sizeX - 2, blizzard.Pos.y);
                
                if (blizzard.Pos.y == 0)
                    blizzard.Pos = new Vec2(blizzard.Pos.x, _sizeY - 2);
                
                if (blizzard.Pos.x == _sizeX - 1)
                    blizzard.Pos = new Vec2(1, blizzard.Pos.y);
                
                if (blizzard.Pos.y == _sizeY - 1)
                    blizzard.Pos = new Vec2(blizzard.Pos.x, 1);

                result._data.Add(blizzard);
            }

            result.UpdateHash();
            
            return result;
        }

        private void UpdateHash()
        {
            _hash.Clear();

            foreach (var blizzard in _data)
            {
                _hash.Add(blizzard.Pos);
            }
        }

        public void Print(Vec2 me)
        {
            var data = new char[_sizeX, _sizeY];
            Util.FillMap(data, '.');

            foreach (var blizzard in _data)
            {
                data[blizzard.Pos.x, blizzard.Pos.y] = '@';
            }
            
            data[_start.x, _start.y] = '%';
            data[_finish.x, _finish.y] = '%';
            
            data[me.x, me.y] = 'X';
            
            Util.PrintMap(data);
        }

        public bool IsValid(Vec2 data)
        {
            Console.WriteLine();
            
            if (data == _start)
            {
                return true;
            }
            
            if (data == _finish)
            {
                return true;
            }
            
            if (data.x <= 0 || data.x >= _sizeX)
            {
                return false;
            }
            
            if (data.y <= 0 || data.y >= _sizeY)
            {
                return false;
            }
            
            return !_hash.Contains(data);
        }
    }
}
