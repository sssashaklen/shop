namespace shop.DB;

public class AccountRepository(DbContext dbContext) : IAccountRepository
{
    private readonly DbContext dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public void Create(Account account)
    {
        dbContext.accounts.Add(account);
    }

    public Account ReadById(int id)
    {
        return dbContext.accounts.FirstOrDefault(p => p.Id == id);
    }

    public List<Account> ReadAll()
    {
        return dbContext.accounts;
    }

    public Account ReadByUsername(string username)
    {
        return dbContext.accounts.FirstOrDefault(acc => acc.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public void Update(Account updatedAccount)
    {
        var account = ReadById(updatedAccount.Id);
        if (account != null)
        {
            account.Name = updatedAccount.Name;
        }
    }

    public void Delete(int id)
    {
        var account = ReadById(id);
        if (account != null)
        {
            dbContext.accounts.Remove(account);
        }
    }
}

public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    private readonly IAccountRepository _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

    public void Create(Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Account cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(account.Name))
        {
            throw new ArgumentException("Account name cannot be empty or whitespace.", nameof(account.Name));
        }

        var existingAccount = _accountRepository.ReadByUsername(account.Name);
        if (existingAccount != null)
        {
            throw new InvalidOperationException("An account with this username already exists.");
        }

        _accountRepository.Create(account);
    }

    public Account ReadById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "ID must be a positive integer.");
        }

        var account = _accountRepository.ReadById(id);
        if (account == null)
        {
            throw new KeyNotFoundException("Account not found.");
        }

        return account;
    }

    public Account ReadByUserName(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or whitespace.", nameof(username));
        }

        var account = _accountRepository.ReadByUsername(username);
        if (account == null)
        {
            throw new KeyNotFoundException("Account with the specified username not found.");
        }

        return account;
    }

    public List<Account> ReadAll()
    {
        var accounts = _accountRepository.ReadAll();
        if (accounts == null || accounts.Count == 0)
        {
            throw new InvalidOperationException("No accounts found.");
        }

        return accounts;
    }

    public void Update(Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Account cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(account.Name))
        {
            throw new ArgumentException("Account name cannot be empty or whitespace.", nameof(account.Name));
        }

        var existingAccount = _accountRepository.ReadById(account.Id);
        if (existingAccount == null)
        {
            throw new KeyNotFoundException("Account not found.");
        }

        _accountRepository.Update(account);
    }

    public void Delete(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "ID must be a positive integer.");
        }

        var account = _accountRepository.ReadById(id);
        if (account == null)
        {
            throw new KeyNotFoundException("Account not found.");
        }

        _accountRepository.Delete(id);
    }

    public CartItem GetCartItem(Account account, int productId)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Account cannot be null.");
        }

        if (productId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(productId), "Product ID must be a positive integer.");
        }

        var cartItem = account.Cart?.Products?.FirstOrDefault(item => item?.Product?.id == productId);
        if (cartItem == null)
        {
            throw new KeyNotFoundException("Cart item not found.");
        }

        return cartItem;
    }

    public void ReduceCartItemQuantity(Account account, int productId, int quantity)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Account cannot be null.");
        }

        if (productId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(productId), "Product ID must be a positive integer.");
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be a positive integer.");
        }

        var cartItem = GetCartItem(account, productId);
        if (cartItem != null)
        {
            cartItem.DecreaseQuantity(quantity);
            if (cartItem.Quantity <= 0)
            {
                RemoveFromCart(account, productId);
            }
        }
    }

    public void RemoveFromCart(Account account, int productId)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Account cannot be null.");
        }

        if (productId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(productId), "Product ID must be a positive integer.");
        }

        var cartItem = GetCartItem(account, productId);
        if (cartItem != null)
        {
            account.Cart.Products.Remove(cartItem);
        }
    }
}
