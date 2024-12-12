namespace shop.DB;

public class OrderRepository(DbContext dbContext) : IOrderRepository
{
    private DbContext DbContext => dbContext;
    
    public void Create(Order order)
    {
        DbContext.orders.Add(order);
    }

    public Order ReadById(int id)
    {
        return DbContext.orders.FirstOrDefault(g => g.OrderId.Equals(id));
    }

    public List<Order> ReadAll()
    {
        return DbContext.orders.Count == 0 ? null : DbContext.orders;
    }
    
    public void Delete(int id)
    {
        var orders = ReadAll();
        var order = orders.FirstOrDefault();

        if (order != null)
        {
            DbContext.orders.Remove(order);
        }
        else
        {
            throw new ArgumentException("Game not found.");
        }
    }
    public List<Order> GetOrdersByAccountId(int accountId)
    {
        return DbContext.orders.Where(order => order.CustomerId == accountId).ToList();
    }
}

public class OrderService(OrderRepository orderRepository) : IOrderService
{
    public void Create(Order order)
    {
        orderRepository.Create(order);
    }

    public Order ReadById(int id)
    {
        return orderRepository.ReadById(id);
    }

    public List<Order> ReadAll()
    {
        return orderRepository.ReadAll();
    }
    
    public void Delete(int id)
    {
        orderRepository.Delete(id);
    }
    public List<Order> GetOrdersByAccountId(int accountId)
    {
        return orderRepository.GetOrdersByAccountId(accountId);
    }
}
