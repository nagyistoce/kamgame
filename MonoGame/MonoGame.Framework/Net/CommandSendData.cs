using System;

namespace Microsoft.Xna.Framework.Net
{
    internal class CommandSendData : ICommand
    {
        internal int count;
        internal byte[] data;
        internal NetworkGamer gamer;
        internal int gamerInternalIndex = -1;
        internal int offset;
        internal SendDataOptions options;
        internal LocalNetworkGamer sender;

        public CommandSendData(byte[] data, int offset, int count, SendDataOptions options, NetworkGamer gamer,
                               LocalNetworkGamer sender)
        {
            if (gamer != null)
                gamerInternalIndex = gamer.Id;
            this.data = new byte[count];
            Array.Copy(data, offset, this.data, 0, count);
            this.offset = offset;
            this.count = count;
            this.options = options;
            this.gamer = gamer;
            this.sender = sender;
        }

        public CommandEventType Command
        {
            get { return CommandEventType.SendData; }
        }
    }
}