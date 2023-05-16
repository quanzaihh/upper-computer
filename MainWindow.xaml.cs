using HandyControl.Expression.Shapes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NEW_DEMO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly int FRAME_LENGTH = 8;
        public Dictionary<int, string> Com_list = new Dictionary<int, string>();
        public string[] Port;
        private FPGA myFPGA = new();
        private List<Byte[]> Serial_read_data_buffer = new List<Byte[]> ();
        private Channel Channel_1 = new Channel();
        private Channel Channel_2 = new Channel();
        private Channel Channel_3 = new Channel();
        private Channel Channel_4 = new Channel();
        public double First_time = 0;
        private float[] save_data_temp = new float[1024];
        private float[] save_data_da = new float[1024];
        private int point = 0;
        private int file_cnt = 0;  

        public MainWindow()
        {
            InitializeComponent();
            Search_port();
            System.Timers.Timer Send_data_to_plot = new System.Timers.Timer();
            Send_data_to_plot.Enabled = true;
            Send_data_to_plot.Interval = 500;
            Send_data_to_plot.Start();
            Send_data_to_plot.Elapsed += new System.Timers.ElapsedEventHandler(Get_data_to_plot);
            System.Timers.Timer Get_serial_data_timer = new System.Timers.Timer();
            Send_data_to_plot.Enabled = true;
            Send_data_to_plot.Interval = 50;
            Send_data_to_plot.Start();
            Send_data_to_plot.Elapsed += new System.Timers.ElapsedEventHandler(Get_serial_data);
            Set_ScottPlot();
        }

        public void Search_port()
        {
            //初始化SerialPort对象
            try
            {
                RegistryKey hklm = Registry.LocalMachine;

                RegistryKey software11 = hklm.OpenSubKey("HARDWARE");

                //打开"HARDWARE"子健
                RegistryKey software = software11.OpenSubKey("DEVICEMAP");

                RegistryKey sitekey = software.OpenSubKey("SERIALCOMM");
                //获取当前子健
                String[] Str2 = sitekey.GetValueNames();

                //获得当前子健下面所有健组成的字符串数组
                int ValueCount = sitekey.ValueCount;
                //获得当前子健存在的健值
                int i;
                Port = new string[ValueCount];
                for (i = 0; i < ValueCount; i++)
                {
                    Port[i] = (string)sitekey.GetValue(Str2[i]);
                }
            }
            catch (Exception e) { }
            for (int i = 0; i < Port.Length; i++)
            {
                Com_list.Add(i, Port[i]);
            }
            Serial_port.ItemsSource = Com_list;
            Serial_port.SelectedIndex = 0;
            Serial_port.SelectedValuePath = "key";
            Serial_port.DisplayMemberPath = "Value";
        }

        private void Connect_serial_Click(object sender, RoutedEventArgs e)
        {
            if (myFPGA.Com.IsOpen)
            {
                myFPGA.Com.Close();
                Brush brush = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                Connect_serial.Background = brush;
                Connect_serial.Content = "连接串口";
            }
            else
            {
                // 端口号
                myFPGA.Com.PortName = Serial_port.Text;
                // 波特率
                myFPGA.Com.BaudRate = int.Parse(Bondrate.Text);
                // 停止位
                if (Stopbit.Text.CompareTo("None") == 0) myFPGA.Com.StopBits = StopBits.None;
                else if (Stopbit.Text.CompareTo("One") == 0) myFPGA.Com.StopBits = StopBits.One;
                else if (Stopbit.Text.CompareTo("Two") == 0) myFPGA.Com.StopBits = StopBits.Two;
                else if (Stopbit.Text.CompareTo("OnePointFive") == 0) myFPGA.Com.StopBits = StopBits.OnePointFive;
                // 校验位
                if (Checkbit.Text.CompareTo("None") == 0) myFPGA.Com.Parity = Parity.None;
                else if (Checkbit.Text.CompareTo("Even") == 0) myFPGA.Com.Parity = Parity.Even;
                else if (Checkbit.Text.CompareTo("Mark") == 0) myFPGA.Com.Parity = Parity.Mark;
                else if (Checkbit.Text.CompareTo("Odd") == 0) myFPGA.Com.Parity = Parity.Odd;
                // 数据位
                myFPGA.Com.DataBits = int.Parse(Databit.Text);
                myFPGA.Com.Open();
                Brush brush = new SolidColorBrush(Color.FromRgb(143, 162, 119));
                Connect_serial.Background = brush;
                Connect_serial.Content = "断开串口";
            }
        }

        private void Serial_read()
        {
            while (myFPGA.Com.BytesToRead >= 8)
            {
                Byte[] data = new Byte[FRAME_LENGTH - 1];
                while (myFPGA.Com.BytesToRead >= 8 && myFPGA.Com.ReadByte() != 0xff) ;
                int AA_cnt = 0;
                int cnt = 0 ;
                while (AA_cnt != 2 && cnt != 7)
                {
                    data[cnt] = (Byte)myFPGA.Com.ReadByte();
                    if (data[cnt]==0xaa)AA_cnt++;
                    cnt++;
                }
                if (cnt==7)
                {
                    Serial_read_data_buffer.Add(data);
                }
            }
        }

        private void Get_serial_data(object source, ElapsedEventArgs e)
        {
            if (myFPGA.Com.IsOpen)
            {
                if (myFPGA.Com.BytesToRead >= 8)
                {
                    Serial_read();
                    int count = 0;
                    if (Serial_read_data_buffer.Count!=0)
                    {
                        foreach (Byte[] data in Serial_read_data_buffer)
                        {
                            Byte2Voltage byte2Voltage = new Byte2Voltage();
                            float Current, Temporate;
                            byte2Voltage.Byte1 = data[2];
                            byte2Voltage.Byte2 = data[1];
                            Temporate = (byte2Voltage.Voltage > 32768) ? -(byte2Voltage.Voltage - 32769) : byte2Voltage.Voltage;
                            Temporate = (float)(Temporate / 256.0);
                            byte2Voltage.Byte1 = data[4];
                            byte2Voltage.Byte2 = data[3];
                            Current = (byte2Voltage.Voltage > 32768) ? -(byte2Voltage.Voltage - 32768) : byte2Voltage.Voltage;
                            Current = (float)(Current * 5 / 32768.0);
                            switch (data[0])
                            {
                                case 1:
                                    {
                                        Current = (float)(-Current * Current * 0.00856 + Current * 20.885 + 100.271);
                                        Channel_1.current = Math.Round(Current, 2);
                                        Channel_1.temperature = Temporate;
                                        Channel_1.Updata_data();
                                        save_data_temp[point] = Temporate;
                                        save_data_da[point] = Current;
                                        point++;
                                        if (point == 1024)
                                        {
                                            Thread thread = new Thread(new ThreadStart(() =>
                                            {
                                                string working_path = System.IO.Directory.GetCurrentDirectory();
                                                string Data_path = working_path + @"\data";
                                                if (!Directory.Exists(Data_path))
                                                {
                                                    Directory.CreateDirectory(Data_path);
                                                }
                                                string file_name = Data_path + @"\" + file_cnt.ToString() + "temp.txt";
                                                using (System.IO.StreamWriter file =
                                                new System.IO.StreamWriter(file_name, false))
                                                {
                                                    foreach (float data in save_data_temp)
                                                    {
                                                        file.WriteLine(data.ToString());
                                                    }
                                                }
                                                file_name = Data_path + @"\" + file_cnt.ToString() + "da.txt";
                                                using (System.IO.StreamWriter file =
                                                new System.IO.StreamWriter(file_name, false))
                                                {
                                                    foreach (float data in save_data_da)
                                                    {
                                                        file.WriteLine(data.ToString());
                                                    }
                                                }
                                                file_cnt++;
                                            }));
                                            thread.Start();
                                            point = 0;
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        Current = (float)(-Current * Current * 0.0118 + Current * 20.841 + 101.2579);
                                        Channel_2.current = Math.Round(Current,2);
                                        Channel_2.temperature = Temporate;
                                        Channel_2.Updata_data();
                                        break;
                                    }
                                case 3:
                                    {
                                        Current = (float)(-Current * Current * 0.01004 + Current * 20.8449 + 101.0015);
                                        Channel_3.current = Math.Round(Current, 2);
                                        Channel_3.temperature = Temporate;
                                        Channel_3.Updata_data();
                                        break;
                                    }
                                case 4:
                                    {
                                        Current = (float)(-Current * Current * 0.01135 + Current * 20.688 + 100.3818);
                                        Channel_4.current = Math.Round(Current, 2);
                                        Channel_4.temperature = Temporate;
                                        Channel_4.Updata_data();
                                        break;
                                    }
                                default: continue;
                            }
                            count++;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            Serial_read_data_buffer.RemoveAt(0);
                        }
                    }
                }
            }
        }

        private void Get_data_to_plot(object source, ElapsedEventArgs e)
        {
            if (myFPGA.Com.IsOpen&&!myFPGA.Stop)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    Channel_1.Updata_data();
                    Channel_2.Updata_data();
                    Channel_3.Updata_data();
                    Channel_4.Updata_data();
                    Text_update();
                    Main_plot.Plot.AxisAuto();
                    Main_plot.Render();
                }));
            }
        }

        private void Set_ScottPlot()
        {
            Main_plot.Plot.XAxis.Label("时间Time/s");
            Main_plot.Plot.YAxis.Label("温度Temp/°");
            Main_plot.Plot.Title("温度波形图");
            Main_plot.Plot.SetAxisLimitsX(0, 20);
            Main_plot.Plot.SetAxisLimitsY(0, 20);
            Channel_1.signalplot = Main_plot.Plot.AddScatter(Channel_1.Time, Channel_1.Temperature);
            Channel_1.signalplot.MaxRenderIndex = Channel_1.MAX_SHOW_POINTS-1;
            Channel_2.signalplot = Main_plot.Plot.AddScatter(Channel_2.Time, Channel_2.Temperature);
            Channel_2.signalplot.MaxRenderIndex = Channel_2.MAX_SHOW_POINTS-1;
            Channel_3.signalplot = Main_plot.Plot.AddScatter(Channel_3.Time, Channel_3.Temperature);
            Channel_3.signalplot.MaxRenderIndex = Channel_3.MAX_SHOW_POINTS-1;
            Channel_4.signalplot = Main_plot.Plot.AddScatter(Channel_4.Time, Channel_4.Temperature);
            Channel_4.signalplot.MaxRenderIndex = Channel_4.MAX_SHOW_POINTS-1;
        }

        private void Channel_1_Click(object sender, RoutedEventArgs e)
        {
            if (Channel_1.is_open) { 
                Channel_1.is_open = false;
                Close_Channel(1);
                Brush brush = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                Channel1.Background = brush;
                Channel1.Content = "打开LD1";
            }
            else{
                Channel_1.is_open = true;
                Open_Channel(1);
                Brush brush = new SolidColorBrush(Color.FromRgb(143, 162, 119));
                Channel1.Background = brush;
                Channel1.Content = "关闭LD1";
            }
        }

        private void Channel2_Click(object sender, RoutedEventArgs e)
        {
            if (Channel_2.is_open)
            {
                Channel_2.is_open = false;
                Close_Channel(2);
                Brush brush = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                Channel2.Background = brush;
                Channel2.Content = "打开LD2";
            }
            else
            {
                Channel_2.is_open = true;
                Open_Channel(2);
                Brush brush = new SolidColorBrush(Color.FromRgb(143, 162, 119));
                Channel2.Background = brush;
                Channel2.Content = "关闭LD2";
            }
        }

        private void Channel3_Click(object sender, RoutedEventArgs e)
        {
            if (Channel_3.is_open)
            {
                Channel_3.is_open = false;
                Close_Channel(3);
                Brush brush = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                Channel3.Background = brush;
                Channel3.Content = "打开LD3";
            }
            else
            {
                Channel_3.is_open = true;
                Open_Channel(3);
                Brush brush = new SolidColorBrush(Color.FromRgb(143, 162, 119));
                Channel3.Background = brush;
                Channel3.Content = "关闭LD3";
            }
        }

        private void Channel4_Click(object sender, RoutedEventArgs e)
        {
            if (Channel_4.is_open)
            {
                Channel_4.is_open = false;
                Close_Channel(4);
                Brush brush = new SolidColorBrush(Color.FromRgb(211, 211, 211));
                Channel4.Background = brush;
                Channel4.Content = "打开LD4";
            }
            else
            {
                Channel_4.is_open = true;
                Open_Channel(4);
                Brush brush = new SolidColorBrush(Color.FromRgb(143, 162, 119));
                Channel4.Background = brush;
                Channel4.Content = "关闭LD4";
            }
        }
        private void Open_Channel(int channel)
        {
            Byte[] commend = new Byte[6] { 0xff, 0x00, (Byte)channel, 0x00, 0x00, 0xaa };
            myFPGA.Com.Write(commend, 0, 6);
        }
        private void Close_Channel(int channel)
        {
            Byte[] commend = new Byte[6] { 0xff, 0x04, (Byte)channel, 0x00, 0x00, 0xaa };
            myFPGA.Com.Write(commend, 0, 6);
        }

