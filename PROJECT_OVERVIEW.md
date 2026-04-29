# v3DesignPatterns — Project Source of Truth

A C# .NET 10 console application that demonstrates eleven classic **Gang of Four design patterns** plus a CQRS-over-Mediator variation and a bonus **Reflection** demo. The app runs as an interactive menu: you pick a pattern, its demo prints to the console, and you press a key to go back to the menu.

This document walks through every file, how the files connect, and what each piece of code does — end to end.

---

## 1. High-Level Purpose

This project is a learning sandbox. Each pattern is implemented in its own folder, isolated from the others, with:

- One or more **"implementation" files** that hold the pattern's moving parts (interfaces, concrete classes, products, states, etc.).
- One **`*Demo.cs` file** that exercises the pattern and prints results to the console.

Every demo implements the same tiny contract (`IPatternDemo`), which lets `Program.cs` display a menu and run any pattern without knowing what's inside it. That polymorphism over demos is itself a mini-lesson in why you'd reach for an interface in the first place.

The project targets **.NET 10** with **nullable reference types enabled** and **implicit usings on**, so the code is modern C# with minimal ceremony.

---

## 2. Repository Layout

```
v3DesignPatterns/
├── .gitignore                         # Build artefacts, Visual Studio, OS files
├── LearningNotes.docx                 # External notes (Word doc, not code)
├── PROJECT_OVERVIEW.md                # ← this file
└── DesignPatterns/                    # The actual .NET project
    ├── DesignPatterns.csproj          # Project file (net10.0, Exe)
    ├── DesignPatterns.slnx            # New-style solution file
    ├── IPatternDemo.cs                # Common contract every demo implements
    ├── Program.cs                     # Menu loop / entry point
    ├── Patterns/
    │   ├── AbstractFactory/
    │   │   ├── AbstractFactoryDemo.cs
    │   │   ├── LoginScreen.cs         # The client
    │   │   └── Widgets.cs             # Products + factories
    │   ├── Builder/
    │   │   ├── BuilderDemo.cs
    │   │   ├── EmailBuilder.cs        # Builder + interface
    │   │   ├── EmailDirector.cs       # Named construction recipes
    │   │   └── EmailMessage.cs        # The immutable product
    │   ├── Facade/
    │   │   ├── CheckoutServices.cs    # Four subsystem services
    │   │   ├── FacadeDemo.cs
    │   │   └── OrderCheckoutFacade.cs # The facade
    │   ├── Mediator/
    │   │   ├── ChatRoom.cs            # Concrete mediator (broadcast variant)
    │   │   ├── Cqrs.cs                # CQRS-flavoured mediator + commands/queries
    │   │   ├── CqrsDemo.cs
    │   │   ├── MediatorDemo.cs
    │   │   └── Users.cs               # Abstract + concrete colleague
    │   ├── Singleton/
    │   │   ├── SingletonDemo.cs
    │   │   └── Singletons.cs          # V1 naive, V2 DCL, V3 Lazy<T>
    │   ├── State/
    │   │   ├── Order.cs               # Context
    │   │   ├── OrderStates.cs         # State interface + four states
    │   │   └── StateDemo.cs
    │   ├── Strategy/
    │   │   ├── Checkout.cs            # Context
    │   │   ├── DiscountStrategies.cs  # Strategy interface + four strategies
    │   │   ├── StrategyDemo.cs
    │   │   └── StrategyModels.cs      # Cart, Customer, CheckoutResult, LoyaltyTier
    │   ├── Adapter/
    │   │   ├── AdapterDemo.cs
    │   │   └── PaymentAdapter.cs      # IPaymentProcessor + LegacyStripeClient + adapter
    │   ├── Decorator/
    │   │   ├── DecoratorDemo.cs
    │   │   └── Notifiers.cs           # INotifier + base decorator + three concrete decorators
    │   ├── Proxy/
    │   │   ├── DocumentService.cs     # IDocumentService + RealDocumentService
    │   │   ├── Proxies.cs             # Lazy / Protection / Caching proxies
    │   │   └── ProxyDemo.cs
    │   └── ChainOfResponsibility/
    │       ├── Approvers.cs           # Abstract Approver + Manager / Director / CFO
    │       └── ChainOfResponsibilityDemo.cs
    └── Reflection/
        ├── Calculator.cs              # Plain class (the target)
        ├── ReflectionCalculator.cs    # Inspect-and-invoke wrapper
        └── ReflectionDemo.cs
```

`bin/` and `obj/` are git-ignored and only exist after a build.

---

## 3. Project Configuration

### `DesignPatterns.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <Folder Include="Patterns\AbstractFactory\" />
    <Folder Include="Patterns\Mediator\" />
    <Folder Include="Patterns\Strategy\" />
    <Folder Include="Patterns\Builder\" />
    <Folder Include="Patterns\Facade\" />
    <Folder Include="Patterns\State\" />
    <Folder Include="Patterns\Singleton\" />
  </ItemGroup>
