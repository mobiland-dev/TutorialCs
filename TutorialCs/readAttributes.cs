using System;
using DataFSAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static void readAttributes(WSupplies pSupplies)
		{
			int hRes;

			// Description
			string strDescription;
			if(0 == (hRes = pSupplies.GetDescription(out strDescription)))
				Console.WriteLine("Supplies description: {0}", strDescription);
			else
				Console.WriteLine("Cannot read supplies description (0x{0:x})", hRes);

			// LastUpdated 
			DateTime dtCurrentTime;
			if(0 == (hRes = pSupplies.GetLastUpdated(out dtCurrentTime)))
				Console.WriteLine("Time last updated: {0} (UTC)", dtCurrentTime.ToLongTimeString());
			else
				Console.WriteLine("Cannot read supplies time (0x{0:x})", hRes);
		}
	}
}
