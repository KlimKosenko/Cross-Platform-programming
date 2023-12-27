using System.Globalization;
using System.Runtime.InteropServices;

namespace ClassLibrary1
{
    public static class Lab1
    {
        static int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        static int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

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
        private static string FileExistsToString(bool exists)
        {
            return exists ? "Exists" : "Not Found";
        }
        public static void Run(string InputFile, string OutputFile)
        {
            bool inExists = System.IO.File.Exists(InputFile);
            bool outExists = System.IO.File.Exists(OutputFile);

            Console.WriteLine("======================================");
            Console.WriteLine($"Input file {InputFile} {FileExistsToString(inExists)}!");
            Console.WriteLine($"Output file {OutputFile} {FileExistsToString(outExists)}!");
            Console.WriteLine("======================================");
            Console.WriteLine("");
            Console.WriteLine("======================================");
            Console.WriteLine("================Input File Content============");
            var inputData = System.IO.File.ReadAllText(InputFile);
            Console.WriteLine(inputData);

            var n = ConvertToInt32(inputData.Substring(0, 1));
            if (n<1 || n>6)
            {
                throw new ArgumentException($"Input value does not match criteria 1 <= n <= 6. Value {n}");
            }
            var map = inputData.Substring(1).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var board = new char[2 * n + 1, 2 * n + 1];
            for (int i = 0; i < 2 * n + 1; i++)
            {
                string line = map[i];
                for (int j = 0; j < 2 * n + 1; j++)
                {
                    board[i, j] = line[j];
                }
            }
            Tuple<int, List<Tuple<int, int>>> result = Solve(n, board);

            Console.WriteLine("================ Result ============");
            using (StreamWriter writer = new StreamWriter(OutputFile))
            {
                if (result.Item1 == -1)
                {
                    Console.WriteLine("IMPOSSIBLE");
                    writer.WriteLine("IMPOSSIBLE");
                }
                else
                {
                    writer.WriteLine(result.Item1);
                    Console.WriteLine(result.Item1);
                    foreach (var move in result.Item2)
                    {
                        writer.WriteLine($"{move.Item1} {move.Item2}");
                        Console.WriteLine($"{move.Item1} {move.Item2}");
                    }
                }
            }
        }

        static Tuple<int, List<Tuple<int, int>>> Solve(int n, char[,] board)
        {
            Queue<Tuple<int, int, List<Tuple<int, int>>>> queue = new Queue<Tuple<int, int, List<Tuple<int, int>>>>();
            bool[,,] visited = new bool[2 * n + 1, 2 * n + 1, 4];
            queue.Enqueue(new Tuple<int, int, List<Tuple<int, int>>>(n, n, new List<Tuple<int, int>>()));
            visited[n, n, 0] = true;

            while (queue.Count > 0)
            {
                Tuple<int, int, List<Tuple<int, int>>> current = queue.Dequeue();
                int x = current.Item1;
                int y = current.Item2;
                List<Tuple<int, int>> path = current.Item3;

                if (board[x, y] == 't')
                {
                    return new Tuple<int, List<Tuple<int, int>>>(path.Count, path);
                }

                for (int i = 0; i < 8; i++)
                {
                    int newX = x + dx[i];
                    int newY = y + dy[i];
                    int newDir = i % 4;

                    if (newX >= 0 && newX < 2 * n + 1 && newY >= 0 && newY < 2 * n + 1 &&
                        !visited[newX, newY, newDir] && board[newX, newY] != 'O')
                    {
                        visited[newX, newY, newDir] = true;
                        List<Tuple<int, int>> newPath = new List<Tuple<int, int>>(path);
                        newPath.Add(new Tuple<int, int>(newX, newY));
                        queue.Enqueue(new Tuple<int, int, List<Tuple<int, int>>>(newX, newY, newPath));
                    }
                }
            }

            return new Tuple<int, List<Tuple<int, int>>>(-1, null);
        }
    }
}