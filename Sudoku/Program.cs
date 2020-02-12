using System;
using System.Diagnostics;

namespace checksud
{
    internal class Program
    {
        public static Stopwatch stopWatch = new Stopwatch();
        public static int inter = 0;

        private static void Main(string[] args)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine();

                int[,] sud1 = new int[9, 9];
                bool[,] fist1 = firstee(sud1);
                Console.WriteLine(" ~ Input");
                Console.WriteLine();
                printe(sud1, fist1, true);

                int input = 0;
                int bankx = 0;
                int banky = 0;
                int choise = 0;
                Console.Write("choose puzzle 1-2 ");
                choise = int.Parse(Console.ReadLine());
                sud1 = TestB(choise);
                Console.Clear();
                fist1 = firstee(sud1);
                printe(sud1, fist1, true);

                while (true)
                {
                    inter = 0;
                    stopWatch.Reset();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.Write(" ~ Enter the row of point you want to change or a negative number to submit: ");
                    input = int.Parse(Console.ReadLine());
                    if (input < 0) { break; }
                    while (input > 8)
                    {
                        Console.Write(" ~ Your input was invalid! please try again: ");
                        input = int.Parse(Console.ReadLine());
                    }
                    bankx = input;
                    Console.Write(" ~ Enter the collumn of point you want to change: ");
                    banky = int.Parse(Console.ReadLine());
                    while (banky > 8 || banky < 0)
                    {
                        Console.Write(" ~ Your input was invalid! please try again: ");
                        banky = int.Parse(Console.ReadLine());
                    }
                    Console.Write(" ~ Enter  the value you want to change (" + bankx + "," + banky + ") to: ");
                    input = int.Parse(Console.ReadLine());
                    while (input > 9 || input < 0)
                    {
                        Console.Write(" ~ Your input was invalid! please try again: ");
                        input = int.Parse(Console.ReadLine());
                    }
                    sud1[bankx, banky] = input;
                    fist1 = firstee(sud1);
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine(" ~ Input");
                    Console.WriteLine();
                    printe(sud1, fist1, true);
                }
                Console.Clear();

