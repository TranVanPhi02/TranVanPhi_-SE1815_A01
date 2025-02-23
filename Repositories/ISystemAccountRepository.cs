using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ISystemAccountRepository
    {
        Task<SystemAccount> GetAccountByEmailAsync(string email);
        Task AddOrUpdateAccountAsync(SystemAccount account);
        Task<SystemAccount> GetProfileAsync(short accountId);
        Task EditProfileAsync(SystemAccount updatedAccount);
        Task<IQueryable<SystemAccount>> GetAllAccountsAsync();
        Task DisableUserAsync(short accountId);
        Task EnableUserAsync(short accountId);
        Task<bool> IsUserDisabledAsync(short accountId);
    }
}
