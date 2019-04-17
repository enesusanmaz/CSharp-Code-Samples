using System;
using System.Collections;

namespace NSort.Sorter
{
	public class ShellSort : SwapSorter
	{
		public ShellSort() : base() {}

		public ShellSort(IComparer comparer, ISwap swapper)
			: base(comparer,swapper)
		{}

		public override void Sort(IList list) 
		{
			int h;
			int i;
			int j;
			object b;
			bool loop=true;

			h = 1;

			while (h * 3 + 1 <= list.Count) 
			{
				h = 3 * h + 1;
			}
			while (h > 0) 
			{
				for (i=h-1;i<list.Count;i++) 
				{
					b = list[i];
					j = i;
					loop=true;
					while (loop) 
					{
						if (j>=h) 
						{
							if (Comparer.Compare(list[j - h], b)>0) 
							{
								Swapper.Set(list, j, j-h);
								j = j - h;
							} 
							else 
							{
								loop=false;
							}
						} 
						else 
						{
							loop=false;
						}
					}
					Swapper.Set(list, j, b);
				}
				h = h / 3;
			}
		}
	}
}
