using System;
using DataFSAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static void readObject(WInventory pInventory)
		{
			int hRes;

			// Open
			WResponsible pResponsible;
			if (0 > (hRes = pInventory.OpenManager(out pResponsible, 0, Transaction.Load)))
			{
				// Load
				pResponsible.Load(_WResponsible.ALL_ATTRIBUTES, Transaction.Load);

				// Execute
				if (0 > (hRes = pResponsible.GetDomain().Execute(Transaction.Load, null)))
				{
					Console.WriteLine("Domain failed to execute the transaction (0x{0:x})", hRes);
				}
				else
				{
					// FullName
					string strFullName;
					pResponsible.GetFullName(out strFullName);
					Console.WriteLine("Responsible full name: {0}", strFullName);

					// Comment
					string strComment;
					pResponsible.GetFullName(out strComment);
					Console.WriteLine("Responsible comment: {0}", strComment);
				}
			}
			else
				Console.WriteLine("Cannot open responsible (0x{0:x})", hRes);
		}
	}
}
