# SAGE Game Framework

The SAGE Game Framework is a modular engine designed for game state management using an asset-based, state-transition model. Instead of using a traditional database, SAGE stores state directly on "assets" (e.g., game cards), and state changes are performed by executing transitions that consume and produce assets. This architecture allows for a flexible, decentralized, and extensible game design.

## Core Concepts

- **Account:**  
  Represents a player or system account. Each account is identified by a unique ID and holds a balance.

- **Asset:**  
  The fundamental game object. Assets have an owner (`OwnerId`), a collection identifier, a score, a genesis timestamp, and a data array (often referred to as DNA) that encodes various properties using compact, bit-level operations.

- **Balance:**  
  Tracks funds for both accounts and assets. Asset-specific balances are managed separately from overall account balances.

- **Transition:**  
  A state change operation that is defined by:
  - A unique **Identifier** (encapsulated by an `ITransitionIdentifier`)
  - A set of **Rules** (implementing `ITransitionRule`)
  - An optional **Fee**
  - A **Transition Function** that computes new assets (the updated state) from the provided inputs

- **Engine:**  
  The central component that manages accounts, assets, balances, and transitions. It relies on external providers (like a blockchain info provider) for block numbers and randomness.

## Architecture Overview

The following Entity-Relationship (ER) diagram shows the relationships between the key entities:

![image](https://github.com/user-attachments/assets/1fc850c2-bdc7-4674-9030-02c316b0b14c)

```plantuml
@startuml ER Diagram
entity "Account" as Account {
  * Id : uint
  * Balance : uint
}

entity "Asset" as Asset {
  * Id : uint
  * OwnerId : uint
  * CollectionId : byte
  * Score : uint
  * Genesis : uint
  * Data : byte[]
}

entity "Balance" as Balance {
  * AssetId : uint
  * BalanceValue : uint
}

entity "Transition" as Transition {
  * Identifier : ITransitionIdentifier
  * Rules : ITransitionRule[]
  * Fee : ITransitioFee?
}

entity "Engine" as Engine {
  * BlockchainInfo : IBlockchainInfoProvider
}

Account ||--o{ Asset : "owns"
Asset ||--|{ Balance : "has"
Engine o-- Account : "manages"
Engine o-- Asset : "manages"
Engine o-- Transition : "registers"
@enduml
```

## Transition Execution Flow

The following UML activity diagram illustrates the high-level flow of a state transition within the engine:

![image](https://github.com/user-attachments/assets/5827a96b-1ef5-49a0-87ba-fb766550aef8)

```plantuml
@startuml Activity Diagram for Transition Execution
start

:Account initiates transition;
:Call Engine.Transition(identifier, assets);

if (assets contain duplicates or lockable assets?) then (yes)
  :Abort transition;
  stop
else (no)
  :Verify rules via VerifyFunction;
  if (rules NOT satisfied?) then (yes)
    :Abort transition;
    stop
  else (no)
    :Check fee requirements;
    if (insufficient balance for fee?) then (yes)
      :Abort transition;
      stop
    else (no)
      :Withdraw fee from account balance;
      :Execute transition function;
      :Obtain output assets;
      :Apply state changes (create, update, delete assets);
      :Update balances accordingly;
    endif
  endif
endif

:Return success with output assets;
stop
@enduml
```

## Summary

By separating accounts, assets, balances, and transitions, the SAGE Game Framework provides a robust foundation for developing complex, asset-based games. Its modular design and use of state transitions make it highly adaptable for various game genres and mechanics.
