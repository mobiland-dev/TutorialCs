using System;
using DataFSAccess;

namespace TutorialCs
{
	partial class Tutorial
	{
		static readonly Guid guidEntryPoint = new Guid("{71E456A5-6A4E-46aa-A727-105D584C7917}");

		static void Main(string[] args)
		{
			// ServerAddress, ServerPort
			String strServerAddress = args[0];
			UInt16 usServerPort = UInt16.Parse(args[1]);

			// Domain-Guid
			Guid guidDomainId = Guid.Parse(args[2]);

			// Storage-ID (we are using the default 0 here)
			UInt32 ulStorageId = 0;

			WDomain pDomain;

			if (0 != init(out pDomain, strServerAddress, usServerPort, guidDomainId, ulStorageId))
			{
				Console.WriteLine("init failed");
				return;
			}

			Guid[] aguidEntryPoint = new Guid[1]; aguidEntryPoint[0] = guidEntryPoint;

			int action;
			WSupplies pSupplies = null;
			WInventory pInventory = null;

			do
			{
				Console.WriteLine("\nselect function:\n  1: create new Supplies object\n  2: open existing Supplies object\n\n  3: create new Inventory object\n  4: extend Supplies object to Inventory object\n  5: open existing Inventory object\n");

				action = (int)Console.ReadKey(true).KeyChar;

				switch (action)
				{
					case '1':
						{
							WSupplies.Create(out pSupplies, pDomain, DataFS.ObjectId.OBJECTID_NULL);

							pDomain.InsertNamedLink(pSupplies.BuildLink(true), guidEntryPoint, "basic entry point", Transaction.Store);

							// Store
							pSupplies.Store(Transaction.Store);

							// Execute
							int hRes;
							if (0 > (hRes = pDomain.Execute(Transaction.Store, null)))
							{
								Console.WriteLine("Domain failed to execute the transaction (0x{0:x})", hRes);

								pSupplies = null;
							}
						}
						break;

					case '2':
						{
							DataFS.Array<DataFS.stcObjectLink> apSuppliesLink = new DataFS.Array<DataFS.stcObjectLink>();
							if (0 == pDomain.QueryNamedLink(aguidEntryPoint, apSuppliesLink, null))
							{
								if (WSupplies.IsOfType(apSuppliesLink.pData[0]))
								{
									WSupplies.Open(out pSupplies, pDomain, apSuppliesLink.pData[0].oiObjectId, 0, Transaction.Load);
									pSupplies.Load(_WSupplies.ALL_ATTRIBUTES, Transaction.Load);

									if (0 > pDomain.Execute(Transaction.Load, null))
									{
										Console.WriteLine("Execute failed...");
										uninit(pDomain, ulStorageId);
										return;
									}
								}
								else
									Console.WriteLine("Object is not of type WSupplies");
							}
							else
								Console.WriteLine("Entry point not found");
						}
						break;

					case '3':
						{
							WInventory.Create(out pInventory, pDomain, DataFS.ObjectId.OBJECTID_NULL);

							pDomain.InsertNamedLink(pInventory.BuildLink(true), guidEntryPoint, "extended entry point", Transaction.Store);

							pSupplies = WSupplies.CastTo(pInventory.GetObject());

							// Store
							pInventory.Store(Transaction.Store);

							// Execute
							int hRes;
							if (0 > (hRes = pDomain.Execute(Transaction.Store, null)))
							{
								Console.WriteLine("Domain failed to execute the transaction (0x{0:x})", hRes);

								pInventory = null;
								pSupplies = null;
							}
						}
						break;

					case '4':
						{
							DataFS.Array<DataFS.stcObjectLink> apSuppliesLink = new DataFS.Array<DataFS.stcObjectLink>();
							if (0 == pDomain.QueryNamedLink(aguidEntryPoint, apSuppliesLink, null))
							{
								if (WInventory.IsOfType(apSuppliesLink.pData[0]))
								{
									Console.WriteLine("Object is already of type WInventory");
								}
								else if (WSupplies.IsOfType(apSuppliesLink.pData[0]))
								{
									WSupplies.Open(out pSupplies, pDomain, apSuppliesLink.pData[0].oiObjectId, 0, Transaction.Load);
									pSupplies.Load(_WSupplies.ALL_ATTRIBUTES, Transaction.Load);

									if (0 > pDomain.Execute(Transaction.Load, null))
									{
										Console.WriteLine("Execute failed...");
										uninit(pDomain, ulStorageId);
										return;
									}

									WInventory.Extend(out pInventory, pSupplies.GetObject());

									pDomain.InsertNamedLink(pInventory.BuildLink(true), guidEntryPoint, "updated entry point", Transaction.Store);

									// Store
									pInventory.Store(Transaction.Store);

									// Execute
									int hRes;
									if (0 > (hRes = pDomain.Execute(Transaction.Store, null)))
									{
										Console.WriteLine("Domain failed to execute the transaction (0x{0:x})", hRes);

										pInventory = null;
										pSupplies = null;
									}
								}
								else
									Console.WriteLine("Object is not of type WSupplies");
							}
						}
						break;

					case '5':
						{
							DataFS.Array<DataFS.stcObjectLink> apInventoryLink = new DataFS.Array<DataFS.stcObjectLink>();
							if (0 == pDomain.QueryNamedLink(aguidEntryPoint, apInventoryLink, null))
							{
								if (WInventory.IsOfType(apInventoryLink.pData[0]))
								{
									WInventory.Open(out pInventory, pDomain, apInventoryLink.pData[0].oiObjectId, 0, Transaction.Load);
									pInventory.Load(_WInventory.ALL_ATTRIBUTES, Transaction.Load);

									if (0 > pDomain.Execute(Transaction.Load, null))
									{
										Console.WriteLine("Execute failed...");
										uninit(pDomain, ulStorageId);
										return;
									}

									pSupplies = WSupplies.CastTo(pInventory.GetObject());
								}
								else if (WSupplies.IsOfType(apInventoryLink.pData[0]))
								{
									Console.WriteLine("Object is not of type WInventory but of type WSupplies");
								}
								else
								{
									Console.WriteLine("Object is not of type WInventory");
								}
							}
							else
								Console.WriteLine("Entry point not found");
						}
						break;
				}

			} while (pSupplies == null);

			do
			{
				if (pInventory == null)
					Console.WriteLine("\nselect function:\n  1: writeAttributes\n  2: readAttribute\n  3: writeList\n  4: readList\n  q: quit\n");
				else
					Console.WriteLine("\nselect function:\n  1: writeAttributes\n  2: readAttribute\n  3: writeList\n  4: readList\n  5: writeObject\n  6: readObject\n  7: writeObjectList\n  8: readObjectList\n  9: modifyObjectInList\n  q: quit\n");

				action = (int)Console.ReadKey(true).KeyChar;

				if (pInventory == null)
				{
					if ((action >= '5') && (action <= '9'))
						action = 0;
				}

				switch (action)
				{
					case '1':
						Console.WriteLine("1: writeAttributes");
						writeAttributes(pSupplies);
						break;

					case '2':
						Console.WriteLine("2: readAttributes");
						readAttributes(pSupplies);
						break;

					case '3':
						Console.WriteLine("3: writeList");
						writeList(pSupplies);
						break;

					case '4':
						Console.WriteLine("4: readList");
						readList(pSupplies);
						break;

					case '5':
						Console.WriteLine("5: writeObject");
						writeObject(pInventory);
						break;

					case '6':
						Console.WriteLine("6: readObject");
						readObject(pInventory);
						break;

					case '7':
						Console.WriteLine("7: writeObjectList");
						writeObjectList(pInventory);
						break;

					case '8':
						Console.WriteLine("8: readObjectList");
						readObjectList(pInventory);
						break;
					case '9':
						Console.WriteLine("9: modifyObjectInList");
						modifyObjectInList(pInventory);
						break;

					default:
						Console.WriteLine("invalid selection");
						break;

					case 'q':
						break;
				}

			} while (action != 'q');

			if (pSupplies != null)
				pSupplies.Dispose();
			if (pInventory != null)
				pInventory.Dispose();

			uninit(pDomain, ulStorageId);
		}
	}
}
