using System;
using DataFoundationAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static int init(out WDomain pDomain, string strServerAddress, UInt16 usServerPort, Guid guidDomainId, UInt32 ulStorageId)
		{
			int hRes;

			// load libraries and initialize thread
			ThreadInit.InitializeThread();

			// build domain object
			pDomain = WDomain.Create();

			if(0 > (hRes = pDomain.Initialize(guidDomainId)))
			{
				Console.WriteLine("Initialize failed (0x{0:x})", hRes);
				WDomain.Destroy(pDomain);
				ThreadInit.UninitializeThread();
				return -1;
			}

			// connect to server
			if(0 > (hRes = pDomain.Connect(strServerAddress, usServerPort, null)))
			{
				Console.WriteLine("Connect failed (0x{0:x})", hRes);
				pDomain.Uninitialize();
				WDomain.Destroy(pDomain);
				ThreadInit.UninitializeThread();
				return -1;
			}

			// connect to storage
			if(0 > (hRes = pDomain.QueryStorage(ulStorageId, false, null)))
			{
				Console.WriteLine("QueryStorage failed (0x{0:x})", hRes);
				pDomain.DisconnectAll();
				pDomain.Uninitialize();
				WDomain.Destroy(pDomain);
				ThreadInit.UninitializeThread();
				return -1;
			}

			// bind to schema
			if(0 > (hRes = AccessDefinition.Bind(pDomain)))
			{
				Console.WriteLine("Bind failed (0x{0:x})", hRes);
				pDomain.ReleaseStorage(ulStorageId);
				pDomain.DisconnectAll();
				pDomain.Uninitialize();
				WDomain.Destroy(pDomain);
				ThreadInit.UninitializeThread();
				return -1;
			}

			return 0;
		}
	}
}
