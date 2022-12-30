using System.IO;

namespace Adventofcode2022.Common
{
    public sealed class FileDebug
    {
        private readonly string _fileName;

        public FileDebug(string puzzleName)
        {
            _fileName = Path.ChangeExtension($"{puzzleName}Log", ".txt");
        }
        
        public void Log(string text)
        {
            File.AppendAllText(_fileName, text+"\n");
        }
        
        public void LogArray(char[,] data)
        {
            File.AppendAllText(_fileName, Util.ToText(data));
        }
        
        public void Clear()
        {
            File.WriteAllText(_fileName, string.Empty);
        }
    }
}
