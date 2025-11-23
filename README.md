# Ultimate State Machine
[![License](https://img.shields.io/github/license/DanSim7/UltimateStateMachine?color=318CE7&style=flat-square)](LICENSE.md) [![Version](https://img.shields.io/github/package-json/v/DanSim7/UltimateStateMachine?color=318CE7&style=flat-square)](package.json) [![Unity](https://img.shields.io/badge/Unity-2021.3+-2296F3.svg?color=318CE7&style=flat-square)](https://unity.com/)

A robust, type-safe state machine implementation for Unity with seamless Zenject integration. Designed for maintainable and testable game architecture.

> Requires C# 8+ version

## 🚀 Features

- **Type-Safe State Management** - Compile-time safety with generic constraints
- **Flexible State Lifecycle** - Support for enter, exit, update, and pre-update states
- **Payload Support** - Pass data between states with strongly-typed payloads
- **Zenject Integration** - First-class dependency injection support
- **Modular Transitions** - Clean separation of state logic and transition conditions
- **Event-Driven** - State change events for reactive programming

# 🌐 Navigation

* [Main](#-finite-state-machine)
* [Installation](#-installation)
* [Architecture Overview](#-architecture-overview)
    * [Core Interfaces](#-core-interfaces)
    * [State Machine Implementation](#2-state-machine-implementation)
    * [Zenject Setup](#3-zenject-setup)
* [Quick Start](#4-quick-start)
  * [1. Define Your States](#1-define-your-states)
  * [2. Create State Machine](#2-create-state-machine)
  * [Configure with Zenject](#-configure-with-zenject)
* [Advanced Usage](#-advanced-usage)

# ▶ Installation

## As a Unity module
Supports installation as a Unity module via a git link in the **PackageManager**
```
https://github.com/dansim7/UltimateStateMachine.git
```
or direct editing of `Packages/manifest.json`:
```
"com.dansim.statemachine": "https://github.com/dansim7/UltimateStateMachine.git",
```
## As source
You can also clone the code into your Unity project.

# 🏗 Architecture Overview

## Core Interfaces

```csharp
public interface IState
{
    Type GetStateType();
    void AddTransition(ITransition transition);
    void RemoveTransition(ITransition transition);
    bool TryTransit(IChangeStateStateMachine stateMachine);
}

public interface IEnterState : IState { void OnEnter(); }

public interface IExitState { void OnExit(); }

public interface IUpdateState { void OnUpdate(); }

public interface IPreUpdateState { void OnPreUpdate(); }```
```

## State Machine Implementation
The StateMachine class provides:
* Automatic state lifecycle management
* Type-safe state transitions
* Event notifications on state changes
* Update cycle integration


## Zenject Setup
> To use Zenject you must enabled define **ZENJECT**

Register the state factory in your installer:

```csharp
Container.Bind<IStateFactory>().To<ZenjectStateFactory>().AsSingle();
```

# 🎯 Quick Start
## 1. Define Your States
   ```csharp
    public interface IExampleBootService
    {
        void Load(Action onLoaded);
    }

    public class BootstrapState : BaseState<BootstrapState>, IEnterState
    {
        private readonly IExampleBootService _bootService;
        private bool _isLoadComplete;

        private const string MENU_SCENE_NAME = "Example Menu Scene";

        public BootstrapState(IExampleBootService bootService)
        {
            _bootService = bootService;
            
            AddTransition(
                new TransitionWithPayload<LoadSceneState, string>(
                    condition:() => _isLoadComplete == true,
                    payload: () => MENU_SCENE_NAME
                    )
                );
        }

        public void OnEnter()
        {
            _isLoadComplete = false;
            _bootService.Load(() => _isLoadComplete = true);
        }
    }
    
    public class LoadSceneState : BaseState<LoadSceneState>, IEnterStateWithPayload<string>
    {
        public void OnEnter(string payload)
        {
            // load scene with name = payload
        }
    }

    public class MenuState : BaseState<MenuState>, IEnterState, IPreUpdateState, IUpdateState, IExitState
    {
        public MenuState()
        {
            // get dependencies
        }
        
        public void OnEnter()
        {
            // initialize something
        }

        public void OnPreUpdate()
        {
            // update something before check transitions
        }

        public void OnUpdate()
        {
            // update something after check transitions
        }

        public void OnExit()
        {
            // cleanup something
        }
    }
   ```
## 2. Create State Machine
   ```csharp
    public class GameStateMachine : StateMachine, IInitializable, ITickable, IDisposable
    {
        public GameStateMachine(IStateFactory stateFactory) : base(stateFactory)
        {
            AddState<BootstrapState>();
            AddState<LoadSceneState>();
            AddState<MenuState>();
        }

        public void Initialize()
        {
            ChangeState<BootstrapState>();
        }

        public void Tick()
        {
            Update();
        }

        public void Dispose()
        {
            Stop();
        }
    }
   ```
## 3. Configure with Zenject
> To use Zenject you must enabled define **ZENJECT**   

```csharp
    public class GameStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStateFactory();
            BindStates();
            BindStateMachine();
        }

        private void BindStateFactory()
        {
            Container.BindInterfacesTo<ZenjectStateFactory>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadSceneState>().AsSingle();
            Container.Bind<MenuState>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
        }
    }
```
# 🔄 Advanced Usage
### Payload Transitions
   ```csharp
var transition = new TransitionWithPayload<PauseState, PauseData>(
    () => gameIsPaused,
    () => new PauseData { Reason = "Menu" }
);

state.AddTransition(transition);
```
### Custom State Factory
Implement IStateFactory for custom state creation logic:

```csharp
public class CustomStateFactory : IStateFactory
{
    public T CreateState<T>() where T : IState
    {
        // Custom instantiation logic
        return Activator.CreateInstance<T>();
    }
}
```

### State Change Events
```csharp
stateMachine.OnStateChanged += (current, previous) =>
{
    Debug.Log($"State changed from {previous?.GetType().Name} to {current?.GetType().Name}");
};
```

### 🎨 Design Patterns
* Strategy Pattern - States implement specific behaviors
* State Pattern - Encapsulates state-specific logic
* Factory Pattern - Flexible state instantiation
* Observer Pattern - State change notifications

### ⚡ Performance
* Minimal Allocation - Reuses state instances and transitions
* Efficient Updates - Only processes relevant state callbacks
* Fast Transitions - O(1) state lookups with dictionary storage

