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
		/// ダイアログからの戻り値 文字列
		/// </summary>
		public ReactivePropertySlim<string> DialogValue1 { get; } = new ReactivePropertySlim<string>();

		/// <summary>
		/// ダイアログからの戻り値 数値
		/// </summary>
		public ReactivePropertySlim<int> DialogValue2 { get; } = new ReactivePropertySlim<int>();

		/// <summary>
		/// IDialogServiceの情報
		/// </summary>
		private readonly IDialogService dlgService;

		/// <summary>
		/// コンストラクタ
		/// 引数 IDialogService dialogServiceを追加する
		/// </summary>
		public MainWindowViewModel(IDialogService dialogService)
        {
			// IDialogServiceの情報を取得
			dlgService = dialogService;
			// Subscribeにダイアログ表示メソッドを設定し、戻り値の処理を行う
			_ = ShowDialgo.Subscribe(_ =>
			{
				IDialogResult result = showDialog("メインウィンドウからのメッセージ1", 2);
				// ButtonResultがYesならKey名「key1」の値を取得する。間違ったkey名だとNullを返すので注意が必要。
				if (result.Result == ButtonResult.Yes)
				{
					DialogValue1.Value = $"Key1の値：{result.Parameters.GetValue<string>("key1")}";
					DialogValue2.Value = result.Parameters.GetValue<int>("key2");
				}
				else
				{
					DialogValue1.Value = "いいえが押されました";
					DialogValue2.Value = 0;
				}
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
			// パラメータを渡す手法その１
			//IDialogParameters parameters = new DialogParameters { { "Message1", message1 }, { "Message2", message2 } };
			// パラメータを渡す手法その２
			IDialogParameters parameters = new DialogParameters();
			parameters.Add("Message1", message1);
			parameters.Add("Message2", message2);

			// ShowDialogの引数1：開くダイアログの名称 引数2：ダイアログに渡すパラメータ(KeyとValueのセット)で複数可 引数3：受け取る戻り値
			dlgService.ShowDialog("Dialog", parameters, dialogResult => result = dialogResult);

			return result;
		}
	}
}
