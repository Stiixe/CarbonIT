using TreasureHunt;

if (args.Length > 0 && (args[0] == "--help" || args[0] == "-h"))
{
    Console.WriteLine("Take as parameter the input file. By default, takes input.txt in the current directory.");
    Console.WriteLine("-g or --graphic : display map after every movement");
}



string inputPath;

if (args.Length > 0 && File.Exists(args[0]))
    inputPath = args[0];
else
{
    Directory.GetCurrentDirectory();
    string path = Path.Combine(Directory.GetCurrentDirectory(), "input.txt");
    if (File.Exists(path))
        inputPath = path;
    else
    {
        Console.WriteLine("No input file given.");
        return;
    }
}

bool IsGraphic = args.Any(arg => arg == "-g" || string.Compare(arg, "--graphic", true) == 0);

Controller controller = new Controller(IsGraphic);
controller.Initialize(inputPath);
Console.WriteLine(controller.Map);
controller.RunExpedition();
Console.WriteLine(controller.Map);
controller.WriteResult();