/*        private void StopShow_Click(object sender, RoutedEventArgs e)
        {
            myFPGA.Stop = !myFPGA.Stop;
            if (myFPGA.Stop)
            {
                StopShow.Content = "开始";
            }
            else { StopShow.Content = "停止"; }
        }*/
        private void Text_update()
        {
            LD1_acTemp.Text = Channel_1.temperature.ToString("0.0");
            LD2_acTemp.Text = Channel_2.temperature.ToString("0.0");
            LD3_acTemp.Text = Channel_3.temperature.ToString("0.0");
            LD4_acTemp.Text = Channel_4.temperature.ToString("0.0");
            LD1_acCurrent.Text = Channel_1.current.ToString("0.0");
            LD2_acCurrent.Text = Channel_2.current.ToString("0.0");
            LD3_acCurrent.Text = Channel_3.current.ToString("0.0");
            LD4_acCurrent.Text = Channel_4.current.ToString("0.0");
        }

        private void LD1_setTemp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    int code = (int)(float.Parse(LD1_setTemp.Text) * 256);
                    Byte High = (Byte)((code>>8)&0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x01, 0x01, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
                LD1_setTemp.Focus();
            }
        }

        private void LD2_setTemp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    int code = (int)(float.Parse(LD2_setTemp.Text) * 256);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x01, 0x02, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void LD3_setTemp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    int code = (int)(float.Parse(LD3_setTemp.Text) * 256);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x01, 0x03, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void LD4_setTemp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    int code = (int)(float.Parse(LD4_setTemp.Text) * 256);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x01, 0x04, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void LD1_setCurrent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    float current = float.Parse(LD1_setCurrent.Text);
                    int code = (int)((current*current*0.0004 + current * 11.3509) * 65535 / 2500);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x03, 0x01, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void LD2_setCurrent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    float current = float.Parse(LD2_setCurrent.Text);
                    int code = (int)((current * current * 0.00036 + current * 11.3952 - 8.2736) * 65535 / 2500);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x03, 0x02, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void LD3_setCurrent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    float current = float.Parse(LD3_setCurrent.Text);
                    int code = (int)((current * current * 0.00035 + current * 11.3655 - 0.5453) * 65535 / 2500);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x03, 0x03, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void LD4_setCurrent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    float current = float.Parse(LD4_setCurrent.Text);
                    int code = (int)((current * current * 0.00034 + current * 11.4027 - 5.1199) * 65535 / 2500);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x03, 0x04, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void set_P_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    int code = int.Parse(set_P.Text);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x05, 0x00, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void set_I_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    int code = int.Parse(set_I.Text);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x05, 0x01, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void set_D_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    int code = int.Parse(set_D.Text);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x05, 0x02, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void LD1_setVol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    double vol = double.Parse(LD1_setVol.Text);
                    vol = ((vol * -18.0) / 47);
                    vol = 0.00008 * vol * vol + 1.0016 * vol + 0.0071;
                    int code = (int)(vol * 65535 / 2.5);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x02, 0x01, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void LD2_setVol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    double vol = double.Parse(LD2_setVol.Text);
                    vol = ((vol * -18.0) / 47);
                    vol = 1.0060 * vol + 0.0043;
                    int code = (int)(vol * 65535 / 2.5);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x02, 0x02, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void LD3_setVol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    double vol = double.Parse(LD3_setVol.Text);
                    vol = ((vol * -18.0) / 47);
                    vol = 0.00008 * vol * vol + 0.9977 * vol + 0.0065;
                    int code = (int)(vol * 65535 / 2.5);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x02, 0x03, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }

        private void LD4_setVol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myFPGA.Com.IsOpen)
                {
                    double vol = double.Parse(LD4_setVol.Text);
                    vol = ((vol * -18.0) / 47);
                    vol = 1.0014 * vol + 0.0045;
                    int code = (int)(vol * 65535 / 2.5);
                    Byte High = (Byte)((code >> 8) & 0xff);
                    Byte Low = (Byte)(code & 0xff);
                    Byte[] commend = new Byte[6] { 0xff, 0x02, 0x04, High, Low, 0xaa };
                    myFPGA.Com.Write(commend, 0, 6);
                }
            }
        }
    }
}