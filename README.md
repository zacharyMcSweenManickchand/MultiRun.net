# MultiRun.net

How to run multiple asp dotnet core projects using cli?

MultiRun.net is a solution to the limitation faced when working with multiple projects within a .NET solution. The dotnet CLI falls short in such scenarios as it only allows running one project at a time. To overcome this issue, developers often resort to switching to IDEs like Rider or Visual Studio, or resort to writing their own bash/batch scripts.

However, using bash/batch scripts can lead to the loss of valuable logging information from the console. MultiRun.net provides a convenient solution that allows running multiple projects simultaneously in the same terminal window.

## Features

- Simultaneous Execution: Run multiple projects within your solution simultaneously.
- Consolidated Logging: View all logging information from multiple projects in the same terminal window.
- Ease of Use: Simplifies the process of running multiple projects without resorting to manual scripting or switching IDEs.

## Installation

To install MultiRun.net, follow these steps:
```
git clone https://github.com/yourusername/MultiRun.net.git             # Clone the repository
cd MultiRun.net                                                        # Navigate to the project directory
dotnet build -c Release && dotnet pack -c Release                      # Build the solution
dotnet tool install --global --add-source ./nupkg mrn --version 1.0.0  # Add Globally
```

## Usage

1. Open a terminal window.
2. Pass the local of your project `mrn /path/to/solution` or Navigate your project and run `mrn .` 

MultiRun.net will automatically detect and execute all projects within your solution, printing logging information for each project in the same terminal window.

## Contributing

Contributions are welcome! If you find any issues or have ideas for improvements, feel free to open an issue or submit a pull request.

## Contact

For any inquiries or feedback, please [send an email](zacharymcsweenmanickchand@gmail.com).

Thank you for using MultiRun.net!
