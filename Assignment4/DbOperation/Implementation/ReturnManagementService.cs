using DbOperation.Interface;
using DbOperation.Models;
using DbOperation.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbOperation.Implementation
{
    public class ReturnManagementService :IReturnmanagementService
    {
        private readonly DbContextOptions<Assignment4Context> _dbConn;

        public ReturnManagementService(string context)
        {
            _dbConn = new DbContextOptionsBuilder<Assignment4Context>().UseSqlServer(context).Options;
        }

        #region AddMultipleReturns
        public bool AddMultipleReturns(List<ReturnManagement> items)
        {
            bool allAdded = true;

            using var Db = new Assignment4Context(_dbConn);
            try
            {
                Db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ReturnManagement] ON");

                if (items.Count > 0)
                {
                    Db.Add(items[0]);
                    Db.SaveChanges();

                    int returnId = items[0].returnId;

                    foreach (var item in items.Skip(1))
                    {
                        item.returnId = returnId;
                        Db.Add(item);
                        Db.SaveChanges();
                    }
                }

                Db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ReturnManagement] OFF");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region AddReturn
        public bool AddReturn(ReturnManagement returnData, List<ReturnItems> returnItems)
        {
            using var Db = new Assignment4Context(_dbConn);
            try
            {
                returnData.sUser = "normaluser";
                returnData.itemId = 1;
                Db.Add(returnData);
                Db.SaveChanges();

                foreach (var item in returnItems)
                {
                    item.fkReturnId = returnData.returnId;
                }
                AddReturnItems(returnItems);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region AddReturnItems
        public bool AddReturnItems(List<ReturnItems> returnItems)
        {
            using var Db = new Assignment4Context(_dbConn);
            try
            {
                foreach (var item in returnItems)
                {
                    Db.Add(item);
                    Db.SaveChanges();
                    var inventoryItem = Db.Inventory.FirstOrDefault(i => i.itemID == item.fkReuseDestianationItemId);
                    if (inventoryItem != null)
                    {
                        inventoryItem.createdDate = DateTime.Now;
                        inventoryItem.updatedDate = DateTime.Now;
                        inventoryItem.sUser = "User";
                        inventoryItem.quantity += (decimal)item.quantity;
                        Db.Update(inventoryItem);
                        Db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region UpdateReturn
        public bool UpdateReturn(ReturnManagement returnData, List<ReturnItems> returnItems)
        {
            using var Db = new Assignment4Context(_dbConn);
            var existingReturn = Db.ReturnManagement.FirstOrDefault(r => r.returnId == returnData.returnId);
            if (existingReturn != null)
            {
                existingReturn.itemId = 1;
                existingReturn.returnDescription = returnData.returnDescription;
                existingReturn.updatedDate = DateTime.Now;
                existingReturn.TotalReturnPrice = returnData.TotalReturnPrice;
                existingReturn.sUser = "User";
                Db.Update(existingReturn);

                Db.SaveChanges();

                foreach (var item in returnItems)
                {
                    item.fkReturnId = returnData.returnId;
                }
                UpdateReturns(returnItems);
                return true;
            }
            return false;
        }
        #endregion

        #region UpdateReturns
        public void UpdateReturns(List<ReturnItems> returnItems)
        {
            using var Db = new Assignment4Context(_dbConn);
            try
            {
                foreach (var item in returnItems)
                {
                    var existingItem = Db.ReturnItems.FirstOrDefault(r => r.fkReturnId == item.fkReturnId&&r.fkInventoryItemId==item.fkInventoryItemId);
                    if (existingItem != null)
                    {
                        existingItem.quantity = item.quantity;
                        existingItem.fkReturnId = item.fkReturnId;
                        existingItem.unit = item.unit;
                        existingItem.returnReason = item.returnReason;
                        existingItem.returnDescription = item.returnDescription;
                        existingItem.discount = item.discount;
                        Db.Update(existingItem);
                        Db.SaveChanges();

                        var inventoryItem = Db.Inventory.FirstOrDefault(i => i.itemID == item.fkReuseDestianationItemId);
                        if (inventoryItem != null)
                        {
                            inventoryItem.createdDate = DateTime.Now;
                            inventoryItem.updatedDate = DateTime.Now;
                            inventoryItem.sUser = "User";
                            inventoryItem.quantity += (decimal)item.quantity;
                            Db.Update(inventoryItem);
                            Db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region DeleteReturn
        public bool DeleteReturn(int returnId)
        {
            DeleteReturnItems(returnId);
            using var Db = new Assignment4Context(_dbConn);
            try
            {
                var returnData = Db.ReturnManagement.FirstOrDefault(r => r.returnId == returnId);
                if (returnData != null)
                {
                   
                    Db.Remove(returnData);
                    Db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region DeleteReturnItems
        public bool DeleteReturnItems(int returnManagementId)
        {
            using var Db = new Assignment4Context(_dbConn);
            try
            {
                var items = Db.ReturnItems.Where(x => x.fkReturnId == returnManagementId).ToList();
                Db.RemoveRange(items);
                Db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
       
    

        #endregion

        #region GetReturnById
        public ReturnManagement GetReturnById(int returnId)
        {
            using var Db = new Assignment4Context(_dbConn);
            return Db.ReturnManagement.FirstOrDefault(r => r.returnId == returnId);
        }
        #endregion

        #region GetReturnItems
        public List<ReturnItems> GetReturnItems()
        {
            using var Db = new Assignment4Context(_dbConn);
            return Db.ReturnItems.ToList();
        }
        #endregion

        #region GetInventoryItems
        public List<InventoryItems> GetInventoryItems()
        {
            using var Db = new Assignment4Context(_dbConn);
            var inventoryItems = Db.InventoryItems.Where(x => x.itemType == "finishedgoods").ToList();
            return inventoryItems;
        }
        #endregion

        public List<ReturnManagementViewModal> GetReturns()
        {
            using (var context = new Assignment4Context(_dbConn))
            {
                var result = (from rm in context.ReturnManagement
                              join c in context.Customers on rm.fkcotomerID equals c.customerId into custGroup
                              from c in custGroup.DefaultIfEmpty() // LEFT JOIN to handle null customers
                              select new ReturnManagementViewModal
                              {
                                  returnId = rm.returnId,
                                  customerId = rm.fkcotomerID ?? 0, // Default to 0 if null
                                  returnDescription = rm.returnDescription,
                                  TotalAmount = rm.TotalReturnPrice ?? 0, // Default to 0 if null
                                  customerName = c != null ? c.customerName : "Unknown" ,// Handle null customer
                                  returnDate=rm.returnDate,
                                  returnPrice = rm.TotalReturnPrice ?? 0,
                              }).ToList();

                return result;
            }
        }


        public List<ReturnItemViewModel> GetReturnItemsWithItemNameWithID(int returnId)
        {
            using (var context = new Assignment4Context(_dbConn))
            {
                // Step 1: Check if return data exists
                var returnData = context.ReturnItems
                                        .Where(r => r.fkReturnId == returnId)
                                        .FirstOrDefault();

                if (returnData == null)
                {
                    return new List<ReturnItemViewModel>(); // Return empty list if no return data
                }

                // Step 2: Get associated return items with item names
                var returnItems = (from ri in context.ReturnItems
                                   join ii in context.InventoryItems on ri.fkInventoryItemId equals ii.itemId
                                   where ri.fkReturnId == returnId
                                   select new ReturnItemViewModel
                                   {
                                       returnItemTblId = ri.returnItemTblId,
                                       fkInventoryItemId = ri.fkInventoryItemId,
                                       quantity = ri.quantity,
                                       returnReason = ri.returnReason,
                                       returnDescription = ri.returnDescription,
                                       returnPrice = ri.returnPrice,
                                       unit = ri.unit,
                                       discount = ri.discount,
                                       ReuseDestination = ri.ReuseDestination,
                                       fkReuseDestianationItemId = ri.fkReuseDestianationItemId,
                                       itemName = ii.itemName,
                                      
                                       PricePerUnit = (decimal)ii.pricePerUnit,

                                   }).ToList();

                return returnItems;
            }
        }



    }
}
