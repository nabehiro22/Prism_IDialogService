using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Reactive.Disposables;

namespace Prism_Dialog.ViewModels
{
	public class DialogViewModel : BindableBase, IDialogAware // IDialogAwareが追加される
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
		public ReactivePropertySlim<string> Message2 { get; } = new ReactivePropertySlim<string>("");

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
			this.YesCommand.Subscribe(_ => this.RequestClose?.Invoke(new DialogResult(ButtonResult.Yes, Result)));
			// 「いいえ」ボタンが押された時はButtonResult.Noだけを返す
			this.NoCommand.Subscribe(_ => this.RequestClose?.Invoke(new DialogResult(ButtonResult.No)));
			// Disposableに追加
			Disposable.Add(YesCommand);
			Disposable.Add(NoCommand);

			// ウィンドウを閉じる時に返す値をセットする例
			Result.Add("key1", "Value1");
			Result.Add("key2", 100);
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
		/// ダイアログClose時のイベントハンドラ
		/// </summary>
		public void OnDialogClosed()
		{
			// まどめてDispose
			Disposable.Dispose();
		}

		/// <summary>
		/// ダイアログOpen時のイベントハンドラ
		/// </summary>
		/// <param name="parameters"></param>
		public void OnDialogOpened(IDialogParameters parameters)
		{
			// 例はダイアログを起動させたViewModelから来たKey「Message1」のValueを取得
			this.Message1.Value = parameters.GetValue<string>("Message1");
			this.Message2.Value = parameters.GetValue<string>("Message2");
		}
	}
}
