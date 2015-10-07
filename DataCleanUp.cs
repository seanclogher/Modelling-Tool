using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Windows.Forms;//datagridview

namespace ModellingTool
{
    class DataCleanUp
    {


        public DataTable ds { get; set; }

        public DataCleanUp()
        {
   
        }

        public bool datechecs()
        {
            #region Date Checks

            //check that you have less than 365 but more than than 40 historic dates
            //check that there is no data older than 365 days as tis may not be reflective.
            //check that there is no more than a 3 day data gap in the dates in the lists..

            #endregion


            int rowcount = ds.Rows.Count;
            int wdrows;

            //clean the data first for dates greater than 365 from latests day in day set

            if (!(rowcount >= 900))//check that the numbers of records is correct for analysis less than a 800 days more than 30
                datecleanup();
            rowcount = ds.Rows.Count;//recount the rows to insure no issues with deleted records...

            if (!(ds.Columns[0].ColumnName.ToLower().Contains("Date") || ds.Columns[0].DataType.ToString() == "System.DateTime"))
                return false;
            if (rowcount <= 30)//check that the numbers of records is correct for analysis less than a year more than 30
                return false;

            //check that there is no gap greater than two working days for the list of dates....
            for (wdrows = 0; wdrows < (rowcount - 1); wdrows++)
            {
                if (GetWorkingDays(Convert.ToDateTime(ds.Rows[wdrows][0]), Convert.ToDateTime(ds.Rows[(wdrows + 1)][0])) > 2)
                return false; 
            }

            return true;
        }
        public void datecleanup()
        {//aim to remove data from dataset greater than 365 of the latest data in dataset...


            DateTime maxDate = Convert.ToDateTime(
                        ((from DataRow dr in ds.Rows
                          orderby Convert.ToDateTime(dr["Date"]) descending
                          select dr).FirstOrDefault()["Date"])); //find max date...in dataset

            // Get first of next year.
            DateTime mindate = new DateTime(maxDate.Year - 1, maxDate.Month, maxDate.Day);
            ds.Rows.Cast<DataRow>().Where(
                         r => Convert.ToDateTime(r.ItemArray[0]) <= Convert.ToDateTime(mindate)).ToList().ForEach(r => r.Delete());
            ds.AcceptChanges();

        }
        public int GetWorkingDays(DateTime from, DateTime to)
        {//find the number of working days between dates in the list for pca analysis...we don't  allow a gap of more than two working days...
            var dayDifference = (int)to.Subtract(from).TotalDays;
            return Enumerable
                .Range(1, dayDifference)
                .Select(x => from.AddDays(x))
                .Count(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday);
        }
        public void setRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = row.Index + 1;
            }

            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

        }
    }
}
