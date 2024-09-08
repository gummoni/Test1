using Processors;
/// <summary>
/// 接続ベースクラス
/// </summary>
public class ConnectableBase : IConnectable
{
    bool IsOnline;

    public void Connect()
    {
        if (IsOnline) return;
        IsOnline = true;
        
        var props = GetType().GetProperties();
        foreach (var prop in props)
        {
            if (prop.GetValue(this) is IConnectable connectable)
                connectable.Connect();
        }
        OnConnect();
    }

    public void Disconnect()
    {
        if (!IsOnline) return;
        IsOnline = false;

        OnDisconnect();

        var props = GetType().GetProperties();
        foreach (var prop in props)
        {
            if (prop.GetValue(this) is IConnectable connectable)
                connectable.Disconnect();
        }
    }

    protected virtual void OnConnect() { }
    protected virtual void OnDisconnect() { }
}
#if false
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp11
{
    public class ButtonAttribute : Attribute
    {
        public string Text { get; }
        public ButtonAttribute(string text)
        {
            Text = text;
        }
    }

    public class YourDataClass
    {
        public string Tool { get; set; }
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float PreZ { get; set; }
        public float PitchX { get; set; }
        public float PitchY { get; set; }
        public int CountX { get; set; }
        public int CountY { get; set; }


        [Button("移動")]
        public void Go()
        {
            MessageBox.Show("Method executed!");
        }

        [Button("書込")]
        public void Write()
        {
            MessageBox.Show("Method executed!");
        }
    }

    public partial class MainWindow : Window
    {
        private DataGrid dataGrid;
        //TextBox filterTextBox;
        ComboBox filterComboBox;
        private ObservableCollection<YourDataClass> allData;
        private ObservableCollection<YourDataClass> filteredData;

        public MainWindow()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            var stackPanel = new StackPanel();

            //filterTextBox = new TextBox { Margin = new Thickness(5) };
            //filterTextBox.TextChanged += FilterTextBox_TextChanged;
            //stackPanel.Children.Add(filterTextBox);

            filterComboBox = new ComboBox {  Margin= new Thickness(5) };
            stackPanel.Children.Add(filterComboBox);

            dataGrid = new DataGrid { Margin = new Thickness(5) };
            stackPanel.Children.Add(dataGrid);

            Content = stackPanel;


            allData = new ObservableCollection<YourDataClass>
        {
            new YourDataClass { Tool = "Pipet", Name = "John", },
            new YourDataClass { Tool = "Pipet", Name = "Alice" },
            new YourDataClass { Tool = "Capper", Name = "Bob" },
            new YourDataClass { Tool = "Syringe", Name = "Catherine" }
        };

            filteredData = new ObservableCollection<YourDataClass>(allData);

            dataGrid.AutoGenerateColumns = false;
            dataGrid.CanUserAddRows = false;

            // プロパティの列を生成
            foreach (var prop in typeof(YourDataClass).GetProperties())
            {
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = prop.Name,
                    Binding = new Binding(prop.Name)
                });
            }

            //コンボボックス作成
            filterComboBox.Items.Clear();
            filterComboBox.Items.Add("全て");
            allData.Select(_ => _.Tool)
                .Distinct()
                .ToList()
                .ForEach(_ => filterComboBox.Items.Add(_));
            filterComboBox.SelectedIndex = 0;
            

            // ボタンの列を追加
            AddButtonColumns();

            dataGrid.ItemsSource = filteredData;
            dataGrid.LoadingRow += DataGrid_LoadingRow;
            filterComboBox.SelectionChanged += FilterComboBox_SelectionChanged;
        }

        private void AddButtonColumns()
        {
            var buttonMethods = GetButtonMethods(typeof(YourDataClass));

            foreach (var method in buttonMethods)
            {
                var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
                var column = new DataGridTemplateColumn
                {
                    Header = buttonAttribute.Text,
                    CellTemplate = CreateButtonTemplate(method.Name, buttonAttribute.Text)
                };
                dataGrid.Columns.Add(column);
            }
        }

        private List<MethodInfo> GetButtonMethods(Type type)
        {
            var methods = new List<MethodInfo>();
            foreach (var method in type.GetMethods())
            {
                var attribute = method.GetCustomAttribute<ButtonAttribute>();
                if (attribute != null)
                {
                    methods.Add(method);
                }
            }
            return methods;
        }

        private DataTemplate CreateButtonTemplate(string methodName, string buttonText)
        {
            var template = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(Button));
            factory.SetValue(Button.ContentProperty, buttonText);
            factory.AddHandler(Button.ClickEvent, new RoutedEventHandler((sender, e) =>
            {
                var button = (Button)sender;
                var dataContext = button.DataContext;
                var method = dataContext.GetType().GetMethod(methodName);
                method.Invoke(dataContext, null);
            }));
            template.VisualTree = factory;
            return template;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        //private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string filterText = filterTextBox.Text.ToLower();
        //    var filtered = allData.Where(item => item.Name.ToLower().Contains(filterText));
        //    filteredData.Clear();
        //    foreach (var item in filtered)
        //    {
        //        filteredData.Add(item);
        //    }
        //}

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string filterText = filterComboBox.SelectedItem as string ?? "全て";
            var filtered = (filterText == "全て") ? allData  : allData.Where(item => item.Tool.Contains(filterText, StringComparison.OrdinalIgnoreCase));
            filteredData.Clear();
            foreach (var item in filtered)
            {
                filteredData.Add(item);
            }
        }
    }
}
#endif
