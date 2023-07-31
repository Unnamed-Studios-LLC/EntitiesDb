using BenchmarkDotNet.Running;
using Sandbox;

#if RELEASE
BenchmarkRunner.Run<BufferTest>();
#else
var test = new BufferTest();
test.Setup();
test.Test();
test.Cleanup();
#endif