using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EndoscopicControl
{
    class StickObject
    {

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

    }

    //只持有对象使用
    class Endoscope
    {
        StickObject m_StickObjectLeft = new StickObject();
        StickObject m_StickObjectRight = new StickObject();
        RotationObject m_RotationObject = new RotationObject();
        MoveObject m_MoveObject = new MoveObject();

        //旋转
        public void setRotation(int f_AngleValue)
        {
            m_RotationObject.setRotationAngle(f_AngleValue);
        }

        //摆动
        public void setSwing()
        {
            
        }

        //进动
        public void setMove()
        {

        }
    }




}
