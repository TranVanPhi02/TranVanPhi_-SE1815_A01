using BusinessObjects;
using Repositories;
using System.Threading.Tasks;

namespace Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _systemAccountRepository;
        private static readonly HashSet<short> DisabledUserIds = new HashSet<short>();
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

        public async Task<IQueryable<SystemAccount>> GetAllAccountsAsync()
        {
            var accounts = await _systemAccountRepository.GetAllAccountsAsync();
            return accounts ?? Enumerable.Empty<SystemAccount>().AsQueryable();

        }

        public async Task<SystemAccount> GetProfileAsync(short accountId)
        {
            return await _systemAccountRepository.GetProfileAsync(accountId);
        }

        public async Task<SystemAccount> LoginAsync(string email, string password)
        {
            var account = await _systemAccountRepository.GetAccountByEmailAsync(email);

            if (account == null || account.AccountPassword != password)
            {
                return null; 
            }

            if (await IsUserDisabledAsync(account.AccountId))
            {
                throw new UnauthorizedAccessException("Account is disabled"); 
            }

            return account;
        }


        // Register (used for Admin, Staff, Lecturer)
        public async Task RegisterAsync(SystemAccount account)
        {
            await _systemAccountRepository.AddOrUpdateAccountAsync(account);
        }

        public async Task DisableUserAsync(short accountId)
        {
            await _systemAccountRepository.DisableUserAsync(accountId);
        }

        public async Task EnableUserAsync(short accountId)
        {
            await _systemAccountRepository.EnableUserAsync(accountId);
        }

        public async Task<bool> IsUserDisabledAsync(short accountId)
        {
            return await _systemAccountRepository.IsUserDisabledAsync(accountId);
        }
    }
}
