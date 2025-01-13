using System;
using DataFSAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static void writeList(WSupplies pSupplies)
		{
			// Shops
			ShopList pList;
			pSupplies.SetShops(out pList);

			ShopListItem Item = new ShopListItem();

			// ShopName
			Console.WriteLine("Shop name:");
			string input1 = Console.ReadLine();
			Item.ShopName = input1;

			// ShopURL
			Console.WriteLine("Shop URL:");
			string input2 = Console.ReadLine();
			Item.ShopName = input2;

			uint _i;
			pList.Insert(out _i, Item);

			// Store
			pSupplies.Store(Transaction.Store); // we added an item to the list

			// Execute
			int hRes;
			if(0 > (hRes = pSupplies.GetDomain().Execute(Transaction.Store, null)))
			{
				Console.WriteLine("Domain failed to execute the transaction (0x{0:x})", hRes);
			}
		}
	}
}
