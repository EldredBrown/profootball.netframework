using System.Windows;
using System.Windows.Controls;

/*
 * Credit to Bob Bao: http: //social.msdn.microsoft.com/Forums/vstudio/en-US/1f58bdab-feb3-4012-aa8d-5b79b70ab1e4/datagrid-not-refreshing-on-bound-collectionviewsource-with-mvvm
 */

namespace EldredBrown.ProFootball.WpfApp.ViewModels
{
    public static class UpdateHelper
	{
		public static bool GetUpdate(DependencyObject obj)
		{
			return (bool)obj.GetValue(UpdateProperty);
		}

		public static void SetUpdate(DependencyObject obj, bool value)
		{
			obj.SetValue(UpdateProperty, value);
		}

		public static readonly DependencyProperty UpdateProperty = 
			DependencyProperty.RegisterAttached("Update", typeof(bool), typeof(UpdateHelper),
			new UIPropertyMetadata(false, RequestUpdate));

		private static void RequestUpdate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if ( (bool)e.NewValue )
			{
				(sender as DataGrid).Items.Refresh();
			}
		}
	}
}
