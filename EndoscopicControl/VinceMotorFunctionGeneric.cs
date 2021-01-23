using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoscopicControl
{
    //考虑对于传感器信息的获取
    enum MOTOR_MODE { VELOCITY = 0x0300, RELATIVE_POS = 0x0302 };

    //考虑电机使用位置模式
    interface VinceMotorFunctionGeneric
    {
        //电机使能
        void motorEnable();

        //电机速度调节
        void setMotorVelocity(float p_VelocityValue);

        //电机速度模式调节
        void setMotorMode(MOTOR_MODE f_MotorMode);

        //电机目标位置设置
        void setMotorTargetPosition(Int32 p_PositionValue);
    }

    class VinceMotorFunction : VinceMotorFunctionGeneric
    {
        uint m_ID = 0;
        SerialPort m_MotorPort = null;

        public VinceMotorFunction(uint f_ID , SerialPort f_MotorPort)
        {
            m_ID = f_ID;
            m_MotorPort = f_MotorPort;
        }
        public void motorEnable()
        {
            WriteVinceMotorMessage l_MotorEnable = new WriteVinceMotorMessage(m_ID, 
                WriteVinceMotorMessage.FOUR_SEGMENT_CODE.MOTOR_CONTROL,0x0101);
            SendMessage(l_MotorEnable);
        }

        public void setMotorMode(MOTOR_MODE f_MotorMode)
        {
            WriteVinceMotorMessage l_MotorChangeMode = null;
            switch (f_MotorMode)
            {
                case MOTOR_MODE.RELATIVE_POS:
                    l_MotorChangeMode = new WriteVinceMotorMessage(m_ID,
               WriteVinceMotorMessage.FOUR_SEGMENT_CODE.MOTOR_CONTROL, 0x0203);
                    break;
                case MOTOR_MODE.VELOCITY:
                    l_MotorChangeMode = new WriteVinceMotorMessage(m_ID,
               WriteVinceMotorMessage.FOUR_SEGMENT_CODE.MOTOR_CONTROL, 0x0003);
                    break;
            }
            SendMessage(l_MotorChangeMode);
        }

        public void setMotorTargetPosition(Int32 p_PositionValue)
        {
            WriteVinceMotorMessageInt32 l_MotorTargetValue = new WriteVinceMotorMessageInt32(m_ID
                , WriteVinceMotorMessage.FOUR_SEGMENT_CODE.TARGET_POSITON, p_PositionValue);
            SendMessage(l_MotorTargetValue);
        }

        public void setMotorVelocity(float p_VelocityValue)
        {
            WriteVinceMotorMessageFloat32 l_VelocityValue = new WriteVinceMotorMessageFloat32(m_ID
                , WriteVinceMotorMessage.FOUR_SEGMENT_CODE.TARGET_VOLECITY, p_VelocityValue);
            SendMessage(l_VelocityValue);
        }

        public void SendMessage(VinceMotorMessageGeneric messageGeneric)
        {
            //发送指令串
            try
            {
                m_MotorPort.Write(messageGeneric.byteList.ToArray(), 0, messageGeneric.byteList.Count());

            }
            catch (Exception e)
            {
                int a = 12;
            }

        }
    }




}
