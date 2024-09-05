using System.Reflection;

namespace Processors
{
    /// <summary>
    /// レシピ１行データ
    /// </summary>
    public class LineInfo
    {
        public string[] Header { get; set; } = new string[] { "", "", "" };
        public string[] Arguments { get; set; } = new string[] { "", "", "", "", "", "", "", "", "", "" };
        public string[] Footer { get; set; } = new string[] { "", "", "" };
        public string[] Results { get; set; } = new string[] { "", "", "", "", "", "", "", "", "", "" };
        public override string ToString() => $"{string.Join(",", Header)}\t{string.Join(",", Arguments)}\t{string.Join(",", Footer)}\t{string.Join(",", Results)}";

        public string DeviceName => Header[0];
        public string CommandName => Header[1];
        public string PosNo => Header[2];
        public string StartTime { get => Footer[0]; private set { Footer[0] = value; } }
        public string FinishTime { get => Footer[1]; private set { Footer[1] = value; } }
        public string ErrorCode { get => Footer[2]; private set { Footer[2] = value; } }

        /// <summary>
        /// 開始時刻を記録する
        /// </summary>
        public void SetStartTime()
        {
            StartTime = DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 終了時刻とエラーコードを記録する
        /// </summary>
        /// <param name="errorCode"></param>
        public void SetErrorCode(string errorCode)
        {
            FinishTime = DateTime.Now.ToString("HH:mm:ss");
            ErrorCode = errorCode;
        }

        /// <summary>
        /// コンストラクタ処理
        /// </summary>
        /// <param name="line"></param>
        public LineInfo(string line)
        {
            var rows = line.Split("\t");
            Header = rows[0].Split(",");
            Arguments = rows[1].Split(",");
            Footer = rows[2].Split(",");
            Results = rows[3].Split(",");
        }

        /// <summary>
        /// コンストラクタ処理(デバッグ用)
        /// </summary>
        /// <param name="device"></param>
        /// <param name="command"></param>
        public LineInfo(string device, string command)
        {
            Header[0] = device;
            Header[1] = command;
        }

        /// <summary>
        /// 解析処理
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static IExecutable Parse(object obj, string line)
        {
            var csv = new LineInfo(line);

            //デバイスインスタンス取得
            var prop = obj.GetType().GetProperty(csv.DeviceName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop is null) throw new InvalidDataException($"{csv.DeviceName}が見つかりません。");
            var pobj = prop.GetValue(obj);
            if (pobj is null) throw new InvalidDataException($"{csv.DeviceName}インスタンスがnullです。");

            //実行コマンド取得            
            var ptype = prop.PropertyType.Assembly.GetTypes().First(_ => _.Name.Equals(csv.CommandName, StringComparison.OrdinalIgnoreCase));
            var ppobj = Activator.CreateInstance(ptype, new object[] { pobj, csv });
            if (ppobj is null) throw new InvalidDataException($"{ptype.Name}型が見つかりませんでした。");
            if (ppobj is not IExecutable exe) throw new InvalidDataException($"{ptype.Name}型にIExecutableインターフェイスが見つかりませんでした。");
            return exe;
        }
    }
}
