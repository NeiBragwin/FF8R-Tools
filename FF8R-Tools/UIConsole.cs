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
                Console.WriteLine("Please input a file");
                return;
            }

            //(new FileFunctions(args[0])).ExtractFiles();
            (new PackFile(args[0])).PackFiles();
        }
    }
}
