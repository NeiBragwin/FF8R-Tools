using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PackReader
{
    public class ZZZFile
    {
        public readonly string Path;
        public List<string> FileNames;
        public List<int> Addresses;
        public List<int> Sizes;
        public List<int> Pointers;
        public readonly int BaseLength;
        public int NumberOfFiles;

        public ZZZFile(string Path, int BaseLength)
        {
            this.Path = Path;
            FileNames = new List<string>();
            Addresses = new List<int>();
            Sizes = new List<int>();
            Pointers = new List<int>();
            this.BaseLength = BaseLength;
            this.NumberOfFiles = 0;
        }
    }
}
