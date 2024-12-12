using shop.commands;
using shop.DB;

namespace shop;

public class UserAccount : Account
{
    public override Dictionary<string, ICommand> Commands { get; protected set; }

    public UserAccount(
        string name, 
        int balance, 
        string email, 
        string password, 
        Cart cart
    ) : base(name, balance, email, password, cart)
    {
        Commands = new Dictionary<string, ICommand>(); 
    }
    public override Dictionary<string, ICommand> CreateCommands(
        ProductService productService, 
        AccountService accountService,
        OrderService orderService)
    {
        return new Dictionary<string, ICommand>
        {
            { "1", new AddBalanceCommand() },
            { "2", new CheckBalanceCommand()},
            { "3", new ViewProductsCommand(productService) },
            { "4", new AddProductToCartCommand(productService) },
            { "5", new DeleteProductFromCartCommand(accountService)},
            { "6", new ViewCartCommand() },
            { "7", new CreateOrderCommand(orderService, productService) },
            { "8", new ViewOrderHistoryCommand(orderService) },
            { "9", new LogoutCommand() },
            { "10", new ExitCommand() }
        };
    }
}