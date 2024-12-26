namespace shop
{
    public class Order
    {
        private static int _globalID = 1; 
        
        public int OrderId { get; private set; }
        public int CustomerId { get; private set; }
        public string OrderDate { get; private set; } 
        public string OrderTime { get; private set; }
        public int OrderPrice { get; private set; }
        public List <CartItem> Products { get; private set; }
        
        public Order(int customerId, Cart cart)
        {
            OrderId = _globalID++;
            CustomerId = customerId;
            OrderDate = DateTime.Now.ToString("yyyy-MM-dd");
            OrderTime = DateTime.Now.ToString("HH:mm:ss");  
            Products = cart.Products; 
            OrderPrice = CalculateOrderPrice(); 
        }
        
        public int CalculateOrderPrice()
        {
            int totalPrice = 0;

            if (Products != null)
            {
                foreach (var item in Products)
                {
                    totalPrice += item.Product.price * item.Quantity; 
                }
            }

            return totalPrice;
        }
    }
}
