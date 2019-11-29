using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;

namespace Prism_Dialog.ViewModels
{
    public class MainWindowViewModel : BindableBase, INotifyPropertyChanged
	{
		/// <summary>
		/// ウィンドウタイトル
		/// </summary>
		public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("ダイアログ表示");

		/// <summary>
		/// まとめてDispose
		/// </summary>
		private CompositeDisposable Disposable { get; } = new CompositeDisposable();

		/// <summary>
		/// ダイアログ表示コマンド
		/// </summary>
		public ReactiveCommand ShowDialgo { get; } = new ReactiveCommand();

		/// <summary>
		/// ダイアログ表示の戻り値
		/// </summary>
		public IDialogResult DialogResult;

		/// <summary>
		/// ダイアログからの戻り値 Value値
		/// </summary>
		public ReactivePropertySlim<string> DialogValue1 { get; } = new ReactivePropertySlim<string>();

		/// <summary>
		/// ダイアログからの戻り値 Value値
		/// </summary>
		public ReactivePropertySlim<string> DialogValue2 { get; } = new ReactivePropertySlim<string>();

		/// <summary>
		/// IDialogServiceの情報
		/// </summary>
		private readonly IDialogService dlgService = null;

		/// <summary>
		/// コンストラクタ
		/// 引数 IDialogService dialogServiceを追加する
		/// </summary>
		public MainWindowViewModel(IDialogService dialogService)
        {
			// IDialogServiceの情報を取得
			this.dlgService = dialogService;
			// Subscribeにダイアログ表示メソッドを設定し、戻り値の処理を行う
			ShowDialgo.Subscribe(_ =>
			{
				DialogResult = showDialog("メインウィンドウからのメッセージ1", 2);
				// ButtonResultがYesならKey名「key1」の値を取得する。間違ったkey名だとNullを返すので注意が必要。
				DialogValue1.Value = DialogResult.Result == ButtonResult.Yes ? $"Key1の値：{DialogResult.Parameters.GetValue<string>("key1")}" : "いいえが押されました";
				// ButtonResultがYesならKey名「key2」の値を取得する。間違ったkey名だとNullを返すので注意が必要。
				DialogValue2.Value = DialogResult.Result == ButtonResult.Yes ? $"Key2の値：{DialogResult.Parameters.GetValue<string>("key2")}" : "いいえが押されました";
			}).AddTo(Disposable);
		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		~MainWindowViewModel()
		{
			Disposable.Dispose();
		}

		/// <summary>
		/// ダイアログを開くメソッド
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		private IDialogResult showDialog(string message1, int message2)
		{
			IDialogResult result = null;
			// ShowDialogの引数1：開くダイアログの名称 引数2：ダイアログに渡すパラメータ(KeyとValueのセット)で複数可 引数3：受け取る戻り値
			this.dlgService.ShowDialog("Dialog", new DialogParameters { { "Message1", message1 }, { "Message2", message2 } }, ret => result = ret);
			return result;
		}
	}
}
