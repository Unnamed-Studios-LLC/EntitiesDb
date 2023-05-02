# EntitiesDb
In memory Entity Component Database. Supports queries, enumeration, multi-threading, and events.

## Setup

Install via nuget: [UnnamedStudios.EntitiesDb](https://www.nuget.org/packages/UnnamedStudios.EntitiesDb)

## Adding and Removing Entities

All entity structural methods **cannot be called** during a ForEach enumerator or an Event

### Entity Layout

Adding or Removing components must be done with **EntityLayout**.

```c#
struct Position
{
    public float X;
    public float Y;
	
    public Position(float x, float y) => (X, Y) = (x, y);
}

struct Size
{
    public float Width;
    public float Height;
	
    public Size(float width, float height) => (Width, Height) = (width, height);
}

// example code
EntityDatabase entityDatabase;

// build an EntityLayout via EntityLayoutBuilder fluent api
// an EntityLayout may be stored and reused for multiple entities (recommended)
var entityLayout = EntityLayoutBuilder.Create()
    .Add<Position>()
    .Add<Size>()
    .Build();
	
// define component data
entityLayout.Set(new Position(10, 25));
entityLayout.Set(new Size(10, 25));

// create entity
uint entityId;

// directly
entityId = entityDatabase.CreateEntity(entityLayout);

// indirectly
entityId = entityDatabase.CreateEntity(); // creates an empty entity
entityDatabase.ApplyLayout(entityLayout);
```

### Clone

Entities may also be created by cloning an existing entity

```c#
// example code
EntityDatabase entityDatabase;
uint existingEntityId;

// clone entity
var entityId = entityDatabase.CloneEntity(existingEntityId);
```

### Destroy

```c#
// example code
EntityDatabase entityDatabase;
uint existingEntityId;

// destroy entity
var destroyed = entityDatabase.DestroyEntity(existingEntityId);
```

### Tags and Empty components

Empty components are considered **Tags** and do not store or take up space in a data chunk.
Tags may be used to help sort entitities for better queries.

## Enumeration

ForEach is the *magic* of an entity database.
Entities with the same component structure are stored immediately next to each other.
This allows the ForEach operation to be cache-safe, maximizing cache line usage.

### ForEach
```c#
struct Position
{
    public float X;
    public float Y;
	
    public Position(float x, float y) => (X, Y) = (x, y);
}

struct Velocity
{
    public float Dx;
    public float Dy;
	
    public Position(float dx, float dy) => (Dx, Dy) = (dx, dy);
}

// example code
EntityDatabase entityDatabase;
float timeDelta = 0.01f;

// component ForEach
entityDatabase.ForEach((ref Position position, ref Velocity velocity) => {
    position.X += velocity.Dx * timeDelta;
    position.Y += velocity.Dy * timeDelta;
});

// id component ForEach
// the entity id may be added as the first parameter of the query function
entityDatabase.ForEach((uint entityId, ref Position position, ref Velocity velocity) => {
    position.X += velocity.Dx * timeDelta;
    position.Y += velocity.Dy * timeDelta;
	
    // do something with entityId
});

// entity ForEach
// slower operation and should be used only when component functions cannot be
entityDatabase.ForEach((in Entity entity) => {
    ref var position = entitiy.GetComponent<Position>();
    ref var velocity = entitiy.GetComponent<Velocity>();
    position.X += velocity.Dx * timeDelta;
    position.Y += velocity.Dy * timeDelta;
	
    var entityId = entity.Id;
});
```

### Filters

ForEach queries support 3 filtering operations
**With**: Entity must contain all With components.
**Any**: Entity must only contains one of the Any components.
**No**: Entity cannot contain one of the No components.

Filters may be mixed and matched in any order.

```c#
struct Player { }
struct Npc { }
struct Locked { }

struct Health
{
    public float Current;
    public float Max;
	
    public Health(int current, int max) => (Current, Max) = (current, max);
}

struct Position
{
    public float X;
    public float Y;
	
    public Position(float x, float y) => (X, Y) = (x, y);
}

struct Velocity
{
    public float Dx;
    public float Dy;
	
    public Position(float dx, float dy) => (Dx, Dy) = (dx, dy);
}

// example code
EntityDatabase entityDatabase;
float timeDelta = 0.01f;

// only update positions that aren't locked
entityDatabase.No<Locked>().ForEach((ref Position position, ref Velocity velocity) => {
    position.X += velocity.Dx * timeDelta;
    position.Y += velocity.Dy * timeDelta;
});

// heal players and npcs
entityDatabase.Any<Player, Npc>().ForEach((ref Health health) => {
    health.Current = Math.Min(health.Max, health.Current + 10);
});

// move all npcs that aren't locked 5 units to the right
entityDatabase.With<Npc>().No<Locked>().ForEach((ref Position position) => {
    position.X += 5;
});
```

### Sub-Queries

ForEach may be used from within another ForEach. *Consider the exponential cost of doing so*
```c#
struct Camera { }
struct Sprite { }

// example code
EntityDatabase entityDatabase;

// sub-query
entityDatabase.ForEach((ref Camera camera) => {
    entityDatabase.ForEach((ref Sprite sprite) => {
        // draw sprite for camera
    });
});
```

### ParallelForEach

All **ForEach** methods also contain an equivalant **ParallelForEach** method.

ParallelForEach will work to split the enumeration job across available threads.
If ParallelForEach is called within an already Parallel options, the enumeration will default to Foreach behavior.
The ParallelOptions may be set via `EntityDatabase.ParallelOptions`

## Events

### Subscribing / Unsubscribing

```c#
struct Component
{
    public int Value;
}

void OnAdd(uint entityId, ref Component component) { }
void OnRemove(uint entityId, ref Component component) { }

// example code
EntityDatabase entityDatabase;

// subscribe
entityDatabase.Subscribe(Event.OnAdd, OnAdd);
entityDatabase.Subscribe(Event.OnRemove, OnAdd);

// unsubscribe
entityDatabase.Unsubscribe(Event.OnAdd, OnAdd);
entityDatabase.Unsubscribe(Event.OnRemove, OnAdd);
```

### Triggers
Component **OnAdd** and **OnRemove** are only called during when a structural change occurs for an entity.
If a layout add is applied and the entity already contains the component, then **OnAdd** event is not triggered.

### Exception Handling
Exceptions that occur during an event **do not effect the operation**, but they will stop subsequent events of the same component to not trigger.
Exceptions thrown from events can be viewed by checking the `EntityDatabase.EventExceptions` list after a structural operation.