# EntitiesDb
In memory Entity Component Database. Supports queries, enumeration, multi-threading, and events.

## Nuget

[UnnamedStudios.EntitiesDb](https://www.nuget.org/packages/UnnamedStudios.EntitiesDb)

## Components and Entities

### Components

Components are used to store data and must be blittable types (`where T : unmanaged`).
They cannot contain reference types. When this constraint is applied, 
enumeration can take advatage of cache-friendly component packing.

#### Struct Examples

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

struct Velocity
{
    public float Dx;
    public float Dy;
    
    public Position(float dx, float dy) => (Dx, Dy) = (dx, dy);
}
```

### Entities

Entities are represented by **entity ids**. An entity id is a unique `uint` value that
can be used as your key to access entity component values.

### Create

```c#
// create entity with auto-generated id
var entityId = entityDatabase.CreateEntity();
entityDatabase.AddComponent(entityId, new Position(50, 50));
entityDatabase.AddComponent(entityId, new Size(100, 100));

// create entity with a given id (id must not be taken)
var entityId = entityDatabase.CreateEntity(123);
entityDatabase.AddComponent(entityId, new Position(50, 50));
entityDatabase.AddComponent(entityId, new Size(100, 100));
```

### EntityLayout

AddComponent and RemoveComponent methods requires copying an entire entities component data for each call.
To avoid this and perform Add/Remove operations in batch, you can use EntityLayout. EntityLayouts may be
cached and reused.

```c#
var entityLayout = new EntityLayout();
entityLayout.Add(new Position(50, 50));
entityLayout.Add(new Size(100, 100));
entityLayout.Remove<Velocity>();

// apply layout
entityDatabase.ApplyLayout(existingEntityId, entityLayout);

// create entity with layout
var entityId = entityDatabase.CreateEntity(entityLayout);
```

### Clone

Entities may also be created by cloning an existing entity.
All component data on the existing entity is copied into the new entity.

```c#
var entityId = entityDatabase.CloneEntity(existingEntityId);
```

### Destroy

```c#
entityDatabase.DestroyEntity(existingEntityId);
```

### Tags and Empty components

Empty components are considered **Tags** and do not store or take up space in a data chunk.
Tags may be used to help sort entitities for better queries.

```c#
struct Player { }
struct Enemy { }
```

## Enumeration

ForEach is the *magic* of an entity database.
Entities with the same component structure are stored immediately next to each other.
This allows the ForEach operation to be cache-friendly, maximizing cache line usage.

### ForEach
```c#
Time.Delta = 0.01f;

// component ForEach
entityDatabase.ForEach((ref Position position, ref Velocity velocity) => {
    position.X += velocity.Dx * Time.Delta;
    position.Y += velocity.Dy * Time.Delta;
});

// id component ForEach
// the entity id may be added as the first parameter of the query function
entityDatabase.ForEach((uint entityId, ref Position position, ref Velocity velocity) => {
    position.X += velocity.Dx * Time.Delta;
    position.Y += velocity.Dy * Time.Delta;
	
    // do something with entityId
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

Time.Delta = 0.01f;

// only update positions that aren't locked
entityDatabase.No<Locked>().ForEach((ref Position position, ref Velocity velocity) => {
    position.X += velocity.Dx * Time.Delta;
    position.Y += velocity.Dy * Time.Delta;
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

### Parallel and Multi-threading

All **ForEach** methods also contain an equivalant **ParallelForEach** method.

ParallelForEach will work to split the enumeration job across available threads.
If ParallelForEach is called within an already Parallel enumeration, the enumeration will default to ForEach behavior.

## Events

### Subscribing / Unsubscribing

```c#
struct Component
{
    public int Value;
}

void OnAddEntity(uint entityId) { }
void OnAddComponent(uint entityId, ref Component component) { }
void OnRemoveEntity(uint entityId) { }
void OnRemoveComponent(uint entityId, ref Component component) { }

// subscribe
entityDatabase.AddEntityEvent(Event.OnAdd, OnAddEntity);
entityDatabase.AddComponentEvent(Event.OnAdd, OnAddComponent);
entityDatabase.AddEntityEvent(Event.OnRemove, OnRemoveEntity);
entityDatabase.AddComponentEvent(Event.OnRemove, OnRemoveComponent);

// unsubscribe
entityDatabase.RemoveEntityEvent(Event.OnAdd, OnAddEntity);
entityDatabase.RemoveComponentEvent(Event.OnAdd, OnAddComponent);
entityDatabase.RemoveEntityEvent(Event.OnRemove, OnRemoveEntity);
entityDatabase.RemoveComponentEvent(Event.OnRemove, OnRemoveComponent);
```

### Triggers
Component **OnAdd** and **OnRemove** are only called when a structural change occurs for an entity.
If a layout add is applied and the entity already contains the component, then **OnAdd** event is not triggered.

### Exception Handling
Event exceptions can cause undesirable behavior within your application, blocking Destroy or Remove calls. 
Properly debug exceptions thrown from events to ensure fluid behavior.

## Read Only

During an enumeration method (ForEach, ParallelForEach) or an event call, all entities and events are set to read-only.
When read-only, entities, components, and events cannot be added or removed.
