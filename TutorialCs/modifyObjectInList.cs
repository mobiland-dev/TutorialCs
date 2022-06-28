using System;
using DataFoundationAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static void modifyObjectInList(WInventory pInventory)
		{
			Console.WriteLine("List index:");
			UInt32 indexInput = UInt32.Parse(Console.ReadLine());

			int hRes;

			ArticleList pList;
			if(0 != (hRes = pInventory.GetItems(out pList)))
			{
				Console.WriteLine("Cannot read article list (0x{0:x})", hRes);
				return;
            }
			
			ArticleListItem pOldItem;
			if(0 != (hRes = pList.Get(indexInput, out pOldItem)))
            {
				Console.WriteLine("There is no article with this index");
				return;
            }

			WShopArticle pArticle;
			WShopArticle.Open(out pArticle, pOldItem.Article.oiObjectId, pInventory, Transaction.Load);

			// Load
			pArticle.Load(_WShopArticle.ALL_ATTRIBUTES, Transaction.Load);
		
			// Execute
			if(0 > (hRes = pInventory.GetDomain().Execute(Transaction.Load)))
			{
				Console.WriteLine("Domain failed to execute the transaction (0x{0:x})", hRes);
			}
			else
			{
				Console.WriteLine("Article name:");
				string input1 = Console.ReadLine();
				if(input1.Length > 0)
					pArticle.SetArticleName(input1);

				Console.WriteLine("Count:");
				string input2 = Console.ReadLine();
				if(input2.Length > 0)
					pArticle.SetCount(ushort.Parse(input2));

				// Store
				pArticle.Store(Transaction.Store);

				// Execute
				if(0 > (hRes = pInventory.GetDomain().Execute(Transaction.Store)))
				{
					Console.WriteLine("Domain failed to execute the transaction (0x{0:x})", hRes);
				}
			}
		}
	}
}
