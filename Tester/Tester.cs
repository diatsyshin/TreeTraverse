using System.Diagnostics;
using ReflectionsTest.ObjectMapper.Visitors;

namespace ReflectionsTest.Tester;

internal class TestClass1 {
    public int IntProp { get; set; }
    public double? DoubleProp { get; set; }

    public IEnumerable<QuantityClass> Samples { get; set; }
}

internal class QuantityClass {
    public string Uom { get; set; }
    public double? Value { get; set; }
}

internal static class Tester {

    public static void Test() {
        var mapper = new AdvancedObjectMapper<object?>();

        var stopwatch = new Stopwatch(); 

        Console.WriteLine("Generating data...");
        stopwatch.Start();
        var data = TestDataGenerator(100000).ToList();
        stopwatch.Stop();
        Console.WriteLine("Data have been generated for {0} sec.", stopwatch.Elapsed.TotalSeconds);

        stopwatch.Reset();
        stopwatch.Start();
        Console.WriteLine("Building map...");
        var tree = mapper.BuildMap(data);
        stopwatch.Stop();
        Console.WriteLine("Map has been built for {0} sec.", stopwatch.Elapsed.TotalSeconds);

        stopwatch.Reset();
        stopwatch.Start();
        Console.WriteLine("Visiting...");
        var simpleVisitor = new SimpleVisitor();
        simpleVisitor.Visit(tree);
        stopwatch.Stop();
        Console.WriteLine("Visited for {0} sec.", stopwatch.Elapsed.TotalSeconds);


        stopwatch.Reset();
        stopwatch.Start();
        Console.WriteLine("Updating...");
        var updater = new UpdaterVisitor();
        updater.Visit(tree);
        stopwatch.Stop();
        Console.WriteLine("Updated for {0} sec.", stopwatch.Elapsed.TotalSeconds);
    }


    private static IEnumerable<object?> TestDataGenerator1(int capacity = 10000, int seed = 1000) {
        var r = new Random();
        for (int i = 0; i < capacity; i++) {
            object? instance = null;
            yield return instance;
        }
    }

    private static IEnumerable<TestClass1> TestDataGenerator(int capacity = 10000, int seed = 1000) {
        var r = new Random();
        for (int i = 0; i < capacity; i++) {
            var instance = new TestClass1() {
                IntProp = 1,
                DoubleProp = 1.1d,
                Samples = new List<QuantityClass>() {
                    new QuantityClass { Uom = "Pressure", Value = 3.0d },
                    new QuantityClass { Uom = "Density", Value = 4.0d },
                }
            };
            yield return instance;
        }
    }
}