using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EndoscopicControl
{
    class SerialPortConfig
    {
        private static bool isOpen = false;
        private static bool isSetProperty = false;
        private SerialPort port = null;
        public  static bool isHex = false;
        private static SerialPortConfig m_SerialPortConfig = null;

        public static SerialPortConfig getSerialPortConfig()
        {
            if (m_SerialPortConfig == null)
            {
                m_SerialPortConfig = new SerialPortConfig();
            }
            return m_SerialPortConfig;
        }

        public  SerialPort getPort()
        {
            return port;
        }

        /// <summary>
        /// 输入参数：
        /// 串口号、波特率、停止位、数据位、奇偶校验位


        /// <summary>
        /// </summary>
        public void SetPortProperty(string PortName, string BaudRate,
            string StopBit, string DataBits, string Pariti)
        {
            //输入参数：
            //串口号、波特率、停止位、数据位、奇偶校验位
            port = new SerialPort();
            port.PortName = PortName.Trim();
            port.BaudRate = Convert.ToInt32(BaudRate.Trim());

            //设置停止位
            float f = Convert.ToSingle(StopBit.Trim());

            if (f == 0)
            {
                port.StopBits = StopBits.None;
            }
            else if (f == 1.5)
            {
                port.StopBits = StopBits.OnePointFive;
            }
            else if (f == 1)
            {
                port.StopBits = StopBits.One;
            }
            else if (f == 2)
            {
                port.StopBits = StopBits.Two;
            }
            else
            {
                port.StopBits = StopBits.One;
            }

            //设置数据位
            port.DataBits = Convert.ToInt16(DataBits.Trim());

            string s = Pariti.Trim(); //设置奇偶校验位
            if (s.CompareTo("无") == 0)
            {
                port.Parity = Parity.None;
            }
            else if (s.CompareTo("奇校验") == 0)
            {
                port.Parity = Parity.Odd;
            }
            else if (s.CompareTo("偶校验") == 0)
            {
                port.Parity = Parity.Even;
            }
            else
            {
                port.Parity = Parity.None;
            }

            //设置超时读取时间
            port.ReadTimeout = -1;
            port.RtsEnable = true;


            isHex = true;


        }

        public SerialPort MotorConnect()
        {
          try
          {
              port.Open();
              isOpen = true;
              //增加串口线程接受
              //port.DataReceived += new SerialDataReceivedEventHandler(MessageReceive.Resolver);
              return port;
          
          }
          catch (Exception)
          {
              isSetProperty = false;
              isOpen = false;
              MessageBox.Show("串口无效或已被占", "Error");
          }
          return null;
        }
    }
   
}
