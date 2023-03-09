using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    internal class Database
    {
        private readonly SortedDictionary<string, string> _data;

        private readonly string _name;

        public SortedDictionary<string, string> Data { get => _data; }
        public string Name { get => _name; }

        public string? this[string key]
        {
            get
            {
                try
                {
                    return _data[key];
                }
                catch (KeyNotFoundException)
                {
                    ConsoleColor color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.Error.WriteLine($"Your entered key ({key}) is not in the database");
                    Console.ForegroundColor = color;

                    return null;
                }
            }
            //can preform validation if necessary
            set
            {
                try
                {
                    if (!string.IsNullOrEmpty(value))
                        _data[key] = value;
                    else
                    {
                        ConsoleColor color = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.Error.WriteLine("The value you entered is not valid. It is null or empty");
                        Console.ForegroundColor = color;
                    }
                }
                catch (KeyNotFoundException)
                {
                    ConsoleColor color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.Error.WriteLine($"Your entered key ({key}) is not in the database");
                    Console.ForegroundColor = color;
                }
            }
        }

        public Database(string name) =>
            (_data, _name) = (new(), name);

        public void Add(string key, string value)
        {
            try
            {
                _data.Add(key, value);
            }
            catch (ArgumentException ex)
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.Error.WriteLine($"Was unable to add key value pair ({key},{value})\n{ex.Message}");
                Console.ForegroundColor = color;
            }
        }

        public string? GetValue(string key)
        {

            try
            {
                return _data[key];
            }
            catch(KeyNotFoundException)
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.Error.WriteLine($"Your entered key ({key}) is not in the database");
                Console.ForegroundColor = color;

                return null;
            }
        }

        public void UpdateValue(string key, string value)
        {
            try
            {
                _data[key] = value;
            }
            catch(KeyNotFoundException)
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.Error.WriteLine($"Your entered key ({key}) is not in the database");
                Console.ForegroundColor = color;
            }
        }

        public void Remove(string key)
        {
            if (!_data.Remove(key))
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.Error.WriteLine($"Your entered key ({key}) could not be removed from the database. Confirm that the database actually contains the key");
                Console.ForegroundColor = color;
            }
        }

        public override string? ToString()
        {
            string db = $"Database \"{this._name}\":\n";

            foreach (KeyValuePair<string, string> pair in _data)
            {
                db += $"{pair.Key} | {pair.Value}\n";
            }

            return db;
        }
    }
}
