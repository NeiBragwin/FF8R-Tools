using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackReader
{
    /**
     * Shows the progress.
     */
    class Progress
    {
        private readonly double length;
        private double current = 0;
        public Progress(int length)
        {
            this.length = length;
        }

        public void Update()
        {
            Console.Write("\r{0}%", Math.Round(++current / length * 100));
        }
    }
}
