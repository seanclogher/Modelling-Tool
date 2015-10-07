using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Configuration;

namespace ModellingTool.MC
{//used to store the multiple time periods for the simulations, i.e. 4 time periods each conatining 1000 simulations

    public class SimulationObjects
    {
           private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

       
        private DataTable datatab = new DataTable();
        public DataTable Datatab
        {
            get { return datatab; }
            set {datatab = value;}
        }

        public SimulationObjects()
        {
            //default constructor
        }
      
        public SimulationObjects(string day):this()
        {
            Name = day;

        }

    }
}
