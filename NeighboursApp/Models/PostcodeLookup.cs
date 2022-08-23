using System;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace neighbours.Models;

public class PostcodeLookup
{
	public static async Task<List<string>> Lookup(string postcode)
	{
		// Request parameters
		string apiKey = "PCWQQ-DWUHF-563KM-RXJFU";
		string countryCode = "UK";
		string searchTerm = $"{postcode}";

		// Prepare request and encode user-entered parameters with %xx encoding
		string requestUrl = $"https://ws.postcoder.com/pcw/{apiKey}/address/{countryCode}/{HttpUtility.UrlEncode(searchTerm)}";

		using (HttpClient client = new HttpClient())
		{
			// Send request
			var response = await client.GetAsync(requestUrl);
			var responseContent = await response.Content.ReadAsStringAsync();
      
      List<string> addresses = new List<string>();

			// Process response
			if (response.IsSuccessStatusCode)
			{
				JArray responseJson = JArray.Parse(responseContent);
				
				if (responseJson.Count > 0)
				{
					foreach (JObject address in responseJson)
					{
						addresses.Add(address["summaryline"].ToString());
					}
				}
      }

      return addresses;
		}
	}
}
