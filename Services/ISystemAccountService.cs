using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ISystemAccountService
    {
        Task<SystemAccount> LoginAsync(string email, string password);
        Task RegisterAsync(SystemAccount account);
        Task<SystemAccount> GetAccountByEmailAsync(string email);
        Task<SystemAccount> GetProfileAsync(short accountId);
        Task EditProfileAsync(SystemAccount updatedAccount);
        Task<IQueryable<SystemAccount>> GetAllAccountsAsync();

        Task DisableUserAsync(short accountId, short currentAccountId);

        Task EnableUserAsync(short accountId);
        Task<bool> IsUserDisabledAsync(short accountId);
    }
}
