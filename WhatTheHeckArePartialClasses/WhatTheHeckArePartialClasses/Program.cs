
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var customer = new Customer();
customer.Name = "Roger";
customer.CreditLimit = 5000;

Console.WriteLine("The customer's credit limit is " + customer.CreditLimit);

customer.MakePurchase(100M);

Console.WriteLine("The customer's credit limit is " + customer.CreditLimit);

public partial class Customer
{

	public string Name { get; set; } = string.Empty;
	public decimal CreditLimit { get; set; }

}


public partial class Customer
{
	public void MakePurchase(decimal amount)
	{
		CreditLimit -= amount;
	}
}