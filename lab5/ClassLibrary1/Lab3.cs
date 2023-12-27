using System.Globalization;
using System.Runtime.InteropServices;

namespace ClassLibrary1
{
    public static class Lab3
    {
        private static int ConvertToInt32(string value)
        {
            int result = 0;
            var parsed = Int32.TryParse(value, out result);

            if (!parsed)
            {
                throw new ArgumentException($"The value {value} was not parsed to Int");
            }
            return result;
        }
        public static string Run(string inputData)
        {
            var inputArray = inputData.Split(new char[] {' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int n = ConvertToInt32(inputArray[0]);
            if (n < 6)
            {
                throw new ArgumentException($"Input value does not match criteria n >= 6. Value {n}");
            }
            int m = ConvertToInt32(inputArray[1]);
            if (!(m <= 100 && m > 0))
            {
                throw new ArgumentException($"Input value does not match criteria 1 <= m <= 100. Value {m}");
            }
            int startI = -1, startJ = -1;
            char[,] map = new char[n, m];
            for (int i = 2; i < inputArray.Length; i++)
            {
                var arr = inputArray[i].ToCharArray();
                for (int j = 0; j < arr.Length; j++)
                {
                    if (arr[j] == 'K')
                    {
                        startI = i - 2;
                        startJ = j;
                        map[i - 2, j] = 'K';
                    }
                    else if (arr[j] == '.')
                    {
                        map[i - 2, j] = '.';
                    }
                }
            }
            int[,] result = KnightTour(n, m, startI, startJ, map);

            string output = "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    output += result[i, j].ToString() + " ";
                }
                output += "\n";
            }
            return output;
        }
        static bool IsValidMove(int x, int y, int[,] visited, int n, int m)
        {
            return 0 <= x && x < n && 0 <= y && y < m && visited[x, y] == 0;
        }

        static int[,] KnightTour(int n, int m, int startX, int startY, char[,] field)
        {
            int[,] visited = new int[n, m];

            int[][] moves = { new int[] { -2, -1 }, new int[] { -2, 1 }, new int[] { -1, -2 }, new int[] { -1, 2 },
                          new int[] { 1, -2 }, new int[] { 1, 2 }, new int[] { 2, -1 }, new int[] { 2, 1 } };

            int step = 1;
            visited[startX, startY] = step;
            var stack = new Stack<(int, int)>();
            stack.Push((startX, startY));

            while (stack.Count > 0)
            {
                var (currentX, currentY) = stack.Pop();
                foreach (var move in moves)
                {
                    int nextX = currentX + move[0];
                    int nextY = currentY + move[1];
                    if (IsValidMove(nextX, nextY, visited, n, m))
                    {
                        step++;
                        visited[nextX, nextY] = step;
                        stack.Push((nextX, nextY));
                    }
                }
            }

            return visited;
        }
    }
}