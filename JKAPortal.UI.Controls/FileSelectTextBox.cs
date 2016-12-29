using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JKAPortal.UI.Controls
{
    public class FileSelectTextBox : TextBox
    {
        private ButtonEx _BrowseButton;

        public bool IsFolderPicker
        {
            get { return (bool)GetValue(IsFolderPickerProperty); }
            set { SetValue(IsFolderPickerProperty, value); }
        }

        public bool MultiSelect
        {
            get { return (bool)GetValue(MultiSelectProperty); }
            set { SetValue(MultiSelectProperty, value); }
        }

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        //public string Path
        //{
        //    get { return (string)GetValue(PathProperty); }
        //    set { SetValue(PathProperty, value); }
        //}

        public static readonly DependencyProperty IsFolderPickerProperty =
            DependencyProperty.Register(
                "IsFolderPicker",
                typeof(bool),
                typeof(FileSelectTextBox),
                new UIPropertyMetadata(false)
                );

        public static readonly DependencyProperty MultiSelectProperty =
            DependencyProperty.Register(
                "MultiSelect",
                typeof(bool),
                typeof(FileSelectTextBox),
                new UIPropertyMetadata(false)
                );

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(
                "Filter",
                typeof(string),
                typeof(FileSelectTextBox),
                new UIPropertyMetadata("All files (*.*)|*.*")
                );

        //public static readonly DependencyProperty PathProperty =
        //    DependencyProperty.Register(
        //        "Path",
        //        typeof(string),
        //        typeof(FileSelectTextBox),
        //        new UIPropertyMetadata(string.Empty)
        //        );

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _BrowseButton = (ButtonEx)GetTemplateChild("PART_BrowseButton");
            _BrowseButton.Click += OnBrowse;
        }

        private void OnBrowse(object sender, RoutedEventArgs e)
        {
            if (IsFolderPicker)
            {
                CommonOpenFileDialog dlg = new CommonOpenFileDialog();
                dlg.Multiselect = MultiSelect;
                dlg.IsFolderPicker = IsFolderPicker;

                if (!string.IsNullOrEmpty(Text))
                    dlg.DefaultDirectory = Text;

                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    Text = dlg.FileName;
                    InvalidateProperty(TextProperty);
                }
            }
            else
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = MultiSelect;
                dlg.Filter = Filter;

                if (!string.IsNullOrEmpty(Text))
                    dlg.InitialDirectory = Text;

                if (dlg.ShowDialog() == true)
                {
                    Text = dlg.FileName;
                    InvalidateProperty(TextProperty);
                }
            }
        }

        static FileSelectTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(FileSelectTextBox), new FrameworkPropertyMetadata(typeof(FileSelectTextBox))
                );
        }
    }
}
