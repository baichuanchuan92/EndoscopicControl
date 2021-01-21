using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoscopicControl
{
    //操作杆
    //
    class InspireMotorMessageGenric
    {
        //字节指令容器
        public List<Byte> byteList = new List<byte>();

        //消息命令头
        public const byte FrameHead1 = 0x55;
        public const byte FrameHead2 = 0xAA;

        //指令类型枚举
        public enum CMD_TYPE { CMD_RD = 0x01, CMD_WR_DRV_LOC_STAT= 0x21,CMD_WR_DRV_BRODACAST= 0xF2, CMD_WR_CONTRAL_TAB = 0x02 ,CMD_SINGLECON = 0x04};

        //控制表索引,对应控制表起始地址
        public enum CONTRAL_TAB { TARGET_POSITON = 0x37 ,OVERCURRENT = 0x20 , DriverID = 0x02 ,OVERTEMP = 0x62};

        //对应控制表数据长度
        public static uint DataSectorLong(CONTRAL_TAB CTValue, CMD_TYPE CMDvalue)
        {
            if(CMDvalue == CMD_TYPE.CMD_RD)
            {
                return 1;
            }
            switch (CTValue)
            {
                case CONTRAL_TAB.TARGET_POSITON:
                    return 2;
                case CONTRAL_TAB.OVERCURRENT:
                    return 2;
                case CONTRAL_TAB.DriverID:
                    return 1;
            }
            return 0;
        }

        //消息发送
        //消息主体，消息分类
        //转换指令为原始字节串，之后合并发送指令头，之后发送
        public void SendMessage()
        {
            List<Byte> lTmpByteList = new List<byte>();
            lTmpByteList.Add(FrameHead1);
            lTmpByteList.Add(FrameHead2);
            lTmpByteList.AddRange(byteList);
            //发送指令串
            try
            {
                SerialPortConfig.getPort().Write(lTmpByteList.ToArray(), 0, lTmpByteList.Count());

            }
            catch (Exception e)
            {
                int a = 0;
            }

        }


        public static void MotorAResolver(object sender, SerialDataReceivedEventArgs e)
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
                        ;
                    }
                    listbyte.Reverse();
                    for (int i = 0; i < listbyte.Count(); i++)
                    {
                        total = total << 8;
                        total = total | listbyte[i];
                    }
                    //CommandMessage message = new CommandMessage(StickId, CMDvalue, CTValue, total);
                    //FindStickByID(StickId).ReceiveMessage(message);
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }

    }
     
    class InspireMotorCommandMessage :  InspireMotorMessageGenric
    {
        public InspireMotorCommandMessage(uint stickID, CMD_TYPE CMDvalue ,CONTRAL_TAB CTValue, uint Para)
        {
            //帧长度
            uint FrameLength = (DataSectorLong(CTValue, CMDvalue) + 2);
            byteList.Add((Byte)(FrameLength));
            //ID号
            byteList.Add((Byte)stickID);
            //指令类型
            byteList.Add((Byte)CMDvalue);
            //指令类型
            byteList.Add((Byte)CTValue);
            //数据段
            for (int i=0; i < DataSectorLong(CTValue,CMDvalue); i++)
            {
                byteList.Add((Byte)((Para >> i * 8) & 0xFF));
            }
            //校验和
            uint CheckSum = 0;
            for (int i=0; i < byteList.Count(); i++)
            {
                CheckSum = byteList[i] + CheckSum;
            }
            byteList.Add((Byte)(CheckSum & 0xFF));
        }    
    }

    class InspireMotorBroadcastMessage : InspireMotorMessageGenric
    {
        List<uint> stickIDs = new List<uint>();
        List<uint> Message = new List<uint>();
        CMD_TYPE CMDvalue;
        CONTRAL_TAB CTValue;
        public InspireMotorBroadcastMessage(CMD_TYPE CMDvalue, CONTRAL_TAB CTValue)
        {
            this.CMDvalue = CMDvalue;
            this.CTValue = CTValue;
        }
        
        public void AddStickAndPara(uint StickId,uint Para)
        {
            stickIDs.Add(StickId);
            Message.Add(Para);
        }

        public void Process()
        {
            // 帧长度
            uint FrameLength = (uint)(1 + 3 * stickIDs.Count());
            byteList.Add((Byte)(FrameLength));
            //广播ID
            byteList.Add((Byte)(0xFF));
            //指令类型
            byteList.Add((Byte)(CMDvalue));
            for(int i = 0;i<stickIDs.Count();i++)
            {
                //增加驱动器ID
                byteList.Add((Byte)(stickIDs[i]));
                //增加驱动器的位置
                for (int j = 0; j < DataSectorLong(CTValue, CMDvalue); j++)
                {
                    byteList.Add((Byte)((Message[j] >> j * 8) & 0xFF));
                }
            }
            //校验和
            uint CheckSum = 0;
            for (int i = 0; i < byteList.Count(); i++)
            {
                CheckSum = byteList[i] + CheckSum;
            }
            byteList.Add((Byte)(CheckSum & 0xFF));
        }
    }

    class InspireMotorSingleControlMessage : InspireMotorMessageGenric
    {
        //单控参数
        public enum SINGLE_PARA { WORK =0x04,STOP = 0x23,PARA_BIND =0x20};
        public InspireMotorSingleControlMessage(uint stickID, CMD_TYPE CMDvalue, SINGLE_PARA CTValue)
        {
            byteList.Clear();
            //帧长度
            uint FrameLength = (Byte)0x03;
            byteList.Add((Byte)(FrameLength));
            //ID号
            byteList.Add((Byte)stickID);
            //指令类型
            byteList.Add((Byte)CMDvalue);
            //保留字段
            byteList.Add((Byte)0x00);
            //指令类型
            byteList.Add((Byte)CTValue);
            //校验和
            uint CheckSum = 0;
            for (int i = 0; i < byteList.Count(); i++)
            {
                CheckSum = byteList[i] + CheckSum;
            }
            byteList.Add((Byte)(CheckSum & 0xFF));
        }
    }


 }
