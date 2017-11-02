using System;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Alidns.Model.V20150109;

namespace AliyunDDNS
{
    class AliyunDDNS
    {
        static readonly String regionId = @"cn-hangzhou"; //必填固定值，必须为“cn-hanghou”
        static readonly String accessKeyId = @""; // your accessKey
        static readonly String accessKeySecret = @"";// your accessSecret
        static readonly IClientProfile profile = DefaultProfile.GetProfile(regionId, accessKeyId, accessKeySecret);
        // 若报Can not find endpoint to access异常，请添加以下此行代码
        // DefaultProfile.addEndpoint("cn-hangzhou", "cn-hangzhou", "Alidns", "alidns.aliyuncs.com");
        static readonly IAcsClient client = new DefaultAcsClient(profile);

        public static string Test()
        {
            DescribeDomainsRequest request = new DescribeDomainsRequest();

            try
            {
                DescribeDomainsResponse response = client.GetAcsResponse(request);

                return response.Domains[0].DomainName;
            }
            catch
            {
                return @"Error";
            }
        }

        private static long longnullable2long(long? z)
        {
            return z.HasValue ? z.GetValueOrDefault() : long.MinValue;

        }
    }
}
