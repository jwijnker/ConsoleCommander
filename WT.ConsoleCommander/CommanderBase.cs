using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WT.ConsoleCommander
{
    public abstract class CommanderBase<T> : CommanderBase
    {
        protected T DataProvider;

        public CommanderBase(T receiver, bool doNullCheck = false)
            : base()
        {
            this.DataProvider = receiver;
        }
    }

    public abstract class CommanderBase
    {
        #region Properties

        protected bool active = true;

        protected Dictionary<string, Tuple<string, Action, bool>> commands = new Dictionary<string, Tuple<string, Action, bool>>();

        #endregion

        #region Events & Handlers

        protected event EventHandler OnStart;
        protected event EventHandler OnClose;
        //protected event EventHandler<UnhandledExceptionEventArgs> OnError;

        protected virtual void OnStarted(object sender, EventArgs eventArgs)
        {

        }

        protected virtual void OnClosed(object sender, EventArgs eventArgs)
        {
            this.OnClose(this, new EventArgs());
            Write($"Closing {this.GetType().Name}.", ConsoleColor.Magenta);
        }

        protected virtual void OnError(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            Write(((Exception)eventArgs.ExceptionObject).Message, ConsoleColor.Red);
        }

        #endregion

        #region Constructor(s)

        protected CommanderBase()
        {
            // Set the default event handlers
            this.OnStart += OnStarted;
            this.OnClose += OnClosed;

            registerCommand("q", "Quit", this.QuitCommander, true);
            registerCommand("clear", "Clear Screen", this.ClearScreen, true);
        }

        #endregion

        public void Run()
        {
            this.OnStart(this, new EventArgs());

            while (active)
            {
                Console.Title = $"Commander '{this.GetType().Name}'.";
                Write($"Current Commander '{this.GetType().Name}'.", ConsoleColor.DarkCyan);

                #region Show commands available

                Console.WriteLine($"Choose command to invoke: ");
                Console.WriteLine();

                foreach (var k in commands.Where(c => !c.Value.Item3).OrderBy(c => c.Key))
                {
                    Console.WriteLine($" '{k.Key}' : {k.Value.Item1}");
                }
                Console.WriteLine();

                foreach (var k in commands.Where(c => c.Value.Item3).OrderBy(c => c.Key))
                {
                    Write($" '{k.Key}' : {k.Value.Item1}");
                }
                Console.WriteLine();

                #endregion

                Console.Write("Command: ");
                var command = Console.ReadLine().ToLower();

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
                    Write($"'{command}' is no command", ConsoleColor.Yellow);
                }
            }
        }

        protected void registerCommand(string command, string description, Action action, bool systemCommand = false)
        {
            commands.Add(command, new Tuple<string, Action, bool>(description, action, systemCommand));
        }

        protected void useCommander(CommanderBase commanderToUse)
        {
            commanderToUse.Run();
        }

        protected void ClearScreen()
        {
            Console.Clear();
        }

        protected void QuitCommander()
        {
            active = false;
            ClearScreen();
        }

        #region Writing messages

        public void Write(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        #endregion

    }
}
