
namespace Processors
{
    /// <summary>
    /// CSV形式で書かれたレシピファイルを解釈＆実行する。
    /// </summary>
    public class Processor : IExecutable
    {
        readonly IExecutable[] Executables;

        /// <summary>
        /// コンストラクタ処理
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="filename"></param>
        Processor(object machine, string filename)
        {
            Executables = File.ReadAllLines(filename).Select(line => LineInfo.Parse(machine, line)).ToArray();
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Processor Parse(object machine, string filename) => new Processor(machine, filename);

        /// <summary>
        /// 実行関数
        /// </summary>
        public void Execute()
        {
            foreach (var exe in Executables)
            {
                exe.Execute();
            }
        }
    }
}
