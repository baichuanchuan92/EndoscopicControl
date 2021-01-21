using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EndoscopicControl
{
    //可以弯曲的卷卷线
    class StickObject
    {
        InspireMotorFunction m_MotorLeft = new InspireMotorFunction(1);
        InspireMotorFunction m_MotorRight = new InspireMotorFunction(2);

        public StickObject()
        {
          SerialPortConfig.getSerialPortConfig().SetPortProperty("COM3",
                      "19200",
                      "1",
                      "8",
                      "无");
          SerialPortConfig.getSerialPortConfig().MotorConnect();
          SerialPortConfig.getSerialPortConfig().getPort().DataReceived += InspireMotorBroadcastMessage.MotorAResolver;
        }

       public void bendStick(int f_BendValue)
        {
            UInt16 l_BendValue = (UInt16)f_BendValue;
            if (f_BendValue <= 2000)
            {
                m_MotorLeft.setImpulseCount(l_BendValue);
            }
            if (f_BendValue >= 2000)
            {
                f_BendValue = 4001 - f_BendValue;
                m_MotorRight.setImpulseCount(l_BendValue);
            }
        }

    }
    class RotationObject
    {
        VinceMotorFunction RotationMotor = new VinceMotorFunction(1);
        //构造的时候初始化电机使能。
        public RotationObject()
        {
            RotationMotor.motorEnable();
        }
        //设置内镜旋转的度数
        public void setRotationAngle(int f_AngleValue)
        {
            int PositionValue = f_AngleValue;
            RotationMotor.setMotorTargetPosition(PositionValue);
        }
    }
    class MoveObject
    {
        VinceMotorFunction MoveMotor = new VinceMotorFunction(1);
        public MoveObject()
        {
            MoveMotor.motorEnable();
        }
        public void setMoveValue(int f_MoveValue)
        {
            int PositionValue = f_MoveValue;
            MoveMotor.setMotorTargetPosition(PositionValue);
        }

    }

    //只持有对象使用
    class Endoscope
    {
        StickObject m_StickObject = new StickObject();
        RotationObject m_RotationObject = new RotationObject();
        MoveObject m_MoveObject = new MoveObject();

        //旋转
        public void setRotation(int f_AngleValue)
        {
            m_RotationObject.setRotationAngle(f_AngleValue);
        }

        //摆动
        public void setSwing(int f_BendValue)
        {
            m_StickObject.bendStick(f_BendValue);
        }

        //进动
        public void setMove(int f_MoveValue)
        {
            m_MoveObject.setMoveValue(f_MoveValue);
        }
    }




}
