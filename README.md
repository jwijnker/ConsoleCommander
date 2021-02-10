# ConsoleCommander	

First, thanks for taking a look into ConsoleCommander.
Finally there is some documentation about this cool tool.

I decided to have a version of this tool on nuget after I used it for several projects in (E2E-, Integration-) testing, experimenting and maintenance (executing several tasks repeately) in different environments.

# QuickStart
 1. Create a ConsoleApplication (.Net Core)
 2. Add a reference to '*ConsoleCommander*' (nuget-) package.
 3. Add a new class **MainCommander** and derive from **CommanderBase\<IServiceProvider\>**
 4. Add command-registration in constructor
>		registerCommand("1", nameof(hello), hello);
5. Create method 'Hello':
>		private void hello()
>		{
>			var name = this.requestValue("Name", "YourName");
>			this.Write($"Hello {name}");
>		}

6. Run the application, and press '1' to start command *hello()*.

## Sample files
### Program.cs

    using CCQS;
	using ConsoleCommander.Extensions;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using System;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;


	namespace CCQS
	{
	    class Program : IHostedService
	    {
	        public static void Main(string[] args)
	        {
	            var builder = new HostBuilder()

	                .ConfigureAppConfiguration((hostingContext, config) =>
	                {
	                    config.SetBasePath(Directory.GetCurrentDirectory());
	                    config.AddJsonFile("appsettings.json", true);
	                    if (args != null) config.AddCommandLine(args);
	                })
	                .ConfigureServices((hostingContext, services) =>
	                {
	                    // Set the services used.
	                    services.AddHostedService(s => new Program(s));

						services.AddCommanders(typeof(Program).Assembly);
	                })
	                .ConfigureLogging((hostingContext, logging) =>
	                {
	                    logging.AddConfiguration(hostingContext.Configuration);
	                    logging.AddConsole();
	                });

	            builder.RunConsoleAsync();
	        }


	        private IServiceProvider _serviceProvider;

	        public Program(IServiceProvider serviceProvider)
	        {
	            _serviceProvider = serviceProvider;
	        }

	        #region Helpers

	        public async Task StartAsync(CancellationToken cancellationToken)
	        {
	            try
	            {
	                new MainCommander(_serviceProvider)
	                    .Run();
	            }
	            catch (Exception e)
	            {
	                Console.ForegroundColor = ConsoleColor.Red;
	                Console.WriteLine(e.Message);

	                Console.ResetColor();
	                Console.ReadLine();
	            }
	            finally
	            {
	                await this.StopAsync(cancellationToken);
	            }
	        }

	        public async Task StopAsync(CancellationToken cancellationToken)
	        {
	            Console.WriteLine("Closing application");
	            await Task.Delay(200);
	        }

	        #endregion
	    }
	}

### MainCommander.cs (Short version without samples, version with samples is shown below.)
	using ConsoleCommander;
	using System;

	namespace CCQS
	{
	    public class MainCommander : CommanderBase<IServiceProvider>
	    {
		public MainCommander(IServiceProvider serviceProvider)
		    : base(serviceProvider)
		{
		    registerCommand("0", "Simple 'Hello World' sample", hello);
		}

		private void hello()
		{
		    // Request the user to provide its name, (with default value 'World'.	
		    var name = this.requestValue("Name", "World");

		    this.WriteLine($"Hello {name}");
		}
	    }
	}

### MainCommander.cs
	using ConsoleCommander;
	using System;

	namespace CCQS
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
		    // Request the user to provide its name, (with default value 'World'.
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

		    // Here 'words' are written as a list.
		    this.WriteList(words, e => e.ToUpper());
		}

		private void sampleWriteAsTable()
		{
		    this.WriteAsTable(people, 
		    	// Create a definition how to the table should look like
		    	new Dictionary<string, Func<Person, object>>()
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
		    	}
		    );
		}

		private void sampleRequestBool()
		{
		    this.WriteLine("Are you getting it? ");
		    
		    // Ask the user to provide a bool value (True/False)
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
		    // Request the use to provide a month
		    var month = this.requestMonth();

		    var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

		    this.WriteLine($"You choose {month} and thats '{monthName}'.");
		}

		private void sampleRequestFromList()
		{
		    // Ask the user to pick an item from the list provided.
		    // BEWARE: use index value, starts with 0!
		    var person = this.requestItem(people, p => $"{ p.name } {p.surname} ", "Pick a friend", 1);

		    this.WriteLine($"{person.name} is your new friend.");
		}

	    }
	}

More information is about to come!

In Version 1.0.6 registration of Commanders is simplified by using:
> 	services.AddCommanders(typeof(Program).Assembly);

