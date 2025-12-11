
namespace GenerateChristmasTree;

public struct TreeElement
{
    public char Element { get; }
    public ConsoleColor Color { get; }

    public TreeElement(char element, ConsoleColor color)
    {
        Element = element;
        Color = color;
    }
}

public struct Garland
{
    public int X { get; }
    public int Y { get; }

    public Garland(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class DrawTree
{
    private readonly Random _rnd;
    private readonly int _tiers;
    private readonly List<List<TreeElement>> _christmasTree = new();
    private readonly List<Garland> _garlandCoordinates = new();

    private readonly char[] _decorations = { 'o', 'O', '0', '&' };
    private readonly ConsoleColor[] _colors = { ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Cyan };

    private int _startY;
    private int _startX;

    public DrawTree(int tiers)
    {
        _tiers = tiers;
        _rnd = new Random();

        BuildTree();

        CalculatePosition();
    }

    private void CalculatePosition()
    {
        int windowHeight = Console.WindowHeight;
        int windowWidth = Console.WindowWidth;
        int treeHeight = _christmasTree.Count;

        int maxTreeWidth = 0;
        foreach (var line in _christmasTree)
        {
            if (line.Count > maxTreeWidth) maxTreeWidth = line.Count;
        }

        _startY = Math.Max(0, windowHeight - treeHeight - 1);
        _startX = Math.Max(0, (windowWidth - maxTreeWidth) / 2);
    }


    public void PrintTree()
    {
        Console.Clear();

        for (int y = 0; y < _christmasTree.Count; y++)
        {
            var line = _christmasTree[y];
            Console.SetCursorPosition(_startX, _startY + y);

            foreach (var coloredChar in line)
            {
                Console.ForegroundColor = coloredChar.Color;
                Console.Write(coloredChar.Element);
            }
        }
        Console.ResetColor();
    }

    private void BuildTree()
    {
        _christmasTree.AddRange(DrawStar());

        for (int i = 2; i <= _tiers; i++)
        {
            _christmasTree.Add(DrawBranch(i));
        }

        _christmasTree.AddRange(DrawStump());

        ScanGarland();
    }

    public void Animate()
    {
        Console.CursorVisible = false;

        foreach (var garland in _garlandCoordinates)
        {

            int screenX = _startX + garland.X;
            int screenY = _startY + garland.Y;

            if (screenY >= Console.WindowHeight || screenX >= Console.WindowWidth) continue;

            Console.SetCursorPosition(screenX, screenY);

            char currentDecoration = _christmasTree[garland.Y][garland.X].Element;

            ConsoleColor randomColor = _colors[_rnd.Next(_colors.Length)];
            Console.ForegroundColor = randomColor;

            Console.Write(currentDecoration);
        }

        Console.SetCursorPosition(0, 0);

        Console.ResetColor();
    }

    private void ScanGarland()
    {
        _garlandCoordinates.Clear();
        
        for(int y = 0; y < _christmasTree.Count; y++)
        {
            var line = _christmasTree[y];
            for(int x = 0; x < line.Count; x++)
            {
                if (_decorations.Contains(line[x].Element))
                    _garlandCoordinates.Add(new Garland(x,y));

            }
        }
    }

    private List<List<TreeElement>> DrawStar()
    {
        var star = new List<List<TreeElement>>();
        int spaceCount = _tiers - 3;
        string indent = GetSpaces(spaceCount);

        var top = new List<TreeElement>();
        top.AddRange(ConvertToTreeElement(indent, ConsoleColor.White));
        top.AddRange(ConvertToTreeElement("  *  ", ConsoleColor.Yellow));
        star.Add(top);


        var middle = new List<TreeElement>();
        middle.AddRange(ConvertToTreeElement(indent, ConsoleColor.White));
        middle.AddRange(ConvertToTreeElement("*****", ConsoleColor.Yellow));
        star.Add(middle);


        var bottom = new List<TreeElement>();
        bottom.AddRange(ConvertToTreeElement(indent, ConsoleColor.White));
        bottom.AddRange(ConvertToTreeElement(" *", ConsoleColor.Yellow));
        bottom.Add(new TreeElement('*', ConsoleColor.Green));
        bottom.AddRange(ConvertToTreeElement("* ", ConsoleColor.Yellow));
        star.Add(bottom);

        return star;
    }


    private List<TreeElement> DrawBranch(int index)
    {
        var branchLine = new List<TreeElement>();
        bool lastWasDecoration = false;
        int spaceNeed = _tiers - index;

        branchLine.AddRange(ConvertToTreeElement(GetSpaces(spaceNeed), ConsoleColor.White));

        for (int i = 0; i < (index * 2) - 1; i++)
        {
            if (_rnd.NextDouble() < 0.2 && !lastWasDecoration)
            {
                branchLine.Add(GetDecoration());
                lastWasDecoration = true;
            }
            else
            {
                branchLine.Add(new TreeElement('*', ConsoleColor.Green));
                lastWasDecoration = false;
            }
        }
        return branchLine;
    }

    private List<List<TreeElement>> DrawStump()
    {
        var stumpLines = new List<List<TreeElement>>();
        int treeHeight = _tiers / 3;
        int treeWidth = _tiers / 4;
        int spaces = (_tiers - 1) - (treeWidth / 2);
        string indent = GetSpaces(spaces);
        string stumpPart = new string('|', treeWidth);

        for (int i = 0; i < treeHeight; i++)
        {
            var line = new List<TreeElement>();
            line.AddRange(ConvertToTreeElement(indent, ConsoleColor.White));
            line.AddRange(ConvertToTreeElement(stumpPart, ConsoleColor.DarkYellow));
            stumpLines.Add(line);
        }

        return stumpLines;
    }

    private TreeElement GetDecoration()
    {
        int randomDecoration = _rnd.Next(_decorations.Length);
        int randomColor = _rnd.Next(_colors.Length);
        return new TreeElement(_decorations[randomDecoration], _colors[randomColor]);
    }
    
    private IEnumerable<TreeElement> ConvertToTreeElement(string text, ConsoleColor color)
    {
        var list = new List<TreeElement>();
        foreach (char c in text)
        {
            list.Add(new TreeElement(c, color));
        }
        return list;
    }

    private string GetSpaces(int count) => new string(' ', count);
    public bool IsTree(int x, int y)
    {
        int localX = x - _startX;
        int localY = y - _startY;

        if (localY < 0 || localY >= _christmasTree.Count)
            return false;

        if (localX < 0 || localX >= _christmasTree[localY].Count)
            return false;

        return _christmasTree[localY][localX].Element != ' ';
    }
}
