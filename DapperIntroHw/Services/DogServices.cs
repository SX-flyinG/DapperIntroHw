using Dapper;
using DapperIntroHw.Data;
using DapperIntroHw.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperIntroHw.Services
{
    public class DogService
    {
        private readonly DogContext _context;

        public DogService(DogContext context)
        {
            _context = context;
        }

        public void AddDog(Dog dog)
        {
            var sql = "INSERT INTO Dogs (Name, Age, Breed, IsAdopted) VALUES (@Name, @Age, @Breed, @IsAdopted)";
            using var conn = Context.CreateConnection();
            conn.Execute(sql, dog);
            Console.WriteLine("Собаку додано!");
        }

        public void ShowAllDogs()
        {
            var sql = "SELECT * FROM Dogs";
            using var conn = Context.CreateConnection();
            var dogs = conn.Query<Dog>(sql).ToList();

            foreach (var dog in dogs)
            {
                PrintDog(dog);
            }
        }

        public void ShowAvailableDogs()
        {
            var sql = "SELECT * FROM Dogs WHERE IsAdopted = 0";
            using var conn = Context.CreateConnection();
            var dogs = conn.Query<Dog>(sql).ToList();

            foreach (var dog in dogs)
            {
                PrintDog(dog);
            }
        }

        public void ShowAdoptedDogs()
        {
            var sql = "SELECT * FROM Dogs WHERE IsAdopted = 1";
            using var conn = Context.CreateConnection();
            var dogs = conn.Query<Dog>(sql).ToList();

            foreach (var dog in dogs)
            {
                PrintDog(dog);
            }
        }

        public void FindById(int id)
        {
            var sql = "SELECT * FROM Dogs WHERE Id = @Id";
            using var conn = Context.CreateConnection();
            var dog = conn.QueryFirstOrDefault<Dog>(sql, new { Id = id });

            if (dog != null) PrintDog(dog);
            else Console.WriteLine("❌ Собаку не знайдено.");
        }

        public void FindByName(string name)
        {
            var sql = "SELECT * FROM Dogs WHERE Name LIKE @Name";
            using var conn = Context.CreateConnection();
            var dogs = conn.Query<Dog>(sql, new { Name = $"%{name}%" }).ToList();

            foreach (var dog in dogs) PrintDog(dog);
        }

        public void FindByBreed(string breed)
        {
            var sql = "SELECT * FROM Dogs WHERE Breed LIKE @Breed";
            using var conn = Context.CreateConnection();
            var dogs = conn.Query<Dog>(sql, new { Breed = $"%{breed}%" }).ToList();

            foreach (var dog in dogs) PrintDog(dog);
        }

        public void UpdateDog(int id, Dog updatedDog)
        {
            var sql = @"UPDATE Dogs SET Name = @Name, Age = @Age, Breed = @Breed, IsAdopted = @IsAdopted WHERE Id = @Id";
            using var conn = Context.CreateConnection();
            var result = conn.Execute(sql, new
            {
                Id = id,
                updatedDog.Name,
                updatedDog.Age,
                updatedDog.Breed,
                updatedDog.IsAdopted
            });

            if (result > 0) Console.WriteLine("Дані оновлено.");
            else Console.WriteLine("Собаку не знайдено.");
        }

        private void PrintDog(Dog dog)
        {
            Console.WriteLine($"[{dog.Id}] {dog.Name}, {dog.Age} років, {dog.Breed}, {(dog.IsAdopted ? "Прилаштований" : "В притулку")}");
        }

        public void AdoptDog(int dogId, int adopterId)
        {
            var sql = @"UPDATE Dogs SET IsAdopted = 1, AdopterId = @AdopterId WHERE Id = @Id";
            using var conn = _context.CreateConnection();
            var result = conn.Execute(sql, new { Id = dogId, AdopterId = adopterId });

            if (result > 0) Console.WriteLine("Собаку прилаштовано!");
            else Console.WriteLine("Не вдалося знайти собаку.");
        }

        public void BulkInsertDogs(List<Dog> dogs)
        {
            var sql = "INSERT INTO Dogs (Name, Age, Breed, IsAdopted) VALUES (@Name, @Age, @Breed, @IsAdopted)";
            using var conn = _context.CreateConnection();
            conn.Open();

            using var transaction = conn.BeginTransaction();
            conn.Execute(sql, dogs, transaction: transaction);
            transaction.Commit();

            Console.WriteLine($"Додано {dogs.Count} собак за один запит.");
        }

        public void InsertTestDogs()
        {
            var random = new Random();
            var names = new[] { "Rex", "Max", "Luna", "Bella", "Charlie", "Buddy", "Rocky", "Milo", "Daisy", "Bailey" };
            var breeds = new[] { "Labrador", "Beagle", "Bulldog", "Shepherd", "Poodle", "Terrier" };

            var dogs = new List<Dog>();

            for (int i = 0; i < 20; i++)
            {
                dogs.Add(new Dog
                {
                    Name = names[random.Next(names.Length)] + "_" + random.Next(100),
                    Age = random.Next(1, 12),
                    Breed = breeds[random.Next(breeds.Length)],
                    IsAdopted = false
                });
            }

            BulkInsertDogs(dogs);
        }

    }
}
