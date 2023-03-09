using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    internal abstract class ICommand
    {
        protected readonly Database _database;

        public Database Database { get => _database; }

        protected ICommand(Database db) =>
            (_database) = (db);

        public abstract void Do();
        public abstract void Undo();
    }
}
