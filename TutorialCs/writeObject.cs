using System;
using DataFoundationAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static void writeObject(WInventory pInventory)
		{
			// create object
			WResponsible pResponsible;
			WResponsible.Create(out pResponsible, pInventory);

			// FullName
			Console.WriteLine("Full name:");
			string input1 = Console.ReadLine();
			pResponsible.SetFullName(input1);

			// Comment
			Console.WriteLine("Comment:");
			string input2 = Console.ReadLine();
			pResponsible.SetComment(input2);

			// set objectlink
			pInventory.LinkManager(pResponsible);

			// Store
			pResponsible.Store(Transaction.Store);  // new object
			pInventory.Store(Transaction.Store);

			// Execute
			int hRes;
			if(0 > (hRes = pInventory.GetDomain().Execute(Transaction.Store)))
			{
				Console.WriteLine("Domain failed to execute the transaction (0x{0:x})", hRes);
			}

			pResponsible.Dispose();
		}
	}
}
