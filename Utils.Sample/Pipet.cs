using Processors;

public class Pipet
{
    public string Name { get; set; } = "";

    [GeneratedInnerClass]
    public class Org : CommandBase
    {
        readonly Pipet Self;
        public Org(Pipet self, LineInfo line) : base(line)
        {
            Self = self;
        }

        protected override void OnExecute()
        {
            Console.WriteLine(Self.Name + ":Org" + Arguments[0]);
        }
    }

    public class TipOn : CommandBase
    {
        readonly Pipet Self;
        public TipOn(Pipet self, LineInfo info) : base(info)
        {
            Self = self;
        }

        protected override void OnExecute()
        {
            Console.WriteLine(Self.Name + ":TipOn" + Arguments[0]);
        }
    }
}

/// <summary>
/// インナークラス生成属性
/// </summary>
public sealed class GeneratedInnerClass : Attribute
{
}