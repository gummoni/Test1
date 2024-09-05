using Processors;
/// <summary>
/// 接続ベースクラス
/// </summary>
public class ConnectableBase : IConnectable
{
    bool IsOnline;

    public void Connect()
    {
        if (IsOnline) return;
        IsOnline = true;
        
        var props = GetType().GetProperties();
        foreach (var prop in props)
        {
            if (prop.GetValue(this) is IConnectable connectable)
                connectable.Connect();
        }
        OnConnect();
    }

    public void Disconnect()
    {
        if (!IsOnline) return;
        IsOnline = false;

        OnDisconnect();

        var props = GetType().GetProperties();
        foreach (var prop in props)
        {
            if (prop.GetValue(this) is IConnectable connectable)
                connectable.Disconnect();
        }
    }

    protected virtual void OnConnect() { }
    protected virtual void OnDisconnect() { }
}
