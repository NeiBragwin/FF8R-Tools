using System;
using System.IO;
using System.Linq;
using PackReader;


namespace UI
{
    public class UIConsole
    {

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please input a file or folder");
                return;
            }

            if (Directory.Exists(args[0]))
            {
                if (File.Exists(args[0] + ".zzz"))
                {
                    Console.WriteLine("The file" + args[0] + ".zzz already exists. Do you want to overwrite it? [y/n]");
                    if (Console.ReadLine().ToLower() == "y") (new PackFile(args[0])).PackFiles();
                    else return;
                }
                else (new PackFile(args[0])).PackFiles();
            }
            else if (File.Exists(args[0]))
            {
                if (Directory.Exists(args[0]))
                {
                    Console.WriteLine("The folder" + args[0] + " already exists. Do you want to overwrite it? [y/n]");
                    if (Console.ReadLine().ToLower() == "y") (new ExtractFile(args[0])).ExtractFiles();
                    else return;
                }
                else (new ExtractFile(args[0])).ExtractFiles();
            }
            else DisplayHelp();
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Usage: ff8r-tools file/folder");
        }
    }
}
