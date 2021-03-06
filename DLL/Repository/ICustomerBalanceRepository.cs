﻿using System;
using System.Threading.Tasks;
using DLL.DbContext;
using DLL.Model;
using DLL.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository
{
    public interface ICustomerBalanceRepository : IRepositoryBase<CustomerBalance>
    {
        Task UpdateCustomerBalanceAsync(decimal amount);
    }

    public class CustomerBalanceRepository : RepositoryBase<CustomerBalance>, ICustomerBalanceRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerBalanceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateCustomerBalanceAsync(decimal amount)
        {
            var customerBalance = await _context.CustomerBalances.FirstOrDefaultAsync(x => x.CustomerBalanceId == 1);
            customerBalance.Balance += amount;
            var saved = false;

            do
            {
                try
                {
                    _context.CustomerBalances.Update(customerBalance);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        saved = true;
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        // if (!(entry.Entity is CustomerBalance)) continue;

                        if (entry.Entity is CustomerBalance)
                        {
                            var databaseEntry = entry.GetDatabaseValues();
                            var databaseValue = (CustomerBalance) databaseEntry.ToObject();
                            databaseValue.Balance += amount;
                            entry.OriginalValues.SetValues((databaseEntry));
                            entry.CurrentValues.SetValues((databaseValue));
                        }
                    }
                }
            } while (!saved);
        }
    }
}