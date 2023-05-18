using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NEW_DEMO
{
    internal class Channel
    {
        public const int MAX_SHOW_POINT = 1024;
        public readonly int MAX_SHOW_POINTS = MAX_SHOW_POINT;
        public double time = -20.48;
        public double temperature = 0;
        public double current = 0;
        public double[] Time = new double[MAX_SHOW_POINT];
        public double[] Temperature = new double[MAX_SHOW_POINT];
        public double[] Current = new double[MAX_SHOW_POINT];
        public int Point = 0;
        public ScatterPlot signalplot = null;
        public bool is_open = false; 

        public Channel()
        {
            for (int i = 0; i < MAX_SHOW_POINT; i++)
            {
                Time[i] = i*0.02+time;
                Temperature[i] = 0;
                Current[i] = 0;
            }
        }
        public void Updata_data()
        {
            time += 0.02;
            for (int i = 0; i < MAX_SHOW_POINT - 1; i++)
            {
                Time[i] = Time[i+1];
                Temperature[i] = Temperature[i+1];
                Current[i] = Current[i+1];
            }
            Temperature[MAX_SHOW_POINT - 1] = temperature;
            Current[MAX_SHOW_POINT - 1] = current;
            Time[MAX_SHOW_POINT - 1] = time;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Byte2Voltage
    {
        [FieldOffset(0)] public uint Voltage;
        [FieldOffset(0)] public byte Byte1;
        [FieldOffset(1)] public byte Byte2;

        public Byte2Voltage(uint voltage) : this()
        {
            Voltage = voltage;
        }
    }
}