</Project>
```

Key points:
- **`OutputType=Exe`** — produces a runnable console app.
- **`TargetFramework=net10.0`** — the latest .NET (requires the net10 SDK installed).
- **`ImplicitUsings=enable`** — removes the need to write `using System;`, `using System.Collections.Generic;`, etc.
- **`Nullable=enable`** — nullable reference types are on. You see this in `private static AppLogger? _instance;` and the `null`-returning `FailureReason`.
- The `<Folder Include=… />` entries just make sure these pattern folders exist in the solution view even if they were empty (which they aren't).

### `DesignPatterns.slnx`

```xml
<Solution>
  <Project Path="DesignPatterns.csproj" />
</Solution>
```

The new XML-based solution format. Just points to the single `.csproj`.

### `.gitignore`

Standard .NET ignores: `bin/`, `obj/`, `.vs/`, user settings, NuGet packages, OS thumbnails.

---

## 4. The Core Abstraction — `IPatternDemo`

[IPatternDemo.cs](DesignPatterns/IPatternDemo.cs)

```csharp
namespace DesignPatterns;

internal interface IPatternDemo
{
    string Name { get; }
    void Run();
}
```

Every pattern demo implements this interface. That's the glue. It has exactly two members:

- **`Name`** — a short human-readable label (e.g. `"Singleton"`), used to build the menu.
- **`Run()`** — runs the demo and writes to the console.

Because `Program.cs` works against `IPatternDemo`, adding a new pattern is just: create a new class that implements `IPatternDemo`, then add one line to the `demos` list.

---

## 5. The Entry Point — `Program.cs`

[Program.cs](DesignPatterns/Program.cs)

Uses **top-level statements** (no explicit `Main` method). Does three things:

1. **Build a list of demos**:
   ```csharp
   var demos = new List<IPatternDemo>
   {
       new SingletonDemo(),
       new BuilderDemo(),
       new AbstractFactoryDemo(),
       new FacadeDemo(),
       new StrategyDemo(),
       new StateDemo(),
       new MediatorDemo(),
       new ReflectionDemo(),
   };
   ```

2. **Render a numbered menu in a loop** — clears the console, prints `1. Singleton`, `2. Builder`, etc., plus `0. Exit`.

3. **Dispatch** — reads the user's number, picks the matching demo out of the list, prints a header, calls `demo.Run()`, and waits for a keypress before looping back.

The loop exits when the user types `0`. Invalid input (non-numeric or out-of-range) silently loops back to the menu.

**Why this design matters:** `Program.cs` never names a specific demo inside the `while` loop. It just iterates `List<IPatternDemo>`. This is the Open/Closed Principle in miniature — the menu is open to extension (new demos) but closed to modification (the loop code never changes).

---

## 6. The Patterns — One by One

Each section below follows the same shape: **problem → players → flow → what's interesting**. Line numbers refer to the listed file.

### 6.1 Singleton — `Patterns/Singleton/`

**Problem:** Guarantee one and only one instance of a class exists process-wide, and provide a global point of access to it.

**Files:**
- [Singletons.cs](DesignPatterns/Patterns/Singleton/Singletons.cs) — three flavours of singleton.
- [SingletonDemo.cs](DesignPatterns/Patterns/Singleton/SingletonDemo.cs) — exercises all three.

**Players:**

| Class | Flavour | Thread safety |
|---|---|---|
| `AppLogger` | V1 — naive lazy init | Unsafe (race in `Instance` getter) |
| `AppLoggerThreadSafe` | V2 — double-checked lock | Safe, faster than naive locking |
| `AppLoggerLazy` | V3 — `Lazy<T>` | Safe, idiomatic modern C# |

Each class:
- Has a **private constructor** so callers can't `new` it.
- Exposes a **public static `Instance`** property.
- Holds an **`InstanceId`** (a `Guid`) so the demo can prove two calls return the same object (by comparing the first 8 chars of the Guid in log output).

**Flow inside `SingletonDemo.Run()`:**
- Gets `Instance` twice for each flavour.
- Calls `Log("from caller A")` and `Log("from caller B")`.
- Prints `ReferenceEquals(a, b)` — always `true`, which is the point.

**What's interesting:**
- **V1** has the classic race bug: two threads can both see `_instance == null` and each create an instance.
- **V2** uses double-checked locking — outer check for fast path, `lock` + inner check for correctness. The inner re-check is the subtle part: while one thread waits on the lock, another may already have created the instance.
- **V3** uses `Lazy<T>`. The framework handles thread-safety (default `ExecutionAndPublication` mode), and it's also lazy — the instance isn't created until `.Value` is first read.

### 6.2 Builder — `Patterns/Builder/`

**Problem:** Construct a complex object step by step. Separate the construction code from the final product, and support multiple construction recipes.

**Files:**
- [EmailMessage.cs](DesignPatterns/Patterns/Builder/EmailMessage.cs) — the **product** (immutable).
- [EmailBuilder.cs](DesignPatterns/Patterns/Builder/EmailBuilder.cs) — the **builder** and its interface.
- [EmailDirector.cs](DesignPatterns/Patterns/Builder/EmailDirector.cs) — a **director** with pre-canned recipes.
- [BuilderDemo.cs](DesignPatterns/Patterns/Builder/BuilderDemo.cs) — exercises all three.

**Players:**

- **`EmailMessage`** — the product. All properties are `get`-only. The constructor is `internal` so only code in the same assembly (i.e. the builder) can construct one. Its `ToString()` formats the whole email on one line for demo output.
- **`IEmailBuilder`** — the contract the director depends on (not the concrete builder).
- **`EmailBuilder`** — fluent builder. Each setter method (`To`, `Subject`, `Body`, `Cc`) stashes the value in a private field and returns `this`, which is what enables chaining. `Build()` validates that `To`, `Subject`, and `Body` are set and **snapshots the `_cc` list** (via `.ToList()`) so later mutations to the builder don't leak into the built product.
- **`EmailDirector`** — holds named construction recipes. `BuildWelcomeEmail(builder, email, name)` takes any `IEmailBuilder` and composes a welcome email. The director is useful when the same email shape is built from multiple places.

**Flow inside `BuilderDemo.Run()`:**
1. **Fluent**: chains `.To().Subject().Body().Cc().Build()` and prints the result.
2. **Director**: asks the director to build a welcome email, passing a fresh `EmailBuilder`.
3. **Validation**: calls `Build()` without a `Body` and catches the `InvalidOperationException`.

**What's interesting:**
- The product is immutable by design — the only way to get one is through the builder.
- `IEmailBuilder` decouples the director from the concrete builder. Swap in a different builder (e.g., one that logs everything) and the director doesn't change.

### 6.3 Abstract Factory — `Patterns/AbstractFactory/`

**Problem:** Create **families** of related objects (e.g., all the widgets that make up a UI theme) without binding the client to any specific concrete class.

**Files:**
- [Widgets.cs](DesignPatterns/Patterns/AbstractFactory/Widgets.cs) — abstract products, concrete products (Light/Dark), abstract factory, concrete factories.
- [LoginScreen.cs](DesignPatterns/Patterns/AbstractFactory/LoginScreen.cs) — the **client** that depends only on `IUiFactory`.
- [AbstractFactoryDemo.cs](DesignPatterns/Patterns/AbstractFactory/AbstractFactoryDemo.cs) — renders the login screen with each factory.

**Players:**

| Role | Types |
|---|---|
| Abstract products | `IButton`, `ITextBox` |
| Light family | `LightButton`, `LightTextBox` |
| Dark family | `DarkButton`, `DarkTextBox` |
| Abstract factory | `IUiFactory` with `CreateButton()` / `CreateTextBox()` |
| Concrete factories | `LightThemeFactory`, `DarkThemeFactory` |
| Client | `LoginScreen` (constructor takes `IUiFactory`) |

**Flow inside `AbstractFactoryDemo.Run()`:**
- Builds one `LoginScreen` per factory and calls `.Render()`. Same client code, two completely different outputs (lowercase/padded vs. uppercase/bordered).

**What's interesting:**
- `LoginScreen.Render()` calls `_factory.CreateTextBox()` twice and `_factory.CreateButton()` once. Nothing inside `LoginScreen` knows about Light vs. Dark — the factory guarantees consistency inside a family.
- Contrast with plain Factory Method: Abstract Factory creates **multiple** related products, not just one.

### 6.4 Facade — `Patterns/Facade/`

**Problem:** Wrap a cluster of subsystems behind one simple interface. Clients call **one method** instead of orchestrating four calls plus rollback logic.

**Files:**
- [CheckoutServices.cs](DesignPatterns/Patterns/Facade/CheckoutServices.cs) — four subsystem services, each with its own interface and concrete class, plus three `record` types for data (`Customer`, `Item`, `OrderResult`).
- [OrderCheckoutFacade.cs](DesignPatterns/Patterns/Facade/OrderCheckoutFacade.cs) — the facade.
- [FacadeDemo.cs](DesignPatterns/Patterns/Facade/FacadeDemo.cs) — runs a happy path and a rollback path.

**Subsystems:**

| Service | Interface | Responsibility |
|---|---|---|
| `InventoryService` | `IInventoryService` | `Reserve(item)` returns a reservation id; `Release(id)` backs it out. |
| `PaymentService` | `IPaymentService` | `Charge(customer, amount)` returns `bool`. Constructor accepts `simulateFailure` so the demo can force a decline. |
| `ShippingService` | `IShippingService` | `CreateShipment(customer, item)` returns a tracking number. |
| `NotificationService` | `INotificationService` | `Notify(customer, tracking)` — emails the customer. |

**The facade (`OrderCheckoutFacade.PlaceOrder`) choreography:**
1. `Reserve` inventory.
2. `Charge` payment. If it fails, **`Release` the reservation** and return a failure `OrderResult`.
3. `CreateShipment`.
4. `Notify` the customer.
5. Return success `OrderResult` with the tracking number.

**Flow inside `FacadeDemo.Run()`:**
- Builds a facade with `simulateFailure=false` and calls `PlaceOrder` — prints the happy path.
- Builds a facade with `simulateFailure=true` and calls `PlaceOrder` — prints the rollback (inventory released, no shipment created).

**What's interesting:**
- The rollback ordering is the non-obvious part. A naive caller writing this themselves would likely forget to release the reservation on a payment failure.
- The facade depends on **interfaces**, so you can swap real services in later without touching this class.

### 6.5 Strategy — `Patterns/Strategy/`

**Problem:** You have a family of interchangeable algorithms (discount calculators) and you want to pick one at runtime without polluting the calling code with `if`/`switch` statements.

**Files:**
- [StrategyModels.cs](DesignPatterns/Patterns/Strategy/StrategyModels.cs) — supporting types: `LoyaltyTier` enum, `Customer`, `CartItem`, `Cart`, `CheckoutResult` records.
- [DiscountStrategies.cs](DesignPatterns/Patterns/Strategy/DiscountStrategies.cs) — the strategy interface and four implementations.
- [Checkout.cs](DesignPatterns/Patterns/Strategy/Checkout.cs) — the **context** that holds a strategy.
- [StrategyDemo.cs](DesignPatterns/Patterns/Strategy/StrategyDemo.cs) — runs every strategy plus a bonus `Func<>`-as-strategy example.

**Strategies:**

| Class | Rule |
|---|---|
| `NoDiscountStrategy` | Always zero (also doubles as a Null Object). |
| `PercentageDiscountStrategy` | Flat % off, percent is constructor-injected. |
| `BulkDiscountStrategy` | 15% off only when `Subtotal > $100`. |
| `LoyaltyDiscountStrategy` | Percent driven by `customer.Tier` (Silver 5%, Gold 10%, Platinum 20%). |

**Flow inside `StrategyDemo.Run()`:**
1. Builds a `Cart` and a `Gold`-tier `Customer`.
2. Loops every strategy, wraps each in a fresh `Checkout`, and prints subtotal / discount / total.
3. Demonstrates that **a `Func<Cart, Customer, decimal>` is a legitimate strategy too** — sometimes you don't need a whole interface.

**What's interesting:**
- The `Checkout` context has zero conditionals on strategy type. Adding a new strategy (say, `SeasonalDiscountStrategy`) doesn't touch `Checkout` at all. That's the Open/Closed payoff.
- `NoDiscountStrategy` as a Null Object means callers never need `if (strategy != null)`.
- The interface method takes both `cart` and `customer` so loyalty-style strategies can use the customer's tier.

### 6.6 State — `Patterns/State/`

**Problem:** An object's behaviour depends on its current state, and the usual `switch (status)` sprawl makes transitions hard to reason about. Replace the switch with a state machine where each state is its own class.

**Files:**
- [OrderStates.cs](DesignPatterns/Patterns/State/OrderStates.cs) — the `IOrderState` interface and four concrete states.
- [Order.cs](DesignPatterns/Patterns/State/Order.cs) — the **context** that holds the current state.
- [StateDemo.cs](DesignPatterns/Patterns/State/StateDemo.cs) — runs the happy path and an illegal transition.

**States:**

| State | `Pay()` | `Ship()` | `Deliver()` |
|---|---|---|---|
| `PendingPaymentState` | → `PaidState` | throws | throws |
| `PaidState` | throws | → `ShippedState` | throws |
| `ShippedState` | throws | throws | → `DeliveredState` |
| `DeliveredState` | throws | throws | throws (terminal) |

**Context (`Order`):**
- Starts in `PendingPaymentState`.
- `Pay/Ship/Deliver` each forward the call to `_state.Pay(this)` / `.Ship(this)` / `.Deliver(this)`.
- States transition the order by calling `order.SetState(newState)`, which also prints the transition line.

**Flow inside `StateDemo.Run()`:**
1. Creates `ORD-01`, then calls `Pay() → Ship() → Deliver()` in sequence. Prints transitions at each step.
2. Creates `ORD-02` and tries to `Ship()` before paying. Catches the `InvalidOperationException` and confirms the state didn't change.

**What's interesting:**
- No `switch` statement anywhere. The allowed/disallowed transitions are encoded in the state classes themselves.
- This is what a domain event sourcing flow looks like in miniature — states are first-class.

### 6.7 Mediator — `Patterns/Mediator/`

**Problem:** A set of objects would normally reference each other directly. As you add objects, the reference graph explodes (n² edges). Route all interactions through a mediator so colleagues only know about the mediator.

**Files:**
- [Users.cs](DesignPatterns/Patterns/Mediator/Users.cs) — abstract `User` colleague and a concrete `StandardUser`.
- [ChatRoom.cs](DesignPatterns/Patterns/Mediator/ChatRoom.cs) — the `IChatRoomMediator` interface and the `ChatRoom` concrete mediator.
- [MediatorDemo.cs](DesignPatterns/Patterns/Mediator/MediatorDemo.cs) — three users in a chat room.

**Players:**
- **`User`** — abstract colleague. Holds only `Mediator` and `Name`. `Send(message)` forwards to the mediator; `Receive(message, from)` is abstract.
- **`StandardUser`** — concrete colleague. Prints `[Name] got "message" from other`.
- **`IChatRoomMediator`** — contract with `Register(user)` and `SendMessage(message, from)`.
- **`ChatRoom`** — holds the user list, broadcasts incoming messages to everyone except the sender.

**Flow inside `MediatorDemo.Run()`:**
1. Creates a `ChatRoom` and three `StandardUser`s (Alice, Bob, Carol).
2. Registers all three.
3. Alice sends a message — everyone else prints a `got "..."` line.
4. Bob replies — same thing.

**What's interesting:**
- **No user holds a reference to another user.** The only reference any user has is to the mediator. This keeps the graph flat and replaceable.
- Swap in a `ModeratedChatRoom : IChatRoomMediator` that filters profanity and the user classes don't change.

### 6.8 Mediator (CQRS variant) — `Patterns/Mediator/`

**Problem:** The chat-room mediator broadcasts a string to many recipients. CQRS-flavoured mediators are different: messages are **strongly typed**, each message has **exactly one** handler, and the handler may return a result. This is what libraries like **MediatR** model and what most "send a command, get a result" pipelines look like in production code.

**Files:**
- [Cqrs.cs](DesignPatterns/Patterns/Mediator/Cqrs.cs) — message marker interfaces, handler interfaces, the `CqrsMediator` itself, a tiny `UserStore`, and one command + two queries with their handlers.
- [CqrsDemo.cs](DesignPatterns/Patterns/Mediator/CqrsDemo.cs) — registers handlers, sends commands and queries through the mediator.

**Players:**

| Role | Type | Role description |
|---|---|---|
| Marker — read | `IQuery<TResult>` | Returns data, doesn't change state. |
| Marker — write | `ICommand` | Changes state, returns nothing. |
| Handler interfaces | `IQueryHandler<TQuery, TResult>`, `ICommandHandler<TCommand>` | Exactly one method: `Handle`. |
| Mediator | `ICqrsMediator` / `CqrsMediator` | Holds a `Dictionary<Type, object>` keyed by message type. `Send` looks up and casts. |
| Domain | `UserStore` | Trivial in-memory dictionary the handlers share. |
| Command | `CreateUserCommand` + `CreateUserHandler` | Adds a user, prints the new id. |
| Queries | `GetUserByIdQuery` + handler, `CountUsersQuery` + handler | Read by id; count all. |

**Flow inside `CqrsDemo.Run()`:**
1. Build a `UserStore` and a `CqrsMediator`.
2. Register one handler per message type.
3. Send two commands (`CreateUser` x2).
4. Send two queries (`GetUserById(1)` hit, `GetUserById(99)` miss, `CountUsers`).
5. Print the result the caller saw.

**What's interesting:**
- **Same pattern, different flavour.** The chat room is a *broadcast* mediator (one sender → many recipients). The CQRS mediator is a *dispatch* mediator (one sender → exactly one handler). Both match GoF's definition because the pattern is "all participants depend on the hub, not on each other".
- **Caller has zero direct references to handlers.** Add a new query type and one handler, register it, done — no other class changes.
- **Pipeline behaviours = Chain of Responsibility on top.** Real-world MediatR adds logging/validation/auth as decorators around the dispatch — patterns compose.
- **Reflection ties in.** A real DI-backed mediator typically uses reflection to discover and register every `IRequestHandler<,>` automatically (see the Reflection demo at the end of the project).

---

## 6b. New Structural & Behavioural Patterns

### 6.9 Adapter — `Patterns/Adapter/`

**Problem:** A third-party library exposes an API in shape *X* but every caller in your codebase already speaks shape *Y* (`IPaymentProcessor`). You can't change the library and you don't want to rewrite every caller.

**Files:**
- [PaymentAdapter.cs](DesignPatterns/Patterns/Adapter/PaymentAdapter.cs) — `IPaymentProcessor`, the native `ModernPaymentProcessor`, the third-party-shaped `LegacyStripeClient`, and the `LegacyStripeAdapter`.
- [AdapterDemo.cs](DesignPatterns/Patterns/Adapter/AdapterDemo.cs).

**Players:**

| Role | Type | Notes |
|---|---|---|
| Target (what callers use) | `IPaymentProcessor` | `Charge(string customerEmail, decimal amount) → bool` |
| Native impl | `ModernPaymentProcessor` | Implements the target directly. |
| Adaptee (third-party) | `LegacyStripeClient` | `ExecutePayment(int cents, string currency, string reference) → string` |
| Adapter | `LegacyStripeAdapter` | Implements `IPaymentProcessor`, holds a `LegacyStripeClient`, translates `Charge` → `ExecutePayment`. |

**Flow inside `AdapterDemo.Run()`:**
- Calls `Charge(...)` on the native processor — prints `[Modern] charged ...`.
- Calls `Charge(...)` on the adapter — internally calls `LegacyStripeClient.ExecutePayment(...)` (dollars → cents, "USD", reference) — prints `[Legacy] executed ...`.
- Same caller code; the adapter hides the API mismatch.

**What's interesting:**
- The adapter's *only* job is translation. No business logic, no orchestration.
- Contrast with **Facade**: a Facade composes a *cluster* of subsystems into one simple call (`OrderCheckoutFacade`). An Adapter translates *one* mismatched API into the shape your code expects. Both wrap, but for different reasons.

### 6.10 Decorator — `Patterns/Decorator/`

**Problem:** You want to add behaviour (logging, retry, timestamping) to a service without modifying its class. Each piece of behaviour should be optional and stackable.

**Files:**
- [Notifiers.cs](DesignPatterns/Patterns/Decorator/Notifiers.cs) — `INotifier`, the real `EmailNotifier`, the abstract `NotifierDecorator`, and three concrete decorators.
- [DecoratorDemo.cs](DesignPatterns/Patterns/Decorator/DecoratorDemo.cs).

**Players:**

| Role | Type |
|---|---|
| Component | `INotifier` |
| Concrete component | `EmailNotifier` |
| Base decorator | `NotifierDecorator` (abstract — holds `Inner`, default-forwards `Send`) |
| Concrete decorators | `TimestampDecorator`, `LoggingDecorator`, `RetryDecorator` |

**Flow inside `DecoratorDemo.Run()`:**
1. Plain `EmailNotifier.Send("hello")`.
2. `new TimestampDecorator(new EmailNotifier()).Send("hello")` — prefixes a `[HH:mm:ss]` stamp.
3. Stacked: `Logging(Retry(Timestamp(Email)))` — outer-to-inner. Logging prints before/after, Retry runs the inner Send `attempts` times, each retry stamps a fresh timestamp before Email prints.

**What's interesting:**
- **Decorators stack** because every decorator implements the same `INotifier` interface as the thing it wraps.
- **Open/Closed payoff** — `EmailNotifier` is closed for modification, but adding `EncryptionDecorator` requires zero changes to it.
- Contrast with **Proxy**: a Proxy controls *whether/when* the inner call runs (auth, lazy load). A Decorator *always* calls the inner and adds something around it. Both share the wrapped interface, the difference is intent.

### 6.11 Proxy — `Patterns/Proxy/`

**Problem:** You want a stand-in for an expensive or sensitive object. The stand-in shares the real object's interface so clients can't tell them apart, but it controls access — lazy creation, authorisation, caching, remote dispatch, etc.

**Files:**
- [DocumentService.cs](DesignPatterns/Patterns/Proxy/DocumentService.cs) — `IDocumentService`, the real `RealDocumentService`.
- [Proxies.cs](DesignPatterns/Patterns/Proxy/Proxies.cs) — three flavours of proxy.
- [ProxyDemo.cs](DesignPatterns/Patterns/Proxy/ProxyDemo.cs).

**Players:**

| Proxy | Flavour | Behaviour |
|---|---|---|
| `LazyDocumentProxy` | Virtual | Real subject is `null` until first call; `??=` constructs on demand. |
| `ProtectionDocumentProxy` | Protection | Throws `UnauthorizedAccessException` if a non-admin requests a `classified-*` document; otherwise delegates. |
| `CachingDocumentProxy` | Caching | Stores results in a `Dictionary<string,string>`; returns cached value on repeat reads of the same id. |

**Flow inside `ProxyDemo.Run()`:**
1. Virtual proxy is constructed (no real subject yet); first `GetDocument` triggers `[Real] constructed`.
2. Protection proxy allows `alice` (admin) to read `classified-x`, blocks `bob` (caught and printed).
3. Caching proxy: first call is `MISS` and reaches `RealDocumentService`; second call for the same id is a `HIT` and never delegates.

**What's interesting:**
- Same `IDocumentService` everywhere — the client cannot distinguish a real subject from a proxy.
- All three flavours can be **stacked** (cache(protection(real))) because they share the interface, identical to Decorator's stacking trick.
- **Proxy vs Decorator** is the most asked-about distinction. Memorise: "Proxy controls access, Decorator adds behaviour. Proxy may skip the inner call; Decorator always runs it."

### 6.12 Chain of Responsibility — `Patterns/ChainOfResponsibility/`

**Problem:** You have a request that *one* of several handlers might process, and you don't want the caller to know which. Each handler decides: handle it, or pass to the next.

**Files:**
- [Approvers.cs](DesignPatterns/Patterns/ChainOfResponsibility/Approvers.cs) — `ExpenseRequest` record, abstract `Approver` (holds `_next`), `ManagerApprover` ($1k limit), `DirectorApprover` ($10k limit), `CfoApprover` ($100k limit).
- [ChainOfResponsibilityDemo.cs](DesignPatterns/Patterns/ChainOfResponsibility/ChainOfResponsibilityDemo.cs).

**Flow inside `ChainOfResponsibilityDemo.Run()`:**
1. Build the chain — `manager.SetNext(director).SetNext(cfo)` (returns the new tail so calls chain fluently).
2. Submit four `ExpenseRequest`s through `manager.Handle(...)`:
   - $250 → Manager approves.
   - $5,000 → Manager escalates → Director approves.
   - $75,000 → Manager → Director → CFO approves.
   - $250,000 → all escalate → end of chain → `[REJECTED]`.

**What's interesting:**
- **No handler knows about any other handler's rules** — only its own limit and a `_next` pointer.
- The chain itself is the abstraction; you add a new approval tier (`VPApprover`) by inserting a node between Director and CFO. No existing handler changes.
- **CoR vs State:** State *replaces itself* (`Pending → Paid`) when an action happens. CoR *forwards through* without anyone replacing anyone — the chain is fixed for the lifetime of the request. The underlying mechanism (a polymorphic next-thing pointer) is similar; the semantics are not.
- **Where you've already seen this:** ASP.NET Core middleware (`app.Use(...)`) is CoR — each middleware decides whether to call `next()` or short-circuit. The MediatR pipeline-behaviours model is CoR layered over the CQRS Mediator.

---

## 7. The Bonus — Reflection Demo

[DesignPatterns/Reflection/](DesignPatterns/Reflection/)

Not a GoF pattern. Included to show the core primitive that powers DI containers, serialisers, test runners, and plugin systems.

**Files:**
- [Calculator.cs](DesignPatterns/Reflection/Calculator.cs) — a plain class with `Add`, `Subtract`, `Multiply`, `Divide`, `Power`. No base class, no dispatcher. (There's also a commented-out `Sqrt` method — uncomment it to see how the discovery list grows at runtime.)
- [ReflectionCalculator.cs](DesignPatterns/Reflection/ReflectionCalculator.cs) — uses `System.Reflection` to inspect the `Calculator` and call methods by name.
- [ReflectionDemo.cs](DesignPatterns/Reflection/ReflectionDemo.cs) — discovery + invocation + error case + DI-container analogy.

**How `ReflectionCalculator` works:**
- Holds a private `Calculator` target.
- **`ListOperations()`** — calls `GetMethods(Public | Instance | DeclaredOnly)` on the target type and returns method names. `DeclaredOnly` excludes inherited members from `object`.
- **`Execute(operationName, a, b)`** — calls `GetMethod(name, Public | Instance)` on the target type, throws `InvalidOperationException` if the name is missing, otherwise uses `method.Invoke(_target, new object[] { a, b })` to call it. Casts the result back to `double`.

**Flow inside `ReflectionDemo.Run()`:**
1. Lists the operations discovered by reflection (prints method names).
2. Calls `Add(5, 3)`, `Subtract(10, 4)`, `Multiply(6, 7)`, `Power(2, 8)` by name.
3. Calls `Execute("Sqrt", …)` — proves that unknown names throw cleanly.
4. Prints a short explanation of how a DI container uses the same "inspect then invoke" pattern, but on **constructors** instead of methods:
   1. Find the concrete type for an interface.
   2. Inspect its constructor parameters.
   3. Recursively resolve each parameter type.
   4. Invoke the constructor with resolved dependencies.

**What's interesting:**
- This single small example unlocks an intuition for why frameworks like ASP.NET Core DI, Entity Framework, and test runners can do what they do without compile-time knowledge of your types.

---

## 8. How Everything Connects

The call graph from top to bottom:

```
Program.cs
  └─ holds a List<IPatternDemo>
       ├─ SingletonDemo                → AppLogger / AppLoggerThreadSafe / AppLoggerLazy
       ├─ BuilderDemo                  → EmailBuilder → EmailMessage  ;  EmailDirector(IEmailBuilder)
       ├─ AbstractFactoryDemo          → LoginScreen(IUiFactory) → IButton / ITextBox
       ├─ FacadeDemo                   → OrderCheckoutFacade → 4 subsystem services
       ├─ StrategyDemo                 → Checkout(IDiscountStrategy) → Cart / Customer
       ├─ StateDemo                    → Order → IOrderState (four concrete states)
       ├─ MediatorDemo                 → ChatRoom(IChatRoomMediator) ← StandardUser(User)
       ├─ CqrsDemo                     → CqrsMediator(ICqrsMediator) → ICommandHandler / IQueryHandler<,>
       ├─ AdapterDemo                  → IPaymentProcessor ← LegacyStripeAdapter → LegacyStripeClient
       ├─ DecoratorDemo                → INotifier stack: Logging(Retry(Timestamp(Email)))
       ├─ ProxyDemo                    → IDocumentService — lazy / protection / caching variants
       ├─ ChainOfResponsibilityDemo    → Approver chain: Manager → Director → CFO
       └─ ReflectionDemo               → ReflectionCalculator → Calculator  (via Reflection)
