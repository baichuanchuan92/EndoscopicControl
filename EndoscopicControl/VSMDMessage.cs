using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace EndoscopicControl
{
    class VSMDMessage
    {
        public List<Byte> byteList = new List<byte>();
        protected uint m_ID;
        protected CMD_TYPE m_FuncCode;
        protected STATE_TYPE m_StateCode;
        protected uint m_StateCodeLength;
        //指令类型枚举 功能码
        public enum CMD_TYPE { CMD_RD_SET = 0x03, CMD_RD_STATE = 0x04, CMD_WR_SINGLE = 0x06, CMD_WR_MULTIES = 0x06 };

        //状态信息
        public enum STATE_TYPE { STATE_CUR_VEL = 0x00, STATE_CUR_LOC = 0x02, STATE_CUR_STATE = 0x04, STATE_CUR_VER = 0x06 };

        //状态信息
        public enum STATE_TYPE_LENGH { STATE_CUR_VEL = 0x02, STATE_CUR_LOC = 0x02, STATE_CUR_STATE = 0x02, STATE_CUR_VER = 0x0E };

        public VSMDMessage()
        {


        }


        //设置消息接受的设备ID号
        public void setDeviceID(uint ID)
        {
            m_ID = ID;
        }

        public void setFuncCode(CMD_TYPE FuncCode)
        {
            m_FuncCode = FuncCode;
        }

        public void setStateCode(STATE_TYPE StateCode)
        {
            StateCode = m_StateCode;
        }
        public void setStateCodeLength(uint StateCodeLength)
        {
            m_StateCodeLength = StateCodeLength;
        }
        void ConstructionMessage()
        {

        }
        public List<Byte> GetByteCammond()
        {
            return byteList;
        }

    }

    class QueryMessage : VSMDMessage
    {
        void ConstructionMessage()
        {
            byteList.Add((byte)m_ID);
            byteList.Add((byte)m_FuncCode);
            byteList.Add((byte)0x00);
            byteList.Add((byte)m_StateCode);
            byteList.Add((Byte)((m_StateCodeLength >> 8) & 0xFF));
            byteList.Add((Byte)(m_StateCodeLength & 0xFF));
            //CRC
            byteList.Add((byte)0x00);
            byteList.Add((byte)0x00);
        }
        
    }

    class WriteMessage : VSMDMessage
    {
        private uint m_byteCount;
        void ConstructionMessage()
        {
            byteList.Add((byte)m_ID);
            byteList.Add((byte)m_FuncCode);
            byteList.Add((byte)0x00);
            byteList.Add((byte)m_StateCode);
            byteList.Add((Byte)((m_StateCodeLength >> 8) & 0xFF));
            byteList.Add((Byte)(m_StateCodeLength & 0xFF));
            //数据长度和内容
            byteList.Add((byte)m_byteCount);
            for(int i=0; i < m_byteCount; i++)
            {
                byteList.Add((byte)0x00);
            }
            //CRC
            byteList.Add((byte)0x00);
        }

    }
}
