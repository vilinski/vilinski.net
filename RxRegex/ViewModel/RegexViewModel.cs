using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using ReactiveUI;

namespace Vilinski.RxRegex.ViewModel
{
	/// <summary>
	/// <see cref="Regex"/> ViewModel.
	/// </summary>
	public class RegexViewModel : ReactiveObject
	{
		public Regex Model { 
			get
			{
				return new Regex(Pattern, Options);
			}
		}

		#region Pattern

		protected RegexOptions Options
		{
			get { 
				RegexOptions options = RegexOptions.None;

				if (IgnoreCase)
					options |= RegexOptions.IgnoreCase;
				if (Multiline)
					options |= RegexOptions.Multiline;
				if (ExplicitCapture)
					options |= RegexOptions.ExplicitCapture;
				if (Compiled)
					options |= RegexOptions.Compiled;
				if (Singleline)
					options |= RegexOptions.Singleline;
				if (IgnorePatternWhitespace)
					options |= RegexOptions.IgnorePatternWhitespace;
				if (RightToLeft)
					options |= RegexOptions.RightToLeft;
				if (ECMAScript)
					options |= RegexOptions.ECMAScript;
				if (CultureInvariant)
					options |= RegexOptions.CultureInvariant;
				return options;
			}
		}

		private string _Pattern;

		public string Pattern
		{
			get { return _Pattern; }
			set { this.RaiseAndSetIfChanged(x => x.Pattern, value); }
		}

		#endregion Pattern

		#region IgnoreCase

		private bool _IgnoreCase;

		public bool IgnoreCase
		{
			get { return _IgnoreCase; }
			set { this.RaiseAndSetIfChanged(x => x.IgnoreCase, value); }
		}

		#endregion IgnoreCase

		#region Multiline

		private bool _Multiline;

		public bool Multiline
		{
			get { return _Multiline; }
			set { this.RaiseAndSetIfChanged(x => x.Multiline, value); }
		}

		#endregion Multiline

		#region ExplicitCapture

		private bool _ExplicitCapture;

		public bool ExplicitCapture
		{
			get { return _ExplicitCapture; }
			set { this.RaiseAndSetIfChanged(x => x.ExplicitCapture, value); }
		}

		#endregion ExplicitCapture

		#region Compiled

		private bool _Compiled;

		public bool Compiled
		{
			get { return _Compiled; }
			set { this.RaiseAndSetIfChanged(x => x.Compiled, value); }
		}

		#endregion Compiled

		#region Singleline

		private bool _Singleline;

		public bool Singleline
		{
			get { return _Singleline; }
			set { this.RaiseAndSetIfChanged(x => x.Singleline, value); }
		}

		#endregion Singleline

		#region IgnorePatternWhitespace

		private bool _IgnorePatternWhitespace;

		public bool IgnorePatternWhitespace
		{
			get { return _IgnorePatternWhitespace; }
			set { this.RaiseAndSetIfChanged(x => x.IgnorePatternWhitespace, value); }
		}

		#endregion IgnorePatternWhitespace

		#region RightToLeft

		private bool _RightToLeft;

		public bool RightToLeft
		{
			get { return _RightToLeft; }
			set { this.RaiseAndSetIfChanged(x => x.RightToLeft, value); }
		}

		#endregion RightToLeft

		#region ECMAScript

		private bool _ECMAScript;

		public bool ECMAScript
		{
			get { return _ECMAScript; }
			set { this.RaiseAndSetIfChanged(x => x.ECMAScript, value); }
		}

		#endregion ECMAScript

		#region CultureInvariant

		private bool _CultureInvariant;

		public bool CultureInvariant
		{
			get { return _CultureInvariant; }
			set { this.RaiseAndSetIfChanged(x => x.CultureInvariant, value); }
		}

		#endregion CultureInvariant
	}
}
