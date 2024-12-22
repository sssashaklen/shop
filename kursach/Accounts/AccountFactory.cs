namespace shop;

public static class AccountFactory
{
    public static Account CreateAccount(int accountType, string name, int balance, string email, string password)
     {
        Account account = accountType switch
        {
            1 => new UserAccount(name, balance, email,  password, new Cart(new List<CartItem>())),
            2 => new AdminAccount(name, balance, email,  password, new Cart(new List<CartItem>())),
            _ => throw new ArgumentException("Invalid account type.")
        };
        return account;
    }
}
