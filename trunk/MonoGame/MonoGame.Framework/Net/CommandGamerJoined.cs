namespace Microsoft.Xna.Framework.Net
{
    internal class CommandGamerJoined : ICommand
    {
        private readonly int gamerInternalIndex = -1;
        private string displayName = string.Empty;
        private string gamerTag = string.Empty;
        internal long remoteUniqueIdentifier = -1;
        private GamerStates states;

        public CommandGamerJoined(int internalIndex, bool isHost, bool isLocal)
        {
            gamerInternalIndex = internalIndex;

            if (isHost)
                states = states | GamerStates.Host;
            if (isLocal)
                states = states | GamerStates.Local;
        }

        public CommandGamerJoined(long uniqueIndentifier)
        {
            remoteUniqueIdentifier = uniqueIndentifier;
        }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public string GamerTag
        {
            get { return gamerTag; }
            set { gamerTag = value; }
        }

        public GamerStates State
        {
            get { return states; }
            set { states = value; }
        }

        public int InternalIndex
        {
            get { return gamerInternalIndex; }
        }

        public CommandEventType Command
        {
            get { return CommandEventType.GamerJoined; }
        }
    }
}