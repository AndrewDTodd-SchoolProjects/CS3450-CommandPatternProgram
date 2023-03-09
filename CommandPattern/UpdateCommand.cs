using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    internal class UpdateCommand : ICommand
    {
        private readonly string _key;
        private readonly string _value;
        private string? _oldValue;

        public UpdateCommand(string key, string value, Database db) : base(db) =>
            (_key, _value) = (key, value);

        public override void Do()
        {
            _oldValue = _database.GetValue(_key);

            _database.UpdateValue(_key, _value);
        }
        public override void Undo()
        {
            if(!string.IsNullOrEmpty(_oldValue))
                _database.UpdateValue(_key, _oldValue);

            Console.WriteLine("Undid UpdateCommand");
        }
    }
}
