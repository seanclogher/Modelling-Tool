using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Services;
using System.Data;


namespace ModellingTool.OPT
{
    public class Optimisation
    {
        #region explanation
        //The optimisation class provides the optimisation of the storage facitlity for each simulated forward curve, this information is then stored and summarised on the window from, 
        //the intrinsic optimisation uses the latest forward curve, while the rolling intrinsic optimise all the simuated forward curves.
        //the class can be broken into serveral components/steps
        //step1. generate the dataset, with every new optimisation object you need to set up the dataset as requried.
        //step2. populate the spread matrix for the prices
        //step3. populate the decision varialbes - this will hold the flwo informaiton
        //step4. populate the constraints
        //step5. convert the above in TERMS
        //step6. specify and generate the solution
        //step7. fill the datset create in step1 with the solution data.
        #endregion
        #region variables

        public DataTable ForCurvetable = new DataTable("ForwardCurve");
            public DataSet SimulationResults = new DataSet();
            public DataTable Withtable = new DataTable("Withdrawals");
            public DataTable Imptable = new DataTable("Injections");
            public DataTable Proftable = new DataTable("Profit");
            public DataTable Positiontable = new DataTable("Positions");
            public Constraints con { get; set; }

        #endregion
            
        public Optimisation(Constraints cons)
            {

                #region Fill DataSet: intialising the dataset to store key results..
                con = cons;
                SimulationResults.Tables.Add(ForCurvetable);
                ForCurvetable.Columns.Add("Apr", typeof(double));
                ForCurvetable.Columns.Add("May", typeof(double));
                ForCurvetable.Columns.Add("Jun", typeof(double));
                ForCurvetable.Columns.Add("Jul", typeof(double));
                ForCurvetable.Columns.Add("Aug", typeof(double));
                ForCurvetable.Columns.Add("Sept", typeof(double));
                ForCurvetable.Columns.Add("Oct", typeof(double));
                ForCurvetable.Columns.Add("Nov", typeof(double));
                ForCurvetable.Columns.Add("Dec", typeof(double));
                ForCurvetable.Columns.Add("Jan", typeof(double));
                ForCurvetable.Columns.Add("Feb", typeof(double));
                ForCurvetable.Columns.Add("Mar", typeof(double));

                SimulationResults.Tables.Add(Withtable);
                Withtable.Columns.Add("Apr", typeof(double));
                Withtable.Columns.Add("May", typeof(double));
                Withtable.Columns.Add("Jun", typeof(double));
                Withtable.Columns.Add("Jul", typeof(double));
                Withtable.Columns.Add("Aug", typeof(double));
                Withtable.Columns.Add("Sept", typeof(double));
                Withtable.Columns.Add("Oct", typeof(double));
                Withtable.Columns.Add("Nov", typeof(double));
                Withtable.Columns.Add("Dec", typeof(double));
                Withtable.Columns.Add("Jan", typeof(double));
                Withtable.Columns.Add("Feb", typeof(double));
                Withtable.Columns.Add("Mar", typeof(double));

                SimulationResults.Tables.Add(Imptable);

                Imptable.Columns.Add("Apr", typeof(double));
                Imptable.Columns.Add("May", typeof(double));
                Imptable.Columns.Add("Jun", typeof(double));
                Imptable.Columns.Add("Jul", typeof(double));
                Imptable.Columns.Add("Aug", typeof(double));
                Imptable.Columns.Add("Sept", typeof(double));
                Imptable.Columns.Add("Oct", typeof(double));
                Imptable.Columns.Add("Nov", typeof(double));
                Imptable.Columns.Add("Dec", typeof(double));
                Imptable.Columns.Add("Jan", typeof(double));
                Imptable.Columns.Add("Feb", typeof(double));
                Imptable.Columns.Add("Mar", typeof(double));

                SimulationResults.Tables.Add(Positiontable);
                Positiontable.Columns.Add("Apr", typeof(double));
                Positiontable.Columns.Add("May", typeof(double));
                Positiontable.Columns.Add("Jun", typeof(double));
                Positiontable.Columns.Add("Jul", typeof(double));
                Positiontable.Columns.Add("Aug", typeof(double));
                Positiontable.Columns.Add("Sept", typeof(double));
                Positiontable.Columns.Add("Oct", typeof(double));
                Positiontable.Columns.Add("Nov", typeof(double));
                Positiontable.Columns.Add("Dec", typeof(double));
                Positiontable.Columns.Add("Jan", typeof(double));
                Positiontable.Columns.Add("Feb", typeof(double));
                Positiontable.Columns.Add("Mar", typeof(double));

                SimulationResults.Tables.Add(Proftable);

                Proftable.Columns.Add("Apr", typeof(double));
                Proftable.Columns.Add("May", typeof(double));
                Proftable.Columns.Add("Jun", typeof(double));
                Proftable.Columns.Add("Jul", typeof(double));
                Proftable.Columns.Add("Aug", typeof(double));
                Proftable.Columns.Add("Sept", typeof(double));
                Proftable.Columns.Add("Oct", typeof(double));
                Proftable.Columns.Add("Nov", typeof(double));
                Proftable.Columns.Add("Dec", typeof(double));
                Proftable.Columns.Add("Jan", typeof(double));
                Proftable.Columns.Add("Feb", typeof(double));
                Proftable.Columns.Add("Mar", typeof(double));
                Proftable.Columns.Add("Total", typeof(double));

                #endregion

            }

