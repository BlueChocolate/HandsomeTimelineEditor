using ControlzEx.Theming;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HandsomeTimelineEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Timeline timeline = new Timeline();
        public MainWindow()
        {
            InitializeComponent();
            InitCurrentTheme();
            InitDatacontext();
        }

        private void InitDatacontext()
        {
            DataContext = timeline;
        }

        private void InitCurrentTheme()
        {
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
        }


        private void ImportFromFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Text file (*.txt)|*.txt|All file (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                timeline.OriginalData = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void ConvertFromText_Click(object sender, RoutedEventArgs e)
        {
            timeline.ReadOriginalData();
        }

        private void ExportFromText_Click(object sender, RoutedEventArgs e)
        {
            timeline.ConvertToRaw();
        }

        private void AddNewRow_Click(object sender, RoutedEventArgs e)
        {
            timeline.EventList.Add(new EventItem("请输入文本", DateTime.Now, EventItem.StatusColor.Info));
        }

        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|All file (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, timeline.OriginalData);
                this.ShowMessageAsync("保存文件", "文件保存成功！");
            }
        }
    }
}