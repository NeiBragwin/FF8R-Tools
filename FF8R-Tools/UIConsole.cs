using System;
using System.IO;
using System.Linq;
using PackReader;


namespace UI
{
    public class UIConsole
    {
        private static FileFunctions fileFunctions;
        private static string outputPath;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please input a file");
                return;
            }

            (new FileFunctions(args[0])).ExtractFiles();
        }
    }
}
