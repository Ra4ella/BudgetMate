using System;
using Dapper;
using Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace BudgetMate
{
    internal class Program
    {
        static int familyId = 0;
        static string userName = "";
        static int userID = 0;
        static int helpTop = 0;
        static void Main(string[] args)
        {
            string stringConnection = "Server=DESKTOP-Q7G097F;Database=BudgetMate;Trusted_Connection=True;TrustServerCertificate=True";
            bool flag1 = true;
            bool flag2 = false;
            using (var connection = new SqlConnection(stringConnection))
            {
                while (true)
                {
                    while (flag1)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("1. Sign up");
                        Console.WriteLine("2. Log in");
                        Console.WriteLine("0. Exit");
                        Console.Write("< ");
                        int choice1 = Convert.ToInt32(Console.ReadLine());

                        if (choice1 > -1 && choice1 < 3)
                        {
                            if (choice1 == 0)
                            {
                                Environment.Exit(0);
                            }
                            if (choice1 == 1)
                            {
                                Console.Write("Enter username: ");
                                userName = Console.ReadLine();
                                string checkUsername = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                                var result_checkUsername = connection.ExecuteScalar<int>(checkUsername, new { username = userName });
                                if (result_checkUsername > 0)
                                {
                                    Console.WriteLine("Sign up isn't successful");
                                    Console.WriteLine($"That username is already in database");
                                }
                                else
                                {
                                    Console.Write("Enter password: ");
                                    string userPassword = Console.ReadLine();
                                    string addUsers = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                                    var result_addUsers = connection.Execute(addUsers, new { Username = userName, Password = userPassword });
                                    string getFamilies = "SELECT * FROM Families";
                                    var result_getFamilies = connection.Query(getFamilies);
                                    if (result_getFamilies.Any())
                                    {
                                        foreach (var family in result_getFamilies)
                                        {
                                            Console.WriteLine($"Family Id - {family.Id} | Family Name - {family.Name}");
                                        }
                                        Console.Write("Enter family id for you: ");
                                        familyId = Convert.ToInt32(Console.ReadLine());
                                        string takeUserId = "SELECT Id FROM Users WHERE Username = @username";
                                        var result_takeUserId = connection.ExecuteScalar<int>(takeUserId, new { username = userName });
                                        userID = result_takeUserId;
                                        string addFamilyMembers = "INSERT INTO FamilyMembers (UserId, FamilyId) VALUES (@UserId, @FamilyId)";
                                        var result_addFamilyMembers = connection.Execute(addFamilyMembers, new { UserId = result_takeUserId, FamilyId = familyId });
                                        Console.WriteLine("Sign up is successful");
                                        Console.WriteLine("Family Members was added");
                                        flag1 = false;
                                        flag2 = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("There aren't families now");
                                    }
                                }
                            }
                            if (choice1 == 2)
                            {
                                Console.Write("Enter username: ");
                                userName = Console.ReadLine();
                                string takeUserId = "SELECT Id FROM Users WHERE Username = @username";
                                var result_takeUserId = connection.ExecuteScalar<int>(takeUserId, new { username = userName });
                                userID = result_takeUserId;
                                Console.Write("Enter password: ");
                                string userPassword = Console.ReadLine();
                                string checkUser = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";
                                var result_checkUser = connection.ExecuteScalar<int>(checkUser, new { username = userName, password = userPassword });
                                if (result_checkUser > 0)
                                {
                                    string takeFamilyId = "SELECT FamilyId FROM FamilyMembers WHERE UserId = @userid";
                                    var result_takeFamilyId = connection.ExecuteScalar<int>(takeFamilyId, new { userid = userID });
                                    familyId = result_takeFamilyId;
                                    Console.WriteLine("Log in is successful");
                                    Console.WriteLine("Family Members was added");
                                    flag1 = false;
                                    flag2 = true;
                                }
                                else
                                {
                                    Console.WriteLine("Log in isn't successful");
                                    Console.WriteLine($"There isn't user with that username and that password");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Enter your choice again!");
                        }
                    }
                    while (flag2)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("1. Create income");
                        Console.WriteLine("2. Create spending");
                        Console.WriteLine("3. Check balance for category");
                        Console.WriteLine("4. Check balance for month");
                        Console.WriteLine("5. Report");
                        Console.WriteLine("6. Log out");
                        Console.Write("< ");
                        int choice2 = Convert.ToInt32(Console.ReadLine());

                        if (choice2 == 1)
                        {
                            Console.Write("Enter amount (in $): ");
                            int user_Amount = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("1. Entertainment");
                            Console.WriteLine("2. Utilities");
                            Console.WriteLine("3. Clothes");
                            Console.WriteLine("4. Food");
                            Console.WriteLine("5. Education");
                            Console.WriteLine("6. Transport");
                            Console.WriteLine("7. Travel");
                            Console.WriteLine("8. Other");
                            Console.Write("Enter category: ");
                            int user_Category_choice = Convert.ToInt32(Console.ReadLine());
                            string user_Category = "";
                            switch (user_Category_choice)
                            {
                                case 1:
                                    user_Category = "Entertainment";
                                    break;
                                case 2:
                                    user_Category = "Utilities";
                                    break;
                                case 3:
                                    user_Category = "Clothes";
                                    break;
                                case 4:
                                    user_Category = "Food";
                                    break;
                                case 5:
                                    user_Category = "Education";
                                    break;
                                case 6:
                                    user_Category = "Transport";
                                    break;
                                case 7:
                                    user_Category = "Travel";
                                    break;
                                case 8:
                                    user_Category = "Other";
                                    break;
                            }
                            string addTransaction = "INSERT INTO Transactions (FamilyId, UserId, Amount, Type, Category, CreatedAt) VALUES (@FamilyId, @UserId, @Amount, @Type, @Category, @CreatedAt)";
                            var result_addTransaction = connection.Execute(addTransaction, new { FamilyId = familyId, UserId = userID, Amount = user_Amount, Type = "Income", Category = user_Category, CreatedAt = DateTime.Now });
                            Console.WriteLine("Income was created");
                        }
                        if (choice2 == 2)
                        {
                            Console.Write("Enter amount (in $): ");
                            int user_Amount = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("1. Entertainment");
                            Console.WriteLine("2. Utilities");
                            Console.WriteLine("3. Clothes");
                            Console.WriteLine("4. Food");
                            Console.WriteLine("5. Education");
                            Console.WriteLine("6. Transport");
                            Console.WriteLine("7. Travel");
                            Console.WriteLine("8. Other");
                            Console.Write("Enter category: ");
                            int user_Category_choice = Convert.ToInt32(Console.ReadLine());
                            string user_Category = "";
                            switch (user_Category_choice)
                            {
                                case 1:
                                    user_Category = "Entertainment";
                                    break;
                                case 2:
                                    user_Category = "Utilities";
                                    break;
                                case 3:
                                    user_Category = "Clothes";
                                    break;
                                case 4:
                                    user_Category = "Food";
                                    break;
                                case 5:
                                    user_Category = "Education";
                                    break;
                                case 6:
                                    user_Category = "Transport";
                                    break;
                                case 7:
                                    user_Category = "Travel";
                                    break;
                                case 8:
                                    user_Category = "Other";
                                    break;
                            }
                            string addTransaction = "INSERT INTO Transactions (FamilyId, UserId, Amount, Type, Category, CreatedAt) VALUES (@FamilyId, @UserId, @Amount, @Type, @Category, @CreatedAt)";
                            var result_addTransaction = connection.Execute(addTransaction, new { FamilyId = familyId, UserId = userID, Amount = user_Amount, Type = "Spending", Category = user_Category, CreatedAt = DateTime.Now });
                            Console.WriteLine("Spending was created");
                        }
                        if (choice2 == 3)
                        {
                            Console.WriteLine("1. Entertainment");
                            Console.WriteLine("2. Utilities");
                            Console.WriteLine("3. Clothes");
                            Console.WriteLine("4. Food");
                            Console.WriteLine("5. Education");
                            Console.WriteLine("6. Transport");
                            Console.WriteLine("7. Travel");
                            Console.WriteLine("8. Other");
                            Console.Write("Enter category: ");
                            int user_Category_choice = Convert.ToInt32(Console.ReadLine());
                            string user_Category = "";
                            switch (user_Category_choice)
                            {
                                case 1:
                                    user_Category = "Entertainment";
                                    break;
                                case 2:
                                    user_Category = "Utilities";
                                    break;
                                case 3:
                                    user_Category = "Clothes";
                                    break;
                                case 4:
                                    user_Category = "Food";
                                    break;
                                case 5:
                                    user_Category = "Education";
                                    break;
                                case 6:
                                    user_Category = "Transport";
                                    break;
                                case 7:
                                    user_Category = "Travel";
                                    break;
                                case 8:
                                    user_Category = "Other";
                                    break;
                            }
                            string checkBalance = "SELECT SUM(CASE WHEN Type = 'Income' THEN Amount WHEN Type = 'Spending' THEN -Amount ELSE 0 END) AS Balance FROM Transactions WHERE FamilyId = @FamilyId AND Category = @Category";
                            var result_balance = connection.ExecuteScalar<decimal>(checkBalance, new { FamilyId = familyId, Category = user_Category });
                            Console.WriteLine($"Balance for category ({user_Category}): {result_balance}$");
                        }
                        if (choice2 == 4)
                        {
                            string balanceByMonth = @"SELECT FORMAT(CreatedAt, 'yyyy-MM') AS Month, SUM(CASE WHEN Type = 'Income' THEN Amount WHEN Type = 'Spending' THEN -Amount ELSE 0 END) AS Balance FROM Transactions WHERE FamilyId = @FamilyId GROUP BY FORMAT(CreatedAt, 'yyyy-MM') ORDER BY Month;";
                            var result_balanceByMonth = connection.Query(balanceByMonth, new { FamilyId = familyId });

                            foreach (var month in result_balanceByMonth)
                            {
                                Console.WriteLine($"Month: {month.Month} | Balance: {month.Balance}$");
                            }
                        }
                        if (choice2 == 5)
                        {
                            Console.WriteLine("1. TOP spending");
                            Console.WriteLine("2. Balance for week");
                            Console.WriteLine("3. Part every member");
                            Console.Write("< ");
                            int choice3 = Convert.ToInt32(Console.ReadLine());

                            if (choice3 == 1)
                            {
                                string topSpending = "SELECT f.Name AS FamilyName, u.Username, t.Amount, t.Type, t.Category, t.CreatedAt FROM Transactions t JOIN Users u ON t.UserId = u.Id JOIN Families f ON t.FamilyId = f.Id WHERE Type = @type AND FamilyId = @familyid ORDER BY Amount DESC";
                                var result_topSpending = connection.Query(topSpending, new { type = "Spending", familyid = familyId });
                                Console.WriteLine($"Family Name | Username | Amount ($) | Type | Category | CreatedAt");

                                foreach (var top in result_topSpending)
                                {
                                    Console.WriteLine($"{top.FamilyName} | {top.Username} | {top.Amount}$ | {top.Type} | {top.Category} | {top.CreatedAt}");
                                }

                            }
                            if (choice3 == 2)
                            {
                                string checkBalance = @"SELECT SUM(CASE WHEN Type = 'Income' THEN Amount WHEN Type = 'Spending' THEN -Amount ELSE 0 END) AS Balance FROM Transactions WHERE FamilyId = @FamilyId AND CreatedAt BETWEEN @StartDate AND @EndDate";
                                var result_balance = connection.ExecuteScalar<decimal>(checkBalance, new { FamilyId = familyId, StartDate = DateTime.Now.AddDays(-7), EndDate = DateTime.Now });
                                Console.WriteLine($"Balance in last week: {result_balance}$");
                            }                                                                                                   
                            if (choice3 == 3)
                            {
                                string everyMember = "SELECT f.Name AS FamilyName, u.Username, t.Amount, t.Type, t.Category, t.CreatedAt FROM Transactions t JOIN Users u ON t.UserId = u.Id JOIN Families f ON t.FamilyId = f.Id WHERE FamilyId = @familyID";
                                var result_everyMember = connection.Query(everyMember, new { familyID = familyId });
                                Console.WriteLine($"Family Name | Username | Amount ($) | Type | Category | CreatedAt");

                                foreach (var member in result_everyMember)
                                {
                                    Console.WriteLine($"{member.FamilyName} | {member.Username} | {member.Amount}$ | {member.Type} | {member.Category} | {member.CreatedAt}");
                                }
                            }
                        }
                        if (choice2 == 6)
                        {
                            flag1 = true;
                            flag2 = false;
                        }
                    }
                }
            }
        }
    }
}
