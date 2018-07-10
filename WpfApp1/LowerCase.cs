using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardSimulator
{
    static internal class LowerCase
    {
        static private bool lowerCase = true;

        static public bool Register
        {
            get => lowerCase;
            set => lowerCase = value;
        }
    }
}