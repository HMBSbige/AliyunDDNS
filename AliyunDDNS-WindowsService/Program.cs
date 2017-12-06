using System.ServiceProcess;

namespace AliyunDDNS_WindowsService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            var ServicesToRun = new ServiceBase[]
            {
                new AliyunDDNS(args)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
