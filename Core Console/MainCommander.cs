using ConsoleCommander;
using Microsoft.Extensions.DependencyInjection;
using Sample_Console.Commanders;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Core_Console
{
    public class MainCommander : CommanderBase<IServiceProvider>
    {
        private IEnumerable<Person> people = new List<Person>();

        public MainCommander(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            people = new List<Person> {
                {
                    new Person {
                        name = "Anabel",
                        surname = "Annore",
                        city = "Austin",
                        dob = new DateTime(2000, 1, 1)
                    }
                },
                {
                    new Person {
                        name = "Bob",
                        surname = "Bellinghi",
                        city = "Boston",
                        dob = new DateTime(2002, 2, 22)
                    }
                },
                {
                    new Person {
                        name = "Charles",
                        surname = "Cotton",
                        city = "Chicago",
                        dob = new DateTime(2013, 8, 13)
                    }
                }
            };

            registerCommand("0", "Simple 'Hello World' sample", hello);
            registerCommand("1", "Colorfull writing", sampleWriteInColors);
            registerCommand("2", "Write as List", sampleWriteAsList);
            registerCommand("3", "Write as Table", sampleWriteAsTable);
            
            registerCommand("4", "Request a bool", sampleRequestBool);
            registerCommand("5", "Request a month(number)", sampleRequestMonth);
            registerCommand("6", "Request an item from list", sampleRequestFromList);

            //registerCommand("a", "Dependency Injection (using .Net Core)", () => useCommander(serviceProvider.GetService<DiCommander>()));
            //registerCommand("b", "Dependency Injection (using Unity)", () => useCommander(serviceProvider.GetService<UnityCommander>()));
            //registerCommand("c", "Dependency Injection (using CastleWindsor)", () => useCommander(serviceProvider.GetService<CastleWindsorCommander>()));
            //registerCommand("d", "Filesystem commander", () => useCommander(serviceProvider.GetService<FilesystemCommander>()));
        }

        private class Person
        {
            internal string name;
            internal string surname;
            internal string city;
            internal DateTime dob;
        }

        private void hello()
        {
            var name = this.requestValue("Name", "World");

            this.WriteLine($"Hello {name}");
        }

        private void sampleWriteInColors()
        {
            this.Write($"This methods shows a handfull of write extensions, especially colors/loglevels.");
            this.WriteEmptyLine();

            this.WriteLine("LOGLEVELS");
            this.Trace("Write a Trace in color.");
            this.Verbose("Write a Verbose in color.");
            this.Debug("Write a Debug in color.");
            this.Info("Write a Info in color.");
            this.Warning("Write a Waring in color.");
            this.Error("Write a Error in color.");
            this.WriteEmptyLine();

            this.WriteLine("RESULT");
            this.Success("Write a Success in color.");
            this.Failed("Write a Failed in color.");
        }

        private void sampleWriteAsList()
        {
            var text = "The quick brown fox jumps over the lazy dog";
            var words = text.Split(" ");

            this.WriteList(words, e => e.ToUpper());
        }

        private void sampleWriteAsTable()
        {
            this.WriteAsTable(people, new Dictionary<string, Func<Person, object>>()
            {
                { "Fullname", d => $"{d.name} {d.surname}" },
                { "Firstname", d => d.name},
                { "Surname", d => d.surname},
                { "DateOfBirth", d => d.dob.ToShortDateString()},
                { "BornInYear", d => d.dob.Year},
                { "Age", d => { 
                    // Calculate the age.
                    var age = DateTime.Today.Year - d.dob.Year;

                    // Go back to the year in which the person was born in case of a leap year
                    return d.dob.Date > DateTime.Today.AddYears(-age)
                        ? --age
                        : age;
                }},
                { "BornBfore2010", d => d.dob.Year < 2010 },
                { "Place", d => d.city.ToUpper()}
            });
        }

        private void sampleRequestBool()
        {
            this.WriteLine("Are you getting it? ");
            var understood = this.requestBool();

            if (understood)
            {
                this.Success($"You seem to get it... :-)");
            }
            else {
                this.Warning($"No problem, lets see and try more samples...");
            }
        }

        private void sampleRequestMonth()
        {
            var month = this.requestMonth();

            var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

            this.WriteLine($"You choose {month} and thats '{monthName}'.");
        }

        private void sampleRequestFromList()
        {
            var person = this.requestItem(people, p => $"{ p.name } {p.surname} ", "Pick a friend", 1);

            this.WriteLine($"{person.name} is your new friend.");
        }

    }
}
