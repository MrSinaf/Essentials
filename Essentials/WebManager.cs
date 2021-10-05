using System.ComponentModel;
using System.Net;
using System;

namespace SinafProduction.Essentials
{
	public static class WebManager
	{
		public static bool HasConnection()
		{
			try
			{
				Dns.GetHostEntry("google.com");
				return true;
			}
			catch { return false; }
		}

		public static WebClient DownloadFile(string url, string path,
												  Action<DownloadProgressChangedEventArgs> progress,
												  Action<AsyncCompletedEventArgs> completed)
		{
			var client = new WebClient();
			client.DownloadFileCompleted += (_,downloadCompleted) => completed(downloadCompleted);
			client.DownloadProgressChanged += (_, downloadProgress) => progress(downloadProgress);
			client.DownloadFileAsync(new Uri(url), path);

			return client;
		}
		
		public static string DownloadFile(string urlFile)
		{
			var client = new WebClient();
			return client.DownloadString(urlFile);
		}
	}
}