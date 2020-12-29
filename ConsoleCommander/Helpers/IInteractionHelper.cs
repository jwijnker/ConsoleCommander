using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCommander.Helpers
{
    public interface IInteractionHelper
    {
        string Title { get; set; }
        ConsoleColor BackgroundColor { get; set; }
        ConsoleColor ForegroundColor { get; set; }

        void Write(string s);
        void WriteLine(string s);
        void Clear();
        void ResetColor();
        string ReadLine();
    }

    public class ConsoleInteractionHelper : IInteractionHelper
    {
        private string _title;
        public string Title { 
            get { return _title; } 
            set { 
                _title = value;
                Console.Title = _title;
            }
        }

        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }

        public void Clear()
        {
            Console.Clear();
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void ResetColor()
        {
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.Gray;
        }

        public void Write(string s)
        {
            Console.BackgroundColor = this.BackgroundColor;
            Console.ForegroundColor = this.ForegroundColor;
            Console.Write(s);
        }

        public void WriteLine(string s)
        {
            Console.BackgroundColor = this.BackgroundColor;
            Console.ForegroundColor = this.ForegroundColor;
            Console.WriteLine(s);

        }
    }
}
