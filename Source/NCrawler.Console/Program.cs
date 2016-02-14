﻿using System.Net;

using NConsoler;

using NCrawler.Console.Extensions;
using NCrawler.Utils;

namespace NCrawler.Console
{
	internal class Program
	{
		#region Class Methods

		static bool s_showDownloadTimes;

		[Action]
		public static void Crawl([Required] string url,
			[Optional(1, AltNames = new[]{ "threads" }, Description = "Number of concurrent threads to use in crawl")] int threadCount,
			[Optional(false, AltNames = new[]{ "robotrules" }, Description = "Adhere to robot rules")] bool adhereToRobotRules,
			[Optional(0, Description = "Maximum crawl depth")] int depth,
			[Optional(20, AltNames = new[] { "connectiontimeout", "ct" }, Description = "Connection timeout in seconds(default 20)")] int connTimeout,
			[Optional(20, Description = "Read timeout in seconds(default 20)")] int timeout,
			[Optional(false, AltNames = new[] { "showtime", "sdt" }, Description = "Show download times")] bool showDownloadTimes,
			[Optional(0, Description = "Maximum number of downloads before stopping")] int maximumCrawlCount,
			[Optional(0, Description = "Maximum number of downloads errors before stopping")] int maximumHttpDownloadErrors,
			[Optional(0, Description = "Maximum crawl time in seconds before stopping")] int maximumCrawlTime,
			[Optional("NCrawl 2.1", Description = "User agent, default is NCrawl 2.1")] string userAgent)
		{
			AspectF.Define.
				Between("threads", threadCount, 1, 999).
				Between("depth", depth, 0, 999).
				Between("timeout", timeout, 0, 999).
				GreaterOrEqual("maximumCrawlCount", maximumCrawlCount, 0).
				GreaterOrEqual("maximumHttpDownloadErrors", maximumHttpDownloadErrors, 0).
				GreaterOrEqual("maximumCrawlTime", maximumCrawlTime, 0).
				Between("connectiontimeout", connTimeout, 0, 999);

			// Remove limits from Service Point Manager
			ServicePointManager.MaxServicePoints = 999999;
			ServicePointManager.DefaultConnectionLimit = 999999;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
			ServicePointManager.CheckCertificateRevocationList = true;
			ServicePointManager.EnableDnsRoundRobin = true;

			s_showDownloadTimes = showDownloadTimes;
			//using (Crawler crawler = new Crawler(new Uri(url), new HtmlDocumentProcessor()))
			//{
			//	crawler.UserAgent = userAgent;
			//	crawler.MaximumCrawlCount = maximumCrawlCount <= 0 ? (int?)null : maximumCrawlCount;
			//	crawler.MaximumHttpDownloadErrors = maximumHttpDownloadErrors <= 0 ? (int?)null : maximumHttpDownloadErrors;
			//	crawler.MaximumCrawlTime = maximumCrawlTime <= 0 ? (TimeSpan?)null : TimeSpan.FromSeconds(maximumCrawlTime);
			//	crawler.MaximumCrawlDepth = depth <= 0 ? (int?)null : depth;
			//	crawler.MaximumThreadCount = threadCount;
			//	crawler.AdhereToRobotRules = adhereToRobotRules;
			//	crawler.ConnectionTimeout = connTimeout <= 0 ? (TimeSpan?)null : TimeSpan.FromSeconds(connTimeout);
			//	crawler.ConnectionReadTimeout = timeout <= 0 ? (TimeSpan?)null : TimeSpan.FromSeconds(timeout);
			//	crawler.AfterDownload += CrawlerAfterDownload;
			//	crawler.PipelineException += CrawlerPipelineException;
			//	crawler.DownloadException += CrawlerDownloadException;
			//	crawler.Crawl();
			//}

			new CrawlerConfiguration()
				.Crawl(url)
				.Run();
		}

		private static void Main(string[] args)
		{
			Consolery.Run(typeof (Program), args);
		}

		#endregion
	}
}