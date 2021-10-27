using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;

namespace Prism_Dialog.ViewModels
{
	public class DialogViewModel : BindableBase, IDialogAware, INotifyPropertyChanged // IDialogAwareが追加される
	{
		/// <summary>
		/// ウィンドウに表示されるタイトル
		/// </summary>
		public string Title => "タイトル";

		/// <summary>
		/// ダイアログのCloseを要求するAction
		/// </summary>
		public event Action<IDialogResult> RequestClose;

		/// <summary>
		/// まとめてDispose
		/// </summary>
		private CompositeDisposable Disposable { get; } = new CompositeDisposable();

		/// <summary>
		/// Yesボタンが押された時
		/// </summary>
		public ReactiveCommand YesCommand { get; } = new ReactiveCommand();

		/// <summary>
		/// Noボタンが押された時
		/// </summary>
		public ReactiveCommand NoCommand { get; } = new ReactiveCommand();

		/// <summary>
		/// 親ウィンドウからの情報
		/// </summary>
		public ReactivePropertySlim<string> Message1 { get; } = new ReactivePropertySlim<string>("");

		/// <summary>
		/// 親ウィンドウからの情報
		/// </summary>
		public ReactivePropertySlim<int> Message2 { get; } = new ReactivePropertySlim<int>();

		/// <summary>
		/// ウィンドウを閉じる時に渡すDialogParameters
		/// </summary>
		public DialogParameters Result { get; } = new DialogParameters();

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public DialogViewModel()
		{
			// 「はい」ボタンが押された時はButtonResult.YesとDialogParametersを返す
			_ = YesCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Yes, Result))).AddTo(Disposable);
			// 「いいえ」ボタンが押された時はButtonResult.Noだけを返す
			_ = NoCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.No))).AddTo(Disposable);
		}

		/// <summary>
		/// ダイアログがClose可能かを取得
		/// </summary>
		/// <returns></returns>
		public bool CanCloseDialog()
		{
			return true;
		}

		/// <summary>
		/// ダイアログClose時実行されるメソッド
		/// </summary>
		public void OnDialogClosed()
		{
			// ウィンドウを閉じる時に返す値をセットする例
			Result.Add("key1", "Value1");
			Result.Add("key2", 100);

			// まどめてDispose
			Disposable.Dispose();
		}

		/// <summary>
		/// ダイアログOpen時に実行されるメソッド
		/// </summary>
		/// <param name="parameters"></param>
		public void OnDialogOpened(IDialogParameters parameters)
		{
			// 例はダイアログを起動させたViewModelから来たKey「Message1」と「Message2」のValueを取得
			Message1.Value = parameters.GetValue<string>("Message1");
			Message2.Value = parameters.GetValue<int>("Message2");
		}
	}
}
