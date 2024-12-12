namespace shop.DB;


    public class OrderRepository(DbContext dbContext) : IOrderRepository
    {
        private readonly DbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public void Create(Order order)
        {
            _dbContext.orders.Add(order);
        }

        public Order ReadById(int id)
        {
            return _dbContext.orders.FirstOrDefault(g => g.OrderId == id);
        }

        public List<Order> ReadAll()
        {
            return _dbContext.orders.ToList();
        }
        
        public void Delete(Order order)
        {
            _dbContext.orders.Remove(order);
        }


        public List<Order> GetOrderByAccountId(int accountId)
        {
            return _dbContext.orders.Where(order => order.CustomerId == accountId).ToList();
        }
    }
    
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

        public void Create(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            if (order.CustomerId <= 0)
            {
                throw new ArgumentException("Customer ID must be a positive integer.", nameof(order.CustomerId));
            }

            _orderRepository.Create(order);
        }

        public Order ReadById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Order ID must be a positive integer.");
            }

            var order = _orderRepository.ReadById(id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            return order;
        }

        public List<Order> ReadAll()
        {
            var orders = _orderRepository.ReadAll();
            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException("No orders found.");
            }

            return orders;
        }
        
        public void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Order ID must be a positive integer.");
            }

            var order = _orderRepository.ReadById(id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            _orderRepository.Delete(order);
        }
        
        public List<Order> GetOrdersByAccountId(int accountId)
        {
            if (accountId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(accountId), "Account ID must be a positive integer.");
            }

            var orders = _orderRepository.GetOrderByAccountId(accountId);
            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException("No orders found for this account.");
            }

            return orders;
        }
    }

