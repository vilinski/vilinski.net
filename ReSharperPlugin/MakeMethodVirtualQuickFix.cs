using System;

using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions;
using JetBrains.TextControl;
using JetBrains.Util;

namespace Vilinski.ReSharperPlugIn
{
    [QuickFix]
    public class MakeMethodVirtualQuickFix : BulbItemImpl, IQuickFix
    {
        private readonly MakeMethodVirtualSuggestion _highlighter;

        // Takes as parameter the Highlighter the quickfix refers to
        public MakeMethodVirtualQuickFix(MakeMethodVirtualSuggestion highlighter)
        {
            _highlighter = highlighter;
        }

        // In the transaction we make the necessary changes to the code

        // Text that appears in the context menu
        public override string Text
        {
            get { return "Make Method Virtual"; }
        }

        // Indicates when the option is available 

        #region IQuickFix Members

        public bool IsAvailable(IUserDataHolder cache)
        {
            return _highlighter.IsValid();
        }

        #endregion

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            _highlighter.Declaration.SetVirtual(true);

            return null;
        }
    }
}