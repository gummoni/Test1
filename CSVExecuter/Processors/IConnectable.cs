namespace Processors
{
    /// <summary>
    /// 接続インターフェイス
    /// </summary>
    public interface IConnectable
    {
        void Connect();
        void Disconnect();
    }
}
