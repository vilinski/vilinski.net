using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ScreenPaste.SendTo
{
	//http://stefanhenneken.wordpress.com/2011/06/05/mef-teil-2-metadaten-und-erstellungsrichtlinien/

	public class SendToFactory
	{
		private static readonly Lazy<SendToFactory> s_lazy = new Lazy<SendToFactory>(() => new SendToFactory());

		public static SendToFactory Instance
		{
			get { return s_lazy.Value; }
		}

		private static FileSystemWatcher _fileSystemWatcher;

		private SendToFactory()
		{
			var path = AppDomain.CurrentDomain.BaseDirectory;
			string pluginsPath = Path.Combine(path, "Plugins");
			if (!Directory.Exists(pluginsPath))
				Directory.CreateDirectory(pluginsPath);

			const string pathFilter = "*.dll";

			var catalog = new AggregateCatalog();
			var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
			catalog.Catalogs.Add(assemblyCatalog);
			var pathCatalog = new DirectoryCatalog(path, pathFilter);
			catalog.Catalogs.Add(pathCatalog);
			var pluginsPathCatalog = new DirectoryCatalog(pluginsPath, pathFilter);
			catalog.Catalogs.Add(pluginsPathCatalog);

			_container = new CompositionContainer(catalog);
			try
			{
				_container.ComposeParts(this);
			}
			catch (CompositionException compositionException)
			{
				Console.WriteLine(compositionException.ToString());
			}

			_fileSystemWatcher = new FileSystemWatcher(path);
			_fileSystemWatcher.Filter = pathFilter;
			_fileSystemWatcher.Changed += (s, e) =>
			                              	{
			                              		pathCatalog.Refresh();
			                              		pluginsPathCatalog.Refresh();
			                              		Console.Out.WriteLine("e = {0}", e);
			                              	};
			_fileSystemWatcher.EnableRaisingEvents = true;
		}

		[ImportMany(typeof (ISendTo), AllowRecomposition = true, RequiredCreationPolicy = CreationPolicy.Shared)]
		public IEnumerable<Lazy<ISendTo, ISendToCapabilities>> Senders { get; set; }

		public IEnumerable<Lazy<ISendTo, ISendToCapabilities>> EnabledSenders
		{
			get { return Senders.Where(sender => sender.Metadata.Enabled); }
		}

		public string SendTo(Bitmap image, Guid senderId)
		{
			var sender = EnabledSenders.FirstOrDefault(s => s.Metadata.ID == senderId);
			if (sender != null)
				return sender.Value.Send(image);
			return string.Empty;
		}

		private readonly CompositionContainer _container;
	}

}