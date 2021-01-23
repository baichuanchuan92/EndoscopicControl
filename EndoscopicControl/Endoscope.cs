using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EndoscopicControl
{
    //可以弯曲的卷卷线
    class StickObject
    {
        InspireMotorFunction m_MotorLeft = null;
        InspireMotorFunction m_MotorRight = null;
        SerialPort m_MotorPort = null;
        //每个对象持有一个端口
        public StickObject(string f_ComPort)
        {
           SerialPortConfig.getSerialPortConfig().SetPortProperty(f_ComPort,
                      "921600",
                      "1",
                      "8",
                      "无");
           m_MotorPort = SerialPortConfig.getSerialPortConfig().MotorConnect();
          if(m_MotorPort == null)
            {
                return;
            }
            m_MotorPort.DataReceived += InspireMotorBroadcastMessage.MotorAResolver;
            m_MotorLeft = new InspireMotorFunction(1, m_MotorPort);
            m_MotorRight = new InspireMotorFunction(2, m_MotorPort);
            MessageBox.Show(f_ComPort + "链接成功", "Success");
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
                l_BendValue = (UInt16)(4001 - f_BendValue);
                m_MotorRight.setImpulseCount(l_BendValue);
            }
        }

    }
    class RotationObject
    {
        VinceMotorFunction RotationMotor = null;
        SerialPort m_MotorPort = null;
        //构造的时候初始化电机使能。
        public RotationObject(string f_ComPort)
        {
           SerialPortConfig.getSerialPortConfig().SetPortProperty(f_ComPort,
           "57600",
           "1",
           "8",
           "无");
            m_MotorPort = SerialPortConfig.getSerialPortConfig().MotorConnect();
            if (m_MotorPort == null)
            {
                return;
            }
            m_MotorPort.DataReceived += InspireMotorBroadcastMessage.MotorAResolver;
            RotationMotor = new VinceMotorFunction(1, m_MotorPort);
            RotationMotor.motorEnable();
            Thread.Sleep(500);
            RotationMotor.setMotorMode(MOTOR_MODE.RELATIVE_POS);
            Thread.Sleep(500);
            MessageBox.Show(f_ComPort + "链接成功", "Success");
        }

        int m_ForwardValue = 0;
        //设置内镜旋转的度数
        public void setRotationAngle(int f_AngleValue)
        {
            //64个微分细步
            int count = Math.Abs(m_ForwardValue - f_AngleValue);
            if (m_ForwardValue  - f_AngleValue > 0)
            {
              RotationMotor.setMotorTargetPosition(-35 * count);
            }
            else
            {
              RotationMotor.setMotorTargetPosition(35 * count);
            }
            m_ForwardValue = f_AngleValue;
        }
    }
    class MoveObject
    {
        VinceMotorFunction MoveMotor = null;
        SerialPort m_MotorPort = null;
        public MoveObject(string f_ComPort)
        {
            SerialPortConfig.getSerialPortConfig().SetPortProperty(f_ComPort, "9600","1","8","无");
            m_MotorPort = SerialPortConfig.getSerialPortConfig().MotorConnect();
            if (m_MotorPort == null)
            {
                return;
            }
            m_MotorPort.DataReceived += InspireMotorBroadcastMessage.MotorAResolver;
            MoveMotor = new VinceMotorFunction(1,m_MotorPort);
            MoveMotor.motorEnable();
            Thread.Sleep(500);
            MoveMotor.setMotorVelocity(0);
            Thread.Sleep(500);
            MoveMotor.setMotorMode(MOTOR_MODE.VELOCITY);
            Thread.Sleep(500);
            MessageBox.Show(f_ComPort + "链接成功", "Success");
        }
        public void setMoveValue(int f_MoveValue)
        {   
            //100最大时10000的脉冲每秒
            int PositionValue = f_MoveValue * 100;
            MoveMotor.setMotorVelocity(PositionValue);
        }

    }

    //只持有对象使用
    class Endoscope
    {
        StickObject m_StickObject = null;
        RotationObject m_RotationObject = null;
        MoveObject m_MoveObject = null;
        public Endoscope(string f_SwingMotorCom, string f_RotatMotorCom , string f_MoveMotorCom)
        {
            m_StickObject = new StickObject(f_SwingMotorCom);
            m_RotationObject = new RotationObject(f_RotatMotorCom);
            m_MoveObject = new MoveObject(f_MoveMotorCom);
        }
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
