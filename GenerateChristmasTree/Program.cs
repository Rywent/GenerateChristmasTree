using GenerateChristmasTree;
using System.Text;

class Program
{
    static private DrawTree? _tree;
    static private Snowfall? _snowfall;
    static void Main(string[] args)
    {
        Input();
    }

    static void Input()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Enter the number of tiers: ");
        if (!int.TryParse(Console.ReadLine(), out int tiers))
        {
            Console.WriteLine("Error!");
        }

        _tree = new DrawTree(tiers);
        _snowfall = new Snowfall(_tree);
        _tree.PrintTree();
        Run();
    }


    static void Run()
    {
        Console.CursorVisible = false;
        while (true)
        {
            if (Console.KeyAvailable) break;

            _snowfall.Update();
            _tree.Animate();

            Thread.Sleep(200);
        }

    }
}