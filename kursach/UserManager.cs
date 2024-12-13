using shop;

namespace kursach;

public class UserManager
{
    private static Account _currentAccount;
    
    public static void Login(Account account)
    {
        _currentAccount = account;
    }
    
    public static Account GetCurrentAccount()
    {
        return _currentAccount;
    }
}
