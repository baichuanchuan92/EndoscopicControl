using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoscopicControl
{
    interface InspireMotorFunctionGeneric
    {
        //重置电机的ID
        void resetID();

        //设置电机的脉冲数
        void setImpulseCount(UInt16 f_ImpulseValue);

        void sendMessage(InspireMotorMessageGenric f_Message);
    }

    class InspireMotorFunction : InspireMotorFunctionGeneric
    {
        uint m_ID = 0;
        SerialPort m_MotorPort = null;
        
        //构造
        public InspireMotorFunction(uint f_ID , SerialPort f_MotorPort)
        {
            m_ID = f_ID;
            m_MotorPort = f_MotorPort;
        }

        //重置ID
        public void resetID()
        {
            //MessageSend.SendMessage(new CommandMessage(iD, CMD_TYPE.CMD_WR_CONTRAL_TAB, CONTRAL_TAB.DriverID, restID));
            //MessageSend.SendMessage(new SingleControlMessage(restID, CMD_TYPE.CMD_SINGLECON, SingleControlMessage.SINGLE_PARA.PARA_BIND));
        }

        //设置脉冲长度
        public void setImpulseCount(UInt16 f_ImpulseValue)
        {
            InspireMotorMovementControlMessage l_MoveCmd = new InspireMotorMovementControlMessage(m_ID, InspireMotorMovementControlMessage.MOVE_CODE.LOC_BACK,
                InspireMotorMovementControlMessage.CONTRAL_TAB.TARGET_POSITON, f_ImpulseValue);
            sendMessage(l_MoveCmd);
        }

        //消息发送
        //消息主体，消息分类
        //转换指令为原始字节串，之后合并发送指令头，之后发送
        public void sendMessage(InspireMotorMessageGenric f_Message)
        {
            List<Byte> lTmpByteList = new List<byte>();
            lTmpByteList.Add(f_Message.FrameHead1);
            lTmpByteList.Add(f_Message.FrameHead2);
            lTmpByteList.AddRange(f_Message.byteList);
            //发送指令串
            try
            {
                m_MotorPort.Write(lTmpByteList.ToArray(), 0, lTmpByteList.Count());
            }
            catch (Exception e)
            {
                string l_exception = e.Message;
            }

        }
    }
}
