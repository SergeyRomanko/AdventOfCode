namespace AdventOfCode.Common
{
    public static class Inputs
    {
        private static readonly FileInfo[] Files;

        static Inputs()
        {
            var directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            Files = directoryInfo.GetFiles("*.txt", SearchOption.AllDirectories);
        }
        
        public static IEnumerable<string> ReadText(string fileName)
        {
            try
            {
                var file = Files.FirstOrDefault(x => x.FullName.Contains(fileName));
                return File.ReadLines(file.FullName);
            }
            catch (Exception e)
            {
                throw new Exception($"File read error {fileName}");
            }
        }
    }
}
