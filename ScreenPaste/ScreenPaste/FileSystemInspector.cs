using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace ScreenPaste
{
	[InheritedExport]
	public interface IFileSystemInspector
	{
		Guid ID { get; }
		string Name { get; }
		Version Version { get; }
		DateTime Date { get; }
		FileSystemWatcher FileSystemWatcher { get; set; }

		void Start();
		void Stop();
	}

	public class DefaultFileSystemInspector : IFileSystemInspector
	{
		public Guid ID
		{
			get { return new Guid("C6BB10A2-70D6-47A3-8AF5-26494CA7E375"); }
		}

		public string Name
		{
			get { return "Default Inspector"; }
		}

		public Version Version
		{
			get { return Assembly.GetExecutingAssembly().GetName().Version; }
		}

		public DateTime Date
		{
			get { return new DateTime(2011, 06, 13); }
		}

		public FileSystemWatcher FileSystemWatcher { get; set; }

		public DefaultFileSystemInspector()
		{
			FileSystemWatcher FileSystemWatcher = new FileSystemWatcher();
			FileSystemWatcher.EnableRaisingEvents = false;
			FileSystemWatcher.Path = @"C:\";
			FileSystemWatcher.Filter = "*.*";
			FileSystemWatcher.IncludeSubdirectories = true;
			FileSystemWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.LastAccess |
			                                 NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size |
			                                 NotifyFilters.FileName | NotifyFilters.DirectoryName;
		}

		public void Start()
		{
			FileSystemWatcher.EnableRaisingEvents = true;
		}

		public void Stop()
		{
			FileSystemWatcher.EnableRaisingEvents = false;
		}
	}

	public class FileSystemInspectorComposer
	{
		#region Fields

		[ImportMany(typeof (IFileSystemInspector), AllowRecomposition = true)]
		public List<IFileSystemInspector> FSInspectorList { get; set; }

		private DirectoryCatalog _catalog;
		private readonly FileSystemWatcher _fileSystemWatcher;
		private readonly string _plugInsPath = string.Empty;

		#endregion

		#region Events

		public event Action FSInspectorListChanged;
		public event FileSystemEventHandler FileChanged;
		public event FileSystemEventHandler FileCreated;
		public event FileSystemEventHandler FileDeleted;
		public event RenamedEventHandler FileRenamed;

		#endregion

		#region Constuctor

		public FileSystemInspectorComposer()
		{
			// Create plugins path
			_plugInsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PlugIns");

			// Filesystem watcher
			_fileSystemWatcher = new FileSystemWatcher();
			_fileSystemWatcher.Path = _plugInsPath;
			_fileSystemWatcher.Filter = "*.dll";
			_fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime;
			_fileSystemWatcher.Changed += InternalFileChanged;
		}

		public void Initialize()
		{
			// Load plugins
			_catalog = new DirectoryCatalog(_plugInsPath);
			var compositionContainer = new CompositionContainer(_catalog);
			compositionContainer.ComposeParts(this);
			AddEventHandler();

			_fileSystemWatcher.EnableRaisingEvents = true;
		}

		#endregion

		#region Monitor changes

		private void RefreshCatalog()
		{
			try
			{
				_catalog.Refresh();

				AddEventHandler();

				if (FSInspectorListChanged != null)
					FSInspectorListChanged();
			}
			catch
			{
			}
		}

		private void InternalFileChanged(object sender, FileSystemEventArgs e)
		{
			// Refresh should be called delayed to ensure the file is completly copied
			RefreshCatalog();
		}

		private void AddEventHandler()
		{
			foreach (IFileSystemInspector inspector in FSInspectorList)
			{
				inspector.FileSystemWatcher.Changed += FileChanged;
				inspector.FileSystemWatcher.Created += FileCreated;
				inspector.FileSystemWatcher.Deleted += FileDeleted;
				inspector.FileSystemWatcher.Renamed += FileRenamed;
			}
		}

		#endregion
	}
}