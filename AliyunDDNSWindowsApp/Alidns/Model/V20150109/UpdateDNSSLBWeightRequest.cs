/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

using AliyunDDNSWindowsApp.Alicore;
using AliyunDDNSWindowsApp.Alicore.Transform;
using AliyunDDNSWindowsApp.Alicore.Utils;
using AliyunDDNSWindowsApp.Alidns.Transform.V20150109;

namespace AliyunDDNSWindowsApp.Alidns.Model.V20150109
{
    public class UpdateDNSSLBWeightRequest : RpcAcsRequest<UpdateDNSSLBWeightResponse>
    {
        public UpdateDNSSLBWeightRequest()
            : base("Alidns", "2015-01-09", "UpdateDNSSLBWeight")
        {
        }

		private string lang;

		private string userClientIp;

		private string recordId;

		private int? weight;

		public string Lang
		{
			get
			{
				return lang;
			}
			set	
			{
				lang = value;
				DictionaryUtil.Add(QueryParameters, "Lang", value);
			}
		}

		public string UserClientIp
		{
			get
			{
				return userClientIp;
			}
			set	
			{
				userClientIp = value;
				DictionaryUtil.Add(QueryParameters, "UserClientIp", value);
			}
		}

		public string RecordId
		{
			get
			{
				return recordId;
			}
			set	
			{
				recordId = value;
				DictionaryUtil.Add(QueryParameters, "RecordId", value);
			}
		}

		public int? Weight
		{
			get
			{
				return weight;
			}
			set	
			{
				weight = value;
				DictionaryUtil.Add(QueryParameters, "Weight", value.ToString());
			}
		}

        public override UpdateDNSSLBWeightResponse GetResponse(UnmarshallerContext unmarshallerContext)
        {
            return UpdateDNSSLBWeightResponseUnmarshaller.Unmarshall(unmarshallerContext);
        }
    }
}