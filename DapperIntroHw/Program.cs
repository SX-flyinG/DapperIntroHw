using DapperIntroHw.Data;
using DapperIntroHw.Entities;
using DapperIntroHw.Services;
using Microsoft.Data.Sqlite;

namespace DapperIntroHw
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=dog_shelter.db";
            var context = new DogContext("Data Source=dog_shelter.db");
            var service = new DogService(context);


            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCommand = @"
    CREATE TABLE IF NOT EXISTS Dogs (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT NOT NULL,
        Age INTEGER NOT NULL,
        Breed TEXT NOT NULL,
        IsAdopted INTEGER NOT NULL DEFAULT 0
    );";

                using var command = new SqliteCommand(tableCommand, connection);
                command.ExecuteNonQuery();
            }
            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Додати собаку");
                Console.WriteLine("2. Переглянути всіх собак");
                Console.WriteLine("3. Переглянути собак у притулку");
                Console.WriteLine("4. Переглянути прилаштованих собак");
                Console.WriteLine("5. Пошук за ID");
                Console.WriteLine("6. Пошук за клічкою");
                Console.WriteLine("7. Пошук за породою");
                Console.WriteLine("8. Оновити дані собаки");
                Console.WriteLine("9. Вихід");
                Console.Write("> Оберіть опцію: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Write("Ім’я: ");
                        var name = Console.ReadLine()!;
                        Console.Write("Вік: ");
                        var age = int.Parse(Console.ReadLine()!);
                        Console.Write("Порода: ");
                        var breed = Console.ReadLine()!;
                        service.AddDog(new Dog { Name = name, Age = age, Breed = breed });
                        break;

                    case "2": service.ShowAllDogs(); break;
                    case "3": service.ShowAvailableDogs(); break;
                    case "4": service.ShowAdoptedDogs(); break;

                    case "5":
                        Console.Write("Введіть ID: ");
                        service.FindById(int.Parse(Console.ReadLine()!));
                        break;

                    case "6":
                        Console.Write("Клічка: ");
                        service.FindByName(Console.ReadLine()!);
                        break;

                    case "7":
                        Console.Write("Порода: ");
                        service.FindByBreed(Console.ReadLine()!);
                        break;

                    case "8":
                        Console.Write("ID собаки: ");
                        var id = int.Parse(Console.ReadLine()!);
                        Console.Write("Нова клічка: ");
                        var newName = Console.ReadLine()!;
                        Console.Write("Новий вік: ");
                        var newAge = int.Parse(Console.ReadLine()!);
                        Console.Write("Нова порода: ");
                        var newBreed = Console.ReadLine()!;
                        Console.Write("Прилаштований? (yes/no): ");
                        var adopted = Console.ReadLine()!.ToLower() == "yes";

                        service.UpdateDog(id, new Dog
                        {
                            Name = newName,
                            Age = newAge,
                            Breed = newBreed,
                            IsAdopted = adopted
                        });
                        break;

                    case "9":
                        return;
                }
            }
        }
    }
}
