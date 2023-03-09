using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommandPattern
{
    internal class MacroCommand : ICommand
    {
        private readonly List<ICommand> commands = new();
        public MacroCommand(Database bd) : base(bd) { }

        public void Add(ICommand command)
        {
            commands.Add(command);
        }

        public override void Do()
        {
            Console.WriteLine("Beginning a Macro");

            foreach (ICommand command in commands)
            {
                command.Do();
            }

            Console.WriteLine("Ending a Macro\n");
        }

        public override void Undo()
        {
            Console.WriteLine("Begin Undoing Macro");

            while (commands.Count > 0) 
            {
                commands.Last().Undo();
                commands.RemoveAt(commands.Count - 1);
            }

            Console.WriteLine("End Undoing Macro\n");
        }
    }
}
