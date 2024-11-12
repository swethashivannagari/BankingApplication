# C# Console Banking Application

 A Simple C# console application for managing bank accounts, user registration, and transactions. This project demonstrates core functionalities like user registration, login, account management, transactions (deposit and withdrawal), and custom exception handling for banking-related scenarios.
 
## Table of Contents
- [Features](#features)
- [Classes](#classes)
- [How to Use](#how-to-use)
- [Error Handling](#error-handling)


---

## Features
 ### User Registration and Login:
-  Users can create an account with a unique username and password.
-   Login verifies credentials before allowing access.
###  Account Opening
- Users can open multiple accounts (e.g., Savings, Checking), each with a unique account number.
- Details includes a unique account number, account holder's name, account type (Savings/Checking), and initial deposit amount.
### Transactions:
- Deposit and withdraw funds from accounts with real-time balance updates.
- Each transaction is logged with a unique ID, date, type (deposit or withdrawal), and amount.
### View Statement:
-  View a transaction history for each account.Each statement includes transaction date, type, and amount.
### Interest Calculation:
-  Monthly interest can be added for savings accounts.Interest is added only once per month.
### View Balance:
-  Users can check the current balance for any of their accounts.

---

## Classes

### 1. `User`
- Represents a bank user with properties for `Name`, `Email`, `Password`, and a list of associated accounts.
- Validates login credentials and email format.

### 2. `Account`
- Represents a bank account with an `AccountId`, `AccountHolder`, `AccountType`, `balance`, and a list of transactions.
- Includes methods for `Deposit`, `Withdraw`, `generateStatement`, `AddMonthlyInterest`, and `CheckBalance`.

### 3. `Transaction`
- Represents a transaction with properties for `TransactionId`, `Type`, `Amount`, and `Date`.
- Used to log deposit and withdrawal activities.

### 4. `Bank`
- Manages user registration and login.
- Maintains a list of registered users and includes methods for account operations.

### 5. `InsufficientFundsException`
- Custom exception thrown when a withdrawal exceeds the available balance.

---

## How to Use

1. **Start the Application**: Run the program in a console.
2. **Main Menu**:
   - Choose `1` to register a new user.
   - Choose `2` to log in.
   - Choose `3` to exit.
3. **Bank Operations** (after login):
   - Choose from various operations like account opening, deposit, withdrawal, statement viewing, interest calculation, and balance check.

---

## Error Handling

- **Invalid Inputs**: Handles errors like incorrect login credentials, negative deposit amounts, and invalid account operations.
- **Insufficient Funds**: A custom exception is raised if a withdrawal amount exceeds the available balance.

---

This application is designed to be a simple, console-based simulation of core banking operations.
