using shop.commands;
using shop.DB;

namespace shop;

public class UserAccount(string name, int balance, string email, string password, Cart cart) : Account(name, balance, email, password, cart)
{
}