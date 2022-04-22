using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tools
{
    public class CompareTool
    {
        public static bool Compare(int item1, double item2)
        {
            return Math.Abs(item2 - item1) < 0.0000001;
        }
    }
}
