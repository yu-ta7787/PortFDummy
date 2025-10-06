using PortFDummy.Models; // ← ここをプロジェクト名に合わせて
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net; // 画像URLバインドのみでOK（HttpClient不要）
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace PortFDummy
{

    public partial class MainWindow : Window
    {
        private List<Game> _games = new();// ここにダミーデータを入れる
        private readonly Random _rand = new();// ランダム選択用
        private Point _dragStart;
        private bool _isDragging;
        private Point _showcaseDragStart;
        private readonly PortFDummy.Services.IGameProvider _provider = new PortFDummy.Services.DummyGameProvider();


        private static readonly string SaveDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PortFDummy");
        private static readonly string ShowcaseSavePath =
            Path.Combine(SaveDir, "showcase.json");

        private List<int> _savedShowcaseAppIds = new();// 保存されているAppId一覧
        private readonly ObservableCollection<Game> _showcase = new();// Showcase用コレクション

        public MainWindow()
        {
            InitializeComponent();

            ShowcaseList.ItemsSource = _showcase;
            _showcase.CollectionChanged += Showcase_CollectionChanged;
            LoadShowcaseIds();

            // ★ ここでダミーデータをロード
            _games = _provider.LoadGames();
            
            FillTopCards();
            FillShelf();
            FillAll();

            RestoreShowcaseFromIds();
        }

        // もしXAMLに「Steam取得テスト」ボタンがあるなら Click をこれに変更
        private void LoadDummy_Click(object sender, RoutedEventArgs e)
        {
            FillTopCards();// 再ランダム
            FillShelf();   // 再表示
            FillAll();      // 再表示
            RestoreShowcaseFromIds();
            MessageBox.Show("ダミーデータを再読込しました。");
        }

        private void FillTopCards()// トップカードの更新
        {
            var yesterday = _games.FirstOrDefault(g => g.LastPlayed?.Date == DateTime.Today.AddDays(-1));
            SetCard(YesterdayImage, YesterdayText, yesterday, "昨日プレイ");

            var top1 = _games.OrderByDescending(g => g.PlaytimeMinutes).FirstOrDefault();
            SetCard(Top1Image, Top1Text, top1, "総プレイ1位", showHours: true);

            var backlog = _games.Where(g => g.PlaytimeMinutes < 120).ToList();
            var r1 = PickRandom(backlog) ?? PickRandom(_games);
            var r2 = PickRandom(backlog) ?? PickRandom(_games);
            SetCard(Random1Image, Random1Text, r1, "ランダム①");
            SetCard(Random2Image, Random2Text, r2, "ランダム②");
        }

        private Game? PickRandom(List<Game> src)// ランダムに1本選ぶ
        {
            if (src == null || src.Count == 0) return null;
            return src[_rand.Next(src.Count)];
        }

        private void FillShelf()// プレイ時間2時間未満のゲーム一覧
        {
            var pile = _games.Where(g => g.PlaytimeMinutes < 120)
                             .OrderBy(g => g.Name)
                             .ToList();
            ShelfList.ItemsSource = pile;
        }

        private void FillAll()// 全ゲーム一覧
        {
            AllList.ItemsSource = _games.OrderBy(g => g.Name).ToList();
        }

        private void SetCard(Image img, TextBlock tb, Game? g, string label, bool showHours = false)// カードの内容をセット
        {
            if (g == null)
            {
                BindingOperations.ClearBinding(img, Image.SourceProperty);
                img.Source = null;
                tb.Text = $"{label}\n（該当なし）";
                return;
            }

            tb.Text = showHours
                ? $"{label}\n{g.Name}\n{g.PlaytimeMinutes / 60.0:F1} h"
                : $"{label}\n{g.Name}";

            BindingOperations.ClearBinding(img, Image.SourceProperty);
            img.Source = null;

            var binding = new Binding("HeaderImageUrl")
            {
                Source = g,
                Mode = BindingMode.OneWay
            };
            BindingOperations.SetBinding(img, Image.SourceProperty, binding);
        }

       
        private void OpenStore_Click(object sender, RoutedEventArgs e)// ストアを開く（Shift+クリックでShowcaseに追加）
        {
            if (_isDragging) { e.Handled = true; return; }// ドラッグ中は無視

            if (sender is Button btn)// ButtonのTagまたはDataContextにGameオブジェクトが入っている前提
            {
                Game? game =
                    btn.Tag as Game ??
                    btn.DataContext as Game ??
                    (btn.Tag is int idOnly ? _games.FirstOrDefault(x => x.AppId == idOnly) : null);

                // Shift+クリックでShowcaseに追加
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift && game != null)
                {
                    if (!_showcase.Any(g => g.AppId == game.AppId))
                        _showcase.Add(game);

                    SaveShowcase();
                    ShowcaseList.SelectedItem = game;
                    ShowcaseList.Focus();
                    e.Handled = true;
                    return;
                }

                // 通常クリックはSteamストアを開く（任意機能）
                int appId = game?.AppId ?? 0;
                if (appId > 0)
                {
                    string url = $"https://store.steampowered.com/app/{appId}/";
                    try { Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true }); }
                    catch (Exception ex) { MessageBox.Show("ブラウザを開けませんでした: " + ex.Message); }
                }
            }
        }
        private void ShowcaseList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // 例: Shift + D で選択アイテムを削除
            if (e.Key == Key.D && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                if (ShowcaseList?.SelectedItem != null)
                {
                    // ObservableCollection<Game> _showcase を使っている前提
                    _showcase.Remove((Game)ShowcaseList.SelectedItem);
                    e.Handled = true;
                }
            }
        }
        private void AllItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)// 全ゲーム一覧からのドラッグ開始
        {
            _dragStart = e.GetPosition(null);
            _isDragging = false;
        }

        private void AllItem_PreviewMouseMove(object sender, MouseEventArgs e)// 全ゲーム一覧からのドラッグ中
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var pos = e.GetPosition(null);
            if (Math.Abs(pos.X - _dragStart.X) < SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(pos.Y - _dragStart.Y) < SystemParameters.MinimumVerticalDragDistance)
                return;

            if (sender is Button btn)
            {
                var game = btn.Tag as Game ?? btn.DataContext as Game;
                if (game is null) return;

                _isDragging = true;
                DragDrop.DoDragDrop(btn, new DataObject("game", game), DragDropEffects.Copy);
                _isDragging = false;
            }
        }
        private void ShowcaseList_DragOver(object sender, DragEventArgs e)// Showcase上でのドラッグオーバー 
        {
            if (e.Data.GetDataPresent("game") || e.Data.GetDataPresent("showcase-move"))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;

            e.Handled = true;
        }

        private void ShowcaseList_Drop(object sender, DragEventArgs e)// Showcase上でのドロップ処理
        {
            if (e.Data.GetDataPresent("game"))
            {
                var game = (Game)e.Data.GetData("game");
                if (!_showcase.Any(g => g.AppId == game.AppId))
                    _showcase.Add(game);

                SaveShowcase();
                return;
            }

            if (e.Data.GetDataPresent("showcase-move"))
            {
                var moving = (Game)e.Data.GetData("showcase-move");
                if (moving is null) return;

                int oldIndex = _showcase.IndexOf(moving);
                if (oldIndex < 0) return;

                _showcase.RemoveAt(oldIndex);
                int insertIndex = GetShowcaseInsertIndex(e.GetPosition(ShowcaseList));
                insertIndex = Math.Max(0, Math.Min(insertIndex, _showcase.Count));
                _showcase.Insert(insertIndex, moving);

                SaveShowcase();
            }
        }

        private int GetShowcaseInsertIndex(Point mousePosInShowcase)// ドロップ位置のインデックスを計算
        {
            int closest = _showcase.Count;
            double best = double.MaxValue;

            for (int i = 0; i < ShowcaseList.Items.Count; i++)// 各アイテムの中心点を計算して、マウス位置に最も近いアイテムを探す
            {
                var containerObj = ShowcaseList.ItemContainerGenerator.ContainerFromIndex(i);
                if (containerObj is not FrameworkElement container) continue;

                var pos = container.TranslatePoint(new Point(0, 0), ShowcaseList);
                var center = new Point(pos.X + container.ActualWidth / 2, pos.Y + container.ActualHeight / 2);

                double d = (center - mousePosInShowcase).Length;
                if (d < best)
                {
                    best = d;//
                    closest = i;
                    if (mousePosInShowcase.X > center.X) closest = i + 1;
                }
            }
            return closest;
        }

        private void RerollRandom_Click(object sender, RoutedEventArgs e) => FillTopCards();// ランダムカードの再抽選


        private void SaveShowcase()  // --- Showcase の保存/復元（AppIdのみJSON保存） ---
        {
            try
            {
                Directory.CreateDirectory(SaveDir);
                var ids = _showcase.Select(g => g.AppId).ToList();
                var json = JsonSerializer.Serialize(ids, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ShowcaseSavePath, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("保存失敗: " + ex.Message);
            }
        }

        private void LoadShowcaseIds()
        {
            try
            {
                _savedShowcaseAppIds.Clear();
                if (!File.Exists(ShowcaseSavePath)) return;

                var json = File.ReadAllText(ShowcaseSavePath);
                var ids = JsonSerializer.Deserialize<List<int>>(json);
                if (ids != null) _savedShowcaseAppIds = ids;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("読込失敗: " + ex.Message);
            }
        }

        private void RestoreShowcaseFromIds()
        {
            if (_savedShowcaseAppIds == null || _savedShowcaseAppIds.Count == 0) return;
            if (_games == null || _games.Count == 0) return;

            _showcase.CollectionChanged -= Showcase_CollectionChanged;
            _showcase.Clear();

            var missing = new List<int>();
            foreach (var id in _savedShowcaseAppIds)
            {
                var g = _games.FirstOrDefault(x => x.AppId == id);
                if (g != null) _showcase.Add(g);
                else missing.Add(id);
            }

            _showcase.CollectionChanged += Showcase_CollectionChanged;
            _savedShowcaseAppIds.Clear();

            Debug.WriteLine($"Showcase 復元: {_showcase.Count} 本, 見つからず: {missing.Count}");
            if (missing.Count > 0)
                Debug.WriteLine("Missing AppIds: " + string.Join(",", missing));
        }
        protected override void OnClosed(EventArgs e)// ウィンドウ終了時に保存
        {
            SaveShowcase();
            base.OnClosed(e);
        }

        private void Showcase_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SaveShowcase();
        }

        // --- Showcase内のドラッグ開始（並べ替え用） ---
        private void ShowcaseItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _showcaseDragStart = e.GetPosition(null);
        }

        private void ShowcaseItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var pos = e.GetPosition(null);
            if (Math.Abs(pos.X - _showcaseDragStart.X) < SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(pos.Y - _showcaseDragStart.Y) < SystemParameters.MinimumVerticalDragDistance)
                return;

            if (sender is FrameworkElement fe && fe.DataContext is Game g)
            {
                DragDrop.DoDragDrop(fe, new DataObject("showcase-move", g), DragDropEffects.Copy);
            }
        }
    }
}
