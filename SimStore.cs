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
{
    public class SimStore
    {//used for the storing the simulations
        private BindingList<SimulationObjects> data = new BindingList<SimulationObjects>();

        public BindingList<SimulationObjects> Data
        {
            get { return data; }
            set { data = value; }
        }

        public SimStore()
        {
        }
        
    }
}
