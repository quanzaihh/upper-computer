using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System;

namespace NEW_DEMO
{
    internal class FPGA
    {
        public SerialPort Com { get; set; } = new SerialPort();
        public bool Stop = false;

    }
}