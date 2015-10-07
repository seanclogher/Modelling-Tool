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
using System.Windows.Forms.DataVisualization.Charting;
using System.Net;
using ModellingTool.MC;
using ModellingTool.PCA;
using ModellingTool.OPT;


namespace ModellingTool
{
    public partial class frmModellingTool : Form
    {
        #region explanation
        //The form application is the main class for the program, as such we need to initialise all the binding sources and datasets
        //we initials the constraints tab with data from the App.configuration file...
        //There are multiple buttons on the form, button to open a directory search, open an upload an excel file, and to process the data to find the optimisation
        //The btProcessPCA button initiaties code that is split into three sections , PCA, Monte Carlo, and the Optimsiation
        //all data that is returned is linked onto various tabs on the form.
        #endregion

        #region BindingSources
        //set up the binding source connections....

        BindingSource bsdata = new BindingSource();

        public BindingSource Bsdata
        {
            get { return bsdata; }
            set { bsdata = value; }
        }

        BindingSource bsPCA1 = new BindingSource();

        public BindingSource BsPCA1
        {
            get { return bsPCA1; }
            set { bsPCA1 = value; }
        }

        BindingSource bsPCA2 = new BindingSource();

        public BindingSource BsPCA2
        {
            get { return bsPCA2; }
            set { bsPCA2 = value; }
        }
        BindingSource bsPCA3 = new BindingSource();

        public BindingSource BsPCA3
        {
            get { return bsPCA3; }
            set { bsPCA3 = value; }
        }

        BindingSource bsSim4 = new BindingSource();
        public BindingSource BsSim4
        {
            get { return bsSim4; }
            set { bsSim4 = value; }
        }

        BindingSource bsSimData5 = new BindingSource();
        public BindingSource BsSimData5
        {
            get { return bsSimData5; }
            set { bsSimData5 = value; }
        }

        BindingSource bsOptimData6 = new BindingSource();
        public BindingSource BsOptimData6
        {
            get { return bsOptimData6; }
            set { bsOptimData6 = value; }
        }

        BindingSource bsOptimData7 = new BindingSource();
        public BindingSource BsOptimData7
        {
            get { return bsOptimData7; }
            set { bsOptimData7 = value; }
        }

        BindingSource bsOptimData8 = new BindingSource();
        public BindingSource BsOptimData8
        {
            get { return bsOptimData8; }
            set { bsOptimData8 = value; }
        }

        BindingSource bsOptimData9 = new BindingSource();
        public BindingSource BsOptimData9
        {
            get { return bsOptimData9; }
            set { bsOptimData9 = value; }
        }

        BindingSource bsOptimData10 = new BindingSource();
        public BindingSource BsOptimData10
        {
            get { return bsOptimData10; }
            set { bsOptimData10 = value; }
        }

        BindingSource bsOptimData11 = new BindingSource();
        public BindingSource BsOptimData11
        {
            get { return bsOptimData11; }
            set { bsOptimData11 = value; }
        }

        #endregion

        #region Datasets //set up datasets
        DataSet ds = new DataSet();//create for initial excel data
        DataTable tbl = new DataTable();
        DataTable intrinsicoutput = new DataTable();
        DataTable rollingInjectiontable = new DataTable();
        DataTable rollingWithdrawal = new DataTable();
        DataTable rollingPosition = new DataTable();
        List<String> items = new List<String>();//create a list for the series names..
        DataGridViewCheckBoxColumn CBColumn = new DataGridViewCheckBoxColumn();
        List<Optimisation> simlist =  new List<Optimisation>();
        List<Optimisation> simoutputlist = new List<Optimisation>();
        DataCleanUp dataclean = new DataCleanUp();//required for cleaning data..
        VectorMatrixCalc dataconversion = new VectorMatrixCalc();//required for datatable converison
        public SimStore simulations = new SimStore(); //required for simulations gridviews...
        //Make some smuiation objects objects...to hold the differnt simulations...
        private SimulationObjects s1 = new SimulationObjects("Initial Day Simulation");
        private SimulationObjects s2 = new SimulationObjects("Initial + 10 days");
        private SimulationObjects s3 = new SimulationObjects("Initial + 20 days");
        private SimulationObjects s4 = new SimulationObjects("Initial + 30 days");
        Constraints con { get; set; }
        double[,] InjectionVol = new double[12, 2] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
        double[,] WithdrawalVol = new double[12, 2] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };

        #endregion

