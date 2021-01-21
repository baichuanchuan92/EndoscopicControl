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
        //功能与界面进行分离。
        Endoscope m_EndScopeObject = new Endoscope();
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
            m_EndScopeObject.setSwing(trackBar1.Value);
            numericUpDown1.Value = trackBar1.Value;
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
            m_EndScopeObject.setRotation(trackBar2.Value);
            numericUpDown2.Value = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            m_EndScopeObject.setMove(trackBar3.Value);
            numericUpDown3.Value = trackBar3.Value;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            numericUpDown1.Value = trackBar1.Value;
            numericUpDown2.Value = trackBar2.Value;
            numericUpDown3.Value = trackBar3.Value;

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
            numericUpDown1.Value = trackBar1.Value;

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
    }
}
