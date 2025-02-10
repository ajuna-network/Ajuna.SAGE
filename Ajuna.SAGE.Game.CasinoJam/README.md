# CasinoJam

CasinoJam is a blockchain-inspired casino game built on top of the Ajuna.SAGE.Game framework. Instead of relying on traditional databases, game state is managed via assets (think “cards”) and state transitions. This design enables a flexible, decentralized, and modular approach to game development with robust rule and balance management.

## Overview

The core idea of CasinoJam is to represent all game entities—such as players, machines, seats, and trackers—as **assets**. These assets carry state information in a compact byte-array (DNA) format that supports bit-level encoding. Game mechanics are implemented as state transitions that:
  
- **Create and modify assets**  
- **Update balances**  
- **Enforce game-specific rules**

Key transitions include:
- **Player/Machine Creation:** Initializes player and machine assets.
- **Deposit/Withdraw:** Handles funds for both players and machines.
- **Gamble:** Simulates a slot-machine gamble with configurable multipliers and rewards.
- **Reservation/Release/Kick:** Manages seat reservations and enforces expiration or cancellation rules.

## Key Features

- **Asset-based State Management:**  
  All game state is encapsulated in assets, each with its own unique identifier and custom data (DNA). Bit-level encoding allows multiple pieces of data to be stored compactly.

- **State Transition Engine:**  
  Transitions are registered using an `EngineBuilder`, which associates each transition with a set of rules, an optional fee, and a transition function that encapsulates game logic.

- **Flexible Rule Verification:**  
  A customizable rule verification function checks conditions such as asset counts, ownership, composite asset types, and balance thresholds before executing transitions.

- **Robust Balance Handling:**  
  The game uses dedicated managers to track asset-specific balances and overall player balances, ensuring that deposits, withdrawals, and fee transactions are performed reliably.

- **Casino-Specific Mechanics:**  
  Implements complex game behaviors such as slot-machine spins, seat reservations, and kick/release transitions. Utility functions provide reward calculations, slot result packing/unpacking, and match-type generation.

- **Comprehensive Testing:**  
  The project includes extensive tests (using NUnit) that simulate various game scenarios and edge cases—from asset creation and funding to gambling and seat management.

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download) (or higher)
- A compatible IDE (e.g., Visual Studio, VS Code, or Rider)

### Installation

Clone the repository:

```bash
git clone https://github.com/yourusername/CasinoJam.git
cd CasinoJam
```

Restore the NuGet packages:

```bash
dotnet restore
```

### Building the Project

To build the project, run:

```bash
dotnet build
```

### Running Tests

CasinoJam comes with a comprehensive test suite. To run the tests, execute:

```bash
dotnet test
```

This will run tests that cover player/machine creation, deposits, gambling, reservations, releases, kicks, and withdrawals.

## Project Structure

- **Ajuna.SAGE.Game:**  
  The underlying framework that provides the state transition engine, account/asset/balance management, and low-level utility functions.

- **CasinoJam.Model:**  
  Contains game-specific asset definitions:
  - `BaseAsset`
  - `MachineAsset` / `BanditAsset`
  - `PlayerAsset`, `HumanAsset`, and `TrackerAsset`
  - `SeatAsset`

- **CasinoJam:**  
  Contains game-specific logic:
  - **Engine Initialization:**  
    `CasinoJameGame` sets up the engine with custom rules, transitions, and a verify function.
  - **Identifiers and Rules:**  
    `CasinoJamIdentifier` and `CasinoJamRule` define unique transition identifiers and rule formats.
  - **Utilities:**  
    `CasinoJamUtil` offers functions for packing slot results, calculating rewards, and generating match types.

- **Tests:**  
  Test projects (e.g., `CasinoJamGameTests`, `CasinoJamGameKickReleaseTests`) simulate full game scenarios and validate each transition’s correctness.

## How to Contribute

Contributions are welcome! To contribute:
1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/my-new-feature`).
3. Implement your changes and add tests.
4. Submit a pull request with a detailed description of your changes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

For questions or suggestions, please open an issue in the GitHub repository or contact [cedric@ajuna.io](mailto:cedric@ajuna.io).

---

Enjoy exploring the power of asset-based state transitions with CasinoJam!

---

