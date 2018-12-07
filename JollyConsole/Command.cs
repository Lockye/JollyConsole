using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyConsole
{
    public class Command
    {
        public string Text { get; set; }
        public bool Enabled { get; set; }
        public int Position { get; set; }
    }
}
