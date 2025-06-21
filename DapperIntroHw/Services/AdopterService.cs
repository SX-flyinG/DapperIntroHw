using DapperIntroHw.Data;
using DapperIntroHw.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperIntroHw.Services
{
    public class AdopterService
    {
        private readonly DogContext _context;

        public AdopterService(DogContext context)
        {
            _context = context;
        }

        public void AddAdopter(Adopter adopter)
        {
            var sql = "INSERT INTO Adopters (Name, Phone) VALUES (@Name, @Phone)";
            using var conn = _context.CreateConnection();
            conn.Execute(sql, adopter);
            Console.WriteLine("Опікуна додано.");
        }

        public void ShowAllAdopters()
        {
            var sql = "SELECT * FROM Adopters";
            using var conn = _context.CreateConnection();
            var adopters = conn.Query<Adopter>(sql).ToList();

            foreach (var adopter in adopters)
            {
                Console.WriteLine($"[{adopter.Id}] {adopter.Name} ({adopter.Phone})");
            }
        }

        public bool AdopterExists(int id)
        {
            var sql = "SELECT COUNT(1) FROM Adopters WHERE Id = @Id";
            using var conn = _context.CreateConnection();
            return conn.ExecuteScalar<bool>(sql, new { Id = id });
        }
    }
}
