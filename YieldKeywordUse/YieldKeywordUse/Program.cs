using System;
using System.Collections.Generic;

namespace YieldKeywordUse
{
    class Program
    {
        static void Main(string[] args)
        {
            //foreach (string day in GetDays())
            //{
            //    Console.WriteLine(day);
            //}
            FillArray();
            foreach (int number in GetTwoEvenNumbers())
            {
                Console.WriteLine(number);
            }

            Console.ReadLine();
        }


        static IEnumerable<string> GetDays()
        {
            yield return "Pazartesi";
            yield return "Salı";
            yield return "Çarşamba";
            yield return "Perşembe";
            yield return "Cuma";
            yield return "Cumartesi";
            yield return "Pazar";
        }

        static int[] numbers = null;
        private static void FillArray()
        {
            Random rnd = new Random();
            numbers = new int[10];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = rnd.Next(1, 30);
            }
        }

        //Eğer çift sayıların sayısı 2 ise döngüyü sonlandır.
        static IEnumerable<int> GetTwoEvenNumbers()
        {
            int evenNumber = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (evenNumber == 2)
                    yield break;
                if (numbers[i] % 2 == 0)
                {
                    evenNumber++;
                    yield return numbers[i];
                }
            }
        }
    }
}
