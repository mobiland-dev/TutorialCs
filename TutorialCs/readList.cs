using System;
using DataFoundationAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static void readList(WSupplies pSupplies)
		{
			int hRes;

			ShopList pList;
			if (0 == (hRes = pSupplies.GetShops(out pList)))
			{
				Console.WriteLine("All ShopList items ({0})\n", pList.GetLength());

				for (UInt32 i = 0; i < pList.GetLength(); i++)
				{
					ShopListItem Item;
					pList.Get(i, out Item);
					Console.WriteLine("Shop {0:F}", i);
					Console.WriteLine("Shop Name: {0}", Item.ShopName);
					Console.WriteLine("Shop URL: {0}\n", Item.ShopURL);
				}
			}
			else
				Console.WriteLine("Cannot read shop list (0x{0:x})", hRes);
		}
	}
}
