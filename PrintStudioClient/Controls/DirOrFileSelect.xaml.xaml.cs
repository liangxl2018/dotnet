using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Forms;
using System.IO;
using PrintStudioModel;

namespace CommonPrintStudio
{
    /// <summary>
    /// 文件或路径选择控件
    /// </summary>
    public partial class DirOrFileSelect : System.Windows.Controls.UserControl
    {
        public static readonly DependencyProperty EnableProperty = DependencyProperty.Register("DialogType",
          typeof(int), typeof(DirOrFileSelect), new PropertyMetadata(new PropertyChangedCallback((sender, e) =>
          {
          })));

        /// <summary>
        /// 对话框类型 0:保存文件 1:选择文件(仅支持一个) 2:选择路径
        /// </summary>
        public int DialogType
        {
            get { return (int)GetValue(EnableProperty); }
            set { this.SetValue(EnableProperty, value); }
        }

        public event EventHandler<PictureSelectedEventArgs> OnSelectedChanged = null;

        private string _dialogValue = string.Empty;
        /// <summary>
        /// 选择结果
        /// </summary>
        public string DialogValue
        {
            get { return _dialogValue; }
            set
            {
                if (_dialogValue != value)
                {
                    if (OnSelectedChanged != null)
                    {
                        OnSelectedChanged(this, new PictureSelectedEventArgs() { Source=this.Tag,PictureResource=value});
                    }
                }
                _dialogValue = value;
                tbFilePath.Text = _dialogValue.Substring(_dialogValue.LastIndexOf("\\") + 1);
            }
        }
        private string _filter = "All(*.*)|*.*";
        /// <summary>
        /// 获取或设置当前文件名筛选器字符串,该字符串决定对话框的“另存为文件类型”或“文件类型”框中出现的选择内容.
        /// 仅在DialogType为0和1时生效.
        /// </summary>
        public string Filter { get { return _filter; } set { _filter = value; } }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DialogType">0:保存文件 1:选择文件 2:选择路径</param>
        /// <param name="filter">获取或设置当前文件名筛选器字符串，该字符串决定对话框的“另存为文件类型”或“文件类型”框中出现的选择内容</param>
        public DirOrFileSelect()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (DialogType == 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = Filter;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DialogValue = tbFilePath.Text = saveFileDialog.FileName;
                }
            }
            else if (DialogType == 1)
            {
                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Filter = Filter;
                myDialog.CheckFileExists = true;
                myDialog.Multiselect = false;

                if (myDialog.ShowDialog() == DialogResult.OK)
                {
                    DialogValue = tbFilePath.Text = myDialog.FileName;
                }
            }
            else if (DialogType == 2)
            {
                FolderBrowserDialog myDialogPath = new FolderBrowserDialog();
                if (myDialogPath.ShowDialog() == DialogResult.OK)
                {
                    DialogValue = tbFilePath.Text = myDialogPath.SelectedPath;
                }
            }
        }
    }
}