                int[,] copy = new int[9, 9];
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++) { copy[i, j] = sud1[i, j] + 0; }
                }
                bool checkedxd = check(sud1);
                bool sul = false;
                if (checkedxd == true) { sul = Solve(sud1, fist1, 0, 0); } else { sul = false; }
                Console.WriteLine();
                Console.WriteLine(" ~ Output");
                Console.WriteLine();
                if (sul == false)
                {
                    printe(copy, fist1, false, ConsoleColor.DarkRed);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.Write(" ~ ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("IMPOSSIBLE, NO SOLUTIONS FOUND!");
                }
                else
                {
                    printe(sud1, fist1);
                    Console.WriteLine();
                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" ~ Thank you for using Sudoku solver Ω!");

                Console.ReadLine();
                Console.Clear();
            }
        }

        private static bool Solve(int[,] sud, bool[,] first, int whoami, int speed = 0)
        {
            inter++;
            stopWatch.Start();
            if (speed != 0)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine(" ~ Process...");
                Console.WriteLine();
                printe(sud, first);
                //Console.ReadLine();
                System.Threading.Thread.Sleep(speed);
                Console.Clear();
            }
            int x = whoami % 9;
            float h = whoami / 9;
            int y = (int)(Math.Floor(h));
            if (first[y, x] == false) { if (whoami == 80) { return (check(sud)); } else { return (Solve(sud, first, whoami + 1, speed)); } }
            else
            {
                int[] poss = Getnums(sud, y, x);
                if (poss.Length == 0) { return (false); }
                if (whoami == 80)
                {
                    sud[y, x] = poss[0];

                    return (true);
                }

                for (int i = 0; i < poss.Length; i++)
                {
                    sud[y, x] = poss[i];
                    bool next = Solve(sud, first, whoami + 1, speed);
                    if (next == true)
                    {
                        sud[y, x] = poss[i];
                        return (true);
                    }
                }
                sud[y, x] = 0;
                return (false);
            }
        }

        private static bool check(int[,] sud)
        {
            for (int j = 0; j < 9; j++)
            {
                bool[] chec = { false, false, false, false, false, false, false, false, false };
                for (int i = 0; i < 9; i++)
                {
                    if (sud[i, j] != 0)
                    {
                        if (chec[sud[i, j] - 1] == true) { return (false); } else { chec[sud[i, j] - 1] = true; }
                    }
                }
            }
            for (int j = 0; j < 9; j++)
            {
                bool[] chec = { false, false, false, false, false, false, false, false, false };
                for (int i = 0; i < 9; i++)
                {
                    if (sud[j, i] != 0)
                    {
                        if (chec[sud[j, i] - 1] == true) { return (false); } else { chec[sud[j, i] - 1] = true; }
                    }
                }
            }
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    bool[] chec = { false, false, false, false, false, false, false, false, false };

                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            if (sud[j * 3 + k, i * 3 + l] != 0)
                            {
                                if (chec[sud[j * 3 + k, i * 3 + l] - 1] == true) { return (false); } else { chec[sud[j * 3 + k, i * 3 + l] - 1] = true; }
                            }
                        }
                    }
                }
            }

            return (true);
        }

        private static bool[,] firstee(int[,] sud)
        { bool[,] firste = new bool[9, 9]; for (int i = 0; i < 9; i++) { for (int j = 0; j < 9; j++) { if (sud[i, j] == 0) { firste[i, j] = true; } } } return (firste); }

        private static void printe(int[,] sud, bool[,] first, bool show = false, ConsoleColor c1 = ConsoleColor.Blue, ConsoleColor c2 = ConsoleColor.Green, ConsoleColor c3 = ConsoleColor.White, ConsoleColor c4 = ConsoleColor.Yellow)
        {
            if (show == true)
            {
                Console.Write("      ");
                for (int i = 0; i < 9; i++)
                {
                    Console.ForegroundColor = c4;
                    Console.Write("{0,2}", i);
                    if ((i + 1) % 3 == 0) { Console.Write("  "); }
                }
            }
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < 9; i++)
            {
                Console.ForegroundColor = c1;
                if (i % 3 == 0) { Console.WriteLine("     +-------+-------+-------+"); }
                Console.ForegroundColor = ConsoleColor.White;
                for (int j = 0; j < 9; j++)
                {
                    Console.ForegroundColor = c1;
                    if (j % 3 == 0) { if (j != 0) { Console.Write(" |"); } else { if (show == true) { Console.ForegroundColor = c4; Console.Write("{0,3}", i); Console.ForegroundColor = c1; } else { Console.Write("   "); } Console.Write("  |"); } }
                    Console.ForegroundColor = ConsoleColor.White;

                    if (first[i, j] == false) { Console.ForegroundColor = c2; } else { Console.ForegroundColor = c3; }
                    Console.Write("{0,2}", sud[i, j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.ForegroundColor = c1;
                Console.Write(" |");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
            Console.ForegroundColor = c1;
            Console.WriteLine("     +-------+-------+-------+");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static int[] Getnums(int[,] sud, int posx, int posy)
        {
            int[] checking = new int[27];
            for (int i = 0; i < 9; i++) { checking[i] = (sud[posx, i]); }
            for (int i = 0; i < 9; i++) { checking[i + 9] = (sud[i, posy]); }

            float hx = posx / 3;
            float hy = posy / 3;
            int xl = (int)(Math.Floor(hx));
            int yl = (int)(Math.Floor(hy));

            int counter = 18;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    checking[counter] = sud[xl * 3 + i, yl * 3 + j];
                    counter++;
                }
            }

            bool[] al = new bool[9];
            int n = 9;
            int[] empt = { };
            for (int i = 0; i < checking.Length; i++) { if (checking[i] != 0) { al[checking[i] - 1] = true; } }
            for (int i = 0; i < al.Length; i++) { if (al[i] == true) { n--; } }
            if (n == 0) { return (empt); }
            int[] nums = new int[n];
            int ino = 0;
            for (int i = 0; i < al.Length; i++) { if (al[i] == false) { nums[ino] = i + 1; ino++; } }
            return (nums);
        }

        public static int[,] TestB(int n)
        {
            int[,] Matrix1 =
            {
                                                { 0 , 0 , 0 , 0 , 0 , 0 , 0 , 2 , 6 } ,
                                                { 7 , 2 , 0 , 0 , 3 , 0 , 9 , 8 , 0 } ,
                                                { 0 , 5 , 0 , 0 , 2 , 4 , 1 , 0 , 7 } ,
                                                { 9 , 3 , 0 , 0 , 7 , 2 , 0 , 0 , 0 } ,
                                                { 4 , 0 , 0 , 5 , 0 , 3 , 0 , 0 , 9 } ,
                                                { 0 , 0 , 0 , 9 , 1 , 0 , 0 , 4 , 3 } ,
                                                { 3 , 0 , 4 , 2 , 5 , 0 , 0 , 6 , 0 } ,
                                                { 0 , 7 , 6 , 0 , 8 , 0 , 0 , 1 , 5 } ,
                                                { 1 , 8 , 0 , 0 , 0 , 0 , 0 , 0 , 0 }
            };
            int[,] Matrix2 =
                {
                                                { 0 , 0 , 0 , 0 , 4 , 0 , 0 , 0 , 0 } ,
                                                { 2 , 0 , 9 , 8 , 0 , 0 , 0 , 6 , 0 } ,
                                                { 3 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 4 } ,
                                                { 0 , 9 , 0 , 0 , 0 , 2 , 0 , 7 , 8 } ,
                                                { 0 , 0 , 0 , 7 , 0 , 1 , 0 , 0 , 0 } ,
                                                { 1 , 2 , 0 , 4 , 0 , 0 , 0 , 5 , 0 } ,
                                                { 8 , 0 , 0 , 0 , 0 , 0 , 0 , 9 , 5 } ,
                                                { 0 , 7 , 0 , 0 , 0 , 5 , 3 , 0 , 2 } ,
                                                { 0 , 0 , 0 , 0 , 6 , 0 , 0 , 0 , 0 }
                                            };
            int[,] Matrix3 = new int[9, 9];
            int[,] Matrix4 =
            {
                {8,0,0,0,0,0,0,0,0 },
                {0,0,3,6,0,0,0,0,0 },
                {0,7,0,0,9,0,2,0,0 },
                {0,5,0,0,0,7,0,0,0 },
                {0,0,0,0,4,5,7,0,0 },
                {0,0,0,1,0,0,0,3,0 },
                {0,0,1,0,0,0,0,6,8 },
                {0,0,8,5,0,0,0,1,0 },
                {0,9,0,0,0,0,4,0,0 }
            };
            if (n == 1)
            {
                return Matrix1;
            }
            else if (n == 2)
            {
                return Matrix2;
            }
            else if (n == 10)
            {
                return Matrix4;
            }
            else
            {
                return Matrix3;
            }
        }
    }
}