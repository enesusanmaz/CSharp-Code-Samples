using NSort.Sorter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NSort.ConsoleUI
{
    class Program : SortManager
    {
        public static int[] array;
        static void Main(string[] args)
        {
            int val = 0;
            string quantity = "";
            do
            {
                Console.WriteLine("Lütfen sıralamak istediğiniz data adedini giriniz");
                quantity = Console.ReadLine().ToString();

            } while (!int.TryParse(quantity, out val));


            try
            {
                var sortManager = new SortManager();
                array = sortManager.GetData(val);

                sortManager.QuickSorterTest(array);

                sortManager.BubbleSorterTest(array);

                sortManager.FastQuickSorterTest(array);

                sortManager.HeapSorterTest(array);

                sortManager.InsertionSorterTest(array);

                sortManager.SelectionSorterTest(array);

                sortManager.ShellSorterTest(array);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Console.WriteLine("Tüm sıralamalar başarıyla tamamlandı\n");
            Console.ReadLine();

        }
    }

    public class SortManager
    {
        private ISorter sorter = null;
        public ISorter Sorter
        {
            get
            {
                return this.sorter;
            }
            set
            {
                this.sorter = value;
            }
        }

        public void QuickSorterTest(int[] array)
        {
            this.Sorter = new QuickSorter();
            StartSort(array);
        }


        public void BubbleSorterTest(int[] array)
        {
            this.Sorter = new BubbleSorter();
            StartSort(array);
        }

        public void HeapSorterTest(int[] array)
        {
            this.Sorter = new HeapSort();
            StartSort(array);
        }


        public void SelectionSorterTest(int[] array)
        {
            this.Sorter = new SelectionSort();
            StartSort(array);
        }


        public void ShellSorterTest(int[] array)
        {
            this.Sorter = new ShellSort();
            StartSort(array);
        }

        public void FastQuickSorterTest(int[] array)
        {
            this.Sorter = new FastQuickSorter();
            StartSort(array);
        }


        public void InsertionSorterTest(int[] array)
        {
            this.Sorter = new InsertionSort();
            StartSort(array);
        }

        public int[] GetData(int data)
        {
            var rndList = new List<int>();

            Random rnd = new Random();
            int[] list = new int[data];

            Console.WriteLine("Algoritmaların sıralayacağı " + list.Length.ToString() + " adet data yükleniyor.\n");

            for (int i = 0; i < list.Length; i++)
            {
                var val = rnd.Next();

                if (rndList.Contains(val))
                    continue;
                else
                {
                    list[i] = val;
                    rndList.Add(val);
                }
            }
            Console.WriteLine("Data yükleme tamamlandı.\n");
            return list;
        }

        private void StartSort(int[] list)
        {
            var watch = Stopwatch.StartNew();

            System.Type type = Sorter.GetType();

            Console.WriteLine(type.ToString() + " metodu sıralamaya başladı.Lütfen bekleyiniz.\n");

            // sort table
            Sorter.Sort(list);
            watch.Stop();
            Console.WriteLine(type.ToString() + " metodu sıralamayı tamamladı.\n");
            Console.WriteLine("Metod Adı : " + type.ToString() + " , Geçen süre => " + watch.ElapsedMilliseconds.ToString() + " milisaniye , " + watch.Elapsed.Seconds + " saniye , " + watch.Elapsed.Minutes + " dakika\n");

        }

    }
}
