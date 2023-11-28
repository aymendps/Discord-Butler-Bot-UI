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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Discord_Butler_Bot_UI.UserControls
{
    /// <summary>
    /// Interaction logic for BotLog.xaml
    /// </summary>
    public partial class BotLog : UserControl
    {
        public static readonly DependencyProperty StatusColorProperty = 
            DependencyProperty.Register("StatusColor", typeof(Brush), typeof(BotLog), new PropertyMetadata(Brushes.LightGray));

        public Brush StatusColor
        {
            get { return (Brush)GetValue(StatusColorProperty); }
            set { SetValue(StatusColorProperty, value); }
        }

        public BotLog()
        {
            InitializeComponent();
            LogDate.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
