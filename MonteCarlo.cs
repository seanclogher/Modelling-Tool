using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ModellingTool.MC
{
    class MonteCarlo//setting up
    {
        #region explanation
        //this class is the main class within the forward curve generation process, the methods are:
        //Process - this method initiaties the simulations, dependent on how many times you want to re-evaluate the optimisation,
        //for the thesis work, there is four revaluation time periods, this will require 4000 simulations.
        //Simulate - intiated from the process method, runs the forward curve calculation for each time period a 1000 times, returns each time period as a datatable of 1000 rows 
        //Calculate - intiated from simualte method, calls the MonteCarloEngine class to carry out the actual simulation, returns a value for each maturity to form each row required for the datatable.
        #endregion

        #region variables
        public double[,] PCAvariables { get; set; }//eigen vector values
        public double[] timedeltas { get; set; }
        public DataRow forwardcurvesimulations { get; set; }//initial forward for simulation 
        MonteCarloEngine MCEng = new MonteCarloEngine();//create an instance of the engine to do the running...

        public DataTable ForwardSimulations = new DataTable();
        private DataRow newdataouput { get; set; }

        //create the data tables to store the multipledate simulations
         public List<DataTable> simlist = new List<DataTable>();
        
        private int itr = 1000;//# of iterations for MC
        private int nSteps;//#number of maturities ie months
        private double[] paths;
        private int counter;
        private double[] fcurvearray;
        //
        #endregion

        public MonteCarlo()
        {


        }
        
        public void Process()
        {

            DataTable temp = ForwardSimulations.Clone();

            //find the number of steps i.e number of maturity months...
            nSteps = forwardcurvesimulations.ItemArray.Length;
            paths = new double[nSteps];
            counter = 0;
        

           //populate the dataset with the initial forward curve...you will need to get the correct date also...
            temp.ImportRow(forwardcurvesimulations);
            
            //set this as the initial foward curve
            newdataouput = forwardcurvesimulations;
            fcurvearray = newdataouput.ItemArray.Cast<double>().ToArray();

            //want to do 4 different runs of the this simulation so we need to set up a double loop...
            for(int iTable= 0; iTable< 4; iTable++)
            {

                temp = Simulate(timedeltas[iTable]);

                    simlist.Add(temp);/* = temp*/;//this is updating the datatable...
                    newdataouput = temp.Rows[temp.Rows.Count - 1];
                    fcurvearray = newdataouput.ItemArray.Cast<double>().ToArray();
              
            }  
       
           }

        public DataTable Simulate(Double Timedetlas)
        {
            DataTable tempstore = ForwardSimulations.Clone(); ;
            tempstore.ImportRow(newdataouput);
            
                //start the calculation process 1000 iterations.....
                for (counter = 0; counter < itr; counter++)
                {
                   DataRow tempnull = tempstore.NewRow();

                   Calculate(tempnull,Timedetlas); //each run of the simulation...doing it in steps..
                   tempstore.Rows.Add(tempnull);
                }

                return tempstore;
    
            
        }

        public void Calculate(DataRow tempnull, double timedeltas)
        {
        
            //now we step into the calculations this will be done in MonteCarloEngine...
              // DataTable tempcalc = ForwardSimulations.Clone();
                int posit = 0;
                ///DataRow tempnull = tempcalc.NewRow();//this is to hold th new data as you go...

               //loop through our curve and updae with pca analysis
                for(int i = 0; i< fcurvearray.Length; i++)
                {
                    double matpoint = fcurvearray[i];
                    double[] pcavalues = {PCAvariables[0,i],PCAvariables[1,i],PCAvariables[2,i]};
                    tempnull[posit] = MCEng.MCprocess(matpoint, pcavalues,timedeltas); //do the processing pass in the foward curve value at the point which we are process, this will be the value from this point on the previous curve
                    posit++;
                
                }

                //populate the dataset with the initial forward curve...you will need to get the correct date also...
                fcurvearray = tempnull.ItemArray.Cast<double>().ToArray();  //update the inputs for the next calculation...
        }

    }
}

