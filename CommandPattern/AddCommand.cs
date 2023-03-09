using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    internal class AddCommand: ICommand
    {
        private readonly string _key;
        private readonly string _value;

        public AddCommand(string key, string value, Database db) : base(db) =>
            (_key, _value) = (key, value);

        public override void Do()
        {
            _database.Add(_key, _value);
        }
        public override void Undo()
        {
            _database.Remove(_key);
            Console.WriteLine("Undid AddCommand");
        }
    }
}
