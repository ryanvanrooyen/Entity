
using UnityEngine;

namespace Entity
{
	public interface IPlatformSource
	{
		bool IsMac { get; }
		bool IsWindows { get; }
	}

	public class PlatformSource : IPlatformSource
	{
		public bool IsMac
		{
			get
			{
				return IsAnyPlatform(
					RuntimePlatform.OSXDashboardPlayer,
					RuntimePlatform.OSXEditor,
					RuntimePlatform.OSXPlayer);
			}
		}

		public bool IsWindows
		{
			get
			{
				return IsAnyPlatform(
					RuntimePlatform.WindowsEditor,
					RuntimePlatform.WindowsPlayer,
					RuntimePlatform.WSAPlayerARM,
					RuntimePlatform.WSAPlayerX64,
					RuntimePlatform.WSAPlayerX86,
					RuntimePlatform.XBOX360,
					RuntimePlatform.XboxOne);
			}
		}

		private bool IsAnyPlatform(params RuntimePlatform[] platforms)
		{
			var currentPlatform = Application.platform;
			return platforms.Any(p => p == currentPlatform);
		}
	}
}