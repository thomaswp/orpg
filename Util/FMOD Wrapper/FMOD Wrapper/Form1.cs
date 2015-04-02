using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FMOD_Test
{
    public partial class Form1 : Form
    {
        private FMOD.System system = null;
        private FMOD.Sound sound1 = null, sound2 = null;
        private FMOD.Channel channel1 = null, channel2 = null;
        private FMOD.DSP dsp = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FMOD.Factory.System_Create(ref system);
            system.init(2, FMOD.INITFLAGS.NORMAL, (IntPtr)null);

            system.createDSPByType(FMOD.DSP_TYPE.PITCHSHIFT, ref dsp);
            dsp.setParameter((int)FMOD.DSP_PITCHSHIFT.PITCH, 1.5f);

            system.createSound("C:/test4.mid", (FMOD.MODE._2D | FMOD.MODE.HARDWARE | FMOD.MODE.CREATESTREAM), ref sound1);
            system.createSound("C:/test2.ogg", (FMOD.MODE._2D | FMOD.MODE.HARDWARE | FMOD.MODE.CREATESTREAM), ref sound2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            system.playSound(FMOD.CHANNELINDEX.FREE, sound1, false, ref channel1);
            //channel1.setFrequency(44100 * 1.5f);
            FMOD.DSPConnection c = null;
            channel1.addDSP(dsp, ref c);
            channel1.setPan(1);
            channel1.setVolume(0.5f);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            system.playSound(FMOD.CHANNELINDEX.FREE, sound2, false, ref channel2);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (channel1 != null)
            {
                float v = 0;
                channel1.getVolume(ref v);
                v += 0.003f;
                channel1.setVolume(v);

                channel1.getPan(ref v);
                v -= 0.003f;
                channel1.setPan(v);
            }
            system.update();
        }

    }
}
