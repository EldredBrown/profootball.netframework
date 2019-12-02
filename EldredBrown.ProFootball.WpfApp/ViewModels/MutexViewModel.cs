using System;
using System.Diagnostics;

namespace EldredBrown.ProFootballApplicationWPF.ViewModels
{
	/// <summary>
	/// ViewModel logic for the RadioButton control.
	/// </summary>
	public class MutexViewModel : ViewModelBase
	{
		#region Fields

		private readonly RankingsControlViewModel _parentControl;

		#endregion

		#region Constructors & Finalizers

		/// <summary>
		/// Initializes a custom instance of the MutexViewModel class.
		/// </summary>
		/// <param name="parentControl"></param>
		public MutexViewModel(RankingsControlViewModel parentControl)
		{
			// Validate arguments.
			if ( parentControl == null )
			{
				throw new ArgumentNullException("parentControl");
			}

			// Assign argument values to member fields.
			_parentControl = parentControl;
		}

        #endregion

        #region Instance Properties

        /// <summary>
        /// Gets or sets the value indicating whether the attached RadioButton is checked.
        /// </summary>
        private bool _IsChecked;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                if (value != _IsChecked)
                {
                    _IsChecked = value;
                    OnPropertyChanged("IsChecked");
                }

                if (_IsChecked)
                {
                    _parentControl.TotalRankingsResult = DataAccess.DbContext.GetRankingsTotal(Text);
                    //_parentControl.OffensiveRankingsResult = DataAccess.DbContext.GetRankingsOffensive(Text);
                    //_parentControl.DefensiveRankingsResult = DataAccess.DbContext.GetRankingsDefensive(Text);
                }
            }
        }

        /// <summary>
        /// Gets or sets the text of the attached RadioButton control.
        /// </summary>
        private string _Text;
		public string Text
		{
			get { return _Text; }
			set
			{
				if ( String.IsNullOrEmpty(value) )
				{
					throw new ArgumentException("value");
				}
				else if ( value != _Text )
				{
					_Text = value;
					OnPropertyChanged("Text");
				}
			}
		}

		#endregion
	}
}
