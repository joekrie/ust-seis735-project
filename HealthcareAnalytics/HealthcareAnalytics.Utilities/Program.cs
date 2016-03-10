using System.IO;
using Fclp;

namespace HealthcareAnalytics.Utilities
{
    public class Program
    {
        private static readonly string DataPath;

        static Program()
        {
            var currentDirectoryPath = Directory.GetCurrentDirectory();
            var currentDirectory = new DirectoryInfo(currentDirectoryPath);
            var projectPath = currentDirectory.Parent.Parent.Parent.Parent.FullName;

            DataPath = Path.Combine(projectPath, "data");
        }
        
        public static void Main(string[] args)
        {
            var argParser = GetArgParser();
        }

        private static FluentCommandLineParser<Args> GetArgParser()
        {
            var argParser = new FluentCommandLineParser<Args>();
            return argParser;
        }

        private class Args
        {
            
        }
    }
}
