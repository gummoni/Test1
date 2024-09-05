using System.Diagnostics;

namespace Utils.Utils
{
    /// <summary>
    /// 扱うフォルダ管理クラス
    /// </summary>
    public static class PathManager
    {
        static readonly string ExeName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule?.FileName ?? "APP");
        static readonly string DocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        static readonly string TodayName = DateTime.Now.ToString("yyyyMMdd");
        static readonly string ProjectPath = Path.Combine(DocumentPath, ExeName);
        static readonly string BasePath = Path.Combine(DocumentPath, ExeName, TodayName);

        /// <summary>
        /// ファイル保存するパスを取得する
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetBaseFullPath(string filename)
        {
            Directory.CreateDirectory(BasePath);
            return Path.Combine(BasePath, filename);
        }

        /// <summary>
        /// 有効期限切れのフォルダを削除する
        /// </summary>
        /// <param name="expiredays"></param>
        public static void DeleteExpireFolders(int expiredays)
        {
            var expire = int.Parse(DateTime.Now.AddDays(-Math.Abs(expiredays)).ToString("yyyyMMdd"));
            var directoies = Directory.GetDirectories(ProjectPath).Select(Path.GetFileName);
            foreach (var directory in directoies)
            {
                if (int.TryParse(directory, out int current))
                {
                    if (current < expire)
                    {
                        try
                        {
                            Directory.Delete(directory, true);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLine("ERR", ex);
                        }
                    }
                }
            }
        }
    }

}
