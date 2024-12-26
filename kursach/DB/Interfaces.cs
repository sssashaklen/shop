namespace shop.DB;

public interface IAccountRepository
{
    void Create(Account account);
    Account ReadById(int id); 
    List<Account> ReadAll();
    void Update(Account account);
    void Delete(int id);
    Account ReadByUsername(string username);
    public CartItem GetCartItem(Account account, int productId);
}

public interface IOrderRepository
{
    void Create(Order order);
    Order ReadById(int id); 
    List<Order> ReadAll();
    void Delete(Order order);
    List<Order> GetOrderByAccountId(int accountId);
}
public interface IAccountService
{
    void Create(Account account);
    Account ReadById(int id); 
    List<Account> ReadAll();
    public Account ReadByUserName(string username);
    void Update(Account account);
    void Delete(int id);
    public int GetCartItemQuantity(Account account, int productId);
    public void ReduceCartItemQuantity(Account account, int productId, int quantity);
    public void RemoveFromCart(Account account, int productId);
}

public interface IOrderService
{
    Order Create(Account account);
    Order ReadById(int id); 
    List<Order> ReadAll();
    void Delete(int id);
    public List<Order> GetOrdersByAccountId(int accountId);
}

public interface IProductRepository
{
    void Create(Product product);
    Product ReadById(int id);
    List<Product> ReadAll();
    void Update(Product product);
    void Delete(Product product);
    void DecreaseQuantity(Product product, int quantityToDecrease);
}

public interface IProductService
{
    void Create(Product product);
    Product ReadById(int id);
    List<Product> ReadAll();
    void Update(Product product);
    void Delete(int id);
    public void DecreaseQuantity(int productId, int quantityToDecrease);
}
