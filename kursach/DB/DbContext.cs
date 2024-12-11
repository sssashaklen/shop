namespace shop.DB
{
    public class DbContext
    {
        public List<Account> accounts = new List<Account>();
        public List<Product> products = new List<Product>();
        public List<Order> orders = new List<Order>();
        
        public void Seed()
        {
            products.Add(new Product("Laptop", "A high-performance laptop with 16GB RAM and 512GB SSD.", 1000, 10));
            products.Add(new Product( "Smartphone", "A modern smartphone with a 6.5-inch display and 128GB storage.", 500, 20));
            products.Add(new Product( "Headphones", "Noise-cancelling over-ear headphones with Bluetooth connectivity.", 1100, 50));
            products.Add(new Product( "Smart Watch", "A smart watch with fitness tracking and heart rate monitor.", 150, 30));
            products.Add(new Product( "Keyboard", "A mechanical keyboard with RGB lighting and programmable keys.", 50, 100));
            products.Add(new Product( "Mouse", "An ergonomic wireless mouse with a high-precision sensor.", 30, 200));
            accounts.Add(new AdminAccount("Oleksandra",100000, "kozachenko.oleksandra@lll.kpi.ua","admin", new Cart(new List<CartItem>())));
            accounts.Add(new UserAccount("sasa",100000, "kozachenko.oleksandra@lll.kpi.ua","admin", new Cart(new List<CartItem>())));
        }
    }
}