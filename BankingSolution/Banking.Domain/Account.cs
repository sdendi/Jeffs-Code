namespace Banking.Domain;

public class Account
{
	private decimal _balance = 5000;
	public void Deposit(decimal amountToDeposit)
	{
		_balance += amountToDeposit;
	}

	public decimal GetBalance()
	{
		// look it up the database
		return _balance;
	}

	public void Withdraw(decimal amountToWithdraw)
	{
		OverdraftNotAllowedGuard(amountToWithdraw);
		AcceptableTransactionAmountGuard(amountToWithdraw);
		_balance -= amountToWithdraw;
	}

	private static void AcceptableTransactionAmountGuard(decimal amountToWithdraw)
	{
		if (amountToWithdraw <= 0)
		{
			throw new ArgumentException("Only amounts > 0 can be used in a transaction");
		}
	}

	private void OverdraftNotAllowedGuard(decimal amountToWithdraw)
	{
		if (amountToWithdraw > _balance)
		{
			throw new ArgumentException("Overdraft Not Allowed");
		}
	}
}