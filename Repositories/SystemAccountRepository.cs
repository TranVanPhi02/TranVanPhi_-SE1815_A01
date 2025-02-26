using BusinessObjects;
using DataAccessObjects;
using Repositories;

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

    public Task<IQueryable<SystemAccount>> GetAllAccountsAsync()
    {
        return _systemAccountDAO.GetAllAccountsAsync();
    }

 

    public async Task EnableUserAsync(short accountId)
    {
        await _systemAccountDAO.EnableUserAsync(accountId);
    }

    public async Task<bool> IsUserDisabledAsync(short accountId)
    {
        return await _systemAccountDAO.IsUserDisabledAsync(accountId);
    }

    public async Task DisableUserAsync(short accountId, short currentAccountId)
    {
        await _systemAccountDAO.DisableUserAsync(accountId, currentAccountId);
    }
}
