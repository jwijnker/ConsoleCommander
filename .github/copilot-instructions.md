# ConsoleCommander - Copilot Instructions

## Build, Test, and Lint Commands

### Solution and Projects
- **SDK:** .NET 10.0.400 is pinned via `global.json`
- **Main solution:** `Core Console Commander.sln`
- **Main library:** `ConsoleCommander/ConsoleCommander.csproj` (multi-targets `net10.0` and `netstandard2.0` for compatibility, published to NuGet)
- **Test project:** `ConsoleCommander.Tests/ConsoleCommander.Tests.csproj` (targets .NET 10.0)
- **Sample apps:** `Simple Console` and `Sample Console` target .NET 10.0

### Build
```powershell
# Restore solution
dotnet restore "Core Console Commander.sln"

# Build the solution in Release mode
dotnet build "Core Console Commander.sln" --configuration Release
```

### Run Tests
This repository is on xUnit v3 with Microsoft Testing Platform, so the project should be run through the MSBuild Test target or directly via the built test assembly.

```powershell
# Run the full test suite
dotnet msbuild "ConsoleCommander.Tests\ConsoleCommander.Tests.csproj" -t:Test -nologo

# Run a single test method (after a build has produced the DLL)
dotnet "ConsoleCommander.Tests\bin\Debug\net10.0\ConsoleCommander.Tests.dll" --filter-method "ConsoleCommander.Tests.<Namespace>.<TestMethod>"

# Example: run one named test method
dotnet "ConsoleCommander.Tests\bin\Debug\net10.0\ConsoleCommander.Tests.dll" --filter-method "ConsoleCommander.Tests.TestCommanderTests.TestDoesSomething"
```

### Lint / Static Analysis
- No dedicated lint tool or `.editorconfig` is present in this repository.
- Build warnings are the current quality signal; `.NET 10` warnings are surfaced by `dotnet build`.

