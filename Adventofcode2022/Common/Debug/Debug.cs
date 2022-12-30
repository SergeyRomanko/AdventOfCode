using System;

namespace Adventofcode2022.Common
{
    public sealed class Debug
    {
        public FileDebug File { get; }
        
        public Debug(string puzzleName)
        {
            File = new FileDebug(puzzleName);
        }
        
        public void Log(string text)
        {
            Console.WriteLine(text);
        }
    }
}
