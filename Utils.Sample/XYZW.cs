
namespace Processors
{
    /// <summary>
    /// XYZW軸制御
    /// </summary>
    public class XYZW
    {
        public class Home : CommandBase
        {
            readonly XYZW Self;
            public Home(XYZW self, LineInfo line) : base(line)
            {
                Self = self;
            }

            protected override void OnExecute()
            {
                Console.WriteLine("HOME" + Arguments[0]);
            }
        }
    }
}
