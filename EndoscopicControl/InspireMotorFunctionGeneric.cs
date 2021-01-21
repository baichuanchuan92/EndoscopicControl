using System;
using System.Collections.Generic;
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
        void setImpulseCount(uint f_ImpulseValue);
    }

    class InspireMotorFunction : InspireMotorFunctionGeneric
    {
        public InspireMotorFunction()
        {
            //链接电机
            SerialPortConfig.SetPortProperty("COM3",
                       "19200",
                       "1",
                       "8",
                       "无");
            SerialPortConfig.MotorConnect();
        }
        public void resetID()
        {
            //MessageSend.SendMessage(new CommandMessage(iD, CMD_TYPE.CMD_WR_CONTRAL_TAB, CONTRAL_TAB.DriverID, restID));
            //MessageSend.SendMessage(new SingleControlMessage(restID, CMD_TYPE.CMD_SINGLECON, SingleControlMessage.SINGLE_PARA.PARA_BIND));
        }

        public void setImpulseCount(uint f_ImpulseValue)
        {
            //MessageSend.SendMessage(new CommandMessage(iD, CMD_TYPE.CMD_WR_DRV_LOC_STAT, CONTRAL_TAB.TARGET_POSITON, length));
        }
    }
}
