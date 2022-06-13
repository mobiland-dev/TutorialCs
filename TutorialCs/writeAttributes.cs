using System;
using DataFoundationAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static void writeAttributes(WSupplies pSupplies)
		{
			// Description
			string input;
			Console.WriteLine("Supplies description:");
			input = Console.ReadLine();
			pSupplies.SetDescription(input);

			// LastUpdated
			pSupplies.SetLastUpdated(DateTime.UtcNow);
			Console.WriteLine("Time was automatically set to current time.");

			// Store
			pSupplies.Store(Transaction.Store);

			// Execute
			int hRes;
			if(0 > (hRes = pSupplies.GetDomain().Execute(Transaction.Store)))
			{
				Console.WriteLine("Domain failed to execute the transaction (0x{0:x})", hRes);
			}
		}
	}
}
