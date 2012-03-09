using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Feature.Services.Navigation.GoToRelated;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace Vilinski.ReSharperPlugIn.estatePro
{
	/*
	[RelatedFilesProvider(typeof (KnownProjectFileType))]
	public class TypeRelatedFilesProvider : IRelatedFilesProvider
	{
		public IEnumerable<JetTuple<IProjectFile, string, IProjectFile>> GetRelatedFiles(IProjectFile projectFile)
		{
			var typesCollector = new RecursiveElementCollector<ITypeDeclaration>(null);
			projectFile.EnumeratePsiFiles().ForEach(file => typesCollector.ProcessElement(file));
			return typesCollector
				.GetResults()
				.SelectMany(declaration =>
				            declaration
				            	.SuperTypes
				            	.Select(type => JetTuple.Of(type.GetTypeElement(), "Base"))
				            	.Concat(JetTuple.Of(declaration.DeclaredElement, "Part")))
				.Where(tuple => tuple.A != null)
				.SelectMany(tuple =>
				            tuple.A
				            	.GetDeclarations()
				            	.Select(declaration =>
				            	        JetTuple.Of(declaration.GetSourceFile().ToProjectFile(), tuple.B, projectFile)))
				.Where(tuple => tuple.A != projectFile)
				.Distinct();
		}
	}

	[RelatedFilesProvider(typeof (KnownProjectFileType))]
	public class DependentRelatedFilesProvider : IRelatedFilesProvider
	{
		public IEnumerable<JetTuple<IProjectFile, string, IProjectFile>> GetRelatedFiles(IProjectFile projectFile)
		{
			return GetAllDependices(projectFile, new IProjectFile[0])
				.Select(file => JetTuple.Of(file, "Dependency", projectFile));
		}

		private static IEnumerable<IProjectFile> GetAllDependices(IProjectFile projectFile, params IProjectFile[] exclude)
		{
			List<IProjectFile> result = projectFile.GetDependentFiles().Except(exclude).ToList();
			foreach (IProjectFile projectFile1 in result)
				yield return projectFile1;
			IProjectFile[] excludeFilesNext = ArrayUtil.Add(exclude, projectFile);
			foreach (IProjectFile projectFile1 in result.SelectMany(file =>GetAllDependices(file, excludeFilesNext)))
				yield return projectFile1;
			IProjectFile parent = projectFile.GetDependsUponItemForItem();
			if (parent != null && !exclude.Contains(parent))
			{
				yield return parent;
				foreach (IProjectFile projectFile1 in GetAllDependices(parent, excludeFilesNext))
					yield return projectFile1;
			}
		}
	}

	[RelatedFilesProvider(typeof (KnownProjectFileType))]
	public class DefaultRelatedFilesProvider : IRelatedFilesProvider
	{
		public virtual IEnumerable<JetTuple<IProjectFile, string, IProjectFile>> GetRelatedFiles(IProjectFile projectFile)
		{
			return
				projectFile
					.EnumeratePsiFiles()
					.SelectMany(psiFile =>
					            (IEnumerable<IPathReference>) new ReferenceCollector<IPathReference>()
					                                          	.Process(psiFile).References,
						(psiFile, reference) =>
						new
							{
								psiFile = psiFile,
								reference = reference
							})
					.Select(x =>
					        new
					        	{
					        		id = x,
					        		resolve = x.reference.Resolve()
					        	})
					.Where(x => x.resolve.ResolveErrorType == ResolveErrorType.OK)
					.Select(x =>
					        new
					        	{
					        		id = x,
					        		declaredElement = x.resolve.DeclaredElement
					        	})
					.Where(param0 => param0.declaredElement != null)
					.SelectMany(x => (IEnumerable<IPsiSourceFile>) x.declaredElement.GetSourceFiles(),
						(x, sourceFile) =>
						new
							{
								id = x,
								sourceFile = sourceFile
							})
					.Select(x =>
					        new
					        	{
					        		id = x,
					        		file = x.sourceFile.ToProjectFile()
					        	})
					.Where(x => x.file != null)
					.Select(x =>
					        JetTuple.Of(x.file,
					        	GetRelationPresentation(
					        		x.id.id.id.id.reference), projectFile));
		}

		private static string GetRelationPresentation([NotNull] IPathReference reference)
		{
			return
				reference.GetType()
					.GetCustomAttributes(typeof (DescriptionAttribute), true)
					.Cast<DescriptionAttribute>()
					.Select(attribute => attribute.Description)
					.FirstOrDefault();
		}
	}
	*/
	[RelatedFilesProvider(typeof(KnownProjectFileType))]
    class BusinessObjectRelatedFilesProvider : IRelatedFilesProvider 
    {
        private ICSharpContextActionDataProvider _provider;

        public BusinessObjectRelatedFilesProvider(ICSharpContextActionDataProvider provider)
        {
            _provider = provider;
        }
        public IEnumerable<JetTuple<IProjectFile, string, IProjectFile>> GetRelatedFiles(IProjectFile projectFile)
        {
			return new List<JetTuple<IProjectFile, string, IProjectFile>>();
  //          IDeclarationsCache dc = CacheManager.GetInstance(_provider.Solution).GetDeclarationsCache(
  //_provider.PsiModule, true, true);
  //          var inheritors = dc.GetPossibleInheritors("BuisnessObjectBuilder").Select(type => type.GetAttributeInstances());
  //          var shortNames = dc.GetAllShortNames();
  //          foreach (var file in projectFile.EnumeratePsiFiles().OfType<ICSharpFile>())
  //          {
  //              var classDeclaration =file.Get
  //              var attribute = file.GetContainingNode<IAttribute>();
  //              if (attribute != null)
  //              {
  //                  var literal = attribute.Arguments.OfType<ICSharpLiteralExpression>();
  //                  //TODO get value and compare with class's name
  //              }
  //          }
  //          // original implementation
  //          return
  //              from file in projectFile.EnumeratePsiFiles()
  //              from reference in new ReferenceCollector<IPathReference>().Process(file).References 
  //              where reference.CheckResolveResult() == ResolveErrorType.OK 
  //              let declaredElement = reference.Resolve().DeclaredElement 
  //              where declaredElement != null 
  //              from a in declaredElement.GetSourceFiles().SelectNotNull(f => f.ToProjectFile()) 
  //              select JetTuple.Of(a, GetRelationPresentation(reference));
        }

        private static string GetRelationPresentation([NotNull] IPathReference reference)
        {
            return reference
                .GetType()
                .GetCustomAttributes(typeof(DescriptionAttribute), true)
                .Cast<DescriptionAttribute>()
                .Select(attribute => attribute.Description)
                .FirstOrDefault();
        }
    }
}
