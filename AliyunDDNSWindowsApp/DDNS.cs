using System.Collections.Generic;
using AliyunDDNSWindowsApp.Alicore;
using AliyunDDNSWindowsApp.Alicore.Profile;
using AliyunDDNSWindowsApp.Alidns.Model.V20150109;

namespace AliyunDDNSWindowsApp
{
	public class DDNS
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
				RecordId = RecordID
			};
			try
			{
				var response = client.GetAcsResponse(request);
				return response.Value;
			}
			catch
			{
				return string.Empty;
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
				SubDomain = SubDomain,
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
				return string.Empty;
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
				return string.Empty;
			}
		}

		/// <summary>
		/// 修改A解析记录
		/// </summary>
		/// <param name="RR">主机记录，如果要解析@.exmaple.com，主机记录要填写"@”，而不是空</param>
		/// <param name="Domain">域名</param>
		/// <param name="Value">记录值</param>
		/// <returns></returns>
		public string UpdateDomainRecord(string RR, string Domain, string Value)
		{
			var request = new UpdateDomainRecordRequest()
			{
				RecordId = GetSubDomainRecordId($@"{RR}.{Domain}"),
				RR = RR,
				Type = @"A",
				Value = Value
			};
			try
			{
				var response = client.GetAcsResponse(request);
				return response.RecordId;
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}
