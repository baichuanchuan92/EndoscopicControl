using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndoscopicControl.MessageGenric;

namespace EndoscopicControl
{
    class Stick
    {
        public uint iD = 0;

        //推杆可以自动设置ID正常来说，先查询在设置
        public Stick(uint iD)
        {
            this.iD = iD;
        }

        public void resetStickID(uint restID)
        {
            MessageSend.SendMessage(new CommandMessage(iD, CMD_TYPE.CMD_WR_CONTRAL_TAB, CONTRAL_TAB.DriverID, restID));
            MessageSend.SendMessage(new SingleControlMessage(restID, CMD_TYPE.CMD_SINGLECON, SingleControlMessage.SINGLE_PARA.PARA_BIND));
        }

        public void SetStickLegth(uint length)
        {
            MessageSend.SendMessage(new CommandMessage(iD, CMD_TYPE.CMD_WR_DRV_LOC_STAT, CONTRAL_TAB.TARGET_POSITON, length));
        }
        

        public void ReceiveMessage(MessageGenric message)
        {
            int a = 10;
        }
    }
}
