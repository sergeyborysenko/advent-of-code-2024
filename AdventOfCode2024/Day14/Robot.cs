using System.Text.RegularExpressions;

internal class Robot
{
    private const string inputPattern = @"p=(?<py>-?\d+),(?<px>-?\d+) v=(?<vy>-?\d+),(?<vx>-?\d+)";

    public (int X, int Y) Position { get; set; }
    public (int X, int Y) Vector { get; set; }

    public void Move(int height, int width, int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            var potentialPosition = (Position.X + Vector.X, Position.Y + Vector.Y);
            Position = IsOutOfBounds(potentialPosition, height, width) ?
                GetTeleportedPosition(potentialPosition, height, width)
                : potentialPosition;
        }
    }

    private static (int X, int Y) GetTeleportedPosition((int X, int Y) potentialPosition, int height, int width)
    {
        if (potentialPosition.X < 0)
        {
            potentialPosition.X = height + potentialPosition.X;
        }
        if (potentialPosition.X >= height)
        {
            potentialPosition.X -= height;
        }
        if (potentialPosition.Y < 0)
        {
            potentialPosition.Y = width + potentialPosition.Y;
        }
        if (potentialPosition.Y >= width)
        {
            potentialPosition.Y -= width;
        }
        return potentialPosition;
    }

    private static bool IsOutOfBounds((int X, int Y) potentialPosition, int height, int width) => potentialPosition.X < 0 || potentialPosition.X >= height || potentialPosition.Y < 0 || potentialPosition.Y >= width;

    public static bool TryParse(string input, out Robot? robot)
    {
        try
        {
            Regex regex = new(inputPattern);
            var match = regex.Match(input);
            if (!match.Success)
            {
                robot = null;
                return false;
            }
            int posX = int.Parse(match.Groups["px"].Value);
            int posY = int.Parse(match.Groups["py"].Value);
            int vecX = int.Parse(match.Groups["vx"].Value);
            int vecY = int.Parse(match.Groups["vy"].Value);

            robot = new Robot
            {
                Position = (posX, posY),
                Vector = (vecX, vecY)
            };
            return true;
        }
        catch
        {
            robot = null;
            return false;
        }
    }
}