using System;

namespace ConsoleCommander.Models
{
    public abstract class Command
    {
        public string Id { get; private set; }
        public string Description { get; private set; }
        public Action Action { get; private set; }

        public Command(string id, string description, Action action)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException(nameof(description));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            this.Id = id.Replace(" ", "_"); // Replace spaces for underscores.
            this.Description = description;
            this.Action = action;
        }
    }
}
