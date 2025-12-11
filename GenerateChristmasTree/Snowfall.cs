namespace GenerateChristmasTree;

public struct SnowCordinates
{
    public int X { get; }
    public int Y { get; }

    public SnowCordinates(int x, int y)
    {
        X = x; 
        Y = y;
    }
}

public class Snowfall
{
    private Random _rnd = new Random();
    private DrawTree _tree;

    private int _windowWidth;
    private int _windowHeight;

    private List<SnowCordinates> _snowflakes = new List<SnowCordinates>();

    private bool[,] _snowMap;

    private double chance = 1.0;

    public Snowfall(DrawTree tree)
    {
        _tree = tree;
        GetWinSize();

        _snowMap = new bool[_windowWidth, _windowHeight];
    }

    private void GetWinSize()
    {
        _windowHeight = Console.WindowHeight;
        _windowWidth = Console.WindowWidth;


        Console.BufferHeight = _windowHeight;
        Console.BufferWidth = _windowWidth;
    }

    private void SpawnNewSnowflakes(List<SnowCordinates> listToAddTo)
    {
        if (_rnd.NextDouble() < chance)
        {
            int x = _rnd.Next(0, _windowWidth);
            if (!_snowMap[x, 0])
            {
                SnowCordinates snowflake = new SnowCordinates(x, 0);
                listToAddTo.Add(snowflake);
            }
        }
    }

    public void Update()
    {
        var nextSnowlakes = new List<SnowCordinates>();

        foreach (var snowflake in _snowflakes)
        {
            int nextY = snowflake.Y + 1;
            bool hitBottom = nextY >= _windowHeight;
            bool hitSnow = !hitBottom && _snowMap[snowflake.X, nextY];

            bool hitTree = _tree.IsTree(snowflake.X, nextY);

            if (hitTree)
            {                
                Console.SetCursorPosition(snowflake.X, snowflake.Y);
                if (!_tree.IsTree(snowflake.X, snowflake.Y))
                {
                    Console.Write(" ");
                }
               
                continue; 
            }

            if (hitBottom || hitSnow)
            {
                
                if(_rnd.NextDouble() > 0.5)
                {
                    _snowMap[snowflake.X, snowflake.Y] = true;
                    Console.SetCursorPosition(snowflake.X, snowflake.Y);
                    Console.Write("█");
                }
                else
                {
                    Console.SetCursorPosition(snowflake.X, snowflake.Y);
                    Console.Write(" ");
                }

                
            }
            else
            {
                Console.SetCursorPosition(snowflake.X, snowflake.Y);
                if (!_tree.IsTree(snowflake.X, snowflake.Y))
                {
                    Console.Write(" ");
                }

                SnowCordinates nextStep = new SnowCordinates(snowflake.X, nextY);
                nextSnowlakes.Add(nextStep);
            }
        }

        SpawnNewSnowflakes(nextSnowlakes);

        foreach (var snowflake in nextSnowlakes)
        {
            Console.SetCursorPosition(snowflake.X, snowflake.Y);
            if (!_tree.IsTree(snowflake.X, snowflake.Y))
            {
                Console.Write("❆");
            }
        }

        _snowflakes = nextSnowlakes;
    }
}
