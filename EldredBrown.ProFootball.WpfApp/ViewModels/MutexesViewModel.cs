using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace EldredBrown.ProFootballApplicationWPF.ViewModels
{
	/// <summary>
	/// ViewModel logic for a group of RadioButton controls.
	/// </summary>
	public class MutexesViewModel : ViewModelBase
	{
        #region Constructors & Finalizers

        /// <summary>
        /// Initializes a custom instance of the MutexesViewModel class.
        /// </summary>
        /// <param name="mutexes"></param>
        public MutexesViewModel(IEnumerable<MutexViewModel> mutexes)
        {
            // Validate mutexes argument.
            if (mutexes == null)
            {
                throw new ArgumentNullException("mutexes");
            }
            else if (mutexes.Count() == 0)
            {
                throw new ArgumentException("mutexes");
            }

            try
            {
                // Populate this ViewModel's collection of mutexes.
                this.Mutexes = new ObservableCollection<MutexViewModel>();
                foreach (var mutex in mutexes)
                {
                    this.Mutexes.Add(mutex);
                    mutex.PropertyChanged += MutexViewModel_PropertyChanged;
                }
                this.Mutexes[0].IsChecked = true;
            }
            catch (Exception ex)
            {
                WpfGlobals.ShowExceptionMessage(ex);
            }
        }

        #endregion

        #region Instance Properties

        /// <summary>
        /// Gets this object's mutex collection.
        /// </summary>
        public ObservableCollection<MutexViewModel> Mutexes { get; private set; }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the PropertyChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MutexViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MutexViewModel mutex = (MutexViewModel)sender;

            // Make sure that only one of the mutexes (radio buttons) is checked at a time.
            if (e.PropertyName != "IsChecked" || !mutex.IsChecked)
            {
                return;
            }
            foreach (MutexViewModel other in Mutexes.Where(x => x != mutex))
            {
                other.IsChecked = false;
            }
        }

        #endregion
    }
}
