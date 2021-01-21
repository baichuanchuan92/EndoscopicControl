using EndoscopicControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EndoscopicControl.InspireMotorMessageGenric;

namespace EndoscopicControl
{
    public partial class Form1 : Form
    {
        Endoscope m_EndScopeObject = new Endoscope();
        uint m_leftBend = 6;
        uint m_rightBend = 1;
        public Form1()
        {
            //Stick stick;
            InitializeComponent();
            SerialPortConfig.MotorConnect();
            //stick = new Stick(m_leftBend);
            //MessageReceive.RegisterStick(stick);
            //stick = new Stick(m_rightBend);
            //MessageReceive.RegisterStick(stick);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //resetStickID();
            //BroadcastMessage bm = new BroadcastMessage(CMD_TYPE.CMD_WR_DRV_BRODACAST, CONTRAL_TAB.TARGET_POSITON);
            //bm.AddStickAndPara(1, 500);
            //bm.AddStickAndPara(2, 500);
            //bm.Process();
            //MessageSend.SendMessage((MessageGenric)bm);
            //Stick stick;
            //stick = MessageReceive.FindStickByID(m_leftBend);
            //stick.SetStickLegth((uint)500);
            //stick = MessageReceive.FindStickByID(m_rightBend);
            //stick.SetStickLegth((uint)2000);

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            /*int length;
            Stick stick;
            //2001为中间值
            uint distance = (uint)trackBar1.Value;
            if(distance <= 2000)
            {
                stick = MessageReceive.FindStickByID(m_leftBend);
                length = trackBar1.Value;
                stick.SetStickLegth((uint)length);

            }
            else if(distance >= 2002)
            {
                stick = MessageReceive.FindStickByID(m_rightBend);
                length = 2000 - (trackBar1.Value - 2000);
                stick.SetStickLegth((uint)length);
            }
            */

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Stick stick = MessageReceive.FindStickByID(1);
            //stick.resetStickID(6);
            //Stick stick = MessageReceive.FindStickByID(2);
            //stick.SetStickLegth((uint)2000);
            //Stick stick1 = MessageReceive.FindStickByID(1);
            //stick1.SetStickLegth((uint)2000);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            m_EndScopeObject.setRotation(0);
        }
    }
}
