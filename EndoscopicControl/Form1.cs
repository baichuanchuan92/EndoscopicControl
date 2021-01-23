using EndoscopicControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EndoscopicControl.InspireMotorMessageGenric;

namespace EndoscopicControl
{
    public partial class Form1 : Form
    {
        //功能与界面进行分离。
        Endoscope m_EndScopeObject = null;
        string[] m_ComList;
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar1.Value + trackBar1.SmallChange > 4001 || trackBar1.Value - trackBar1.SmallChange < 500)
                return;           
            if (trackBar1.Value <= 2000)
            {
                numericUpDown1.Value = trackBar1.Value;
            }
            if (trackBar1.Value >= 2000)
            {
                numericUpDown1.Value = 4001 - trackBar1.Value;
            }
            if(m_EndScopeObject != null)
            {
                m_EndScopeObject.setSwing(trackBar1.Value);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_EndScopeObject.setRotation(10000);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (m_EndScopeObject != null)
            {
                Thread.Sleep(100);
              m_EndScopeObject.setRotation(trackBar2.Value);
            }
            numericUpDown2.Value = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            if(m_EndScopeObject != null)
            {
              m_EndScopeObject.setMove(trackBar3.Value);
            }
            numericUpDown3.Value = trackBar3.Value;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            numericUpDown1.Value = trackBar1.Value;
            numericUpDown2.Value = trackBar2.Value;
            numericUpDown3.Value = trackBar3.Value;
            m_ComList = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(m_ComList);
            comboBox2.Items.AddRange(m_ComList);
            comboBox3.Items.AddRange(m_ComList);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (trackBar1.Value + trackBar1.SmallChange > 4001 || trackBar1.Value - trackBar1.SmallChange < 500)
                return;
            if (e.KeyChar == 'a')
            {
                trackBar1.Value = trackBar1.Value - trackBar1.SmallChange;
            }
            else if (e.KeyChar == 'd')
            {
                trackBar1.Value = trackBar1.Value + trackBar1.SmallChange;
            }
            if (trackBar1.Value <= 2000)
            {
                numericUpDown1.Value = trackBar1.Value;
            }
            if (trackBar1.Value >= 2000)
            {
                numericUpDown1.Value = 4001 - trackBar1.Value;
            }
            m_EndScopeObject.setSwing(trackBar1.Value);

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            string l_SwingMotorCom;
            if(comboBox1.SelectedItem == null)
            {
                l_SwingMotorCom = "COM20";
            }
            else
            {
                l_SwingMotorCom = comboBox1.SelectedItem.ToString();
            }
            string l_RotatMotorCom;
            if (comboBox2.SelectedItem == null)
            {
                l_RotatMotorCom = "COM21";
            }
            else
            {
                l_RotatMotorCom = comboBox2.SelectedItem.ToString();
            }
            string l_MoveMotorCom;
            if (comboBox3.SelectedItem == null)
            {
                l_MoveMotorCom = "COM22";
            }
            else
            {
                l_MoveMotorCom = comboBox3.SelectedItem.ToString();
            }
            m_EndScopeObject = new Endoscope(l_SwingMotorCom, l_RotatMotorCom, l_MoveMotorCom);
        }
    }
}
