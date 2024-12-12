namespace shop.DB;

public interface IAccountRepository
{
    void Create(Account account);
    Account ReadById(int id); 
    List<Account> ReadAll();
    void Update(Account account);
    void Delete(int id);
    Account ReadByUsername(string username);
}

public interface IOrderRepository
{
    void Create(Order order);
    Order ReadById(int id); 
    List<Order> ReadAll();
    void Update(Order order);
    void Delete(int id);
}
public interface IAccountService
{
    void Create(Account account);
    Account ReadById(int id); 
    List<Account> ReadAll();
    void Update(Account account);
    void Delete(int id);
}

public interface IOrderService
{
    void Create(Order order);
    Order ReadById(int id); 
    List<Order> ReadAll();
    void Update(Order order);
    void Delete(int id);
}

public interface IProductRepository
{
    void Create(Product product);
    Product ReadById(int id);
    List<Product> ReadAll();
    void Update(Product product);
    void Delete(int id);
}

public interface IProductService
{
    void Create(Product product);
    Product ReadById(int id);
    List<Product> ReadAll();
    void Update(Product product);
    void Delete(int id);
}

public interface ICartRepository
{
    void Create(Cart cart);
    List<CartItem> ReadAll();
    void Update(Cart cart);
    void Delete(Cart cart);
}

public interface ICartService
{
    void Create(Cart cart);
    List<CartItem> ReadAll();
    void Update(Cart cart);
    void Delete(Cart cart);
}