using System;
using DataFoundationAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		public static void writeObjectList(WInventory pInventory)
		{
			// create object
			WShopArticle pShopArticle;
			WShopArticle.Create(out pShopArticle, pInventory);

			// ArticleName
			Console.WriteLine("Article name:");
			string input1 = Console.ReadLine();
			pShopArticle.SetArticleName(input1);

			// Count
			Console.WriteLine("Count:");
			ushort input2 = ushort.Parse(Console.ReadLine());
			pShopArticle.SetCount(input2);

			// insert objectlink into list
			ArticleList pArticleList;
			pInventory.SetItems(out pArticleList);

			ArticleListItem Item = new ArticleListItem();
			Item.WShopArticle = pShopArticle;

			uint _i;
			pArticleList.Insert(out _i, Item);

			// Store
			pShopArticle.Store(Transaction.Store);	// new object
			pInventory.Store(Transaction.Store);	// we added an item to the list

			// Execute
			int hRes;
			if(0 > (hRes = pInventory.GetDomain().Execute(Transaction.Store)))
			{
				Console.WriteLine("Domain failed to execute the transaction (0x{0:x})", hRes);
			}
		}
	}
}