### CI/CD
- **Pipeline:** `azure-pipelines.yml` (Azure DevOps)
- Triggers on commits to master, feature/*, and release/* branches
- Runs on Windows (windows-latest)
- Builds, restores, and runs all tests

### NuGet Packaging
- Package auto-generates on build when `GeneratePackageOnBuild=true` in ConsoleCommander.csproj
- Version: Currently 8.0.0.0
- Package ID: `ConsoleCommander`
- Published to NuGet (see `pushToNuget.bat`)

## High-Level Architecture

### Core Design Pattern
ConsoleCommander is a **hierarchical menu-driven console framework** for .NET. The architecture revolves around:

1. **Commander Pattern**: Each console menu is a `Commander` class inheriting from `CommanderBase` or `CommanderBase<T>`
2. **Command Registration**: Commands are registered in the commander's constructor using `registerCommand()`
3. **Navigation**: Users navigate between commander instances using `useCommander<T>()`
4. **Interactive I/O**: Abstracted via `IInteractionHelper` for console interaction (colors, input/output)

### Key Components

#### CommanderBase (`CommanderBase.cs`)
- Abstract base class for all commanders
- Manages command registration via `CommandsContainer`
- Handles the run loop and command execution
- Events: `OnStart`, `OnClose`, `OnError`
- Generic variant `CommanderBase<T>` holds a `DataProvider` (dependency injection)

#### Command Registration (Models/)
- **Command** (abstract): Base class with Id, Description, and Action
  - Ids with spaces are automatically converted to underscores
- **NumericCommand**: Commands registered with numbers (e.g., `registerCommand(1, ...)`)
- **StringCommand**: Commands registered with string keys (e.g., `registerCommand("Two", ...)`)
- **SystemCommand**: Internal commands (exit, etc.)
- **CommandsContainer**: Manages all commands and organizes them by type

#### Extensions (Extensions/)
- **CommanderWriteExtensions**: `WriteLine()`, `Write()`, `Trace()`, `Debug()`, `Info()`, `Warning()`, `Error()`, `Success()`, `Failed()`, `WriteList()`, `WriteAsTable()`
- **CommanderRequestExtensions**: `requestValue()`, `requestBool()`, `requestMonth()`, `requestItem()`, `requestFromList()`
- **CommanderContextExtensions**: `useCommander<T>()`
- **ServiceCollectionAddCommandersExtensions**: `AddCommanders()` for DI registration

#### Helpers (Helpers/)
- **IInteractionHelper**: Abstraction for console operations (write, read, colors)
- **ConsoleInteractionHelper**: Console implementation
- **GeneralHelpers**: Utility functions
- **AsyncHelper**: Async operation support

#### Dependency Injection
- Uses Microsoft.Extensions.DependencyInjection
- `IDefaultCommanderProvider`: Interface to determine the starting commander
  - **ConfiguredDefaultCommanderProvider**: Reads from appsettings.json (key: "defaultCommander")
  - **DefinedDefaultCommanderProvider**: Hardcoded in code
- Commanders auto-discovered and registered via `AddCommanders()` from an assembly

### Typical Application Flow
1. Host initializes DI container and calls `AddCommanders()` with an assembly
2. Commanders in the assembly are auto-discovered and registered as transient services
3. Default commander is resolved from `IDefaultCommanderProvider`
4. `CommanderBase.Run()` starts the menu loop
5. User selects a command → action executes
6. User can navigate to child commanders via `useCommander<T>()`

### Multi-Project Structure
- **ConsoleCommander** (main library): Framework code, target .NET Standard 2.0 (widest compatibility)
- **ConsoleCommander.Tests**: xUnit tests targeting .NET 10.0
- **Sample Console** and **Simple Console**: Example applications demonstrating usage
- **Sample Console** shows full DI setup with Program.cs hosting pattern

## Key Conventions

### Command Registration
```csharp
// Numeric quick commands (referenced by number)
registerCommand(0, "Description", methodName);

// String commands (referenced by string key)
registerCommand("key", "Description", methodName);

// Spaces in IDs are replaced with underscores automatically
registerCommand(1, "My Command", action); // ID becomes "My_Command"
```

### Writing Output
- Always use extension methods from `CommanderWriteExtensions`
- Log levels map to colors: `Trace()`, `Verbose()`, `Debug()`, `Info()`, `Warning()`, `Error()`
- Results: `Success()` (green), `Failed()` (red)
- Structured output: `WriteList()`, `WriteAsTable()` with delegates for formatting
- `WriteEmptyLine()` for spacing

### User Input
- Always use extension methods from `CommanderRequestExtensions`
- `requestValue(prompt, defaultValue)`: String input with default
- `requestBool()`: Y/N input
- `requestMonth()`: 1-12 month input
- `requestItem<T>(collection, formatter, prompt, defaultIndex)`: Pick from list
- All return typed values, no string parsing needed by caller

### Commander Composition
```csharp
// In parent commander
registerCommand(0, "Child Menu", () => this.useCommander<ChildCommander>(serviceProvider));
```
- Always pass `serviceProvider` to `useCommander<T>()`
- Child commanders run in their own loop; parent resumes when child exits

### Testing Conventions
- Tests inherit from `TestBase` in ConsoleCommander.Tests
- Use `DefaultTestFixture` for setup
- xUnit v3 with `[Fact]` and `[Theory]` attributes
- Logging via `XunitLogger` and `XunitLoggerFactory`
- Test commanders can use `IServiceProvider` for DI testing

### Naming
- Commander classes: PascalCase ending in "Commander" (e.g., `MainCommander`, `SamplesCommander`)
- Command methods: camelCase
- Avoid hardcoded command strings; use `nameof(methodName)` where possible
- Use descriptive command IDs (alphanumeric or numbers)

### DI Integration
- Register commanders via `AddCommanders(provider, assembly)` extension
- Commander constructors receive `IServiceProvider` parameter
- Use `serviceProvider.GetService<T>()` to resolve dependencies in methods
- Keep commanders stateless where possible; use the DataProvider for persistent data
