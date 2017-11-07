using System.IO;
using System.Text;

namespace AliyunDDNSWindowsApp
{
    internal static class Config
    {
        public static void WriteConfig(string Domain,string RR,string accessKeyId,string accessKeySecret)
        {
            using (var bw = new BinaryWriter(File.Open(MainForm.configfile, FileMode.Create), Encoding.UTF8))
            {
                bw.Write(Domain);
                bw.Write(RR);
                bw.Write(accessKeyId);
                bw.Write(accessKeySecret);
            }
        }

        public static string[] ReadConfig()
        {
            using (var br = new BinaryReader(File.Open(MainForm.configfile, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.UTF8))
            {
                return new[] {br.ReadString(), br.ReadString(), br.ReadString(), br.ReadString()};
            }
        }
    }
}
