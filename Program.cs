#nullable enable

using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using ReflectionsTest.Tester;

Tester.Test();

// var type = typeof(TestClass);
// var inst1 = new TestClass();
// var inst2 = new TestClass();
// var inst3 = new TestClass();

// var props = type.GetProperties();
// var setters = new List<MethodInfo>();
// IPropertySetter? superSetter = null;
// IPropertyGetter? superGetter = null;
// foreach(var prop in  props) {
//     Console.WriteLine(prop.Name);
//     if(prop.PropertyType == typeof(int)) {
//         setters.Add(prop.GetSetMethod() ?? throw new ArgumentException());
//     }
//     if(prop.PropertyType == typeof(double?)) {
//         superSetter = prop.CreateSetter();
//         superGetter = prop.CreateGetter();
//     }
// }

// superSetter?.SetValue(inst1, 1.001);
// superSetter?.SetValue(inst2, 2.002);
// superSetter?.SetValue(inst3, 3.003);

// var val1 = superGetter?.GetValue(inst1);
// var val2 = superGetter?.GetValue(inst2);
// var val3 = superGetter?.GetValue(inst3);

// Console.WriteLine("val1: {0}", val1);
// Console.WriteLine("val2: {0}", val2);
// Console.WriteLine("val3: {0}", val3);



// var seed = 1000;
// var r = new Random(seed);
// var stopwatch = new Stopwatch();
// stopwatch.Start();
// for(int i = 0; i < 100000000; i++) {
//     setters[0].Invoke(inst1, new object[] { r.Next() });
// }
// stopwatch.Stop();
// Console.WriteLine("Elapsed time: {0} sec.", stopwatch.Elapsed.TotalSeconds);


// stopwatch.Reset();
// stopwatch.Start();
// var setter = inst1.CreateSetter<TestClass, int>(nameof(TestClass.IntProp));
// for(int i = 0; i < 100000000; i++) {
//     setter.SetValue(inst1, r.Next());
//     //setter.SetValue(inst2, r.Next());
//     //setter.SetValue(inst3, r.Next());
// }
// stopwatch.Stop();
// Console.WriteLine("Elapsed time: {0} sec.", stopwatch.Elapsed.TotalSeconds);


// stopwatch.Reset();
// stopwatch.Start();
// for(int i = 0; i < 100000000; i++) {
//     inst1.IntProp = r.Next();
// }
// stopwatch.Stop();
// Console.WriteLine("Elapsed time: {0} sec.", stopwatch.Elapsed.TotalSeconds);


// Console.WriteLine(inst1.IntProp);
// Console.WriteLine("{0:F3}", inst1.DoubleValue);
// Console.WriteLine("{0:F4}", inst2.DoubleValue);
// Console.WriteLine("{0:F5}", inst3.DoubleValue);






// internal class TestClass {
//     public double? DoubleValue { get; set; } = 0L;
//     public int IntProp { get; set; } = 0;
//     public string StringProp { get; set; }
// };