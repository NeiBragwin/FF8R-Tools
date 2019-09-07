using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace PackReader
{
    public class FileFunctions
    {
        public readonly ZZZFile file;
        private BinaryReader bin;
        private byte[] header;

        public FileFunctions(ZZZFile file)
        {
            this.file = file;
            bin = new BinaryReader(File.OpenRead(file.Path));
            header = ReadBaseSection();
            ParseHeader();
        }

        public int GetNumberOfFiles()
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(file.Path)))
            {
                var num = BitConverter.ToInt32(br.ReadBytes(8), 0);
                file.NumberOfFiles = num;
                return num;
            }
        }

        public byte[] ReadBaseSection()
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(file.Path)))
            {
                return br.ReadBytes(file.BaseLength).ToArray();
            }
        }

        public byte[] ReverseBaseBytes()
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(file.Path)))
            {
                return ReadBaseSection().Reverse().ToArray();
            }
        }

        public byte[] ReverseBytes(byte[] bytes)
        {
            return bytes.Reverse().ToArray();
        }

        public void PrintFile(int i)
        {
            // Console.WriteLine(file.FileNames[i] + '\t' + file.Sizes[i] + "bytes");
            Console.WriteLine(file.FileNames[i]);
        }

        public void SaveFiles(string prefix)
        {
            // skip to the offset where the contents section starts
            bin.ReadBytes(file.BaseLength);
            for (int a = 0; a < file.FileNames.Count; a++)
            {
                // create the directories from the path of the current filename
                var dir = file.FileNames[a].Split('\\');
                var path = prefix;
                for (int i = 0; i < dir.Length; i++)
                {
                    if (i < dir.Length - 1)
                    {
                        Directory.CreateDirectory(path + dir[i]);
                        path += dir[i] + "\\";
                    }
                }
                // save the current file
                using (FileStream fs = new FileStream(path + dir[dir.Length - 1], FileMode.Create))
                {
                    byte[] fileContent = bin.ReadBytes(file.Sizes[a]);
                    fs.Write(fileContent, 0, file.Sizes[a]);
                    PrintFile(a);
                }
            }
        }

        private void ParseHeader()
        {
            

            int index = header.Length - 1;
            byte[] firstBytes = new byte[4];
            int byteIndex = firstBytes.Length - 1;
            for (int i = header.Length - 1; i > header.Length - 1 - 4; i--)
            {
                firstBytes[byteIndex--] = header[i];
                index--;
            }
            file.Sizes.Add(BitConverter.ToInt32(firstBytes, 0));

            for (int i = header.Length - 1 - 4; i > 7; i--)
            {
                if (index > 7)
                {
                    file.Pointers.Add(ReadPointer(ref index));
                    file.FileNames.Add(ReadFilename(ref index));
                    file.Sizes.Add(ReadSize(ref index));
                }
            }

            file.Sizes.Remove(file.Sizes.Count - 1);
            file.Pointers.Reverse();
            file.FileNames.Reverse();
            file.Sizes.Reverse();
        }

        private int ReadPointer(ref int index)
        {
            byte[] a = new byte[8];
            int byteIndex = a.Length - 1;
            for (int i = index; i > index - a.Length; i--)
            {
                a[byteIndex--] = header[i];
            }
            index -= a.Length;
            return BitConverter.ToInt32(a, 0);
        }

        private int ReadSize(ref int index)
        {
            byte[] a = new byte[8];
            int byteIndex = a.Length - 1;
            for (int i = index; i > index - a.Length; i--)
            {
                a[byteIndex--] = header[i];
            }
            index -= a.Length;
            return BitConverter.ToInt32(a, 0);
        }

        private string ReadFilename(ref int index)
        {
            List<byte> filenameBytes = new List<byte>();
            while (Regex.IsMatch(Convert.ToChar(header[index]).ToString(), "([aA0-zZ9.-])"))
            {
                filenameBytes.Add(header[index]);
                index--;
            }

            StringBuilder filename = new StringBuilder(filenameBytes.Count);
            foreach (byte b in ReverseBytes(filenameBytes.ToArray()))
            {
                filename.Append(Convert.ToChar(b));
            }
            return filename.ToString();
        }
    }
}
