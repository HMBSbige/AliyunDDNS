using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AliyunDDNSWindowsApp
{
	public static class Util
	{
		private static readonly string[] IpApi = {
		@"http://www.3322.org/dyndns/getip",
		@"https://api.ip.la/",
		@"https://myip.ipip.net/",
		@"https://ip.cip.cc/",
		@"https://ipinfo.io/ip",
		@"https://ipv4.icanhazip.com/"
		};

		private static async Task<string> Get(string uri, bool useProxy = false)
		{
			var httpClientHandler = new HttpClientHandler
			{
				UseProxy = useProxy
			};
			var httpClient = new HttpClient(httpClientHandler);
			var response = await httpClient.GetAsync(uri);
			response.EnsureSuccessStatusCode();
			var resultStr = await response.Content.ReadAsStringAsync();

			Debug.WriteLine(resultStr);
			return resultStr;
		}

		private static string GetIPv4(string str)
		{
			var ip = new Regex(@"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)");
			return ip.Match(str).Value;
		}

		public static bool IsIPv4Address(string input)
		{
			return IPAddress.TryParse(input, out var ip) && IsIPv4Address(ip);
		}

		private static bool IsIPv4Address(IPAddress ipAddress)
		{
			return ipAddress.AddressFamily == AddressFamily.InterNetwork;
		}

		public static string GetPublicIp(bool useProxy = false)
		{
			foreach (var api in IpApi)
			{
				try
				{
					var rawStr = Get(api, useProxy).Result;
					var ip = GetIPv4(rawStr);
					if (IsIPv4Address(ip))
					{
						return ip;
					}
				}
				catch
				{
					// continue;
				}
			}
			return string.Empty;
		}
	}
}
