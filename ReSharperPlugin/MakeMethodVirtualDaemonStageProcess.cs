using System;
using System.Collections.Generic;

using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace Vilinski.ReSharperPlugIn
{
    public class MakeMethodVirtualDaemonStageProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess _process;


        public MakeMethodVirtualDaemonStageProcess(IDaemonProcess process)
        {
            _process = process;
        }

        #region IDaemonStageProcess Members

        public void Execute(Action<DaemonStageResult> commiter)
        {
            if (_process.InterruptFlag)
            {
                return;
            }


            var file = _process.SourceFile as ICSharpFile;

            if (file != null)
            {
                var highlights = new List<HighlightingInfo>();

                var processor = new RecursiveElementProcessor<IMethodDeclaration>(declaration =>
                                                                                      {
                                                                                          AccessRights accessRights =
                                                                                              declaration.
                                                                                                  GetAccessRights();

                                                                                          if (accessRights ==
                                                                                              AccessRights.PUBLIC &&
                                                                                              !declaration.IsStatic &&
                                                                                              !declaration.IsVirtual &&
                                                                                              !declaration.IsOverride)
                                                                                          {
                                                                                              DocumentRange docRange =
                                                                                                  declaration.
                                                                                                      GetNameDocumentRange
                                                                                                      ();

                                                                                              highlights.Add(
                                                                                                  new HighlightingInfo(
                                                                                                      docRange,
                                                                                                      new MakeMethodVirtualSuggestion
                                                                                                          (declaration)));
                                                                                          }
                                                                                      });

                file.ProcessDescendants(processor);

                commiter(new DaemonStageResult(highlights));
            }
        }

        public IDaemonProcess DaemonProcess
        {
            get { return _process; }
        }

        #endregion
    }
}