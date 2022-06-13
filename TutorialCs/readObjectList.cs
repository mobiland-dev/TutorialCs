using System;
using DataFoundationAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static void readObjectList(WInventory pInventory)
		{
			int hRes;

			ArticleList pList;
			if(0 == (hRes = pInventory.GetItems(out pList)))
			{
				WShopArticle[] apArticle = new WShopArticle[pList.GetLength()];

				for(UInt32 i = 0; i < pList.GetLength(); i++)
				{
					ArticleListItem pItem;
					pList.Get(i, out pItem);

					// Open
					WShopArticle.Open(out apArticle[i], pItem.ShopArticle.oiObjectId, pInventory, Transaction.Load);

					// Load
					apArticle[i].Load(_WShopArticle.ALL_ATTRIBUTES, Transaction.Load);
				}

				// Execute
				if(0 > (hRes = pInventory.GetDomain().Execute(Transaction.Load)))
				{
					Console.WriteLine("Domain failed to execute the transaction (0x{0:x})", hRes);
				}
				else
				{
					Console.WriteLine("All ArticleList items ({0})\n", pList.GetLength());

					for(UInt32 i = 0; i < pList.GetLength(); i++)
					{
						string strArticleName;
						apArticle[i].GetArticleName(out strArticleName);

						UInt16 usCount;
						apArticle[i].GetCount(out usCount);

						Console.WriteLine("LinkIndex: {0} || Article: {1}x {2}", i, usCount, strArticleName);
					}
				}
			}
			else
				Console.WriteLine("Cannot read article list (0x{0:x}))", hRes);
		}
	}
}
