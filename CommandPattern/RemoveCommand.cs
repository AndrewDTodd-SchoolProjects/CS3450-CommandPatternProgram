using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    internal class RemoveCommand : ICommand
    {
        private readonly string _key;
        private string? _value;

        public RemoveCommand(string key, Database db) : base(db) =>
            _key = key;

        public override void Do()
        {
            _value = _database.GetValue(_key);

            _database.Remove(_key);
        }
        public override void Undo()
        {
            if(!string.IsNullOrEmpty(_value))
                _database.Add(_key, _value);

            Console.WriteLine("Undid RemoveCommand");
        }
    }
}
