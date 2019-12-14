using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace PackReader
{
    public class ExtractFile
    {
        public readonly string file;
        private readonly BinaryReader bin;

        public ExtractFile(string file)
        {
            this.file = file;
            // bin = new BinaryReader(File.OpenRead(file));
        }

        public void ExtractFiles()
        {
            // First int in file indicates the number of files
            int numberOfFiles = bin.ReadInt32();
            // Key => offset to file start
            // Value => filesize
            string[] filenames = new string[numberOfFiles];
            int[] pointers = new int[numberOfFiles];
            int[] filesizes = new int[numberOfFiles];

            // Loop through the header and get information about the files in the archive
            for (int i = 0; i < numberOfFiles; i++)
            {
                // Get the number of characters in the file name/path
                int numChar = bin.ReadInt32();
                // Read the file path
                filenames[i] = Encoding.UTF8.GetString(bin.ReadBytes(numChar));
                // 4 bytes after the filename is the pointer to the file contents
                pointers[i] = bin.ReadInt32();
                // 4 bytes after the pointer seems to be nothing, so skip it
                bin.ReadInt32();
                // 4 bytes after nothing is the filesize
                filesizes[i] = bin.ReadInt32();
            }

            // Save the files
            for (int i = 0; i < numberOfFiles; i++)
            {
                Console.Write(filenames[i] + "\t");
                string directory = file.Split('.')[0] + "/";
                // Create the directories and sub-directories based on the filepath
                new FileInfo(directory + "\\" + filenames[i]).Directory.Create();
                using (BinaryWriter writer = new BinaryWriter(File.Open(directory + "\\" + filenames[i], FileMode.Create, FileAccess.Write)))
                {
                    writer.Write(bin.ReadBytes(filesizes[i]));
                }
                Console.Write(filesizes[i] + " bytes saved" + "\n");
            }
        }

        
    }
}
