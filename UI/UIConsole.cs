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
            if (args.Length == 0) PrintHelp();
            else
            {
                // read the zzz file
                var path = args[args.Length - 1].Split('\\');
                var fileInPath = path[path.Length - 1];

                // only main and other are compatible
                // TODO: make custom zzz files compatible
                try
                {
                    int baseLength = fileInPath == "main.zzz" ? 828718 : fileInPath == "other.zzz" ? 14496 : throw new FileLoadException();
                    fileFunctions = new FileFunctions(new ZZZFile(args[args.Length - 1], baseLength));
                }
                catch (FileLoadException e)
                {
                    Console.WriteLine("Cannot read file");
                    System.Environment.Exit(0);
                }

                if (args.Contains("-o"))
                {
                    outputPath = args[Array.FindIndex(args, (x => x == "-o")) + 1];
                    // suffix a "\" to outpath path if it doesn't exist
                    if (outputPath[outputPath.Length - 1] != '\\') outputPath += '\\';
                }
                if (args.Contains("-l")) SaveListFiles(fileInPath);
                if (args.Contains("-x")) ExtractFiles();

            }
        }

        static void SaveListFiles(string b)
        {
            File.WriteAllLines(b.Split('.')[0] + ".txt", fileFunctions.file.FileNames);
        }

        static void ExtractFiles()
        {
            fileFunctions.SaveFiles(outputPath);
        }

        static void PrintHelp()
        {
            Console.WriteLine("Usage: FF8R-Tools [options] file \n" +
                "Options: \n" +
                "   -x \t unpack the file \n" +
                "   -p \t pack into zzz file (not implemented) \n" +
                "   -l \t list the files into a textfile \n" +
                "   -o <path> \t output to path");
        }
    }
}
