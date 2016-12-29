using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JKAPortal.UI.Controls
{
    [TemplatePart(Name = "PART_TextRenderer", Type = typeof(TextBlock))]
    public class Q3ColoredTextBlock : Control
    {
        private TextBlock _textBlock;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(Q3ColoredTextBlock),
                new UIPropertyMetadata(string.Empty, OnTextPropertyChanged)
                );

        static Q3ColoredTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Q3ColoredTextBlock), new FrameworkPropertyMetadata(typeof(Q3ColoredTextBlock)));
        }

        private static void OnTextPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            Q3ColoredTextBlock control = (Q3ColoredTextBlock)source;
            control.RefreshText();            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Loaded,
                new Action(() =>
            {
                _textBlock = (TextBlock)GetTemplateChild("PART_TextRenderer");
                RefreshText();
            }));
        }

        public void RefreshText()
        {
            if (_textBlock == null)
                return;

            if (string.IsNullOrEmpty(Text))
                return;

            _textBlock.Inlines.Clear();

            //DropShadowEffect textEffect = new DropShadowEffect();
            //textEffect.ShadowDepth = 1;
            //textEffect.Opacity = 1f;
            //textEffect.Direction = 310;
            //textEffect.Color = Colors.Black;

            //_textBlock.Effect = textEffect;

            Run run = new Run();
            run.Foreground = Brushes.White;
            run.Text = string.Empty;

            int i = 0;
            while(i < Text.Length)
            {
                if (Text[i] == '^' && i < Text.Length - 2)
                {
                    if (i != 0)
                        _textBlock.Inlines.Add(run);

                    run = new Run();
                    run.Foreground = GetColorByNumber((int)char.GetNumericValue(Text[i + 1]));
                    i++;
                }
                else
                {
                    run.Text += Text[i];
                }

                i++;
            }

            if (run.Text != string.Empty)
                _textBlock.Inlines.Add(run);
        }

        private SolidColorBrush GetColorByNumber(int number)
        {
            SolidColorBrush color = Brushes.White;

            switch(number)
            {
                case 0:
                case 9:
                    color = Brushes.Black;
                    break;
                case 1:
                    color = Brushes.Red;
                    break;
                case 2:
                    color = Brushes.Green;
                    break;
                case 3:
                    color = Brushes.Yellow;
                    break;
                case 4:
                    color = Brushes.Blue;
                    break;
                case 5:
                    color = Brushes.Cyan;
                    break;
                case 6:
                    color = Brushes.Magenta;
                    break;
                case 7:
                    color = Brushes.White;
                    break;
            }

            return color;
        }
    }
}
