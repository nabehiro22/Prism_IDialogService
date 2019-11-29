using Prism.Ioc;
using Prism_Dialog.Views;
using System.Windows;

namespace Prism_Dialog
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
			// ダイアログ表示時に追加される
			containerRegistry.RegisterDialog<Views.Dialog, ViewModels.DialogViewModel>();
		}
    }
}
