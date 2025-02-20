using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class SystemAccountDAO
    {
        private readonly FunewsManagementContext _context;

        public SystemAccountDAO(FunewsManagementContext context)
        {
            _context = context;
        }

        // Get a system account by email (used for login)
        public async Task<SystemAccount> GetAccountByEmailAsync(string email)
        {
            return await _context.SystemAccounts
                                 .FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        // Get a system account by AccountId
        public async Task<SystemAccount> GetProfileAsync(short accountId)
        {
            return await _context.SystemAccounts
                                 .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        // Add or update a system account
        public async Task AddOrUpdateAccountAsync(SystemAccount account)
        {
            if (_context.SystemAccounts.Any(a => a.AccountEmail == account.AccountEmail))
            {
                _context.Update(account);
            }
            else
            {
                _context.Add(account);
            }

            await _context.SaveChangesAsync();
        }

        // Edit profile - Update a system account by AccountId
        public async Task EditProfileAsync(SystemAccount updatedAccount)
        {
            var account = await _context.SystemAccounts
                                         .FirstOrDefaultAsync(a => a.AccountId == updatedAccount.AccountId);

            if (account != null)
            {
                // Update the properties of the account
                account.AccountName = updatedAccount.AccountName;
                account.AccountEmail = updatedAccount.AccountEmail; // Optional if you want to allow email changes
                account.AccountPassword = updatedAccount.AccountPassword; // Optional if you want to update the password

                await _context.SaveChangesAsync();
            }
        }
    }
}
