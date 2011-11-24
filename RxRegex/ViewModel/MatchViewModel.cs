using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using ReactiveUI;

namespace Vilinski.RxRegex.ViewModel
{
    public class MatchViewModel : ReactiveObject
    {
        protected MatchViewModel(Match model)
        {
            Model = model;
            var groups = model.Groups;
            var capture = model.Captures;// Capture has Index, Length, Value
            var index = model.Index;
            var length = model.Length;
            var next = model.NextMatch();
            var result = model.Result(stringg);
            var x = model.Success;
            var y = model.Value;
            groups[0].

        }

        #region Model

        [UsedImplicitly]
        private Match _Model;

        public Match Model
        {
            get { return _Model; }
            set { this.RaiseAndSetIfChanged(_ => _.Model, value); }
        }

        #endregion Model

        #region Captures

        [UsedImplicitly]
        private ObservableAsPropertyHelper<List<Capture>> _Captures;

        /// <summary>
        /// Gets a collection of all the captures matched by the capturing group, 
        /// in innermost-leftmost-first order (or innermost-rightmost-first order 
        /// if the regular expression is modified with the RightToLeft option). 
        /// The collection may have zero or more items.
        /// </summary>
        public List<Capture> Captures
        {
            get { return _Captures.Value; }
        }

        #endregion Captures

        #region Groups

        [UsedImplicitly]
        private ObservableAsPropertyHelper<List<Group>> _Groups;

        /// <summary>
        /// Gets a collection of character groups matched by the regular expression.
        /// </summary>
        public List<Group> Groups
        {
            get { return _Groups.Value; }
        }

        #endregion Groups

        #region Index

        [UsedImplicitly]
        private int _Index;

        /// <summary>
        /// The position in the original string where the first character of the captured substring was found.
        /// </summary>
        /// <value>
        /// The zero-based starting position in the original string where the captured substring was found.
        /// </value>
        public int Index
        {
            get { return _Index; }
            set { this.RaiseAndSetIfChanged(_ => _.Index, value); }
        }

        #endregion Index

        #region Length

        [UsedImplicitly]
        private int _Length;

        /// <summary>
        /// Gets or sets the length of the captured substring.
        /// </summary>
        /// <value>
        /// The length of the captured substring.
        /// </value>
        public int Length
        {
            get { return _Length; }
            set { this.RaiseAndSetIfChanged(_ => _.Length, value); }
        }

        #endregion Length

        #region Success

        [UsedImplicitly]
        private bool _Success;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Match"/> is successful.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the match is successful; otherwise, <c>false</c>.
        /// </value>
        public bool Success
        {
            get { return _Success; }
            set { this.RaiseAndSetIfChanged(_ => _.Success, value); }
        }

        #endregion Success

        #region Value

        [UsedImplicitly]
        private string _Value;

        /// <summary>
        /// Gets the captured substring from the input string.
        /// </summary>
        /// <value>
        /// The actual substring that was captured by the match.
        /// </value>
        public string Value
        {
            get { return _Value; }
            set { this.RaiseAndSetIfChanged(_ => _.Value, value); }
        }

        #endregion Value
    }
}
