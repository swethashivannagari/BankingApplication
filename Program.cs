using System.Security.Principal;
using System.Collections;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;
using System.Security.AccessControl;


namespace BankingApplication
{
    //User class to intialize the users
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Account> Account { get; set; }

        public User(string name, string email, string password)
        {
            if(!ValidEmail(email)) throw new ArgumentException("Invalid email format");
            if (password.Length < 6) throw new ArgumentException("Password must be at least 6 characters.");

            Name = name;
            Email = email;
            Password = password;
            Account = new List<Account>();
            
        }

        public bool ValidateUser(string user, string password)
        {
            if (Name == user && Password == password)
            {
                return true;
            }
            return false;
        }
        private bool ValidEmail(string email) =>
            Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        
    }

    //Account class to create bank account
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountHolder { get; set; }
        public string AccountType { get; set; }
        public decimal balance { get; set; }
        private List<Transaction> transactions = new List<Transaction>();
        private static int intialId = 2000;
        private const decimal interestRate = 2;
        private DateTime LastInterestDate = DateTime.Now.AddMonths(-1);

        public Account(string accountHolder,string accountType,decimal amount)
        {
            AccountId = intialId++;
            AccountHolder= accountHolder;
            AccountType = accountType; 
            balance= amount;
        }

        //to add amount to the account
        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Deposit amount must be positive.");
            balance += amount;
            AddTransaction("Deposit", amount);
            Console.WriteLine($"{amount} deposited Successfully..");
            
        }

        //to withdraw the amount from account
        public void Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive.");
            if (balance < amount)
            {
                throw new InsufficientFundsException("Insufficient Funds for withdrawl");
            }
            balance -= amount;
            AddTransaction("Withdrawal", amount);
            Console.WriteLine($"{amount} Withdrawn Successfully..");

        }

        //to create transaction history, adding transaction
        private void AddTransaction(string type,decimal amount)
        {
            transactions.Add(new Transaction(type,amount));
        }


        public void generateStatement()
        {
            Console.WriteLine($"\nStatement for Account {AccountId}-{AccountHolder}");
            Console.WriteLine("DateTime\tTransaction Type\tAmount");
            foreach(var transaction in transactions)
            {
                Console.WriteLine($"{transaction.Date.ToShortDateString()} \t {transaction.Type}\t\t {transaction.Amount}");
            }
            Console.WriteLine($"\nCurrent Balance:Rs.{balance}");
        }

        //to add interest once a month
        public void AddMonthlyInterest()
        {
            if (AccountType.Equals("Savings", StringComparison.OrdinalIgnoreCase)&&(DateTime.Now-LastInterestDate).TotalDays>=30)
            {
                decimal interest = balance * (interestRate / 100);
                balance += interest;
                LastInterestDate = DateTime.Now;
                Console.WriteLine($"Interest added: {interest} ");
            }
            else if(AccountType.Equals("Checking", StringComparison.OrdinalIgnoreCase)){
                Console.WriteLine("Interest is only applicable for saving accounts");
            }
            else
            {
                Console.WriteLine("Interest has already been added this month.");
            }
        }

        //to check account balance
        public void CheckBalance()
        {
            Console.WriteLine($"Current balance for Account {AccountId} is {balance}");
        }


    }

    //To store every transaction
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }

        public DateTime Date { get; }
        private int intialTransactionId=1000;

        public Transaction(string type, decimal amount)
        {
            TransactionId = intialTransactionId++;
            Type = type;
            Amount = amount;
            Date = DateTime.Now;
        }
    }

    //Custom Exception ,When the balance is less than withdrawal amount
    public class InsufficientFundsException : ApplicationException
    {
        public InsufficientFundsException(string message) : base(message) { }
    }

    //contains all the users
    public class Bank
    {
        private List<User> users = new List<User>();
        public void RegisterUser(string name,string email,string password)
        {

            if (EmailRegistered(email)) throw new ArgumentException("Email is already registred. Please try using different email");
            users.Add(new User(name, email, password));
            Console.WriteLine("\nUser Registered Successfully!");
            Console.WriteLine("Login to perform bank operations...");
        }
        public User LoginUser(string name,string password)
        {
            foreach(var user in users)
            {
                if (user.ValidateUser(name, password))
                {
                    return user;
                }
            }
            Console.WriteLine("\nInvalid details. please try again!");
            return null;
        }
        private bool EmailRegistered(string email)
        {
            return users.Exists(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
    class Program
    {
        public static void Main(string [] args)
        {
            Bank bank = new Bank();
            User loggedUser = null;

            //Main Menu
            while (true)
            {
                
                Console.WriteLine("\n1.Register");
                Console.WriteLine("2.Login");
                Console.WriteLine("3.Exit");
                Console.WriteLine("Choose an option");
                string input = Console.ReadLine();
                int choice;
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            RegisterUser(bank);
                            break;
                        case 2:
                            loggedUser = Login(bank);
                            if (loggedUser != null)
                            {
                                Console.WriteLine("User Logged in Successfully");
                                BankOperations(loggedUser);
                            }
                            break;
                        case 3:
                            return;
                        default:
                            Console.WriteLine("invalid Option. Try Again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a valid number.");
                }


            }
        }
        static void RegisterUser(Bank bank)
        {
            Console.WriteLine("Enter Username");
            string username = Console.ReadLine();
            Console.WriteLine("Enter Email Address");
            string email = Console.ReadLine();
            Console.WriteLine("Enter Password");
            string password = Console.ReadLine();
            try
            {
                bank.RegisterUser(username, email, password);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        static User Login(Bank bank)
        {
            Console.WriteLine("Enter username");
            string username = Console.ReadLine();
            Console.WriteLine("Enter Password");
            string password = Console.ReadLine();
            User loggedUser = bank.LoginUser(username, password);
            return loggedUser;
        }

        //Bank Operations Menu after login
        static void BankOperations(User user)
        {
            while (true)
            {
                Console.WriteLine("Enter any key to continue");
                string s=Console.ReadLine();
                Console.WriteLine("\n***************************************************");
                Console.WriteLine("\nBank Operations:");
                Console.WriteLine("1.Open Account");
                Console.WriteLine("2.Deposit");
                Console.WriteLine("3.Withdraw");
                Console.WriteLine("4.View Statement");
                Console.WriteLine("5.Check Monthly interest");
                Console.WriteLine("6.Check Balance");
                Console.WriteLine("7.LogOut");
                Console.WriteLine("Choose an option");
                 string input = Console.ReadLine();
                int choice;

                Console.WriteLine();
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            openAccount(user);
                            break;
                        case 2:
                            depositAmount(user);
                            break;
                        case 3:
                            withdrawAmount(user);
                            break;
                        case 4:
                            showStatement(user);
                            break;
                        case 5:
                            monthlyInterest(user);
                            break;
                        case 6:
                            CheckBalance(user);
                            break;
                        case 7:
                            Console.WriteLine("Logging out...");
                            return;
                        default:
                            Console.WriteLine("Invalid Option");
                            break;

                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a valid number.");
                }
            }
        }


        static void openAccount(User user)
        {
            Console.WriteLine("Enter Account Holder's Name:");
            string name = Console.ReadLine();
            string accType;
            string type;
            while (true)
            {
                Console.WriteLine("Enter Account Type (s for Savings/c for Checking)");
                accType = Console.ReadLine().ToLower();
                if (accType == "s")
                {
                    type = "Savings";
                    break;
                }
                else if (accType == "c")
                {
                    type = "Checking";
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid account type. Please enter 's' for Savings or 'c' for Checking.");
                }
            }
            Console.WriteLine("Enter Intial Deposit");
            decimal amount = Convert.ToDecimal(Console.ReadLine());
            if (amount <= 0)
            {
                Console.WriteLine("Deposit amount cannot be negative.");
            }
            else
            {
                Account newAccount = new Account(name, type, amount);

                user.Account.Add(newAccount);
                Console.WriteLine($"Account with Account Number {newAccount.AccountId} Created Successfully!!");
            }
        }
        static void depositAmount(User user)
        {
            Console.WriteLine("Enter the Account Number");
            int accountNo = Convert.ToInt32(Console.ReadLine());
            var account = getAccount(user, accountNo);
            if (account != null)
            {
                Console.WriteLine("Enter Deposit Amount:");
                decimal amount = Convert.ToDecimal(Console.ReadLine());
                try
                {
                    account.Deposit(amount);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Account Not found");
            }
        }
        static void withdrawAmount(User user)
        {
            Console.WriteLine("Enter the Account Number");
            int accountNo = Convert.ToInt32(Console.ReadLine());
            Account account = getAccount(user, accountNo);
            if (account != null)
            {
                Console.WriteLine("Enter Withdrawal Amount:");
                decimal amount = Convert.ToDecimal(Console.ReadLine());
                try
                {
                    account.Withdraw(amount);
                }
                
                catch (InsufficientFundsException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Account Not found");
            }
        }
        static void showStatement(User user)
        {
            Console.WriteLine("Enter the Account Number");
            int accountNo = Convert.ToInt32(Console.ReadLine());
            Account account = getAccount(user, accountNo);
            if (account != null)
            {
                account.generateStatement();

            }
            else
            {
                Console.WriteLine("Account Not found");
            }
        }

        static void monthlyInterest(User user)
        {
            Console.WriteLine("Enter the Account Number");
            int accountNo = Convert.ToInt32(Console.ReadLine());
            Account account = getAccount(user, accountNo);
            if (account != null)
            {
                account.AddMonthlyInterest();

            }
            else
            {
                Console.WriteLine("Account Not found");
            }
        }
        static void CheckBalance(User user)
        {
            Console.WriteLine("Enter the Account Number");
            int accountNo = Convert.ToInt32(Console.ReadLine());
            Account account = getAccount(user, accountNo);
            if (account != null)
            {
                account.CheckBalance();
            }
            else
            {
                Console.WriteLine("Account Not found");
            }
        }

        public static Account getAccount(User user, int accountNo)
        {
            return user.Account.Find(a => a.AccountId == accountNo);
        }
    }
}
