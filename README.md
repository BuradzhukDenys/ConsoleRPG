# ConsoleRPG
A fully object-oriented console role-playing game built with C# and .NET 8.
This project was developed to practice and demonstrate knowledge of architectural design patterns, object-oriented programming and memory management.

## ScreenShots

| Map Exploration | Combat System |
| :---: | :---: |
| <img width="356" height="354" alt="Screenshot 2026-09-22 124241" src="https://github.com/user-attachments/assets/17084161-cba8-4766-91d4-44211e97085c" /> | <img width="558" height="410" alt="Screenshot 2026-09-22 124306" src="https://github.com/user-attachments/assets/5365abf6-82d5-4eaa-a966-2c3653c7b024" /> |
| Shop | Shop after purchasing the item |
|<img width="306" height="269" alt="Screenshot 2026-09-22 124326" src="https://github.com/user-attachments/assets/055c794a-a8a4-4f80-8c98-8478ccdf7425" /> | <img width="348" height="245" alt="Screenshot 2026-09-22 124335" src="https://github.com/user-attachments/assets/3aa26299-3e40-4935-9fe9-33042ed402e9" /> |
| Player inventory page 1 | Player inventory page 2 |
| <img width="578" height="411" alt="Screenshot 2026-09-22 124418" src="https://github.com/user-attachments/assets/eeedf735-aca8-4247-8ac8-a739dee06bf0" /> | <img width="567" height="295" alt="Screenshot 2026-09-22 124355" src="https://github.com/user-attachments/assets/611098e8-b220-484a-a629-327307b597c3" /> |
<h4 align="center">Player stats</h4>
<p align="center">
  <img width="308" height="151" alt="Screenshot 2026-09-22 124424" src="https://github.com/user-attachments/assets/550b5e1a-2b18-4346-909f-af2ad055c23d" />
</p>

## Key features

* **Turn-based Combat:** Strategic battles featuring melee/ranged weapons, damage modifiers, and status effects (e.g., Burn effect with probability scaling).
* **Dynamic Inventory & Shop:** Advanced item management with stacking, equipping, and paginated UI navigation.
* **Map Exploration:** Grid-based map movement when you can find enemies and shops.
* **Save/Load System:** Full game state persistence using `System.Text.Json` serialization.

## Architecture & Design Patterns

* **Generic Factory with Reflection:** Implemented a `UniversalFactory<T>` that dynamically discovers and instantiates subclasses (enemies, items, locations) at runtime. This completely eliminates hardcoded `switch-case` statements and makes the game highly extensible.
* **State Machine:** The game loop is governed by strict `GameState` and `BattleState` enumerations, separating UI rendering from background game logic.
* **Event-Driven Design:** `Location` classes communicate with the main game engine exclusively through C# `Action` delegates and events (e.g., `StartBattle`, `LocationCompleted`), ensuring loose coupling.
* **Prototype Pattern:** Utilized `.Clone()` (`MemberwiseClone`) in the shop system to prevent reference mutation bugs when purchasing items (preventing shared reference modifications).
* **Custom Pagination:** Built a reusable `Paginator<T>` class using LINQ (`.Skip()`, `.Take()`) to handle the UI rendering of multi-page lists like the inventory and shop.
* **Interface Segregation (SOLID):** Game items implement specific, lightweight interfaces (`IEquipable`, `IUsable`, `IEffectProvider`, `IHasAmmo`) instead of inheriting bloated base class methods.

## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/BuradzhukDenys/ConsoleRPG.git
   cd ConsoleRPG
   dotnet run
   ```
