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
        private static SerialPort port;
        public  static bool isHex = false;


        public static SerialPort getPort()
        {
            return port;
        }

        /// <summary>
        /// 输入参数：
        /// 串口号、波特率、停止位、数据位、奇偶校验位
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// </summary>
        public static void SetPortProperty(string PortName, string BaudRate,
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


        public static void MotorConnect()
        {
            string ComName = "Silicon Labs CP210x USB to UART Bridge";

            string COMStr = "COM" + SerialRecognition.GetSpecifiedSerialPortNum(ComName).ToString();
            //MessageBox.Show(COMStr, COMStr);
            //连接电机
            if (isOpen == false)
            {

                if (!isSetProperty) //串口
                {
                    //因时的并联机器人和自带的电控板波特率不一样
                    //自带：921600
                    //并联机器人：115200
                    SetPortProperty("COM3",
                        "921600",
                        "1",
                        "8",
                        "无");
                    isSetProperty = true;
                }

                try
                {
                    port.Open();
                    isOpen = true;
                    //增加串口线程接受
                    port.DataReceived += new SerialDataReceivedEventHandler(MessageReceive.Resolver);
                    MessageBox.Show("链接成功", "Success");

                }
                catch (Exception)
                {
                    isSetProperty = false;
                    isOpen = false;
                    MessageBox.Show("串口无效或已被占", "Error");
                }
            }
            else
            {
                try
                {
                    port.Close();
                    isOpen = false;
                    isSetProperty = false;


                }
                catch (Exception)
                {
                    //MessageBox.Show("关闭串口时发生错误", "Error");
                }
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
   
}
