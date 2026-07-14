# ⚓ Battleship MonoGame

<div align="center">

**2D Battleship game built with MonoGame and C#.**

</div>

## 📖 Overview

Battleship MonoGame is a desktop implementation of the classic Battleship game built with C#, MonoGame, and .NET. The project demonstrates modern software engineering practices, including Dependency Injection, Entity Framework Core, SQLite, modular architecture, and automated unit testing. It was developed as a portfolio project to showcase clean architecture and maintainable game development.

## ✨ Features

- 🎯 Classic Battleship gameplay
- 🎮 Interactive 2D game board
- 🚢 Ship placement system
- 🤖 AI opponent
- 💾 Persistent data storage using SQLite and Entity Framework Core
- 🧩 Dependency Injection for modular and maintainable architecture
- 🏗️ Modular scene- and state-based architecture
- 🧪 Unit-tested core game logic

## 🖥️ Screenshots


## 🛠️ Tech Stack

- **Game Framework:** MonoGame
- **Language:** C#
- **Runtime:** .NET 9
- **Database:** SQLite
- **ORM:** Entity Framework Core
- **Dependency Injection:** Microsoft.Extensions.DependencyInjection
- **Testing:** xUnit

## 🏗️ Architecture

The project follows a modular architecture inspired by common application design patterns to improve maintainability, scalability, and testability.

### Highlights

- **Dependency Injection** using `Microsoft.Extensions.DependencyInjection`
- **Entity Framework Core** with **SQLite** for persistent game data
- Automatic database migrations on application startup
- Separation of concerns through **Scenes**, **Controllers**, **Panels**, and **Services**
- State-based game flow management
- Reusable game components
- Unit-tested core game logic

## 🚀 Quick Start

Follow these steps to get a development environment up and running on your local machine.

### Prerequisites

- .NET 9 SDK

### Recommended Development Environment

- Visual Studio Code
  - C# Dev Kit
  - MonoGame extension by r88


### Installation

1. **Clone the repository**

```bash
git clone https://github.com/Ayden760/battleship-monogame.git
cd battleship-monogame
```

2. **Restore dependencies and build the solution**

```bash
dotnet restore
dotnet build
```

3. **Run the game**

```bash
dotnet run --project BattleShip
```

> **Note:** When launching the project through **Visual Studio Code** (F5), the configured pre-launch task automatically executes the unit tests before starting the game.

## 📁 Project Structure

```text
battleship-monogame/
├── .github/                 # GitHub Actions workflows
├── .vscode/                 # VS Code configuration
├── BattleShip/              # Main game project
│   ├── Content/             # Textures, fonts, sounds and other game assets
│   ├── Features/            # Feature modules (Scenes, Controllers, Panels)
│   ├── Functions/           # Shared helper and utility functions
│   ├── GameData/            # Application data, EF Core models, configuration and session management
│   ├── GameObjects/         # Core gameplay objects and entities
│   ├── Services/            # Shared application services
│   ├── BattleShip.csproj    # Main project file
│   └── Program.cs           # Dependency Injection configuration and application startup
├── BattleShip.Tests/        # Unit tests for game logic
│   └── BattleShip.Tests.csproj
├── MonoGameLibrary/         # Reusable MonoGame extensions and utilities
│   └── MonoGameLibrary.csproj
├── .gitignore
└── battleship-monogame.sln
```

### Development

When launching the project through **Visual Studio Code** (F5), the configured `preLaunchTask` automatically executes the unit test suite before starting the game.

To run the game directly without the VS Code debug configuration:

```bash
dotnet run --project BattleShip
```

## 🧪 Testing

The project includes a dedicated test suite to ensure the correctness of game logic.

To run all tests:
```bash
dotnet test BattleShip.Tests
```

## 🔄 Continuous Integration

The project uses **GitHub Actions** to automatically validate every push and pull request.

The CI pipeline performs the following steps:

- Restores all project dependencies
- Sets up the required .NET SDKs
- Installs required system dependencies for MonoGame tests
- Builds the test project
- Executes the complete unit test suite

This ensures that changes are automatically verified and helps prevent regressions before code is merged.

The `main` branch is protected by required status checks, ensuring that all automated tests pass before changes can be merged.

## 🎯 Project Goals

This project was developed to demonstrate:

- Object-oriented design
- Clean architecture
- Dependency Injection
- Entity Framework Core
- SQLite
- Unit testing
- CI with GitHub Actions

<div align="center">

Made with ❤️ by [Ayden760]

</div>
