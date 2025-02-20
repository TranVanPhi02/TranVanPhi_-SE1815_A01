using BusinessObjects;
using Repositories;
using System.Threading.Tasks;

namespace Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public SystemAccountService(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        public async Task EditProfileAsync(SystemAccount updatedAccount)
        {
              await _systemAccountRepository.EditProfileAsync(updatedAccount);
        }

        // Get a system account by email
        public async Task<SystemAccount> GetAccountByEmailAsync(string email)
        {
            return await _systemAccountRepository.GetAccountByEmailAsync(email);
        }

        public async Task<SystemAccount> GetProfileAsync(short accountId)
        {
            return await _systemAccountRepository.GetProfileAsync(accountId);
        }

        // Login 
        public async Task<SystemAccount> LoginAsync(string email, string password)
        {
            var account = await _systemAccountRepository.GetAccountByEmailAsync(email);
            if (account != null && account.AccountPassword == password)
            {
                return account;
            }

            return null; // Return null if login failed
        }

        // Register (used for Admin, Staff, Lecturer)
        public async Task RegisterAsync(SystemAccount account)
        {
            await _systemAccountRepository.AddOrUpdateAccountAsync(account);
        }
    }
}
