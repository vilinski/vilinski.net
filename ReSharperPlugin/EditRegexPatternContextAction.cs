using System;

using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.CodeStyle;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace Vilinski.ReSharperPlugIn
{
    [ContextAction(Group = "C#", Name = "Edit Regex pattern",
        Description = "Edits a Regex pattern and options in a dialog")]
    public class EditRegexPatternContextAction : BulbItemImpl, IContextAction
    {
        private readonly ICSharpContextActionDataProvider _provider;
        private IExpression myStringLiteral;
        private IExpression myEditedStringLiteral;

        public EditRegexPatternContextAction(ICSharpContextActionDataProvider provider)
        {
            _provider = provider;
        }

        public override string Text
        {
            get { return "Edit Regex pattern"; }
        }

        #region IContextAction Members

        public bool IsAvailable(IUserDataHolder cache)
        {
            using (ReadLockCookie.Create())
            {
                myStringLiteral = GetSelectedString();
                if (myStringLiteral != null)
                    return !IsConstantExpression(myStringLiteral);// && IsEmptyString(myStringLiteral);
                return false;
            }
            var item = _provider.GetSelectedElement<IMethodDeclaration>(false, true);

            if (item != null)
            {
                AccessRights accessRights = item.GetAccessRights();

                if (accessRights == AccessRights.PUBLIC && !item.IsStatic && !item.IsVirtual && !item.IsOverride)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsEmptyString([CanBeNull] IExpression literal)
        {
            return string.Empty == GetValue(literal);
        }

        private static string GetValue([CanBeNull] IExpression literal)
        {
            if (literal != null && literal.IsValid())
            {
                ConstantValue constantValue = literal.ConstantValue;
                if (constantValue.IsString())
                    return Convert.ToString(constantValue.Value);
            }
            return null;
        }

        private IExpression GetSelectedString()
        {
            return _provider.GetSelectedElement<IExpression>(true, true);
        }

        private bool IsConstantExpression(IExpression expression)
        {
            if (expression == null)
                return false;
            if (expression.GetContainingNode<IAttribute>(true) == null && expression.GetContainingNode<ISwitchLabelStatement>(true) == null && (expression.GetContainingNode<IGotoCaseStatement>(true) == null && expression.GetContainingNode<ILocalConstantDeclaration>(true) == null))
                return expression.GetContainingNode<IConstantDeclaration>(true) != null;
            return true;
        }

        protected void OptimizeRefs(IRangeMarker marker, IFile file)
        {
            file.OptimizeImportsAndRefs(marker, false, true, NullProgressIndicator.Instance);
        }

        protected IExpression CreateExpression()
        {
            return _provider.ElementFactory.CreateExpression("string.Empty", new object[0]);
        }

        #endregion

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var method = _provider.GetSelectedElement<IMethodDeclaration>(false, true);

            if (method != null)
            {
                method.SetVirtual(true);
            }

            return null;
        }
    }
}