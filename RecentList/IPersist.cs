using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RecentList
{
	public interface IPersist<T>
	{
		T Get();
		void Set(T item);
	}

	public class PersistXmlFile<T> : IPersist<T>
	{
		private XmlSerializer _serializer = new XmlSerializer(typeof(T));

		public string Path { get; set; }

		public T Get()
		{
			using (var reader = new XmlTextReader(Path))
				return (T)_serializer.Deserialize(reader);
		}

		public void Set(T item)
		{
			using (var writer = new XmlTextWriter(Path, Encoding.UTF8))
				_serializer.Serialize(writer, item);
		}
	}
}