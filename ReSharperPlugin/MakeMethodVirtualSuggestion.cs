using System;

using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Vilinski.ReSharperPlugIn
{
    [StaticSeverityHighlighting(Severity.SUGGESTION, "C#")]
    public class MakeMethodVirtualSuggestion : CSharpHighlightingBase, IHighlighting
    {
        public MakeMethodVirtualSuggestion(ICSharpTypeMemberDeclaration memberDeclaration)
        {
            Declaration = memberDeclaration;
        }

        public ICSharpTypeMemberDeclaration Declaration { get; private set; }

        #region IHighlighting Members

        public string ToolTip
        {
            get { return "Method could be marked as virtual"; }
        }

        public string ErrorStripeToolTip
        {
            get { return ToolTip; }
        }

        public override bool IsValid()
        {
            return Declaration.IsValid();
        }

        public int NavigationOffsetPatch
        {
            get { return 0; }
        }

        #endregion
    }
}