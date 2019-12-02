/*
 * Credit to Josh Smith: http: //joshsmithonwpf.wordpress.com/2010/03/16/control-input-focus-from-viewmodel-objects/
 */

using System;
using System.Windows;

namespace EldredBrown.ProFootball.WpfApp.ViewModels.FocusVMLib
{
    public class FocusBinding : BindingDecoratorBase
    {
        public override object ProvideValue(IServiceProvider provider)
        {
            DependencyObject elem;
            DependencyProperty prop;
            if (base.TryGetTargetItems(provider, out elem, out prop))
            {
                FocusController.SetFocusableProperty(elem, prop);
            }

            return base.ProvideValue(provider);
        }
    }
}
