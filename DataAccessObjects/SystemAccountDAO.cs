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
        // Get all accounts (Admin only)
        public async Task<IQueryable<SystemAccount>> GetAllAccountsAsync()
        {
            return _context.SystemAccounts;
        }
        // Disable a user - Prevent Admin from disabling their own account
        public async Task DisableUserAsync(short accountId, short currentAccountId)
        {
            var account = await _context.SystemAccounts
                                         .FirstOrDefaultAsync(a => a.AccountId == accountId);

            var currentAccount = await _context.SystemAccounts
                                                .FirstOrDefaultAsync(a => a.AccountId == currentAccountId);

            // Prevent disabling if the account is Admin and is trying to disable their own account
            if (account != null && currentAccount != null)
            {
                if (currentAccount.AccountRole == 3 && account.AccountId == currentAccountId) // Assuming AccountRole == 1 is Admin
                {
                    throw new InvalidOperationException("You cannot disable your own account.");
                }

                account.IsDisabled = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task EnableUserAsync(short accountId)
        {
            var account = await _context.SystemAccounts
                                         .FirstOrDefaultAsync(a => a.AccountId == accountId);
            if (account != null)
            {
                account.IsDisabled = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUserDisabledAsync(short accountId)
        {
            var account = await _context.SystemAccounts
                                         .FirstOrDefaultAsync(a => a.AccountId == accountId);
            return account?.IsDisabled ?? false;
        }

    }
}
