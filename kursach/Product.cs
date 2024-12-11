namespace shop
{
    public class Product
    {

        private static int _globalId = 1;
        
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        
        public Product(string Name, string Description, int Price, int Quantity)
        {
            id = _globalId++;  
            name = Name;
            description = Description;
            price = Price;
            quantity = Quantity;
        }
    }
}