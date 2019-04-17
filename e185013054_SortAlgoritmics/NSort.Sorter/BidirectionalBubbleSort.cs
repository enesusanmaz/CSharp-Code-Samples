using System;
using System.Collections;

namespace NSort.Sorter
{
	public class BiDirectionalBubbleSort : SwapSorter
	{
		public BiDirectionalBubbleSort() : base() {}

		public BiDirectionalBubbleSort(IComparer comparer, ISwap swapper)
			:base(comparer,swapper)
		{
		}

		public override void Sort(IList list) 
		{
			int j;
			int limit;
			int st;
			bool flipped;

			st = -1;
			limit = list.Count;
			flipped=true;
			while (st < limit & flipped) 
			{
				flipped = false;
				st++;
				limit--;
				for (j=st;j<limit;j++) 
				{
					if (Comparer.Compare(list[j],list[j+1])>0) 
					{
						Swapper.Swap(list, j, j+1);
						flipped = true;
					}
				}

				if (flipped) 
				{
					for (j=limit-1;j>=st;j--) 
					{
						if (Comparer.Compare(list[j],list[j+1])>0) 
						{
							Swapper.Swap(list, j, j+1);
							flipped = true;
						}
					}
				}
			}
		}
	}
}
