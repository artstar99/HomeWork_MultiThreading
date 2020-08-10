using System;
using System.Threading;

namespace Task_01
{
    internal class FactorialCalculator
    {
        private int n;
        private int result;
        private readonly object obj = new object();
        private bool flag;

        public int Calculate(int userNumber)
        {
            n = userNumber;
            result = 1;

            for (int i = 1; i < userNumber; i++)
            {
                Thread calc = new Thread(ThreadMethod);
                calc.Start();
            }

            if (!flag) { Thread.Sleep(50); }
            return result;
        }

        private void ThreadMethod()
        {
            lock (obj)
            {
                Console.Write($"Thread HashCode:{Thread.CurrentThread.GetHashCode()}\t n={n}\tresult=");
                result *= n--;
                Console.WriteLine($"{result}");
                if (n == 0) { flag = true; }
            }
        }
    }
    internal class IntSumm
    {
        private int z;
        private int result;
        private object lockObject = new object();
        private bool flag;

        public int Calculate(int userNumber)
        {
            z = Math.Abs(userNumber);
            result = 0;

            for (int i = 0; i < Math.Abs(userNumber); i++)
            {
                Thread calc = new Thread(ThreadMethod);
                calc.Start();
            }

            if (!flag)
            { Thread.Sleep(50); }

            if (userNumber > 0)
            {
                return result;
            }
            return -result;
        }

        private void ThreadMethod()
        {
            lock (lockObject)
            {
                Console.Write($"Thread HashCode:{Thread.CurrentThread.GetHashCode()}\t z={z}\tresult=");
                result += z--;
                Console.WriteLine($"{result}");
                if (z == 0)
                {
                    flag = true;
                }
            }
        }

    }

    class Program
    {
        static void Main()
        {
            int n = 7;
            FactorialCalculator f = new FactorialCalculator();
            Console.WriteLine($"\n{n}!={f.Calculate(n)}");
            Console.WriteLine(new string('-', 10));
            Console.WriteLine();
            Console.ReadKey();

            IntSumm s = new IntSumm();
            int result = s.Calculate(n);
            Console.WriteLine($"\nСумма целых чисел до {n}={result}");
            //Console.WriteLine($"\nСумма целых чисел до {n}={s.Calculate(n)}");
            Console.WriteLine(new string('-', 10));
            Console.ReadKey();

        }
    }
}