```

### Cross-file contracts to remember

| Interface | Implementations | Who depends on it |
|---|---|---|
| `IPatternDemo` | All `*Demo` classes | `Program.cs` |
| `IEmailBuilder` | `EmailBuilder` | `EmailDirector` |
| `IUiFactory` | `LightThemeFactory`, `DarkThemeFactory` | `LoginScreen` |
| `IButton` / `ITextBox` | `LightButton`/`DarkButton`, `LightTextBox`/`DarkTextBox` | `LoginScreen` |
| `IInventoryService` / `IPaymentService` / `IShippingService` / `INotificationService` | `InventoryService` / `PaymentService` / `ShippingService` / `NotificationService` | `OrderCheckoutFacade` |
| `IDiscountStrategy` | `NoDiscountStrategy`, `PercentageDiscountStrategy`, `BulkDiscountStrategy`, `LoyaltyDiscountStrategy` | `Checkout` |
| `IOrderState` | `PendingPaymentState`, `PaidState`, `ShippedState`, `DeliveredState` | `Order` |
| `IChatRoomMediator` | `ChatRoom` | `User` (abstract) |
| `ICqrsMediator` | `CqrsMediator` | `CqrsDemo` (caller); handlers register against it |
| `ICommandHandler<T>` / `IQueryHandler<T,R>` | `CreateUserHandler`, `GetUserByIdHandler`, `CountUsersHandler` | `CqrsMediator` (resolves by type) |
| `IPaymentProcessor` | `ModernPaymentProcessor`, `LegacyStripeAdapter` | `AdapterDemo` (any caller) |
| `INotifier` | `EmailNotifier` + decorator chain (`TimestampDecorator`, `LoggingDecorator`, `RetryDecorator`) | `DecoratorDemo` (any caller) |
| `IDocumentService` | `RealDocumentService`, `LazyDocumentProxy`, `ProtectionDocumentProxy`, `CachingDocumentProxy` | `ProxyDemo` (any caller) |
| `Approver` (abstract) | `ManagerApprover`, `DirectorApprover`, `CfoApprover` | `ChainOfResponsibilityDemo` |

Every pattern boils down to the same sentence: **depend on the interface, inject the concrete thing, and the calling code doesn't change when the concrete thing does.** The patterns differ in *what* is being varied — a family of products, a construction recipe, an algorithm, a lifecycle stage, a routing hub — but the mechanism is the same.

---

## 9. Build & Run

From the `DesignPatterns/` folder (or anywhere above it with a recursive restore):

```bash
dotnet run --project DesignPatterns
```

The menu appears. Enter a number (1–8) to run a demo, press a key to return, enter `0` to exit.

---

## 10. Adding a New Pattern Demo

If you want to add, say, an **Observer** demo, the checklist is:

1. Create `Patterns/Observer/` with the pattern's implementation files (interface, concrete participants).
2. Add `ObserverDemo.cs`:
   ```csharp
   namespace DesignPatterns.Patterns.Observer;

   internal class ObserverDemo : IPatternDemo
   {
       public string Name => "Observer";
       public void Run() { /* … */ }
   }
   ```
3. In `Program.cs`, `using DesignPatterns.Patterns.Observer;` and add `new ObserverDemo(),` to the `demos` list.
4. Optionally add `<Folder Include="Patterns\Observer\" />` to the `.csproj` if you want the solution view to list an empty folder (not strictly required).

