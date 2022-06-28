using System;
using DataFoundationAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static void uninit(WDomain pDomain, UInt32 ulStorageId)
		{
			// unbind from schema
			AccessDefinition.Unbind();

			// disconnect from storage
			pDomain.ReleaseStorage(ulStorageId);

			// disconnect from server
			pDomain.DisconnectAll();

			// give more time for cleanup (prevents false error messages from the debugger)
			System.Threading.Thread.Sleep(400);

			// delete domain object
			pDomain.Uninitialize();
			WDomain.Destroy(pDomain);

			// uninitialize thread and free libraries
			ThreadInit.UninitializeThread();
		}
	}
}
