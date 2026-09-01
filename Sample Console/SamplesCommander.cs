using ConsoleCommander;
using System.Globalization;

namespace Sample_Console
{
    public class SamplesCommander : CommanderBase<IServiceProvider>
    {
        private IEnumerable<Person> People = new List<Person> {
                {
                    new Person {
                        Name = "Anabel",
                        Surname = "Annore",
                        City = "Austin",
                        DateOfBirth = new DateTime(2000, 1, 1)
                    }
                },
                {
                    new Person {
                        Name = "Bob",
                        Surname = "Bellinghi",
                        City = "Boston",
                        DateOfBirth = new DateTime(2002, 2, 22)
                    }
                },
                {
                    new Person {
                        Name = "Charles",
                        Surname = "Cotton",
                        City = "Chicago",
                        DateOfBirth = new DateTime(2013, 8, 13)
                    }
                }
            };

        public SamplesCommander(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            registerCommand("1", "Simple 'Hello World' sample", hello);
            registerCommand("2", "Colorfull writing", sampleWriteInColors);
            registerCommand("3", "Write as List", sampleWriteAsList);
            registerCommand("4", "Write as Table", sampleWriteAsTable);

            registerCommand("5", "Request a bool", sampleRequestBool);
            registerCommand("6", "Request a month(number)", sampleRequestMonth);
            registerCommand("7", "Request an item from list", sampleRequestFromList);

        }

        internal class Person
        {
            internal string Name { get; set; } = null!;
            internal string Surname { get; set; } = null!;
            internal string City { get; set; } = null!;
            internal DateTime DateOfBirth;
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
            this.WriteAsTable(People, new Dictionary<string, Func<Person, object>>()
            {
                { "Fullname", d => $"{d.Name} {d.Surname}" },
                { "Firstname", d => d.Name},
                { "Surname", d => d.Surname},
                { "DateOfBirth", d => d.DateOfBirth.ToShortDateString()},
                { "BornInYear", d => d.DateOfBirth.Year},
                { "Age", d => { 
                    // Calculate the age.
                    var age = DateTime.Today.Year - d.DateOfBirth.Year;

                    // Go back to the year in which the person was born in case of a leap year
                    return d.DateOfBirth.Date > DateTime.Today.AddYears(-age)
                        ? --age
                        : age;
                }},
                { "BornBfore2010", d => d.DateOfBirth.Year < 2010 },
                { "Place", d => d.City.ToUpper()}
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
            else
            {
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
            try
            {
                var person = this.requestItem(People, p => $"{p.Name} {p.Surname} ", "Pick a friend", 1);

                this.WriteLine($"{person.Name} is your new friend.");
            }
            catch (NotSupportedException)
            {
                this.Error("Your new friend is not available.");
            }
        }

    }
}