            public void OptFunc(double[,] Forcurve,double[,]injectionVol,double[,]withdrawal)
            {
                
                double[,] PriceArray =  Forcurve;
                double[,] InjectionVol = injectionVol;
                double[,] Withdrawal = withdrawal;

                //is a combination from the price spread array....
                double[,] monthspread = new double[12, 12];
                double results = 0;
                for (int row = 0; row < PriceArray.GetLength(0); row++)
                {

                    int sprow = row; int i = 1;

                    for (int col = sprow; col < PriceArray.GetLength(0) - 1; col++)
                    {
                        results = Math.Round(((PriceArray[row + i, 1] - con.Widthdrawl) - (PriceArray[row, 1] + con.Injection)), 5);
                        monthspread[row, sprow + 1] = results;

                        sprow++;
                        i++;
                    };

                }

                Term goal;
                Term[,] ty;
                Term[,] tv;

               
                SolverContext context = SolverContext.GetContext();             // Get context environment
                Model model = context.CreateModel();                            // Create a new model

                #region decision and constraints
                //need 12 decisions for every month remaining in the year......
                Decision I1 = new Decision(Domain.RealNonnegative, "I1");       
                Decision W1 = new Decision(Domain.RealNonnegative, "W1");      
                Decision I2 = new Decision(Domain.RealNonnegative, "I2");       
                Decision I3 = new Decision(Domain.RealNonnegative, "I3");    
                Decision I4 = new Decision(Domain.RealNonnegative, "I4");       
                Decision I5 = new Decision(Domain.RealNonnegative, "I5");       
                Decision I6 = new Decision(Domain.RealNonnegative, "I6");       
                Decision I7 = new Decision(Domain.RealNonnegative, "I7");       
                Decision I8 = new Decision(Domain.RealNonnegative, "I8");       
                Decision I9 = new Decision(Domain.RealNonnegative, "I9");      
                Decision I10 = new Decision(Domain.RealNonnegative, "I10");      
                Decision I11 = new Decision(Domain.RealNonnegative, "I11");       
                Decision I12 = new Decision(Domain.RealNonnegative, "I12");      
                Decision W2 = new Decision(Domain.RealNonnegative, "W2");       
                Decision W3 = new Decision(Domain.RealNonnegative, "W3");      
                Decision W4 = new Decision(Domain.RealNonnegative, "W4");     
                Decision W5 = new Decision(Domain.RealNonnegative, "W5");       
                Decision W6 = new Decision(Domain.RealNonnegative, "W6");      
                Decision W7 = new Decision(Domain.RealNonnegative, "W7");       
                Decision W8 = new Decision(Domain.RealNonnegative, "W8");      
                Decision W9 = new Decision(Domain.RealNonnegative, "W9");       
                Decision W10 = new Decision(Domain.RealNonnegative, "W10");      
                Decision W11 = new Decision(Domain.RealNonnegative, "W11");       
                Decision W12 = new Decision(Domain.RealNonnegative, "W12");
                model.AddDecisions(I1, I2, I3, I4, I5, I6, I7, I8, I9, I10, I11, I12, W1, W2, W3, W4, W5, W6, W7, W8, W9, W10, W11, W12);                                 // Add these to the model (this is where the outputs will be stored)

                model.AddConstraints("limits",
                    //monthly injection withdrawl constraints
                   W1 + Withdrawal[9,1] <= con.JulExport,//13333333.2,
                   I1 + InjectionVol[9, 1] == con.JanImport,//0
                   W2 + Withdrawal[10, 1] <= con.FebExport,//11999999.88,
                   I2 + InjectionVol[10, 1] == con.FebImport,//0,
                   W3 + Withdrawal[11, 1] <= con.MarExport,//5333333.28,
                   I3 + InjectionVol[11, 1] == con.MarImport,//0,
                   W4 + Withdrawal[0,1] == con.AprExport,//0,
                   I4 + InjectionVol[0, 1] == con.AprImport,//0,
                   W5 + Withdrawal[1, 1] == con.MayExport,//0,
                   I5 + InjectionVol[1, 1]<= con.MayImport,//3000000,
                   W6 + Withdrawal[2, 1] == con.JunExport,//0,
                   I6 + InjectionVol[2, 1] <= con.JunImport,//16800000,
                   W7 + Withdrawal[3, 1] == con.JulExport,//0,
                   I7 + InjectionVol[3, 1] <= con.JulImport,//16800000,
                   W8 + Withdrawal[4, 1] == con.AugExport,//0,
                   I8 + InjectionVol[4, 1] <= con.AugImport,//12600000,
                   W9 + Withdrawal[5, 1] == con.SeptExport,//0,
                   I9 + InjectionVol[5, 1] <= con.SeptImport,//10800000,
                   W10 + Withdrawal[6, 1] <= con.OctExport,//6000000,
                   I10 + InjectionVol[6, 1] == con.OctImport,//0,
                   W11 + Withdrawal[7,1] <= con.NovExport,//6000000,
                   I11 + InjectionVol[7, 1] == con.NovImport,//0,
                   W12 + Withdrawal[8, 1] <= con.DecExport,//17333333,
                   I12 + InjectionVol[8, 1] == con.DecImport,//0,
                    
                   //maximum capacity constraints...
                   I4 -  - W4 <= con.MaxCap,
                   I4 - W4 + I5 - W5 <= con.MaxCap,
                   I4 - W4 + I5 - W5 + I6 - W6 <= con.MaxCap,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 <= con.MaxCap,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 <= con.MaxCap,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 <= con.MaxCap,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 + I10 - W10 <= con.MaxCap,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 + I10 - W10 + I11 - W11 <= con.MaxCap,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 + I10 - W10 + I11 - W11 + I12 - W12 + I1 - W1 <= con.MaxCap,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 + I10 - W10 + I11 - W11 + I12 - W12 + I1 - W1 + I2 - W2 <= con.MaxCap,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 + I10 - W10 + I11 - W11 + I12 - W12 + I1 - W1 + I2 - W2 + I3 - W3 <= con.MaxCap,
                    //minimum capacity constraints
                    //you need to take into account any volumes currently in storage...
                   I4 - W4 >= 0,
                   I4 - W4 + I5 - W5 >= 0,
                   I4 - W4 + I5 - W5 + I6 - W6 >= 0,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 >= 0,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 >= 0,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 >= 0,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 + I10 - W10 >= 0,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 + I10 - W10 + I11 - W11 >= 0,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 + I10 - W10 + I11 - W11 + I12 - W12 >= 0,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 + I10 - W10 + I11 - W11 + I12 - W12 + I1 - W1 >= 0,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 + I10 - W10 + I11 - W11 + I12 - W12 + I1 - W1 + I2 - W2 >= 0,
                   I4 - W4 + I5 - W5 + I6 - W6 + I7 - W7 + I8 - W8 + I9 - W9 + I10 - W10 + I11 - W11 + I12 - W12 + I1 - W1 + I2 - W2 + I3 - W3 == 0
                 );
                
                #endregion
               
                ty = matrix(monthspread);
                tv = new Term[,] { { (I4 - W4), (I5 - W5), (I6 - W6), (I7 - W7), (I8 - W8), (I9 - W9), (I10 - W10), (I11 - W11), (I12 - W12), (I1 - W1), (I2 - W2), (I3 - W3) } };

                //to create the goal we need to find the volumes for each month, if injection greater than 
                //withdrawals vol is positive vica versa, then multiply by spread and reverse sign to find profit, which is what we want to maximise
                //goal = matMult(matSubtract(tx, tw), ty)[0, 0];
                goal = matMult(tv, ty)[0, 0];
                model.AddGoal("goal", GoalKind.Minimize, goal);

                // Specifying the IPM solver, as we have a quadratic goal 
                Solution solution = context.Solve(new InteriorPointMethodDirective());


                //Profit calculation section, you need to store decisions and profit figures......

                //  DataSet SimulationResults = new DataSet();
                #region Fill DataSet

                DataRow rowinfo = Withtable.NewRow();

                rowinfo[0] = Convert.ToDouble(W4.GetDouble());
                rowinfo[1] = Convert.ToDouble(W5.GetDouble());
                rowinfo[2] = Convert.ToDouble(W6.GetDouble());
                rowinfo[3] = Convert.ToDouble(W7.GetDouble());
                rowinfo[4] = Convert.ToDouble(W8.GetDouble());
                rowinfo[5] = Convert.ToDouble(W9.GetDouble());
                rowinfo[6] = Convert.ToDouble(W10.GetDouble());
                rowinfo[7] = Convert.ToDouble(W11.GetDouble());
                rowinfo[8] = Convert.ToDouble(W12.GetDouble());
                rowinfo[9] = Convert.ToDouble(W1.GetDouble());
                rowinfo[10] = Convert.ToDouble(W2.GetDouble());
                rowinfo[11] = Convert.ToDouble(W3.GetDouble());
                SimulationResults.Tables[1].Rows.Add(rowinfo);

                rowinfo = Imptable.NewRow();
                rowinfo[0] = Convert.ToDouble(I4.GetDouble());
                rowinfo[1] = Convert.ToDouble(I5.GetDouble());
                rowinfo[2] = Convert.ToDouble(I6.GetDouble());
                rowinfo[3] = Convert.ToDouble(I7.GetDouble());
                rowinfo[4] = Convert.ToDouble(I8.GetDouble());
                rowinfo[5] = Convert.ToDouble(I9.GetDouble());
                rowinfo[6] = Convert.ToDouble(I10.GetDouble());
                rowinfo[7] = Convert.ToDouble(I11.GetDouble());
                rowinfo[8] = Convert.ToDouble(I12.GetDouble());
                rowinfo[9] = Convert.ToDouble(I1.GetDouble());
                rowinfo[10] = Convert.ToDouble(I2.GetDouble());
                rowinfo[11] = Convert.ToDouble(I3.GetDouble());
                SimulationResults.Tables[2].Rows.Add(rowinfo);

                rowinfo = Proftable.NewRow();
                rowinfo[0] = (Convert.ToDouble(W4.GetDouble()) - Convert.ToDouble(I4.GetDouble())) * PriceArray[0, 1]/100;
                rowinfo[1] = (Convert.ToDouble(W5.GetDouble()) - Convert.ToDouble(I5.GetDouble())) * PriceArray[1, 1]/100;
                rowinfo[2] = (Convert.ToDouble(W6.GetDouble()) - Convert.ToDouble(I6.GetDouble())) * PriceArray[2, 1]/100;
                rowinfo[3] = (Convert.ToDouble(W7.GetDouble()) - Convert.ToDouble(I7.GetDouble())) * PriceArray[3, 1]/100;
                rowinfo[4] = (Convert.ToDouble(W8.GetDouble()) - Convert.ToDouble(I8.GetDouble())) * PriceArray[4, 1]/100;
                rowinfo[5] = (Convert.ToDouble(W9.GetDouble()) - Convert.ToDouble(I9.GetDouble())) * PriceArray[5, 1]/100;
                rowinfo[6] = (Convert.ToDouble(W10.GetDouble()) - Convert.ToDouble(I10.GetDouble())) * PriceArray[6, 1]/100;
                rowinfo[7] = (Convert.ToDouble(W11.GetDouble()) - Convert.ToDouble(I11.GetDouble())) * PriceArray[7, 1]/100;
                rowinfo[8] = (Convert.ToDouble(W12.GetDouble()) - Convert.ToDouble(I12.GetDouble())) * PriceArray[8, 1]/100;
                rowinfo[9] = (Convert.ToDouble(W1.GetDouble()) - Convert.ToDouble(I1.GetDouble())) * PriceArray[9, 1]/100;
                rowinfo[10] = (Convert.ToDouble(W2.GetDouble()) - Convert.ToDouble(I2.GetDouble())) * PriceArray[10, 1]/100;
                rowinfo[11] = (Convert.ToDouble(W3.GetDouble()) - Convert.ToDouble(I3.GetDouble())) * PriceArray[11, 1]/100;
                rowinfo[12] = ((double)rowinfo[0] + (double)rowinfo[1] + (double)rowinfo[2] + (double)rowinfo[3] + (double)rowinfo[4] + (double)rowinfo[5] + (double)rowinfo[6] + (double)rowinfo[7] + (double)rowinfo[8] + (double)rowinfo[9] + (double)rowinfo[10] + (double)rowinfo[11]);
                SimulationResults.Tables[4].Rows.Add(rowinfo);

                rowinfo = ForCurvetable.NewRow();
                rowinfo[0] = PriceArray[0, 1];
                rowinfo[1] = PriceArray[1, 1];
                rowinfo[2] = PriceArray[2, 1];
                rowinfo[3] = PriceArray[3, 1];
                rowinfo[4] = PriceArray[4, 1];
                rowinfo[5] = PriceArray[5, 1];
                rowinfo[6] = PriceArray[6, 1];
                rowinfo[7] = PriceArray[7, 1];
                rowinfo[8] = PriceArray[8, 1];
                rowinfo[9] = PriceArray[9, 1];
                rowinfo[10] = PriceArray[10, 1];
                rowinfo[11] = PriceArray[11, 1];
                SimulationResults.Tables[0].Rows.Add(rowinfo);

                rowinfo = Positiontable.NewRow();
                rowinfo[0] = (Convert.ToDouble(I4.GetDouble()) - Convert.ToDouble(W4.GetDouble()));
                rowinfo[1] = (double)rowinfo[0] + (Convert.ToDouble(I5.GetDouble()) - Convert.ToDouble(W5.GetDouble()));
                rowinfo[2] = (double)rowinfo[1] + (Convert.ToDouble(I6.GetDouble()) - Convert.ToDouble(W6.GetDouble()));
                rowinfo[3] = (double)rowinfo[2] + (Convert.ToDouble(I7.GetDouble()) - Convert.ToDouble(W7.GetDouble()));
                rowinfo[4] = (double)rowinfo[3] + (Convert.ToDouble(I8.GetDouble()) - Convert.ToDouble(W8.GetDouble()));
                rowinfo[5] = (double)rowinfo[4] + (Convert.ToDouble(I9.GetDouble()) - Convert.ToDouble(W9.GetDouble()));
                rowinfo[6] = (double)rowinfo[5] + (Convert.ToDouble(I10.GetDouble()) - Convert.ToDouble(W10.GetDouble()));
                rowinfo[7] = (double)rowinfo[6] + (Convert.ToDouble(I11.GetDouble()) - Convert.ToDouble(W11.GetDouble()));
                rowinfo[8] = (double)rowinfo[7] + (Convert.ToDouble(I12.GetDouble()) - Convert.ToDouble(W12.GetDouble()));
                rowinfo[9] = (double)rowinfo[8] + (Convert.ToDouble(I1.GetDouble()) - Convert.ToDouble(W1.GetDouble()));
                rowinfo[10] = (double)rowinfo[9] + (Convert.ToDouble(I2.GetDouble()) - Convert.ToDouble(W2.GetDouble()));
                rowinfo[11] = (double)rowinfo[10] + (Convert.ToDouble(I3.GetDouble()) - Convert.ToDouble(W3.GetDouble()));
                SimulationResults.Tables[3].Rows.Add(rowinfo);
                #endregion
                
              //  System.Diagnostics.Process.GetCurrentProcess().Kill();
                context.ClearModel();
            }

            static Term[,] matrix(Double[,] m)
            {
                int rows = m.GetLength(0);
                int cols = m.GetLength(1);
                Term[,] r = new Term[rows, cols];

                for (int row = 0; row < rows; row++)
                    for (int col = 0; col < cols; col++)
                        r[row, col] = m[row, col];

                return r;
            }
            static Term[,] array(Double[] m)
            {
                int rows = m.GetLength(0);

                Term[,] r = new Term[rows, 1];

                for (int row = 0; row < rows; row++)
                    r[row, 0] = m[row];

                return r;
            }
            static Term[,] matMult(Term[,] a, Term[,] b)
            {
                int rows = a.GetLength(0);
                int cols = b.GetLength(1);
                Term[,] r = new Term[rows, cols];

                for (int row = 0; row < rows; row++)
                    for (int col = 0; col < cols; col++)
                    {
                        r[row, col] = 0;
                        for (int k = 0; k < a.GetLength(1); k++)
                        {
                            r[row, col] += a[row, k] * b[k, col];
                        }
                    }

                return r;
            }
            public double[,] forcurvExtrac(DataRow forcurve)

            {
                double[,] temp = null;
        
                return temp;
            
            }
        }
} 
