using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MiniOP.Model
{
    public class MainModel
    {
        SerialPort serialPort;
        byte[] buffer;
        public MainModel()
        {
            serialPort = new SerialPort();
            XmlReader reader = XmlReader.Create("Serial.xml");
            while (reader.Read())
            {
                if (reader.Name == "PortNum")
                {
                    serialPort.PortName = reader.ReadElementContentAsString();
                }
            }

            serialPort.BaudRate = 115200;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            serialPort.ReadTimeout = SerialPort.InfiniteTimeout;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            buffer = new byte[Packet.PACKETSIZE];
            serialPort.Open();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            serialPort = (SerialPort)sender;
            while (serialPort.BytesToRead > 0)
            {
                buffer[0] = (byte)serialPort.ReadByte();

                if (buffer[0] == 0x02)
                    for (int i = 1; i < Packet.PACKETSIZE; i++)
                        buffer[i] = (byte)serialPort.ReadByte();

                if (buffer[Packet.ETXINDEX] == 0x03)
                    if (buffer[Packet.CHECKSUMINDEX] == Packet.MakeChecksum(buffer))
                    {
                        var id = Encoding.Default.GetString(buffer, Packet.IDINDEX, Packet.IDSIZE);
                        if (id == " KV")
                        {
                            int kv = int.Parse(Encoding.Default.GetString(buffer, Packet.DATAINDEX, Packet.DATASIZE));
                            Ioc.Default.GetService<ViewModels.MainViewModel>().Kv = kv;

                        }
                        if (id == " MA")
                        {
                            if (Ioc.Default.GetService<ViewModels.MainViewModel>().Mode != 3)
                            {
                                double mas = double.Parse(Encoding.Default.GetString(buffer, Packet.DATAINDEX, Packet.DATASIZE));
                                Ioc.Default.GetService<ViewModels.MainViewModel>().MaMas = mas / 10;
                            }
                        }

                        if (id == "MAS")
                        {
                            if (Ioc.Default.GetService<ViewModels.MainViewModel>().Mode == 3)
                            {
                                double mas = double.Parse(Encoding.Default.GetString(buffer, Packet.DATAINDEX, Packet.DATASIZE));
                                Ioc.Default.GetService<ViewModels.MainViewModel>().MaMas = mas / 100;
                            }
                        }

                        if (id == "MOD")
                        {
                            int mode = int.Parse(Encoding.Default.GetString(buffer, Packet.DATAINDEX, Packet.DATASIZE));
                            Ioc.Default.GetService<ViewModels.MainViewModel>().Mode = mode;
                            switch (mode)
                            {
                                case 1:
                                    Ioc.Default.GetService<ViewModels.MainViewModel>().IsFluoro = true; break;
                                case 2:
                                    Ioc.Default.GetService<ViewModels.MainViewModel>().IsPulse = true; break;
                                case 3:
                                    Ioc.Default.GetService<ViewModels.MainViewModel>().IsRad = true; break;
                            }
                        }

                        if (id == "ETM")
                        {
                            int sec = int.Parse(Encoding.Default.GetString(buffer, Packet.DATAINDEX, Packet.DATASIZE));
                            Ioc.Default.GetService<ViewModels.MainViewModel>().Sec = sec % 60;
                        }

                        if (id == "STA")
                        {
                            int data = int.Parse(Encoding.Default.GetString(buffer, Packet.DATAINDEX, Packet.DATASIZE));
                            switch (data)
                            {
                                case 1:
                                    Ioc.Default.GetService<ViewModels.MainViewModel>().StatusImg = "Resources/STATUS_BEGIN.png"; break;
                                case 3:
                                    Ioc.Default.GetService<ViewModels.MainViewModel>().StatusImg = "Resources/STATUS_READY.png"; break;
                                case 4:
                                    Ioc.Default.GetService<ViewModels.MainViewModel>().StatusImg = "Resources/STATUS_XRAY.png"; break;
                                case 9:
                                    Ioc.Default.GetService<ViewModels.MainViewModel>().StatusImg = "Resources/STATUS_WAIT.png"; break;
                                case 11:
                                    Ioc.Default.GetService<ViewModels.MainViewModel>().StatusImg = "Resources/STATUS_ERROR.png"; break;
                            }
                        }


                    }

            }


        }


        ~MainModel()
        {
            serialPort.Close();
        }

        public void Send(string id, string data)
        {
            byte[] bytes = new byte[11];
            bytes = Packet.MakePacket(id, data);
            serialPort.Write(bytes, 0, bytes.Length);
        }
    }
}
