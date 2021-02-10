using ConsoleCommander.Helpers;
using System;
using System.Collections.Generic;
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

        protected Dictionary<string, Tuple<string, Action, bool>> commands = new Dictionary<string, Tuple<string, Action, bool>>();

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

                foreach (var k in commands.Where(c => !c.Value.Item3).OrderBy(c => c.Key))
                {
                    WriteLine($" '{k.Key}' : {k.Value.Item1}");
                }
                WriteLine(string.Empty);

                foreach (var k in commands.Where(c => c.Value.Item3).OrderBy(c => c.Key))
                {
                    WriteLine($" '{k.Key}' : {k.Value.Item1}");
                }
                WriteLine(string.Empty);

                #endregion

                Write("Command: ");
                var command = interactionHelper.ReadLine().ToLower();
                WriteLine(string.Empty);

                if (commands.ContainsKey(command))
                {
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    try
                    {
                        commands[command].Item2();
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
                    WriteLine($"'{command}' is no command", ConsoleColor.Yellow);
                }
            }
        }

        protected void registerCommand(string command, string description, Action action, bool systemCommand = false)
        {
            commands.Add(command, new Tuple<string, Action, bool>(description, action, systemCommand));
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

        protected void useCommander(CommanderBase commanderToUse)
        {
            commanderToUse.Run();
        }

        protected void ClearScreen()
        {
            interactionHelper.Clear();
        }

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

        internal string GetInput()
        {
            return interactionHelper.ReadLine();
        }

    }
}
