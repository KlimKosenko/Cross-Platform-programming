using System.Globalization;
using System.Runtime.InteropServices;

namespace ClassLibrary1
{
    public static class Lab2
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

            var n = ConvertToInt32(inputData.Substring(0, 1));
            if (n < 1 || n > 200)
            {
                throw new ArgumentException($"Input value does not match criteria 1 <= n <= 6. Value {n}");
            }
            var chips = inputData.Substring(1).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select((item) => ConvertToInt32(item)).ToArray();
            foreach (var item in chips)
            {
                if (item < 1 || item > Math.Pow(10, 5))
                {
                    throw new ArgumentException($"Input value does not match criteria 1 <= a <= 10^5. Value{item}");
                }
            }
            List<int> winningMoves = Solve(chips);

            return $"{winningMoves.Count}\n{String.Join(" ", winningMoves.ToArray())}";
        }
        public static List<int> Solve(int[] cards)
        {
            var ans = new List<int>();
            for (int i = 0; i < cards.Length; i++)
            {
                if (!Wins(0, 2, i, cards))
                {
                    ans.Add(cards[i]);
                }
            }
            return ans;
        }
        public static bool Wins(int step, int player, int firstIndex, int[] a)
        {
            int n = a.Length;
            int curLen;
            if (step > 0)
            {
                curLen = 0;
                for (int i = 0; i < n; i++)
                {
                    if ((a[i] - a[firstIndex]) % step == 0)
                    {
                        curLen++;
                    }
                }
            }
            else
            {
                curLen = 1;
            }
            if (curLen % 2 == 2 - player)
            {
                if (!Wins(step, 3 - player, firstIndex, a))
                {
                    return true;
                }
            }
            for (int i = 0; i < n; i++)
            {
                int newStep = Gcd(Math.Abs(a[i] - a[firstIndex]), step);
                if (step == 0)
                {
                    if (newStep > 1)
                    {
                        if (!Wins(newStep, 3 - player, firstIndex, a))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (newStep > 1 && newStep < step)
                    {
                        if (!Wins(newStep, 3 - player, firstIndex, a))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static int Gcd(int a, int b)
        {
            while (b != 0)
            {
                int t = a % b;
                a = b;
                b = t;
            }
            return a;
        }
    }
}