namespace Processors
{
    /// <summary>
    /// 実行インターフェイス
    /// </summary>
    public abstract class CommandBase : IExecutable
    {
        readonly LineInfo LineInfo;
        protected string[] Arguments => LineInfo.Arguments;
        protected string[] Results => LineInfo.Results;

        /// <summary>
        /// コンストラクタ処理
        /// </summary>
        /// <param name="lineInfo">1行情報</param>
        protected CommandBase(LineInfo lineInfo)
        {
            LineInfo = lineInfo;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        public void Execute()
        {
            try
            {
                LineInfo.SetStartTime();
                OnExecute();
                LineInfo.SetErrorCode("OK");
            }
            catch (Exception ex)
            {
                LineInfo.SetErrorCode(ex.Message);
            }
        }

        /// <summary>
        /// 実装処理
        /// </summary>
        protected abstract void OnExecute();
    }
}
