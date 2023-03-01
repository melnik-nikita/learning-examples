using BenchmarkDotNet.Attributes;

namespace Sandbox.Benchmarks;

[MemoryDiagnoser]
public class ListBenchmark
{
    [Params(1000, 100_000)]
    public int Size { get; set; }

    [Benchmark(Baseline = true)]
    public void ListFill()
    {
        var list = new List<int>();
        for (var i = 0; i < Size; i++)
        {
            list.Add(i);
        }
    }

    [Benchmark()]
    public void ListRefFill()
    {
        var list = new List<BenchmarkClass>();
        for (var i = 0; i < Size; i++)
        {
            list.Add(new BenchmarkClass());
        }
    }

    [Benchmark()]
    public void ListConstSizeFill()
    {
        var list = new List<int>(Size);
        for (var i = 0; i < Size; i++)
        {
            list.Add(i);
        }
    }

    [Benchmark()]
    public void ListConstSizeRefFill()
    {
        var list = new List<BenchmarkClass>(Size);
        for (var i = 0; i < Size; i++)
        {
            list.Add(new BenchmarkClass());
        }
    }

    [Benchmark()]
    public void LinkedListFill()
    {
        var list = new LinkedList<int>();
        for (var i = 0; i < Size; i++)
        {
            list.AddLast(i);
        }
    }

    [Benchmark()]
    public void LinkedListRefFill()
    {
        var list = new LinkedList<BenchmarkClass>();
        for (var i = 0; i < Size; i++)
        {
            list.AddLast(new BenchmarkClass());
        }
    }
}

public class BenchmarkClass
{
}
