using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace lab2
{
    internal class Program
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
        private static string FileExistsToString(bool exists)
        {
            return exists ? "Exists" : "Not Found";
        }
        [Option(ShortName = "i")]
        public string InputFile { get; }
        [Option(ShortName = "o")]
        public string OutputFile { get; }

        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        private void OnExecute()
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
            if (n < 1 || n > 200)
            {
                throw new ArgumentException($"Input value does not match criteria 1 <= n <= 6. Value {n}");
            }
            var chips = inputData.Substring(1).Split(' ',StringSplitOptions.RemoveEmptyEntries).Select((item)=>ConvertToInt32(item)).ToArray();
            foreach (var item in chips)
            {
                if(item<1 || item > Math.Pow(10, 5))
                {
                    throw new ArgumentException($"Input value does not match criteria 1 <= a <= 10^5. Value{item}");
                }
            }
            List<int> winningMoves = Solve(chips);

            Console.WriteLine("=========================OUTPUT=============================");
            using (StreamWriter sw = new StreamWriter(OutputFile))
            {
                Console.WriteLine(winningMoves.Count);
                sw.WriteLine(winningMoves.Count);
                if (winningMoves.Count > 0)
                {
                    Console.WriteLine(string.Join(" ", winningMoves));
                    sw.WriteLine(string.Join(" ", winningMoves));
                }
            }
        }
        public static List<int> Solve(int[] cards)
        {
            var ans = new List<int>();
            for(int i = 0; i < cards.Length; i++)
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
            if(step > 0){
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
            if(curLen % 2 == 2-player)
            {
                if(!Wins(step, 3 - player, firstIndex, a))
                {
                    return true;
                }
            }
            for(int i = 0; i < n; i++)
            {
                int newStep = Gcd(Math.Abs(a[i] - a[firstIndex]), step);
                if(step == 0)
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