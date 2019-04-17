using NSort.Sorter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace NSort.Test
{
    public class SortTest
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

        [Fact]
        public void QuickSorterTest()
        {
            this.Sorter = new QuickSorter();
            StartSort();
        }

        [Fact]
        public void BubbleSorterTest()
        {
            this.Sorter = new BubbleSorter();
            StartSort();
        }

        [Fact]
        public void HeapSorterTest()
        {
            this.Sorter = new HeapSort();
            StartSort();
        }

        [Fact]
        public void SelectionSorterTest()
        {
            this.Sorter = new SelectionSort();
            StartSort();
        }

        [Fact]
        public void ShellSorterTest()
        {
            this.Sorter = new ShellSort();
            StartSort();
        }

        [Fact]
        public void FastQuickSorterTest()
        {
            this.Sorter = new FastQuickSorter();
            StartSort();
        }

        [Fact]
        public void InsertionSorterTest()
        {
            this.Sorter = new InsertionSort();
            StartSort();
        }



        private void StartSort()
        {
            var rndList = new List<int>();

            Random rnd = new Random();
            int[] list = new int[1000000];

            for (int i = 0; i < list.Length; i++)
            {
                var val = rnd.Next();

                if (rndList.Contains(val))
                    return;

                else
                {
                    list[i] = val;
                    rndList.Add(val);
                }
            }


            // create sorted list
            SortedList sl = new SortedList();
            foreach (int key in list)
                sl.Add(key, null);

            var watch = Stopwatch.StartNew();

            // sort table
            Sorter.Sort(list);
            watch.Stop();

            Assert.True(watch.ElapsedMilliseconds > 0);
        }
    }
}
