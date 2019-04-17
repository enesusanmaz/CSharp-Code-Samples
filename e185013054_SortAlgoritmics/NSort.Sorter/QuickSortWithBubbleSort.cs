using System;
using System.Collections;

namespace NSort.Sorter
{
	public class QuickSortWithBubbleSort : SwapSorter
	{
		public QuickSortWithBubbleSort() : base() {}
		
		public QuickSortWithBubbleSort(IComparer comparer, ISwap swapper)
			:base(comparer,swapper)
		{
		}

		public override void Sort(IList list) 
		{
			Sort1(list, 0, list.Count-1);
		}

		private void Sort2(IList list, int low, int high) 
		{
			int j;
			int i;
    
			for (j=high;j>low;j--) 
			{
				for (i=low;i<j;i++) 
				{
					if (Comparer.Compare(list[i], list[i+1])>0)
					{
						Swapper.Swap(list, i, i+1);
					}
				}
			}
		}

		private void Sort1(IList list, int fromPos, int toPos) 
		{
			int low;
			int high;
			object pivot;
			
			low = fromPos;
			high = toPos;
			if (high-low <= 16) 
			{
				Sort2(list, low, high);
			} 
			else 
			{
				pivot = list[(low + high) / 2];
				list[(low + high) / 2] = list[high];
				list[high] = pivot;
				while (low < high) 
				{
					while (Comparer.Compare(list[low], pivot)<=0 & low < high) 
					{
						low++;
					}
					while (Comparer.Compare(pivot, list[high])<=0 & low < high) 
					{
						high--;
					}
					if (low < high) 
					{
						Swapper.Swap(list, low, high);
					}
				}
				Swapper.Set(list, toPos, high);
				Swapper.Set(list, high, pivot);
				Sort1(list, fromPos, low - 1);
				Sort1(list, high + 1, toPos);
			}
		}
	}
}
