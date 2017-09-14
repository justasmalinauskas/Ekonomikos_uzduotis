using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Ekonomika
{
    /// <summary>
    /// Interaction logic for DataControl.xaml
    /// </summary>
    /// 
    public partial class DataControl : UserControl
    {
        private bool changed = false;
        public static readonly DependencyProperty GoodNameProp = DependencyProperty.Register("GoodName", typeof(string), typeof(DataControl));
        public string GoodName
        {
            get { return (string)GetValue(GoodNameProp); }
            set { SetValue(GoodNameProp, value); }
        }
        public static readonly DependencyProperty EnterNextProp = DependencyProperty.Register("GoodsNew", typeof(double), typeof(DataControl));
        public double GoodsNew
        {
            get { return (double)GetValue(EnterNextProp); }
            set { SetValue(EnterNextProp, value); }
        }
        public static readonly DependencyProperty GoodsSumProp = DependencyProperty.Register("GoodSum", typeof(double), typeof(DataControl));
        public double GoodsSum
        {
            get { return (double)GetValue(GoodsSumProp); }
            set { SetValue(GoodsSumProp, value); }
        }
        public DataControl()
        {
            InitializeComponent();
        }
        private void txtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
            Debug.Write(e.Text);
        }
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex(@"^\d+([\.\,]?\d+)?$");
            return regex.IsMatch(text);
        }

        private void bringlast_Click(object sender, RoutedEventArgs e)
        {
            if (changed == false)
            {
                GoodsSum -= GoodsNew;
                changed = true;
            }
            ((MainWindow)Application.Current.MainWindow).TriggerEvent();

        }

        private void addtosum_Click(object sender, RoutedEventArgs e)
        {
            GoodsSum += GoodsNew;
            changed = false;
            ((MainWindow)Application.Current.MainWindow).TriggerEvent();
        }

        private void DeleteandClose_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).DeleteItem(GoodName);
        }

        private void enternext_KeyDown(object sender, KeyEventArgs e)
        {
        }
    }
}
