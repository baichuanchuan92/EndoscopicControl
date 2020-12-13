using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoscopicControl
{
    class MessageSend
    {
        
        public const byte FrameHead1 = 0x55;
        public const byte FrameHead2 = 0xAA;
        private static MessageSend body = null;

        public static MessageSend Getbody()
        {
            if (body == null)
            {
                body = new MessageSend();
            }
            return body;
        }

        //消息发送
        //消息主体，消息分类
        //转换指令为原始字节串，之后合并发送指令头，之后发送
        public static void  SendMessage(MessageGenric message)
        {
            List<Byte> byteList  = new List<Byte>();
            byteList.Add(0x55);
            byteList.Add(0xAA);
            byteList.AddRange(message.GetByteCammond());
            //发送指令串
            try
            {
                SerialPortConfig.getPort().Write(byteList.ToArray(), 0, byteList.Count());

            }
            catch (Exception e)
            {
                int a = 0;
            }

        }

       

       
    }
}
