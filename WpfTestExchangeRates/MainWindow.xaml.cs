using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
using WpfTestExchangeRates.Models;

namespace WpfTestExchangeRates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            InitializeComponent();
            DateFrom.SelectedDate = DateTime.Today;
            DateTo.SelectedDate = DateTime.Today;
        }        

        private const string CurrencyBaseRequest = "https://www.nbrb.by/api/exrates/currencies";
        private const string RateBaseRequest = "https://www.nbrb.by/api/exrates/rates";
        private void GetCurrencyBtn_Click(object sender, RoutedEventArgs e)
        {
            if(DateFrom.SelectedDate > DateTo.SelectedDate)
            {
                MessageBox.Show("Wrong date", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            string dateFrom = DateFrom.Text;
            string dateTo = DateTo.Text;
            GetCurrancyDataFromJson(CurrencyBaseRequest);
            GetRateDataFromJson(RateBaseRequest + "?periodicity=0");

        }

        private void GetCurrancyDataFromJson(string req)
        {
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(req).Normalize();
                var currency = JsonConvert.DeserializeObject<List<Currency>>(json);
                foreach (Currency cur in currency)
                {
                    CurGrid.Items.Add(cur);
                }
            }

            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = await client.GetAsync("https://www.nbrb.by/api/exrates/currencies/298");
            //response.EnsureSuccessStatusCode();
            //string responseBody = await response.Content.ReadAsStringAsync();
            //var currencyList = JsonConvert.DeserializeObject<List<Currency>>(responseBody);
            //foreach(Currency cur in currencyList)
            //{
            //    CurGrid.Items.Add(currencyList);
            //}

            //tbRate.Text = "ID = " + rate.Cur_ID + "; Date= " + rate.Date + "; Abbr= " + rate.Cur_Abbreviation;
        }

        private void GetRateDataFromJson(string req)
        {
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(req).Normalize();
                var rates = JsonConvert.DeserializeObject<List<Rate>>(json);
                foreach (Rate rate in rates)
                {
                    RateGrid.Items.Add(rate);
                }
            }
        }               

        private void ExportCurrencyToExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, CurGrid.Items.ToString());
            saveFileDialog.Filter = "Excel file (*.xls)|*.xls";
            //Microsoft.Office.Interop.Excel.Application xlApp;
            //Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            //Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            //object misValue = System.Reflection.Missing.Value;
            //Microsoft.Office.Interop.Excel.Range rangeToHoldHyperlink;
            //Microsoft.Office.Interop.Excel.Range CellInstance;
            //xlApp = new Microsoft.Office.Interop.Excel.Application();
            //xlWorkBook = xlApp.Workbooks.Add(misValue);

            //xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            //xlApp.DisplayAlerts = false;
            ////Dummy initialisation to prevent errors.
            //rangeToHoldHyperlink = xlWorkSheet.get_Range("A1", Type.Missing);
            //CellInstance = xlWorkSheet.get_Range("A1", Type.Missing);

            //for (int i = 0; i < CurGrid.Columns.Count; i++)
            //{
            //    for (int j = 0; j <= CurGrid.Items.Count; j++)
            //    {
            //        xlWorkSheet.Cells[j + 1, i + 1] = ((DataRowView)CurGrid.Items[i]).Row.ItemArray[j].ToString();
            //    }
            //}            
        }

        private void BackToList_Click(object sender, RoutedEventArgs e)
        {
            tbctrl.SelectedIndex = 0;
        }               
    }

    //RequestHandler
    public class RequestHelper
    {
        public const string CurrencyBaseRequest = "https://www.nbrb.by/api/exrates/currencies";
        public const string RateBaseRequest = "https://www.nbrb.by/api/exrates/rates";

        public string RequestCurrencyByID(int cur_id)
        {
            return CurrencyBaseRequest + "//" + cur_id;
        }
        public string RequestRateInPeriod(DateTime DateFrom, DateTime DateTo, int cur_id)
        {
            string req;
            if(DateFrom.Equals(DateTo))
            {
                req = RateBaseRequest + "?ondate=" + DateFrom + "&periodicity=0";
            }
            else
            {
                req = RateBaseRequest + cur_id + "?startDate=" + DateFrom + "&endDate=" + DateTo;
            }
            return req;        }

        
    }
}
