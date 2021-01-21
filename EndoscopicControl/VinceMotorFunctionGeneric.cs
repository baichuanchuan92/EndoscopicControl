using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoscopicControl
{
    //考虑对于传感器信息的获取
    //考虑电机使用位置模式
    interface VinceMotorFunctionGeneric
    {

        //电机使能
        void motorEnable();

        //电机速度调节
        void setMotorVelocity();

        //电机速度模式调节
        void setMotorMode();

        //电机目标位置设置
        void setMotorTargetPosition(Int32 p_PositionValue);
    }

    class VinceMotorFunction : VinceMotorFunctionGeneric
    {
        uint m_ID = 0;
        public VinceMotorFunction(uint f_ID)
        {
            m_ID = f_ID;
            //链接电机
            SerialPortConfig.SetPortProperty("COM3",
                       "19200",
                       "1",
                       "8",
                       "无");
            SerialPortConfig.MotorConnect();
        }
        public void motorEnable()
        {
            WriteVinceMotorMessage l_MotorEnable = new WriteVinceMotorMessage(m_ID, 
                WriteVinceMotorMessage.FOUR_SEGMENT_CODE.MOTOR_CONTROL,0x0101);
            l_MotorEnable.SendMessage();
        }

        public void setMotorMode()
        {
            throw new NotImplementedException();
        }

        public void setMotorTargetPosition(Int32 p_PositionValue)
        {
            WriteVinceMotorMessageInt32 l_MotorTargetValue = new WriteVinceMotorMessageInt32(m_ID
                , WriteVinceMotorMessage.FOUR_SEGMENT_CODE.TARGET_POSITON, p_PositionValue);
            l_MotorTargetValue.SendMessage();
        }

        public void setMotorVelocity()
        {
            throw new NotImplementedException();
        }
    }




}
