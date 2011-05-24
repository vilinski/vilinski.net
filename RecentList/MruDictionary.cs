using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecentList
{
	public class MruDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		/// <summary>
		/// The dictionary of keys and nodes
		/// </summary>
		private readonly Dictionary<TKey, LinkedListNode<MruItem>> itemIndex;

		/// <summary>
		/// The linked list of items in MRU order
		/// </summary>
		private readonly LinkedList<MruItem> items;

		private int _MaxCapacity = 128;

		public MruDictionary(int maxCapacity)
		{
			MaxCapacity = maxCapacity;
			items = new LinkedList<MruItem>();
			itemIndex = new Dictionary<TKey, LinkedListNode<MruItem>>(MaxCapacity);
		}

		public MruDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items)
			: this()
		{
			foreach (var pair in items.Reverse())
				this[pair.Key] = pair.Value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MruDictionary{TKey, TValue}"/> class.
		/// </summary>
		public MruDictionary()
		{
			items = new LinkedList<MruItem>();
			itemIndex = new Dictionary<TKey, LinkedListNode<MruItem>>(MaxCapacity);
		}

		public int MaxCapacity
		{
			get { return _MaxCapacity; }
			set
			{
				_MaxCapacity = value;
				shrinkToMaxCapacity();
			}
		}

		public int Count
		{
			get { return itemIndex.Count; }
		}

		public TValue this[TKey key]
		{
			get
			{
				TValue value;
				TryGetValue(key, out value);
				return value;
			}
			set
			{
				LinkedListNode<MruItem> node;
				if (itemIndex.TryGetValue(key, out node))
					node.Value.Value = value;
				else
					add(key, value);
			}
		}

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return itemIndex
				.Select(pair => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value.Value.Value))
				.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		private void shrinkToMaxCapacity()
		{
			if (itemIndex != null)
				while (itemIndex.Count > MaxCapacity)
				{
					LinkedListNode<MruItem> node = items.Last;
					items.RemoveLast();
					itemIndex.Remove(node.Value.Key);
				}
		}

		/// <summary>
		/// Adds the specified key and value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="ArgumentException">An item with the same key already exists.</exception>
		public void Add(TKey key, TValue value)
		{
			// Check to see if the key is already in the dictionary
			if (itemIndex.ContainsKey(key))
				throw new ArgumentException("An element with the same key already exists in the MruDictionary<TKey,TValue>.");

			// If the list is at capacity, remove the LRU item.
			add(key, value);
		}

		public bool ContainsKey(TKey key)
		{
			return itemIndex.ContainsKey(key);
		}

		private void add(TKey key, TValue value)
		{
			if (itemIndex.Count == MaxCapacity)
			{
				LinkedListNode<MruItem> node = items.Last;
				items.RemoveLast();
				itemIndex.Remove(node.Value.Key);
			}

			// Create a node for this key/value pair
			var newNode = new LinkedListNode<MruItem>(new MruItem(key, value));
			// Add to the items list
			items.AddFirst(newNode);
			// and to the dictionary
			itemIndex.Add(key, newNode);
		}

		public void Touch(TKey key)
		{
			TValue value;
			TryGetValue(key, out value);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			LinkedListNode<MruItem> node;
			if (itemIndex.TryGetValue(key, out node))
			{
				value = node.Value.Value;
				// move this node to the front of the list
				items.Remove(node);
				items.AddFirst(node);
				return true;
			}
			value = default(TValue);
			return false;
		}

		#region Nested type: MruItem

		private sealed class MruItem
		{
			public MruItem(TKey k, TValue v)
			{
				Key = k;
				Value = v;
			}

			public TKey Key { get; private set; }

			public TValue Value { get; internal set; }
		}

		#endregion
	}
}