        public frmModellingTool()
        {
            InitializeComponent();
            txtLocation.Text = ConfigurationManager.AppSettings["txtLocation"];//pre set location of the file to automatically open....
            bsdata.DataSource = this;
          
            con = new Constraints();
            #region constraints to optimisation
            txtMaxCap.DataBindings.Add("Text", con.MaxCap, "");
            txtbxJanIMCap.DataBindings.Add("Text", con.JanImport, "");
            txtbxFebIMCap.DataBindings.Add("Text", con.FebImport, "");
            txtbxMarIMCap.DataBindings.Add("Text", con.MarImport, "");
            txtbxAprIMCap.DataBindings.Add("Text", con.AprImport, "");
            txtbxMayIMCap.DataBindings.Add("Text", con.MayImport, "");
            txtbxJunIMCap.DataBindings.Add("Text", con.JunImport, "");
            txtbxJulIMCap.DataBindings.Add("Text", con.JulImport, "");
            txtbxAugIMCap.DataBindings.Add("Text", con.AugImport, "");
            txtbxSeptIMCap.DataBindings.Add("Text", con.SeptImport, "");
            txtbxOctIMCap.DataBindings.Add("Text", con.OctImport, "");
            txtbxNovIMCap.DataBindings.Add("Text", con.NovImport, "");
            txtbxDecIMCap.DataBindings.Add("Text", con.DecImport, "");
            txtbxJanExpCap.DataBindings.Add("Text", con.JanExport, "");
            txtbxFebExpCap.DataBindings.Add("Text", con.FebExport, "");
            txtbxMarExpCap.DataBindings.Add("Text", con.MarExport, "");
            txtbxAprExpCap.DataBindings.Add("Text", con.AprExport, "");
            txtbxMayExpCap.DataBindings.Add("Text", con.MayExport, "");
            txtbxJunExpCap.DataBindings.Add("Text", con.JunExport, "");
            txtbxJulExpCap.DataBindings.Add("Text", con.JulExport, "");
            txtbxAugExpCap.DataBindings.Add("Text", con.AugExport, "");
            txtbxSeptExpCap.DataBindings.Add("Text", con.SeptExport, "");
            txtbxOctExpCap.DataBindings.Add("Text", con.OctExport, "");
            txtbxNovExpCap.DataBindings.Add("Text", con.NovExport, "");
            txtbxDecExpCap.DataBindings.Add("Text", con.DecExport, "");
            
            //txtbxExportCharges.Text = Convert.ToString(Cons.ExportCharge);
            txtbxExportCharge.DataBindings.Add("Text", con.Injection, "");
            txtbxInjectionCharge.DataBindings.Add("Text", con.Widthdrawl, "");
            txtbxRoIntrinsDays.DataBindings.Add("Text", con.Rollingdays, "");


            ////txtbxMayIMCap.Text = Convert.ToString(Cons.ImportCapacity);
            ////txtMaxCap.Text = Convert.ToString(Cons.ExportCapcity);
            ////txtbxOctIMCap.Text = Convert.ToString(Cons.MaxStorageCap);

            #endregion
        }

