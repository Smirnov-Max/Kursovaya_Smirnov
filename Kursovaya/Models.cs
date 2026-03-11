using System;

namespace Smirnov_kursovaya.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Fio { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public decimal Discount { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string Notes { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Fio { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int? OrderId { get; set; }
    }
}