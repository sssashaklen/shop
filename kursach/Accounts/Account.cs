using shop.commands;
using shop.DB;

namespace shop;

public abstract class Account
{
    public string Name { get; set; } 
    public int Balance { get; set; } 
    public string Email { get; set; } 
    public int Id { get; set; }
    public string Password { get; set; } 
    private static int _globalId=1;
    
   public Cart Cart { get; set; } 
   public abstract Dictionary<string, ICommand> Commands { get; protected set; }

   protected Account(string name, int balance, string email, string password, Cart? cart = null)
   {
       Id = _globalId++;
       Name = name;
       Balance = balance;
       Email = email;
       Password = password;
       Cart = cart ?? new Cart(new List<CartItem>());
   }


   public abstract Dictionary<string, ICommand> CreateCommands(
       ProductService productService,
       AccountService accountService,
       OrderService orderService);
}