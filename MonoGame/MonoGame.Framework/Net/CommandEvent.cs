namespace Microsoft.Xna.Framework.Net
{
    internal class CommandEvent
    {
        private readonly CommandEventType command;
        private readonly object commandObject;

        public CommandEvent(CommandEventType command, object commandObject)
        {
            this.command = command;
            this.commandObject = commandObject;
        }

        public CommandEvent(ICommand command)
        {
            this.command = command.Command;
            commandObject = command;
        }

        public CommandEventType Command
        {
            get { return command; }
        }

        public object CommandObject
        {
            get { return commandObject; }
        }
    }
}