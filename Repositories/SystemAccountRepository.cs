using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly SystemAccountDAO _systemAccountDAO;

        public SystemAccountRepository(SystemAccountDAO systemAccountDAO)
        {
            _systemAccountDAO = systemAccountDAO;
        }
        public async Task<SystemAccount> GetAccountByEmailAsync(string email)
        {
            return await _systemAccountDAO.GetAccountByEmailAsync(email);
        }

        public async Task AddOrUpdateAccountAsync(SystemAccount account)
        {
            await _systemAccountDAO.AddOrUpdateAccountAsync(account);
        }

        public async Task<SystemAccount> GetProfileAsync(short accountId)
        {
            return await _systemAccountDAO.GetProfileAsync(accountId);
        }

        public async Task EditProfileAsync(SystemAccount updatedAccount)
        {
            await _systemAccountDAO.EditProfileAsync(updatedAccount);
        }
    }
}