        #region Phase1, opening data, updating data
        private void btBrowse_Click(object sender, EventArgs e)
        {//open a browswer to select data....
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    string text = file;
                    size = text.Length;
                    txtLocation.Text = file;
                }
                catch (IOException)
                {
                }
            }

        }
        private void btOpen_Click(object sender, EventArgs e)
        {
            //clear any existing data
            ds.Reset();
            tbl.Reset();
            dgvHistoricForwardCurve.DataSource = null;
            dgvHistoricForwardCurve.Columns.Clear();
            dgvHistoricForwardCurve.Refresh();


            OleDbConnection oledbConn = new OleDbConnection();//initiate the connection
            Boolean myBool = true;//used to check dates etc...

            #region Column Creation

            //create a check box column to bue used in the try section

            CBColumn.HeaderText = "Selection";
            CBColumn.ValueType = typeof(bool);
            //CBColumn.FalseValue = "0";
            //CBColumn.TrueValue = "1";
            CBColumn.Name = "Selection";

            #endregion
            try
            {
                //need to open the data source and then push into a data set...

                string path = txtLocation.Text;

                #region upload from excel explanation
                //firstly check to see if the file is xls or xlsx..
                // explaination of how data is formated <summary>
                // connection string  to work with excel file. HDR=Yes - indicates 
                //    that the first row contains columnnames, not data. HDR=No - indicates 
                //    the opposite. "IMEX=1;" tells the driver to always read "intermixed" 
                //   (numbers, dates, strings etc) data columns as text. 
                #endregion

                if (Path.GetExtension(path) == ".xls")
                {
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
                }
                else if (Path.GetExtension(path) == ".xlsx")
                {
                    oledbConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=2;';");
                }

                oledbConn.Open();//open the connection
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);//write the select code...
                cmd.CommandType = CommandType.Text;//defining the command type
                OleDbDataAdapter adapter = new OleDbDataAdapter();//create a new link between data and dataset


                adapter = new OleDbDataAdapter(cmd);//create a new link between data and dataset
                adapter.Fill(ds);//fill the data set...


                //pass through the table to Dataclean up!
                dataclean.ds = ds.Tables[0];

                ////this is for test puprposess
                //if (ds.Tables[0].Columns.Count > 3)
                //{
                    //rename date column
                    ds.Tables[0].Columns["F1"].ColumnName = "Date";

                    //check the data quality (from a date perspective)
                    myBool = dataclean.datechecs();

                    if (!myBool)
                    {//if boolean to see if we can move on or if there is an error in the code...
                        MessageBox.Show("Error in Data Input, Issue with gaps in working days..");
                        return;
                    }

                    //update grid view
                    Bsdata.DataSource = ds.Tables[0];
                    dgvHistoricForwardCurve.DataSource = Bsdata;
                    dgvHistoricForwardCurve.RowHeadersDefaultCellStyle.Format = "MM/YYYY";//format the headers roow

                    dgvHistoricForwardCurve.AllowUserToAddRows = false;//prevent any new rows being added

                    dgvHistoricForwardCurve.Columns.Insert(0, CBColumn); //insert the tick box column }
                    //create the chart series....
                    #region ChartSetUp

                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        items.Add(dc.ColumnName.ToString());

                    }

                    #endregion

                    //initial setting of the chart...
                    int index = dgvHistoricForwardCurve.Rows.Count - 1;
                    dgvHistoricForwardCurve.Rows[index].SetValues(true);


                //    #region Testing purposessss
                //}
                //else
                //{ //extra for testing purposes..............
                //    Bsdata.DataSource = ds.Tables[0]; dgvHistoricForwardCurve.DataSource = Bsdata;
                //    dgvHistoricForwardCurve.RowHeadersDefaultCellStyle.Format = "MM/YYYY";//format the headers roow
                //    dgvHistoricForwardCurve.Columns.Insert(0, CBColumn); //insert the tick box column }
                //    dgvHistoricForwardCurve.AllowUserToAddRows = false;//prevent any new rows being added
                //    foreach (DataColumn dc in ds.Tables[0].Columns)
                //    {
                //        items.Add(dc.ColumnName.ToString());

                //    }
                //    #endregion
                //}

            }

            // need to catch possible exceptions or just through them...
            catch (Exception ex)
            {


                MessageBox.Show("Incorrect Data fit", "Critical Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            finally
            {
                oledbConn.Close();//close the connection to the excel sheet...
            }
        }
        public void HookUpData(DataSet ds)
        {
            if (txtLocation.Text != ConfigurationManager.AppSettings["txtLocation"])
                txtLocation.Text = txtLocation.Text;
            else txtLocation.Text = ConfigurationManager.AppSettings["txtLocation"];//displaying the data in the text boses on the form, datasource is the binding list Bsparams;
        }
        private void dgvHistoricForwardCurve_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {//first create the dataset of checked boxes
                tbl.Reset();//clear any existing datastet
                tbl = ((DataTable)Bsdata.DataSource).Clone();//Clone structure first

                foreach (DataGridViewRow Row in dgvHistoricForwardCurve.Rows.OfType<DataGridViewRow>())
                {
                    if (Convert.ToBoolean(Row.Cells["Selection"].Value))
                    {
                        tbl.ImportRow(((DataRowView)Row.DataBoundItem).Row);// this.dgvHistoricForwardCurve.Rows[Row.Index].Selected = true;
                    }
                   
                }
                     chartcreator(tbl, items);//update the chart...
            }
        }
        private void dgvHistoricForwardCurve_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            {
                dgvHistoricForwardCurve[0, e.RowIndex].Value = Convert.ToBoolean(dgvHistoricForwardCurve[0, e.RowIndex].Value);
            }
        }
        private void chartcreator(DataTable dschart, List<String> items)
        {//create update the chart....with the fresh dataeries...

            //clear any previous series...
            chrtCurve.Series.Clear();//clear all existing sereies...

            //find the number of rows and columns in dataset..
            int rowCount = dschart.Rows.Count;
            // Get the no. of columns in the first row.
            int colCount = dschart.Columns.Count;


            #region find the min and max of the data
            // minimum and maximum

            int max = Convert.ToInt32(dschart.Rows[0][1]);  // assuming you want the third column (index 2)
            int min = Convert.ToInt32(dschart.Rows[0][1]);  // assuming you want the third column (index 2)

            for (int drows = 0; drows < dschart.Rows.Count; drows++)
            {
                for (int dcols = 1; dcols < dschart.Columns.Count; dcols++)
                {
                    if (max < Convert.ToInt32(dschart.Rows[drows][dcols])) max = Convert.ToInt32(dschart.Rows[drows][dcols]);
                    if (min > Convert.ToInt32(dschart.Rows[drows][dcols])) min = Convert.ToInt32(dschart.Rows[drows][dcols]);
                }
            }
            #endregion

            //populate each requied series

            foreach (DataRow drow in dschart.Rows)
            {
                DateTime date = Convert.ToDateTime(drow[0]);
                var shortDate = date.ToString("yyyy-MM-dd"); 
                chrtCurve.Series.Add(shortDate);//add each row as a series...
                chrtCurve.Series[shortDate].ChartType = SeriesChartType.Line;//set the chart to line
                chrtCurve.ChartAreas[0].AxisY.Maximum = max + 1;
                chrtCurve.ChartAreas[0].AxisY.Minimum = min - 1;
                chrtCurve.ChartAreas[0].AxisY.Interval = 2;
                
                //chrtCurve.ChartAreas[0].AxisY.Format = "0.0000";
               // chrtCurve.ChartAreas[0].AxisY.LabelStyle.Format = "{0.0000}";
    

                //populate each month of the data series
                for (int pointIndex = 0; pointIndex < colCount; pointIndex++)
                {
                    chrtCurve.Series[shortDate].Points.AddXY(items[pointIndex], drow[pointIndex]);
                }
            }

        }
        private void chartdatatable(DataTable dt, Chart Chart1)
        {
            Chart1.Series.Clear();
            Chart1.DataSource = dt;
            string serieName = "";
            int amountofrows = Convert.ToInt32(dt.Rows.Count);
            List<string> xvals = new List<string>();
            List<double> yvals = new List<double>();

            foreach (DataColumn column in dt.Columns)
            {
                xvals.Add(column.ColumnName);

            }
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                serieName = "Simulation" + i;
                Chart1.Series.Add(serieName);
                Chart1.Series[serieName].ChartType = SeriesChartType.Line;
                //       List<double> yvals = new List<double>();
                foreach (DataColumn column in dt.Columns)
                {
                    yvals.Add(Convert.ToDouble(row[column]));
                }
                try//add the newly created sereies...
                {
                    Chart1.Series[serieName].XValueType = ChartValueType.String;
                    Chart1.Series[serieName].YValueType = ChartValueType.Auto;
                    Chart1.Series[serieName].Points.DataBindXY(xvals.ToArray(), yvals.ToArray());
                    yvals.Clear();
                }
                catch (Exception)
                {

                    throw new InvalidOperationException("Kunde inte bind punkterna till Diagrammet");
                }


                i++;
            }


            Chart1.DataBind();
            Chart1.Visible = true;

        }

        
        #endregion

        private void btProcessPCA_Click(object sender, EventArgs e)
        {


            #region link up PCA stuff...
            PCA.PCAinitialCalcs InitialCalcs = new PCA.PCAinitialCalcs(ds.Tables[0]);

            //update grid view for principal components
            BsPCA1.DataSource = InitialCalcs.dtEigenV;
            dgvEigenVectorAnalysis.DataSource = BsPCA1;

            //update grid view for vector matrix
            BsPCA2.DataSource = InitialCalcs.dtEigenM;
            dgvEigenVectors.DataSource = BsPCA2;

            //upate the pca gridview for the pCA datatable
            BsPCA3.DataSource = InitialCalcs.PCAcomp;
            dgvPrincipalComponents.DataSource = BsPCA3;
            dgvPrincipalComponents.AllowUserToAddRows = false;//prevent any new rows being added

            #endregion

            #region Monte Carlo Simulation

            //initialise new MonteCarlo..
            MC.MonteCarlo MCclac = new MC.MonteCarlo();
            //convert the pcatable into an array[]
            MCclac.PCAvariables = dataconversion.arrayconvert(InitialCalcs.PCAcomp);//convert the pca array into a usuable format..
            MCclac.timedeltas = new double[] { 1, 10, 20, 30 };//(double)1, (double)(1 + (con.Rollingdays / 252)), (1 + ((con.Rollingdays * 2) / 252)), (1 + ((con.Rollingdays * 3) / 252)) };
            //select the initial curve...
            MCclac.forwardcurvesimulations = InitialCalcs.initdata.Rows[InitialCalcs.initdata.Rows.Count - 1];//select last row...used a initial curve....
            MCclac.ForwardSimulations = InitialCalcs.initdata.Clone();

            //set of the process
            MCclac.Process();
            s1.Datatab = MCclac.simlist[0];
            s2.Datatab = MCclac.simlist[1];
            s3.Datatab = MCclac.simlist[2];
            s4.Datatab = MCclac.simlist[3];
            simulations.Data.Add(s1);
            simulations.Data.Add(s2);
            simulations.Data.Add(s3);
            simulations.Data.Add(s4);

            BsSim4.DataSource = simulations.Data;
            dgvsimulations.DataSource = BsSim4;
            dgvsimulations.AllowUserToAddRows = false;
            dgvsimulations.AutoGenerateColumns = true;


            BsSimData5.DataSource = BsSim4;
            BsSimData5.DataMember = "Datatab";
            dgvForCurveSim.DataSource = BsSimData5;
            //dgvForCurveSim.DataMember = "Datatab";
            dgvForCurveSim.AllowUserToAddRows = false;
            dgvForCurveSim.AutoGenerateColumns = true;

            //chart bind below for the moment leave commented out...
            //chartdatatable(MCclac.ForwardSimulations, chtSimForwCurve);

            #endregion

            #region optimisation

            #region optimisation code
            //initialise new optimisation....
            OPT.Optimisation Ops = new OPT.Optimisation(con);

            //this is for th intrinsic evaluation....
            //set up the inital curve, pass through to the find the optimisation...
            //you are starting with april ...ending with march....
            var stringArray = InitialCalcs.initdata.Rows[InitialCalcs.initdata.Rows.Count - 1].ItemArray.Cast<double>().ToArray();
            //take april to january 2016-2017 period...
            double[,] curve = new double[12, 2];
            for (int r = 0; r < 12; r++)
            {
                curve[r, 0] = r;
                curve[r, 1] = (double)stringArray[r + 3];
            }

            //compute the optimisation...
            Ops.OptFunc(curve, InjectionVol, WithdrawalVol);//simulation 1 intrinsic...
            simoutputlist.Add(Ops);

            //you need to pass on the open positions so they can be taken into consideration for the next sale,
            //to do this we need to pass through current injections and widthdrawls...
            for (int i = 0; i < 12; i++)
            {
                InjectionVol[i, 0] = i;
                InjectionVol[i, 1] = (double)Ops.Imptable.Rows[0][i];

            }


            for (int i = 0; i < 12; i++)
            {
                WithdrawalVol[i, 0] = i;
                WithdrawalVol[i, 1] = (double)Ops.Withtable.Rows[0][i];

            }

            //collapse the intrinsic model...
            Ops = null;

            //find the four forward curves your going to use...0
            //you have to effective create 1 datatables, populate with the simulations from the four runs....
            // Ops.SimulationResults;//simulation1 instrinsic....
            //we need to initial a new sim results for every run...
            //initialise new optimisation....
            double len = (double)s1.Datatab.Rows.Count;


            for (int si = 0; si < len; si++)
            {
                OPT.Optimisation tempOps = new OPT.Optimisation(con);//create a new instance of the optimisation..
                //run each of the simulations, if one of the revaluations does not add value, exclude it!
                //first find the curve to use...
                int iloop = 0;
                for (int a = 0; a < 4; a++)
                {
                    var tempcurve = MCclac.simlist[a].Rows[si].ItemArray.Cast<double>().ToArray();
                    //take april to january 2016-2017 period...
                    double[,] tempforwardcurve = new double[12, 2];
                    for (int r = 0; r < 12; r++)
                    {
                        tempforwardcurve[r, 0] = r;
                        tempforwardcurve[r, 1] = (double)tempcurve[r + 3];
                    }

                    //
                    tempOps.OptFunc(tempforwardcurve, InjectionVol, WithdrawalVol);//carry out the optimisation....


                    //check to see if the new simulation adds value exclude it if it doesn't

                    double resutl = Convert.ToDouble(tempOps.Proftable.Rows[a - iloop]["Total"]);
                    if (a > 0 && resutl < Convert.ToDouble(tempOps.Proftable.Rows[(a - iloop) - 1]["Total"]))
                    {//if the new optimisation is worse than the previous delete the optimisation...
                        tempOps.Proftable.Rows[a - iloop].Delete();
                        tempOps.ForCurvetable.Rows[a - iloop].Delete();
                        tempOps.Withtable.Rows[a - iloop].Delete();
                        tempOps.Imptable.Rows[a - iloop].Delete();
                        tempOps.Positiontable.Rows[a - iloop].Delete();
                        iloop++;
                    }
                    else
                    {
                        // {};
                        //  simoutputlist.Add(tempOps);
                        //to do this we need to pass through current injections and widthdrawls...
                        for (int i = 0; i < 12; i++)
                        {
                            InjectionVol[i, 0] = i;
                            InjectionVol[i, 1] = (double)tempOps.Imptable.Rows[a - iloop][i];

                        }
                        for (int i = 0; i < 12; i++)
                        {
                            WithdrawalVol[i, 0] = i;
                            WithdrawalVol[i, 1] = (double)tempOps.Withtable.Rows[a - iloop][i];

                        }
                    }
                }
                simoutputlist.Add(tempOps);
            }

            //need to create the optimisation output....
            //Review the intrinsic...
            double highestval = 0;
            int datatableno = 0;
            double rownumb = 0;
            double rownumcalc = 0;
            for (int ti = 0; ti < simoutputlist.Count; ti++)
            {
                rownumcalc = 0;
                foreach (DataRow dr in simoutputlist[ti].Proftable.Rows)
                {

                    double currentval = Convert.ToDouble(dr["Total"]);
                    if (highestval < currentval)
                    {
                        datatableno = ti;
                        rownumb = rownumcalc;
                        highestval = currentval;
                    }
                    rownumcalc++;
                }
            }


            intrinsicoutput = simoutputlist[0].Positiontable.Clone();
            //you need to create a datatable to hold injections withdrawals and profit for intrinsic, 
            //also create three tables to hold injections , withdrawals and prositions for rolling...with reference to each revaluation period...
            

            #endregion

            #region binding sources optimisations..
            //update grid view for  intrinsic injections grid view...
            BsOptimData6.DataSource = intrinsicoutput;//simoutputlist[0].Imptable;
            dgvIntInjections.DataSource = BsOptimData6;
            dgvIntInjections.AllowUserToAddRows = false;//prevent any new rows being added

            //update grid view for  intrinsic Withdrawals grid view...
            BsOptimData7.DataSource = simoutputlist[0].Withtable;
            dgvIntrinsicWithdrawals.DataSource = BsOptimData7;
            dgvIntrinsicWithdrawals.AllowUserToAddRows = false;//prevent any new rows being added

            //upate the  gridview intrinsic profit/loss
            BsOptimData8.DataSource = simoutputlist[0].Proftable;
            dgvIntrinsicProfit.DataSource = BsOptimData8;
            dgvIntrinsicProfit.AllowUserToAddRows = false;//prevent any new rows being added

            //update grid view for rolling intrinsic injections grid view...
            BsOptimData9.DataSource = simoutputlist[datatableno].Imptable;
            dgvRolntrinsicInjection.DataSource = BsOptimData9;
            dgvRolntrinsicInjection.AllowUserToAddRows = false;//prevent any new rows being added

            //update grid view for rolling intrinsic Withdrawals grid view...
            BsOptimData10.DataSource = simoutputlist[datatableno].Withtable;
            dgvRoIntrinsicWithdrawal.DataSource = BsOptimData10;
            dgvRoIntrinsicWithdrawal.AllowUserToAddRows = false;//prevent any new rows being added

            //upate the gridview rolling intrinsic profit/loss
            BsOptimData11.DataSource = simoutputlist[datatableno].Proftable;
            dgvRolntrinsicProfit.DataSource = BsOptimData11;
            dgvRolntrinsicProfit.AllowUserToAddRows = false;//prevent any new rows being added
            #endregion
            #endregion
        }


    }

}
