using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecentList
{
	public interface IRecentList<T> : ICollection<T>
	{
		IPersist<IList<T>> Persister { get; set; }
		int MaxEntriesNumber { get; set; }
		void Get();
		void Set();
	}

	public class RecentList<T> : List<T>, IRecentList<T>
	{
		public RecentList()
		{
			MaxEntriesNumber = 64;
		}

		public IPersist<IList<T>> Persister { get; set; }

		public int MaxEntriesNumber { get; set; }

		public void Get()
		{
			var list = Persister.Get();
			Clear();
			AddRange(list.Take(MaxEntriesNumber));
		}

		public void Set()
		{
			Persister.Set(this);
		}
	}
}
