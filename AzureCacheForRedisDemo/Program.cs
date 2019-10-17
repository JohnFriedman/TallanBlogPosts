using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureCacheForRedisDemo
{
    class Program
    {
        static List<Dwarf> dwarves;

        static void Main(string[] args)
        {
            dwarves = initializeSevenDwarves();

            Dwarf doc = AzureRedisCache.GetEntity("Doc", GetDwarfByName, TimeSpan.FromMinutes(5));
            Console.WriteLine($"Name: {doc.Name}. Age: {doc.Age}. Is Happy?: {doc.IsHappy}.\n");

            Dwarf grumpy = AzureRedisCache.GetEntity("Grumpy", GetDwarfByName, TimeSpan.FromMinutes(5));
            Console.WriteLine($"Name: {grumpy.Name}. Age: {grumpy.Age}. Is Happy?: {grumpy.IsHappy}.\n");

            Console.WriteLine("Now it's Grumpy's Birthday!");
            AzureRedisCache.UpdateEntity(grumpy.Name, grumpy, TriggerDwarfBirthday, TimeSpan.FromMinutes(5));
            Console.WriteLine($"Name: {grumpy.Name}. Age: {grumpy.Age}. Is Happy?: {grumpy.IsHappy}.");
            Console.ReadKey();
        }

        static Dwarf GetDwarfByName(string name)
        {
            return dwarves.FirstOrDefault(d => d.Name == name);
        }

        static void TriggerDwarfBirthday(Dwarf dwarf)
        {
            dwarf.Age++;
            dwarf.IsHappy = true;
        }

        class Dwarf
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public bool IsHappy { get; set; }
        }

        static List<Dwarf> initializeSevenDwarves()
        {
            return new List<Dwarf>
            {
                new Dwarf
                {
                    Name = "Bashful",
                    Age = 80,
                    IsHappy = true
                },
                new Dwarf
                {
                    Name = "Doc",
                    Age = 85,
                    IsHappy = true
                },
                new Dwarf
                {
                    Name = "Grumpy",
                    Age = 99,
                    IsHappy = false
                },
                new Dwarf
                {
                    Name = "Happy",
                    Age = 77,
                    IsHappy = true
                },
                new Dwarf
                {
                    Name = "Sneezy",
                    Age = 84,
                    IsHappy = true
                },
                new Dwarf
                {
                    Name = "Sleepy",
                    Age = 98,
                    IsHappy = false
                },
                new Dwarf
                {
                    Name = "Dopey",
                    Age = 50,
                    IsHappy = false
                }
            };
        }
    }
}
