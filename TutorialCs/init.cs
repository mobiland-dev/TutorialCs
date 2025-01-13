using System;
using DataFSAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static int init(out WDomain pDomain, string strServerAddress, UInt16 usServerPort, Guid guidDomainId, UInt32 ulStorageId)
		{
			int hRes;

			// load libraries and initialize thread
			ThreadInit.InitializeThread();

			// build connection object
			Connection pConnection = Connection.Create();

			if(0 > (hRes = pConnection.Initialize(guidDomainId)))
			{
				Console.WriteLine("Initialize connection failed (0x{0:x})", hRes);
				Connection.Destroy(pConnection);
				ThreadInit.UninitializeThread();
				pDomain = null;
				return -1;
			}

			// connect to server
			if(0 > (hRes = pConnection.Connect(strServerAddress, usServerPort, null, null)))
			{
				Console.WriteLine("Connect failed (0x{0:x})", hRes);
				pConnection.Uninitialize();
				Connection.Destroy(pConnection);
				ThreadInit.UninitializeThread();
				pDomain = null;
				return -1;
			}

			// connect to storage
			if(0 > (hRes = pConnection.QueryStorage(ulStorageId, false, null)))
			{
				Console.WriteLine("QueryStorage failed (0x{0:x})", hRes);
				pConnection.DisconnectAll();
				pConnection.Uninitialize();
				Connection.Destroy(pConnection);
				ThreadInit.UninitializeThread();
				pDomain = null;
				return -1;
			}

			// build domain object
			pDomain = WDomain.Create();

			if (0 > (hRes = pDomain.Initialize(pConnection, 0)))
			{
				Console.WriteLine("Initialize domain failed (0x{0:x})", hRes);
				WDomain.Destroy(pDomain);
				pConnection.ReleaseStorage(ulStorageId);
				pConnection.DisconnectAll();
				pConnection.Uninitialize();
				Connection.Destroy(pConnection);
				ThreadInit.UninitializeThread();
				pDomain = null;
				return -1;
			}

			// bind to schema
			if (0 > (hRes = AccessDefinition.Bind(pDomain)))
			{
				Console.WriteLine("Bind failed (0x{0:x})", hRes);
				pDomain.Uninitialize();
				WDomain.Destroy(pDomain);
				pConnection.ReleaseStorage(ulStorageId);
				pConnection.DisconnectAll();
				pConnection.Uninitialize();
				Connection.Destroy(pConnection);
				ThreadInit.UninitializeThread();
				pDomain = null;
				return -1;
			}

			return 0;
		}
	}
}
