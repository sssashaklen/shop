
using kursach;
using shop;
using shop.commands;
using shop.DB;

public class Program
{
    public static void Main(string[] args)
    {
        UserManager userManager = new UserManager();
        DbContext db = new DbContext();
        db.Seed();
        AccountRepository accountRepository = new AccountRepository(db);
        ProductRepository productRepository = new ProductRepository(db);
        OrderRepository orderRepository = new OrderRepository(db);

        AccountService accountService = new AccountService(accountRepository);
        ProductService productService = new ProductService(productRepository);
        OrderService orderService = new OrderService(orderRepository);
        
        Console.WriteLine("Welcome to the Shop Application!");
        Console.WriteLine("Please log in or register to continue.");

        Account currentAccount = null;

        while (true)
        {
            var commands = CommandFactory.CreateCommands(currentAccount, accountService, orderService, productService);
            
            ShowMenu(commands);
            
            currentAccount = UserManager.GetCurrentAccount();
    
            if (currentAccount == null)
            {
                Console.WriteLine("You have been logged out. Please log in or register to continue.");
            }
        }
    }

    private static void ShowMenu(Dictionary<string, ICommand> commands)
    {
        Console.WriteLine("\nChoose an option:");
        foreach (var entry in commands)
        {
            Console.WriteLine($"{entry.Key}. {entry.Value.ShowInfo()}");
        }
        
        string input = Console.ReadLine();
        if (commands.TryGetValue(input, out ICommand command))
        {
            try
            {
                command.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid option, please try again.");
        }
    }
}
