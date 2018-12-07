using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JollyConsole
{
    public class Macro
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Command> Commands { get; set; }
        public int Position { get; set; }
    }
}
