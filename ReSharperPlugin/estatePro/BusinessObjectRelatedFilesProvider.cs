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
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace Vilinski.ReSharperPlugIn.estatePro
{
    [RelatedFilesProvider]
    class BusinessObjectRelatedFilesProvider : IRelatedFilesProvider 
    {
        private ICSharpContextActionDataProvider _provider;

        public BusinessObjectRelatedFilesProvider(ICSharpContextActionDataProvider provider)
        {
            _provider = provider;
        }
        public IEnumerable<JetTuple<IProjectFile, string>> GetRelatedFiles(IProjectFile projectFile)
        {
            return new List<JetTuple<IProjectFile, string>>();
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
