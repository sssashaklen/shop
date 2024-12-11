namespace shop.DB;

public class AccountRepository(DbContext dbContext) : IAccountRepository
{
    private DbContext _DbContext { get; } = dbContext;

    public void Create(Account account)
    {
        if (_DbContext == null || _DbContext.accounts == null)
        {
            throw new InvalidOperationException("DbContext or accounts collection is not initialized.");
        }

        var existingAccount = _DbContext.accounts.FirstOrDefault(a => a.Name == account.Name);
        if (existingAccount != null)
        {
            throw new Exception("An account with this username already exists.");
        }

        _DbContext.accounts.Add(account);
    }

    public Account ReadById(int id)
    {
        var account = _DbContext.accounts.FirstOrDefault(p => p.Id == id);
        return account;
    }

    public List<Account> ReadAll()
    {
        return _DbContext.accounts;
    }

    public Account ReadByUsername(string username)
    {
        return _DbContext.accounts.FirstOrDefault(acc => acc.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public void Update(Account updatedAccount)
    {
        var account = _DbContext.accounts.FirstOrDefault(a => a.Id == updatedAccount.Id);

        if (account != null)
        {
            account.Name = updatedAccount.Name;
        }
        else
        {
            throw new ArgumentException("Account not found.");
        }
    }

    public void Delete(int id)
    {
        var account = _DbContext.accounts.FirstOrDefault(p => p.Id == id);
        if (account != null)
        {
            _DbContext.accounts.Remove(account);
        }
        else
        {
            throw new ArgumentException("Account not found.");
        }
    }

    public void AddToCart(Account account, int productId, int quantity)
    {
        if (dbContext?.products == null)
        {
            Console.WriteLine("Database context or products list is not initialized.");
            return;
        }

        var product = dbContext.products.FirstOrDefault(p => p.id == productId);

        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        if (account == null)
        {
            Console.WriteLine("Account is not initialized.");
            return;
        }

        if (account.Cart == null)
        {
            account.Cart = new Cart(new List<CartItem>());
        }

        if (quantity <= 0)
        {
            Console.WriteLine("Quantity must be greater than zero.");
            return;
        }

        var existingCartItem = account.Cart.Products.FirstOrDefault(item => item?.Product?.id == product.id);

        if (existingCartItem != null)
        {
            existingCartItem.IncreaseQuantity(quantity);
            Console.WriteLine($"Updated {product.name} quantity to {existingCartItem}.");
        }
        else
        {
            account.Cart.AddToCart(product, quantity);
            Console.WriteLine($"Added {product.name} to your cart. Quantity: {quantity}.");
        }
    }

    public CartItem GetCartItem(Account account, int productId)
    {
        return account.Cart.Products.FirstOrDefault(item => item?.Product?.id == productId);
    }

    public void ReduceCartItemQuantity(Account account, int productId, int quantity)
    {
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
        var cartItem = GetCartItem(account, productId);
        if (cartItem != null)
        {
            account.Cart.Products.Remove(cartItem);
        }
    }
}

public class AccountService(AccountRepository accountRepository) : IAccountService
    {
        private AccountRepository _accountRepository = accountRepository;

        public void Create(Account account)
        {
            _accountRepository.Create(account);
        }

        public Account ReadById(int id)
        {
            return _accountRepository.ReadById(id);
        }

        public Account ReadByUserName(string username)
        {
            return _accountRepository.ReadByUsername(username);
        }

        public List<Account> ReadAll()
        {
            return _accountRepository.ReadAll();
        }

        public void Update(Account account)
        {
            _accountRepository.Update(account);
        }

        public void Delete(int id)
        {
            _accountRepository.Delete(id);
        }

        public void AddToCart(Account account, int productId, int quantity)
        {
            _accountRepository.AddToCart(account, productId, quantity);
        }

        public CartItem GetCartItem(Account account, int productId)
        {
            return account.Cart.Products.FirstOrDefault(item => item?.Product?.id == productId);
        }
        public void ReduceCartItemQuantity(Account account, int productId, int quantity)
        {
            _accountRepository.ReduceCartItemQuantity(account, productId, quantity);
        }

        public void RemoveFromCart(Account account, int productId)
        {
            _accountRepository.RemoveFromCart(account, productId);
        }
    }
