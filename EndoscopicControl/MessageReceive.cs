using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndoscopicControl.MessageGenric;

namespace EndoscopicControl
{
    class MessageReceive
    {
        private static List<Stick> stikers = new List<Stick>();
        public static void RegisterStick(Stick stick)
        {
            stikers.Add(stick);
        }

        public static Stick FindStickByID(uint ID)
        {
            for(int i = 0; i< stikers.Count(); i++)
            {
                if(stikers[i].iD == ID)
                {
                    return stikers[i];
                }
            }
            return null;
        }

        public static void Resolver(object sender, SerialDataReceivedEventArgs e)
        {
            //判断是否是群体操作
            try
            {
                byte head1 = (byte)((SerialPort)sender).ReadByte();
                byte head2 = (byte)((SerialPort)sender).ReadByte();
                if (head1 == 0xAA && head2 == 0x55)
                {
                    int FreamLength = ((SerialPort)sender).ReadByte();
                    uint StickId = (uint)((SerialPort)sender).ReadByte();
                    CMD_TYPE CMDvalue = (CMD_TYPE)((SerialPort)sender).ReadByte();
                    CONTRAL_TAB CTValue = (CONTRAL_TAB)((SerialPort)sender).ReadByte();
                    uint total = 0;
                    List<byte> listbyte = new List<byte>();
                    for (int i = 0; i < FreamLength - 2; i++)
                    {
                        listbyte.Add((byte)((SerialPort)sender).ReadByte());        
;                    }
                    listbyte.Reverse();
                    for(int i = 0; i < listbyte.Count(); i++)
                    {
                        total = total << 8;
                        total = total | listbyte[i];
                    }
                    CommandMessage message = new CommandMessage(StickId, CMDvalue, CTValue, total);
                    FindStickByID(StickId).ReceiveMessage(message);
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }

        }


    }
}
