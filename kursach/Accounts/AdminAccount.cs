using shop.commands;

namespace shop;

public class AdminAccount (string name, int balance, string email, string password, Cart cart) : Account(name, balance, email, password, cart)
{
}
