using BenchmarkDotNet.Attributes;
using EntitiesDb;

namespace Sandbox;

public class BufferTest
{
    private EntityDatabase? _entities;

    [GlobalSetup]
    public void Setup()
    {
        _entities = new EntityDatabase();

        var layout = new EntityLayout();
        var bufferedComponents = new BufferedComponent[12];

        var random = new Random(71345);
        for (int i = 0; i < 10_000; i++)
        {
            var r = random.Next();
            layout.AddComponent<Component>(new Component
            {
                ValueA = r,
                ValueB = r + 1
            });

            var length = random.Next(0, bufferedComponents.Length);
            var span = bufferedComponents.AsSpan(0, length);
            int index = 0;
            foreach (ref var bufferedComponent in span)
            {
                bufferedComponent = new BufferedComponent
                {
                    ValueA = r + index++
                };
            }
            layout.AddBuffer<BufferedComponent>(span);
            _entities.CreateEntity(layout);
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _entities?.Clear();
    }

    [Benchmark]
    public void Test()
    {
        var state = 5;
        _entities?.GetQueryFilter().ForEach(state, static (uint entityId, ref Component component, ref ComponentBuffer<BufferedComponent> bufferedComponents, ref int state) =>
        {
            var bufferSpan = bufferedComponents.GetSpan();
            foreach (ref var bufferComponent in bufferSpan)
            {

            }
        });
    }
}

public struct Component
{
    public int ValueA;
    public int ValueB;
}

[Bufferable(8)]
public struct BufferedComponent
{
    public int ValueA;
}