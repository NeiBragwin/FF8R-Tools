using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackReader
{
    public class PackFile
    {
        private string path;
        private List<string> filenames;
        private List<byte[]> contents;

        public PackFile(string path)
        {
            this.path = path;
            this.filenames = new List<string>();
            this.contents = new List<byte[]>();
        }

        public void PackFiles()
        {
            GetFilePaths(path);
            using (BinaryWriter writer = new BinaryWriter(File.Open(path + ".zzz", FileMode.OpenOrCreate)))
            {
                // number of files
                writer.Write(filenames.Count);
                // Calculate length of the header before writing the file contents
                int headerLength = 4;
                foreach (string filename in filenames)
                {
                    string file = filename.Substring(path.Length + 1);
                    headerLength += file.Length + 16;
                }

                foreach (string filename in filenames)
                {
                    byte[] fileContents = File.ReadAllBytes(filename);
                    contents.Add(fileContents);

                    string file = filename.Substring(path.Length + 1);
                    // char count
                    writer.Write(file.Length);
                    // filename
                    writer.Write(file.ToCharArray());
                    // pointer
                    writer.Write(headerLength);
                    // extra pad
                    writer.Write(0);
                    // filesize
                    writer.Write(fileContents.Length);
                    // update the pointer for the next file
                    headerLength += fileContents.Length;
                }

                // Write all the contents
                foreach (byte[] content in contents)
                    writer.Write(content);
            }
        }

        private void GetFilePaths(string dir)
        {
            foreach (string file in Directory.GetFiles(dir))
                filenames.Add(file);

            foreach (string d in Directory.GetDirectories(dir))
                GetFilePaths(d);
        }
    }
}
