using MSBlogsFunction.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MSBlogsFunction
{
    public static class RssHelper
	{
		private readonly static HttpClient httpClient;

		private const string devBlogsFeedsLink = "http://sxp.microsoft.com/feeds/3.0/devblogs";
		private const string cloudFeedsLink = "https://sxp.microsoft.com/feeds/3.0/cloud";

		static RssHelper()
		{
			if (httpClient == null)
			{
				httpClient = new HttpClient();
			}
		}

		public static async Task<List<RssEntity>> GetRss(string url)
		{
			var blogs = new List<RssEntity>();
			string xmlString = await httpClient.GetStringAsync(url);
			if (!string.IsNullOrEmpty(xmlString))
			{
				var xmlDoc = XDocument.Parse(xmlString);

				XNamespace nspc = "http://sxpdata.microsoft.com/metadata";
				IEnumerable<XElement> xmlList = xmlDoc.Root.Element("channel")?.Elements("item");
				//TODO:根据作者进行筛选
				string[] authorfilter = { "[MSFT]", "Team", "Microsoft", "Visual", "Office", "Blog" };


				blogs = xmlList?.Where(x => x.Name == "item")
					.Where(x => IsContainKey(authorfilter, x.Element("author").Value))
					.Select(x =>
					{
						DateTime createTime = DateTime.Now;

						string createTimeString = x.Element("pubDate")?.Value;
						if (!string.IsNullOrEmpty(createTimeString))
						{
							createTime = DateTime.Parse(createTimeString);
						}

						return new RssEntity
						{
							Title = x.Element("title")?.Value,
							Description = x.Element("description")?.Value,
							CreateTime = createTime,
							Author = x.Element("author")?.Value,
							Link = x.Element("link")?.Value,
							Categories = x.Element("source")?.Value,
							LastUpdateTime = createTime,
							//MobileContent = x.Element("sxp_MobileContent")?.Value
						};
					})
					.ToList();
			}
			return blogs;
		}

		public static bool IsContainKey(string[] strArray, string key)
		{
			foreach (string item in strArray)
			{
				if (key.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		public static async Task<ICollection<RssEntity>> GetDevBlogs()
		{
			return await GetRss(devBlogsFeedsLink);
		}

		public static async Task<ICollection<RssEntity>> GetCloudNews()
		{
			return await GetRss(cloudFeedsLink);
		}
	}
}