No menu code, no dispatch logic, no other file needs to change.

---

## 11. Conventions Used Throughout

- **`internal` visibility** — every pattern class is `internal`, since the project is a console app and nothing is consumed externally. Keeps the public surface clean.
- **File-scoped namespaces** — every file uses `namespace Foo.Bar;` instead of the older block form.
- **Records for data** — immutable DTOs (`Customer`, `Item`, `OrderResult`, `Cart`-related types, `CheckoutResult`) use C# `record` for structural equality and a free `ToString()`.
- **Console as the UI** — every demo writes to `Console`. `Console.Clear()` and `Console.ReadKey` drive the menu loop.
- **Indented console output** — demos prefix output lines with two or more spaces to visually group them under the header `Program.cs` prints.
- **Guid-based instance IDs** — `Singleton` and `Facade` demos both use `Guid.NewGuid().ToString()[..8]` to show short stable ids.
- **Nullable reference types** — `_instance` fields and failure reasons are `string?` / `AppLogger?`; non-nullable ones are enforced by the compiler.

---

## 12. External Artefacts

- **`LearningNotes.docx`** — a Word document at the repo root. Not code; appears to hold study notes alongside this project. It's not referenced by the build.

---

## 13. TL;DR

A single .NET 10 console app wires eight self-contained pattern demos behind one `IPatternDemo` interface. Each demo folder is independent, each illustrates a single idea, and `Program.cs` only ever talks to the abstraction. Read one pattern folder at a time — the code is the lesson.
