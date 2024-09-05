using Processors;
using Utils.Utils;

Console.WriteLine("Hello, World!");

var lines = new List<string>();
lines.Add(new LineInfo("Pipet1", "Org").ToString());
lines.Add(new LineInfo("Pipet2", "TipOn").ToString());
lines.Add(new LineInfo("XYZW", "HOME").ToString());

File.WriteAllLines("test.txt", lines);

var machine = new Machine();
var processor = Processor.Parse(machine, "test.txt");
processor.Execute();


for (var i = 0; i < 100; i++)
{
    Logger.WriteLine("SYS", "Hello");
    Logger.WriteLine("EXE", "Hello");
}
Thread.Sleep(1000);