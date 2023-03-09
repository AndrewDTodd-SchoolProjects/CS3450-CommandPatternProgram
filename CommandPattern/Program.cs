using System.Drawing;

namespace CommandPattern
{
    internal class Program
    {
        static readonly Dictionary<string, Database> _databases = new();
        static readonly List<ICommand> _pendingCommands = new();
        static readonly CommandInvoker _commandInvoker = new();

        static void Main(string[] args)
        {
            string? path;

            if(args.Length != 0)
            {
                path = args[0];
            }
            else 
            {
                Console.WriteLine("Enter a path to a text file containing commands");
                path = Console.ReadLine();
                Console.WriteLine();
            }

            try
            {
                ParseFile(path);
            }
            catch(Exception ex)
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.Error.WriteLine($"Was unable to parse the file you input\n{path} may be a bad path, or invalid file\n{ex.Message}");
                Console.ForegroundColor = color;
                return;
            }

            if (_pendingCommands.Count > 0)
                _commandInvoker.InvokeRangeCommands(_pendingCommands);

            Console.WriteLine("Contents of Databases:");
            foreach(Database bd in _databases.Values) 
            {
                Console.WriteLine(bd.ToString());
            }

            while(_commandInvoker.UndoLastCommand(out Database? db))
            {
                if (db != null)
                    Console.WriteLine(db.ToString());
            }

            Console.WriteLine("Databases Final State:");
            foreach (Database bd in _databases.Values)
            {
                Console.WriteLine(bd.ToString());
            }

            Console.WriteLine("Press any key to exit the application");
            Console.ReadKey();
        }

        private static void ParseFile(string? filePath)
        {
            if(string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            int lineNum = 0;
            using var reader = new StreamReader(filePath);
            do
            {
                string? line = reader.ReadLine();
                lineNum++;

                if (!string.IsNullOrEmpty(line))
                {
                    string[] lineComps = line.Split(' ');

                    string command = lineComps[0];
                    command = command.ToUpper();

                    if(command == "B")
                    {
                        ICommand? macro = CreateMacro(reader, ref lineNum);
                        if(macro != null)
                            _pendingCommands.Add(macro);

                        continue;
                    }

                    if (lineComps.Length > 1)
                    {
                        string dbName = lineComps[1];
                        dbName = dbName.ToLower();

                        string key = lineComps[2];
                        key = key.ToLower();

                        if (!_databases.TryGetValue(dbName, out Database? db))
                        {
                            db = new(dbName);

                            _databases.Add(dbName, db);
                        }

                        switch (command)
                        {
                            case "A":
                                _pendingCommands.Add(new AddCommand(key, line[(command.Length + 1 + dbName.Length + 1 + key.Length + 1)..], db));
                                break;

                            case "U":
                                _pendingCommands.Add(new UpdateCommand(key, line[(command.Length + 1 + dbName.Length + 1 + key.Length + 1)..], db));
                                break;

                            case "R":
                                _pendingCommands.Add(new RemoveCommand(key, db));
                                break;

                            default:
                                ConsoleColor color = Console.ForegroundColor;
                                Console.ForegroundColor = ConsoleColor.Red;

                                Console.Error.WriteLine($"The command ({command}) on line {lineNum} is unrecognized.\nWould you like to skip this line and continue execution on the next line?\nEnter Y for yes, any other key for no");
                                Console.ForegroundColor = color;

                                if (Console.ReadKey().Key == ConsoleKey.Y)
                                {
                                    Console.WriteLine();
                                    continue;
                                }
                                else
                                    return;
                        }
                    }
                }

            } while (!reader.EndOfStream);
        }

        private static MacroCommand? CreateMacro(StreamReader reader, ref int lineNum)
        {
            MacroCommand? macro = null;

            bool run = true;
            do
            {
                string? line = reader.ReadLine();
                lineNum++;

                if (!string.IsNullOrEmpty(line))
                {
                    string[] lineComps = line.Split(' ');

                    string command = lineComps[0];
                    command = command.ToUpper();

                    if (command == "B")
                    {
                        CreateMacro(reader, ref lineNum);
                        continue;
                    }
                    else if(command == "E")
                    {
                        break;
                    }

                    if (lineComps.Length > 1)
                    {
                        string dbName = lineComps[1];
                        dbName = dbName.ToLower();

                        string key = lineComps[2];
                        key = key.ToLower();

                        if (!_databases.TryGetValue(dbName, out Database? db))
                        {
                            db = new(dbName);

                            _databases.Add(dbName, db);
                        }

                        macro ??= new(db);

                        switch (command)
                        {
                            case "A":
                                macro.Add(new AddCommand(key, line[(command.Length + 1 + dbName.Length + 1 + key.Length + 1)..], db));
                                break;

                            case "U":
                                macro.Add(new UpdateCommand(key, line[(command.Length + 1 + dbName.Length + 1 + key.Length + 1)..], db));
                                break;

                            case "R":
                                macro.Add(new RemoveCommand(key, db));
                                break;

                            default:
                                ConsoleColor color = Console.ForegroundColor;
                                Console.ForegroundColor = ConsoleColor.Red;

                                Console.Error.WriteLine($"The command ({command}) on line {lineNum} is unrecognized.\nWould you like to skip this line and continue execution on the next line?\nEnter Y for yes, any other key for no");
                                Console.ForegroundColor = color;

                                if (Console.ReadKey().Key == ConsoleKey.Y)
                                {
                                    Console.WriteLine();
                                    continue;
                                }
                                else
                                    run = false;
                                break;
                        }
                    }
                }

            } while (run);

            return macro;
        }
    }
}