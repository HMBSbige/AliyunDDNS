using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Alidns.Model.V20150109;

namespace AliyunDDNS
{
    internal class DDNS
    {
        private const string regionId = @"cn-hangzhou";
        private readonly IAcsClient client;

        public DDNS(string accessKeyId, string accessKeySecret)
        {
            IClientProfile profile = DefaultProfile.GetProfile(regionId, accessKeyId, accessKeySecret);
            // 若报Can not find endpoint to access异常，请添加以下此行代码
            // DefaultProfile.addEndpoint("cn-hangzhou", "cn-hangzhou", "Alidns", "alidns.aliyuncs.com");
            client = new DefaultAcsClient(profile);
        }

        /// <summary>
        /// 获取某域名的解析记录列表
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        public List<DescribeDomainRecordsResponse.Record> GetDomainRecords(string Domain)
        {
            var request = new DescribeDomainRecordsRequest()
            {
                DomainName = Domain,
                PageNumber = 1,
                PageSize = 500
            };
            try
            {
                var response = client.GetAcsResponse(request);
                return response.DomainRecords;
            }
            catch
            {
                return new List<DescribeDomainRecordsResponse.Record>();
            }
        }

        /// <summary>
        /// 获取解析的记录值
        /// </summary>
        /// <param name="RecordID"></param>
        /// <returns></returns>
        public string GetDomainRecordValue(string RecordID)
        {
            var request = new DescribeDomainRecordInfoRequest()
            {
                RecordId= RecordID
            };
            try
            {
                var response = client.GetAcsResponse(request);
                return response.Value;
            }
            catch
            {
                return @"";
            }
        }

        /// <summary>
        /// 获取某子域名A记录的解析值
        /// </summary>
        /// <param name="SubDomain"></param>
        /// <returns></returns>
        public string GetSubDomainARecord(string SubDomain)
        {
            var request = new DescribeSubDomainRecordsRequest()
            {
                SubDomain= SubDomain,
                PageNumber = 1,
                PageSize = 1,
                Type = @"A"

            };
            try
            {
                var response = client.GetAcsResponse(request);
                return response.DomainRecords[0].Value;
            }
            catch
            {
                return @"";
            }
        }

        /// <summary>
        /// 获取某子域名A记录的RecordId
        /// </summary>
        /// <param name="SubDomain"></param>
        /// <returns></returns>
        public string GetSubDomainRecordId(string SubDomain)
        {
            var request = new DescribeSubDomainRecordsRequest()
            {
                SubDomain = SubDomain,
                PageNumber = 1,
                PageSize = 1,
                Type = @"A"

            };
            try
            {
                var response = client.GetAcsResponse(request);
                return response.DomainRecords[0].RecordId;
            }
            catch
            {
                return @"";
            }
        }

        /// <summary>
        /// 修改A解析记录
        /// </summary>
        /// <param name="RR">主机记录，如果要解析@.exmaple.com，主机记录要填写"@”，而不是空</param>
        /// <param name="Domain">域名</param>
        /// <param name="Value">记录值</param>
        /// <returns></returns>
        public string UpdateDomainRecord(string RR, string Domain,string Value)
        {
            var request = new UpdateDomainRecordRequest()
            {
                RecordId = GetSubDomainRecordId(RR+@"."+ Domain),
                RR =RR,
                Type = @"A",
                Value= Value
            };
            try
            {
                var response = client.GetAcsResponse(request);
                return response.RecordId;
            }
            catch
            {
                return @"";
            }
        }

        /// <summary>
        /// 获取公网IPv4地址
        /// </summary>
        /// <returns>公网IPv4地址点分十进制字符串</returns>
        public static string GetLocalIP()
        {
            string IP = @"0.0.0.0";
            WebRequest wr = WebRequest.Create(@"https://myip.ipip.net/");
            Stream s = wr.GetResponse().GetResponseStream();
            if (s != null)
            {
                StreamReader sr = new StreamReader(s, Encoding.UTF8);
                IP = sr.ReadToEnd();
                int start = IP.IndexOf(@"当前 IP：", StringComparison.Ordinal) + 6;
                int end = IP.IndexOf(@"  来自于：", start, StringComparison.Ordinal);
                IP = IP.Substring(start, end - start);
                sr.Close();
                s.Close();
            }
            return IP;
        }

        public static long Longnullable2long(long? z)
        {
            return z.HasValue ? z.GetValueOrDefault() : long.MinValue;

        }
    }
}
