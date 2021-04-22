using ConsoleCommander.Helpers;
using ConsoleCommander.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace ConsoleCommander
{
    [DebuggerStepThrough]
    public abstract class CommanderBase<T> : CommanderBase
    {
        protected T DataProvider;

        public CommanderBase(T receiver, bool doNullCheck = false)
            : base()
        {
            this.DataProvider = receiver;
        }
    }

    /// <summary>
    /// Base class for all commanders.
    /// This base class contains all basic logic to:
    /// - Run a commander.
    /// - Register commands.
    /// - Easily connect a command to an other commander.
    /// - Write messages.
    /// </summary>
    [DebuggerStepThrough]
    public abstract class CommanderBase
    {
        #region Properties

        protected IInteractionHelper interactionHelper;

        protected bool active = true;

        internal CommandsContainer commands = new CommandsContainer();

        #endregion

        #region Events & Handlers

        protected event EventHandler OnStart;
        protected event EventHandler OnClose;
        protected event EventHandler<UnhandledExceptionEventArgs> OnError;

        protected virtual void OnStarted(object sender, EventArgs eventArgs)
        {

        }

        protected virtual void OnClosed(object sender, EventArgs eventArgs)
        {
            this.OnClose(this, new EventArgs());
            WriteLine($"Closing {this.GetType().Name}.", ConsoleColor.Magenta);
        }

        protected virtual void OnErrorCaught(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            WriteLine(((Exception)eventArgs.ExceptionObject).Message, ConsoleColor.Red);
        }

        #endregion

        #region Constructor(s)

        protected CommanderBase(IInteractionHelper interactionHelper = null)
        {
            // Set the default event handlers
            this.OnStart += OnStarted;
            this.OnClose += OnClosed;
            this.OnError += OnErrorCaught;

            this.interactionHelper = interactionHelper ?? new ConsoleInteractionHelper();

            registerCommand("q", "Quit", this.QuitCommander, true);
            registerCommand("clear", "Clear Screen", this.ClearScreen, true);
        }

        #endregion

        /// <summary>
        /// Execute the commander.
        /// </summary>
        public void Run()
        {
            this.OnStart(this, new EventArgs());

            while (active)
            {
                WriteLine(string.Empty);

                interactionHelper.Title = $"Commander '{this.GetType().Name}'.";
                WriteLine($"Current Commander '{this.GetType().Name}'.", ConsoleColor.DarkCyan);

                #region Show commands available

                WriteLine($"Choose command to invoke: ");
                WriteLine(string.Empty);

                // Check if there are any numeric commands.
                if (commands.NumericCommands.Any())
                {
                    // Numeric Commands
                    //WriteLine($"Quickstart commands: ", ConsoleColor.White);

                    foreach (var nc in commands.NumericCommands)
                    {
                        WriteLine($" {nc.Id} : {nc.Description}");
                    }
                    WriteLine(string.Empty);
                }

                // Check if there are any string commands.
                if (commands.StringCommands.Any())
                {
                    // Textual Commands
                    //WriteLine($"Standard commands: ", ConsoleColor.White);

                    foreach (var dc in commands.StringCommands)
                    {
                        WriteLine($" '{dc.Id}' : {dc.Description}");
                    }
                    WriteLine(string.Empty);
                }

                if (commands.SystemCommands.Any())
                {
                    // System commands
                    //WriteLine($"System commands: ", ConsoleColor.White);
                    
                    foreach (var sc in commands.SystemCommands)
                    {
                        WriteLine($" '{sc.Id}' : {sc.Description}", ConsoleColor.White);
                    }
                    WriteLine(string.Empty);
                }

                #endregion

                Write("Command: ");
                var commandId = interactionHelper.ReadLine();
                WriteLine(string.Empty);

                if (commands.IsRegistered(commandId))
                {
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    try
                    {
                        commands.GetCommand(commandId).Action();
                    }
                    catch (Exception e)
                    {
                        OnError(this, new UnhandledExceptionEventArgs(e, false));
                    }

                    s.Stop();
                    //Write($"Command took: {s.Elapsed}");
                }
                else
                {
                    WriteLine($"'{commandId}' is no command", ConsoleColor.Yellow);
                }
            }
        }

        /// <summary>
        /// Register a command.
        /// </summary>
        /// <param name="command">The command to use.</param>
        /// <param name="description">The command title/description.</param>
        /// <param name="action">The action to invoke<./param>
        /// <param name="systemCommand">Wether the command is typed as a systemCommand.</param>
        protected void registerCommand(string command, string description, Action action, bool systemCommand = false)
        {
            if (command.Contains(" "))
            {
                this.Warning("Spaces ' ' in command are replaced with underscores '_'.");
                command = command.Replace(" ", "_");
            }

            commands.Add(
                systemCommand
                    ? new SystemCommand(command, description, action)
                    : new StringCommand(command, description, action)
                    );
        }

        /// <summary>
        /// Register a command (numeric).
        /// </summary>
        /// <param name="command">The command to use.</param>
        /// <param name="description">The command title/description.</param>
        /// <param name="action">The action to invoke<./param>
        /// <param name="systemCommand">Wether the command is typed as a systemCommand.</param>
        protected void registerCommand(int command, string description, Action action)
        {
            commands.Add(new NumericCommand(command, description, action));
        }

        /// <summary>
        /// Define the commander to use.
        /// </summary>
        /// <typeparam name="TCommander"></typeparam>
        /// <param name="serviceProvider"></param>
        protected void useCommander<TCommander>(IServiceProvider serviceProvider) where TCommander : CommanderBase
        {
            var commander = (CommanderBase)serviceProvider.GetService(typeof(TCommander));
            useCommander(commander);
        }

        /// <summary>
        /// Define the commander to use.
        /// </summary>
        /// <param name="commanderToUse"></param>
        protected void useCommander(CommanderBase commanderToUse)
        {
            commanderToUse.Run();
        }

        /// <summary>
        /// Clear the console screen.
        /// </summary>
        protected void ClearScreen()
        {
            interactionHelper.Clear();
        }

        /// <summary>
        /// Stop the command an give the control back to the previous commander or quit application.
        /// </summary>
        protected void QuitCommander()
        {
            active = false;
            ClearScreen();
        }

        #region Writing messages

        public void Write(string message, ConsoleColor color = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            interactionHelper.BackgroundColor = backgroundColor;
            interactionHelper.ForegroundColor = color;
            interactionHelper.Write(message);
            interactionHelper.ResetColor();
        }

        public void WriteLine(string message, ConsoleColor color = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            interactionHelper.BackgroundColor = backgroundColor;
            interactionHelper.ForegroundColor = color;
            interactionHelper.WriteLine(message);
            interactionHelper.ResetColor();
        }

        #endregion

        /// <summary>
        /// Request Input from the input provider.
        /// </summary>
        /// <returns></returns>
        internal string GetInput()
        {
            return interactionHelper.ReadLine();
        }

    }
}
