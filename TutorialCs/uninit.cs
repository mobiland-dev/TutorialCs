using System;
using DataFSAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static void uninit(WDomain pDomain, UInt32 ulStorageId)
		{
			Connection pConnection = pDomain.GetConnection();

			// unbind from schema
			AccessDefinition.Unbind();

			// delete domain object
			pDomain.Uninitialize();
			WDomain.Destroy(pDomain);

			// disconnect from storage
			pConnection.ReleaseStorage(ulStorageId);

			// disconnect from server
			pConnection.DisconnectAll();

			// delete connection object
			pConnection.Uninitialize();
			Connection.Destroy(pConnection);

			// uninitialize thread and free libraries
			ThreadInit.UninitializeThread();
		}
	}
}
