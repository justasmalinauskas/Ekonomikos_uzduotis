using Microsoft.Win32;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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


namespace Ekonomika
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<ContentControl> aList = new ObservableCollection<ContentControl>();
        List<GoodsName> goods = new List<GoodsName>();
        public MainWindow()
        {
            InitializeComponent();
            //aListBox.ItemsSource = CreateFake();
            //aListBox.ItemsSource = null;
            TriggerEvent();
            //double suam = aListBox.Items.Cast<double>.Sum();
            //aListBox.Items.Add(new DataControl { GoodName = "Tom", GoodsSum = 50.25 });

            //parts.Find(x => x.PartName.Contains("seat")));

            //aListBox.ItemsSource = aList;
        }


        public static ObservableCollection<ContentControl> CreateFake()
        {
            return new ObservableCollection<ContentControl>
            {
                new DataControl { GoodName="Tom", GoodsSum=50.25 },
                new DataControl { GoodName="Tom", GoodsSum=50.25 },
                new DataControl { GoodName="Tom", GoodsSum=50.25 },
            };
        }

        public void TriggerEvent()
        {
            if (aListBox.Items.Count != 0)
                sumall.Content = "Viso išleista: " + aListBox.Items.Cast<DataControl>().Sum(x => Convert.ToDouble(x.GoodsSum)).ToString("0.00") +"€";
            else
                sumall.Content = "Viso išleista: 0,00" + "€";
        }

        public void DeleteItem(string itemname)
        {
            System.Diagnostics.Debug.WriteLine(itemname);
            MessageBoxResult res = MessageBox.Show("Ar tikrai norite pašalinti suvestus duomenys?\nIštrinus jų bus nebegalima atkurti.", "Svarbus pranešimas!", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                comboBox.Items.Add(itemname);
                aListBox.Items.Remove(aListBox.Items.Cast<DataControl>().First(x => x.GoodName == itemname));
                aListBox.Items.Cast<DataControl>().OrderBy(x => x.GoodName);
                TriggerEvent();
            }

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FileStream f = new FileStream(Directory.GetCurrentDirectory() + "/data.txt", FileMode.Open);
                using (StreamReader r = new StreamReader(f))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        comboBox.Items.Add(line);
                    }
                }
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Nerastas failas, kuriame yra išlaidų klasifikatoriaus įrašai.\nPažiūrėkite ar tas failas egzistuoja (data.txt) ir sutvarkę bandykite paleisti programą iš naujo", "Klaida!", MessageBoxButton.OK, MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
            
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (comboBox.SelectedIndex != -1)
            {
                string name = comboBox.SelectedItem.ToString();
                aListBox.Items.Add(new DataControl { GoodName = name });
                comboBox.Items.Remove(name);
                comboBox.SelectedIndex = -1;
            }

            //


        }

        private void savetocsv_Click(object sender, RoutedEventArgs e)
        {
            string csv = null;
            csv += string.Format("{0};{1};{2}\n", "Kategorijos kodas", "Kategorijos pavadinimas", "Išleistų pinigų suma");
            for (int i = 0; i < aListBox.Items.Count; i++)
            {
                string vara = ((DataControl)aListBox.Items[i]).GoodName;
                string[] items = vara.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string a1 = items[0];
                string a2 = string.Join(" ", items.Skip(1).ToList());
                double a3 = ((DataControl)aListBox.Items[i]).GoodsSum;
                csv += string.Format("{0};{1};{2}\n", a1, a2, a3);
                System.Diagnostics.Debug.WriteLine(csv);
                
            }
            SaveFileDialog savefile = new SaveFileDialog();
            // set a default file name
            savefile.FileName = "analize.csv";
            // set filters - this can be done in properties as well
            savefile.Filter = "csv įrašai (*.csv)|*.csv|Visi įrašai (*.*)|*.*";

            Nullable<bool> result = savefile.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                try
                {
                    File.WriteAllText(savefile.FileName, csv, Encoding.UTF8);
                }
                catch
                {
                    MessageBox.Show("Failas šiuo metu yra naudojamas.\nPrašome išungti visas programas,\nkurios naudoją tą failą", "Klaida!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private double sumtospend()
        {
            double atsk=0.00;
            for (int i = 0; i < aListBox.Items.Count; i++)
            {
                atsk += ((DataControl)aListBox.Items[i]).GoodsSum;

            }
            return atsk;
        }
        
        private void saveasxlsx_Click(object sender, RoutedEventArgs e)
        {
            IWorkbook wb = new XSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");
            ICreationHelper cH = wb.GetCreationHelper();
            IRow row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("Kategorijos kodas");
            row.CreateCell(1).SetCellValue("Kategorijos pavadinimas");
            row.CreateCell(2).SetCellValue("Išleistų pinigų suma");

            for (int i = 0; i < aListBox.Items.Count; i++)
            {

                IRow rowa = sheet.CreateRow(i + 1);
                string vara = ((DataControl)aListBox.Items[i]).GoodName;
                string[] items = vara.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                rowa.CreateCell(0).SetCellValue("\t" + items[0]);
                rowa.CreateCell(1).SetCellValue(string.Join(" ", items.Skip(1).ToList()));
                ICell cell2 = rowa.CreateCell(2);
                ICellStyle cellStyle2 = wb.CreateCellStyle();
                IDataFormat format = wb.CreateDataFormat();
                cellStyle2.DataFormat = format.GetFormat("#.##€");
                cell2.CellStyle = cellStyle2;
                cell2.SetCellValue(((DataControl)aListBox.Items[i]).GoodsSum);
            }
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(aListBox.Items.Count + 1, aListBox.Items.Count + 1, 0, 1));
            IRow rows = sheet.CreateRow(aListBox.Items.Count + 1);
            rows.CreateCell(0).SetCellValue("Visa išleista pinigų suma");
            ICell cell1 = rows.CreateCell(2);
            ICellStyle cellStyle1 = wb.CreateCellStyle();
            IDataFormat format1 = wb.CreateDataFormat();
            cellStyle1.DataFormat = format1.GetFormat("#.##€");
            cell1.CellStyle = cellStyle1;
            cell1.SetCellValue(sumtospend());
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "analize.xlsx";
            savefile.Filter = "xlsx įrašai (*.xlsx)|*.csv|Visi įrašai (*.*)|*.*";
            Nullable<bool> result = savefile.ShowDialog();
            if (result == true)
            {
                try
                {
                    using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create, FileAccess.Write))
                    {
                        wb.Write(stream);
                    }
                    MessageBox.Show("Failas įrašytas sėkmingai", "Pavyko!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Failas šiuo metu yra naudojamas.\nPrašome išungti visas programas,\nkurios naudoją tą failą", "Klaida!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
