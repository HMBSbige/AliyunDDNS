using System;

namespace AliyunDDNS
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine(@"Hello World!");

            var t1=new AliyunDDNS(@"", @"");

            
            const string Domain = @"bige0.com";
            const string RR = @"school";
            const string Value = @"8.8.8.8";

            const string SubDomain = RR+@"."+ Domain;
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
            Console.WriteLine(@"Goodbye!");
        }
    }
}