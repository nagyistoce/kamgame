namespace Microsoft.Xna.Framework.Net
{
    internal class CommandGamerStateChange : ICommand
    {
        private readonly NetworkGamer gamer;
        private readonly GamerStates newState;
        private readonly GamerStates oldState;

        public CommandGamerStateChange(NetworkGamer gamer)
        {
            this.gamer = gamer;
            newState = gamer.State;
            oldState = gamer.OldState;
        }

        public NetworkGamer Gamer
        {
            get { return gamer; }
        }

        public GamerStates NewState
        {
            get { return newState; }
        }

        public GamerStates OldState
        {
            get { return oldState; }
        }

        public CommandEventType Command
        {
            get { return CommandEventType.GamerStateChange; }
        }
    }
}