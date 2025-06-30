using System;

namespace Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class Families
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class FamilyMembers
    {
        public int Id { get; set; }
        public int FamilyId { get; set; }
        public int UserId { get; set; }
    }
    public class Transactions
    {
        public int Id { get; set; }
        public int FamilyId { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
