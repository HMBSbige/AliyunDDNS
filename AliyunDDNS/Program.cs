using System;
using System.Threading;

namespace AliyunDDNS
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine(@"Hello World!");
            var p=new Program();
            const int minute = 59*1000;
            var threadTimer = new Timer(Update,null,0,6* minute);
            Console.WriteLine(@"Running!");
            while (true)
            {
                
            }
        }

        private static void Update(object state)
        {
            var t1 = new AliyunDDNS(@"", @"");

            const string Domain = @"bige0.com";
            const string RR = @"school";
            var Value = AliyunDDNS.GetLocalIP();
            if (Value == @"0.0.0.0")
            {
                Console.WriteLine(@"IP 地址未改变");
                return;
            }

            Console.Write("{0}:\t{1}\t\t", DateTime.Now, Value);

            const string SubDomain = RR + @"." + Domain;
            var lastValue = t1.GetSubDomainARecord(SubDomain);
            var lastRecordId = t1.GetSubDomainRecordId(SubDomain);

            if (lastValue != Value)
            {
                if (t1.UpdateDomainRecord(@"school", @"bige0.com", Value) == lastRecordId &&
                    t1.GetSubDomainARecord(SubDomain) == Value)
                {
                    Console.WriteLine(@"IP 地址更改成功");
                }
                else
                {
                    Console.WriteLine(@"IP 地址更改失败");
                }

            }
            else
            {
                Console.WriteLine(@"IP 地址未改变");
            }
        }
    }
}