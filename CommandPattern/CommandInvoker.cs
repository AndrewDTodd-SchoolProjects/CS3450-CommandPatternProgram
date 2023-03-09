using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    internal class CommandInvoker
    {
        private readonly List<ICommand> _commands = new();

        public void InvokeCommand(ICommand command)
        {
            command.Do();

            _commands.Add(command);
        }

        public void InvokeRangeCommands(IEnumerable<ICommand> commands)
        {
            foreach(ICommand command in commands) 
            {
                command.Do();
            }

            _commands.AddRange(commands);
        }

        public void UndoCommands()
        {
            while(_commands.Count > 0 ) 
            {
                _commands.Last().Undo();
                _commands.RemoveAt(_commands.Count - 1);
            }
        }
        public void UndoRangeCommands(int begin, int end)
        {
            for(int i = begin; i < end; i++) 
            {
                _commands[i].Undo();
            }

            _commands.RemoveRange(begin, (end - begin));
        }
        public bool UndoLastCommand(out Database? db)
        {
            if (_commands.Count > 0)
            {
                db = _commands.Last().Database;

                _commands.Last().Undo();
                _commands.RemoveAt(_commands.Count - 1);

                return true;
            }
            else
                db = null;
                return false;
        }
    }
}
