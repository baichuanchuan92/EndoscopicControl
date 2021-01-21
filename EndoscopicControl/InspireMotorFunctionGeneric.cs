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
        void setImpulseCount(UInt16 f_ImpulseValue);
    }

    class InspireMotorFunction : InspireMotorFunctionGeneric
    {
        uint m_ID = 0;
        public InspireMotorFunction(uint f_ID)
        {
            m_ID = f_ID;
        }
        public void resetID()
        {
            //MessageSend.SendMessage(new CommandMessage(iD, CMD_TYPE.CMD_WR_CONTRAL_TAB, CONTRAL_TAB.DriverID, restID));
            //MessageSend.SendMessage(new SingleControlMessage(restID, CMD_TYPE.CMD_SINGLECON, SingleControlMessage.SINGLE_PARA.PARA_BIND));
        }

        public void setImpulseCount(UInt16 f_ImpulseValue)
        {
            InspireMotorMovementControlMessage l_MoveCmd = new InspireMotorMovementControlMessage(m_ID, InspireMotorMovementControlMessage.MOVE_CODE.LOC_BACK,
                InspireMotorMovementControlMessage.CONTRAL_TAB.TARGET_POSITON, f_ImpulseValue);
            l_MoveCmd.SendMessage();
        }
    }
}
