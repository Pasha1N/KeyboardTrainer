using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp1
{
    static class PreviousColor
    {
        static private Brush previousColor = null;

        static public Brush Color
        {
            get { return previousColor; }
            set
            {
                if (previousColor != value)
                {
                    previousColor = value;
                }
            }
        }
    }
}