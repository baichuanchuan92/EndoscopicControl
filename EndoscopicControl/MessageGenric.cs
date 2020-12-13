using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoscopicControl
{
    //操作杆
    //
    class MessageGenric
    {
        //字节指令容器
        public List<Byte> byteList = new List<byte>();

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
        public List<Byte> GetByteCammond()
        {
            return byteList;
        }
    }
     
    class CommandMessage :  MessageGenric
    {
        public CommandMessage(uint stickID, CMD_TYPE CMDvalue ,CONTRAL_TAB CTValue, uint Para)
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

    class BroadcastMessage : MessageGenric
    {
        List<uint> stickIDs = new List<uint>();
        List<uint> Message = new List<uint>();
        CMD_TYPE CMDvalue;
        CONTRAL_TAB CTValue;
        public BroadcastMessage(CMD_TYPE CMDvalue, CONTRAL_TAB CTValue)
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

    class SingleControlMessage : MessageGenric
    {
        //单控参数
        public enum SINGLE_PARA { WORK =0x04,STOP = 0x23,PARA_BIND =0x20};
        public SingleControlMessage(uint stickID, CMD_TYPE CMDvalue, SINGLE_PARA CTValue)
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
