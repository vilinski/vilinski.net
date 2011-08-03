using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using ReactiveUI;

namespace Vilinski.RxRegex.ViewModel
{
	/// <summary>
	/// <see cref="Regex"/> ViewModel.
	/// </summary>
	public class RegexViewModel : ReactiveObject
	{
		protected RegexViewModel()
		{
			var optionChanges = this.WhenAny(
				_ => _.Input,
				_ => _.Pattern,
				_ => _.IgnoreCase,
				_ => _.Multiline,
				_ => _.ExplicitCapture,
				_ => _.Compiled,
				_ => _.Singleline,
				_ => _.IgnorePatternWhitespace,
				_ => _.RightToLeft,
				_ => _.ECMAScript,
				_ => _.CultureInvariant,
				(
					input,
					pattern,
					ignoreCase,
					multiline,
					explicitCapture,
					compiled,
					singleline,
					ignorePatternWhitespace,
					rightToLeft,
					ecmaScript,
					cultureInvariant
					) => Options //Regex.Match(input.Value, pattern.Value, Options)
				);

			// TODOo make _Options rxoprop, update match from throttled input and pattern, better with sanduhr
			//_SpinnerVisibility = new ObservableAsPropertyHelper<bool>(optionChanges, x => this.RaisePropertyChanged(_ => _.Pattern), Visibility.Collapsed);

			this.ObservableForProperty(_=>_.Pattern)
				.Throttle(TimeSpan.FromMilliseconds(800))
				.Select(x => x.Value).DistinctUntilChanged()
				.Where(x => !String.IsNullOrWhiteSpace(x));
			//.Subscribe(ExecuteSearch.Execute);
		}

		public Regex Model
		{
			get { return new Regex(Pattern, Options); }
		}

		protected RegexOptions Options
		{
			get
			{
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

		#region SpinnerVisibility

		[UsedImplicitly]
		private ObservableAsPropertyHelper<bool> _SpinnerVisibility;

		public bool SpinnerVisibility
		{
			get { return _SpinnerVisibility.Value; }
		}

		#endregion SpinnerVisibility


		#region Input

		[UsedImplicitly]
		private string _Input;

		public string Input
		{
			get { return _Input; }
			set { this.RaiseAndSetIfChanged(x => x.Input, value); }
		}

		#endregion Input

		#region Pattern

		[UsedImplicitly]
		private string _Pattern;

		public string Pattern
		{
			get { return _Pattern; }
			set { this.RaiseAndSetIfChanged(x => x.Pattern, value); }
		}

		#endregion Pattern

		#region IgnoreCase

		[UsedImplicitly]
		private bool _IgnoreCase;

		public bool IgnoreCase
		{
			get { return _IgnoreCase; }
			set { this.RaiseAndSetIfChanged(x => x.IgnoreCase, value); }
		}

		#endregion IgnoreCase

		#region Multiline

		[UsedImplicitly]
		private bool _Multiline;

		public bool Multiline
		{
			get { return _Multiline; }
			set { this.RaiseAndSetIfChanged(x => x.Multiline, value); }
		}

		#endregion Multiline

		#region ExplicitCapture

		[UsedImplicitly]
		private bool _ExplicitCapture;

		public bool ExplicitCapture
		{
			get { return _ExplicitCapture; }
			set { this.RaiseAndSetIfChanged(x => x.ExplicitCapture, value); }
		}

		#endregion ExplicitCapture

		#region Compiled

		[UsedImplicitly]
		private bool _Compiled;

		public bool Compiled
		{
			get { return _Compiled; }
			set { this.RaiseAndSetIfChanged(x => x.Compiled, value); }
		}

		#endregion Compiled

		#region Singleline

		[UsedImplicitly]
		private bool _Singleline;

		public bool Singleline
		{
			get { return _Singleline; }
			set { this.RaiseAndSetIfChanged(x => x.Singleline, value); }
		}

		#endregion Singleline

		#region IgnorePatternWhitespace

		[UsedImplicitly]
		private bool _IgnorePatternWhitespace;

		public bool IgnorePatternWhitespace
		{
			get { return _IgnorePatternWhitespace; }
			set { this.RaiseAndSetIfChanged(x => x.IgnorePatternWhitespace, value); }
		}

		#endregion IgnorePatternWhitespace

		#region RightToLeft

		[UsedImplicitly]
		private bool _RightToLeft;

		public bool RightToLeft
		{
			get { return _RightToLeft; }
			set { this.RaiseAndSetIfChanged(x => x.RightToLeft, value); }
		}

		#endregion RightToLeft

		#region ECMAScript

		[UsedImplicitly]
		private bool _ECMAScript;

		public bool ECMAScript
		{
			get { return _ECMAScript; }
			set { this.RaiseAndSetIfChanged(x => x.ECMAScript, value); }
		}

		#endregion ECMAScript

		#region CultureInvariant

		[UsedImplicitly]
		private bool _CultureInvariant;

		public bool CultureInvariant
		{
			get { return _CultureInvariant; }
			set { this.RaiseAndSetIfChanged(x => x.CultureInvariant, value); }
		}

		#endregion CultureInvariant
	}
}