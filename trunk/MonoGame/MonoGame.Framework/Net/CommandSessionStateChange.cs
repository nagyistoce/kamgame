namespace Microsoft.Xna.Framework.Net
{
    internal class CommandSessionStateChange : ICommand
    {
        private readonly NetworkSessionState newState;
        private readonly NetworkSessionState oldState;

        public CommandSessionStateChange(NetworkSessionState newState, NetworkSessionState oldState)
        {
            this.newState = newState;
            this.oldState = oldState;
        }

        public NetworkSessionState NewState { get { return newState; } }

        public NetworkSessionState OldState { get { return oldState; } }

        public CommandEventType Command { get { return CommandEventType.SessionStateChange; } }
    }
}