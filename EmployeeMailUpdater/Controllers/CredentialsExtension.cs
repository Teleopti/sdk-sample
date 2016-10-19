using System;

namespace EmployeeMailUpdater.Controllers
{
	public static class CredentialsExtension
	{
		[ThreadStatic]
		private static AuthHeader _header;

		public static AuthHeader Header { get { return _header; } }

		public static void Set(AuthHeader header)
		{
			_header = header;
		}
	}
}