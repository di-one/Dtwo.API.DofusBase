using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtwo.API.DofusBase.Network.Messages
{
    public abstract class MessageParser
    {
        public abstract void OnGetPacket(byte[] data, int length);
        public Action<string, DofusMessage> OnGetMessage;
    }
}
