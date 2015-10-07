using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace ModellingTool.PCA
{
    class PCAinitialCalcs
    {
        #region Class Aims
        //the aim of this is class is the prep for PCA analysis, 
        //using Math.Net find the eigenvalues and eigenvectors, use these to find the principal components.
        //set up datatables to bind to the datagrid views.
        //
        #endregion

        #region variables
        double[,] initmatrix;
        public DataTable initdata { get; set; }
        VectorMatrixCalc calclass = new VectorMatrixCalc();
        double[,] eigenvalues { get; set; }
        public double[] eigenvector { get; set; }
        public DataTable dtEigenV = new DataTable();
        public DataTable dtEigenM = new DataTable();
        public DataTable PCAcomp = new DataTable();
        #endregion

        public PCAinitialCalcs(DataTable initialData)
        {

            initdata = cleandataset(initialData);///clean up DataSet for calculations...
            initmatrix = calclass.arrayconvert(initdata);

            #region explanation of math.numerics covariance calcuation....
            //using alternative method...http://stattrek.com/matrix-algebra/covariance-matrix.aspx?Tutorial=matrix
            ////find the covariance matrix using Math.Net.Statistics  
            //the solution for the covariance matrix is a three step process...
            //First, we transform the raw scores in matrix A to deviation scores in matrix a, using the transformation formula
            //      a = A - 11'A ( 1 / n )
            //A deviation score is the difference between a raw score and the mean. di = xi - x
            //Let X be an r x c matrix holding raw scores; and let x be the corresponding r x c matrix holding deviation scores.
            //When transforming raw scores from X into deviation scores for x, we often want to compute deviation scores separately within columns.
            //xrc = Xrc - Xc
            //where xrc is the deviation score from row r and column c of matrix x
            //   Xrc is the raw score from row r and column c of matrix X
            //   Xc is the mean score, based on all r scores from column c of matrix X
            //
            //To transform the raw scores from matrix X into deviation scores for matrix x, we use this matrix equation.
            //x = X - 11'X ( 1'1 )-1 = X - 11'X ( 1 / r )
            //where
            //     1 is an r x 1 column vector of ones
            //     x is an r x c matrix of deviation scores: x11, x12, . . . , xrc
            //     X is an r x c matrix of raw scores: X11, X12, . . . , Xrc
            //step 2 Then, to find the deviation score sums of squares matrix, we compute a'a
            //finally step 3 to create the variance-covariance matrix, we divide each element in the deviation sum of squares matrix by n
            #endregion

            //Step 1:First, we transform the raw scores in matrix A to deviation scores in matrix a, using the 
            var M = Matrix<double>.Build;
            Matrix<double> A = M.DenseOfArray(initmatrix);
            Vector<double> Ones = Vector<double>.Build.Dense(A.RowCount,1);//raw scores...
        
            Matrix<double> onetranspose = M.SparseOfColumnVectors(Ones);
            double n = A.RowCount;

            Matrix<double> rawscores = M.Dense(A.RowCount, A.RowCount, 1);
            Matrix<double> stepA = rawscores.Multiply(A);
            Matrix<double> stepB = stepA.Divide(n);
            Matrix<double> rawscoretransoformMatrix = A.Subtract(stepB);

            //step2  Then, to find the deviation score sums of squares matrix, we compute a'a, as shown below.

            Matrix<double> step2a = rawscoretransoformMatrix.Transpose();
            Matrix<double> step2b = step2a.Multiply(rawscoretransoformMatrix);

            //step3  to create the variance-covariance matrix, we divide each element in the deviation sum of squares matrix by n
            Matrix<double> covarinacematrix = step2b.Divide(n);

            //Find EigenValues and Vectors....these are returned ordered....
            Evd<double> eigen = covarinacematrix.Evd();
            Vector<Complex> eigenvector = eigen.EigenValues;
            Matrix<double> eigenvalueMat = eigen.EigenVectors;

            //populate the prinicipal components grid view...slightly more complicated...
            //principal components  σ_i (t,t+τ_j )=υ_ji √(λ_i )
            
            //first convert the Matrixs into datasets to be used,these will be displayed in order of signficance.......
            dtEigenV = calclass.tableconvert(eigenvector);
            double[,] tempmatrixes = eigenvalueMat.ToArray();//you will need to invert the matrix to match it the inverted matrixes!
            tempmatrixes = calclass.invertmatrix(tempmatrixes);
            dtEigenM = calclass.tableconvert(tempmatrixes);//calclass.tableconvert(eigenvalueMat.ToArray());


            //step 4: find the principal components next...
            pcatable(3);



        }

       private DataTable cleandataset(DataTable initialData)
        {//aim is to clean up the data to allow it to be easily worked on in calculations, remove date column..
            DataTable dataresults = initialData.Copy();
            if (dataresults.Columns.Count <= 3) { dataresults.Columns.Remove("F1"); } else { dataresults.Columns.Remove("Date"); }
           dataresults.AcceptChanges();
            return dataresults;
        }//clean the dataset
       
        public void pcatable(int compents)
        {
           //we are selecting three principal componentss...
            int pcomponts = compents;


            //calculate the Principal components table...
            //clone the PCA dataset off vector dataset
            PCAcomp = dtEigenM.Clone();


           //there is an issue with the c# code supplementing in the mathematica results
            //double[,] mathemticaresults = {{48.9464, 	0.192127, 	0.425903, 	0.0786216, 	0.874816, 	0.170827, 	0.856863, 	0.540663, 	0.502095, 	0.270175, 	0.0252588, 	0.280004, 	0.0167778, 	0.212924, 	0.0281066, 	0.198029, 	0.125869, 	0.00545993, 	0.0420015, 	0.0211407, 	0.0131051, 	0.0101825, 	0.0299716, 	0.005143367} ,
            //                              {49.5101, 	0.0926577, 	0.536289, 	-0.0191826, 	0.803702, 	0.0395924, 	0.662079, 	0.534619, 	0.476322, 	0.21173, 	-0.0361671, 	0.285039, 	-0.0395457, 	0.232922, 	0.08726, 	0.171834, 	0.088185, 	-0.0252003, 	0.0317773, 	0.0202661, 	0.0151452, 	-0.00222893, 	0.0278938, 	-0.0000363305} ,
            //                    {50.1383, 	0.395712, 	0.296622, 	-0.189981, 	1.02647, 	-0.12505, 	0.442367, 	0.504851, 	0.496631, 	0.0835673, 	0.0799085, 	0.235213, 	-0.0131059, 	0.127787, 	0.100764, 	0.138836, 	0.0353083, 	0.0173735, 	0.000669638, 	0.00472186, 	0.0212482, 	-0.0355784, 	-0.0094382, 	0.012387}};

        
            ////update each risk volatility with the appriopriate number of time periods....
            //add each row of the array into the datatable.
            //first loop through the risk function and population each of the columns representing the maturities..
            for (int riskfun = 0; riskfun < pcomponts; riskfun++)
            {
                //populate each column...
                DataRow newRow = PCAcomp.NewRow();
                for (int colIndex = 0; colIndex < dtEigenM.Columns.Count; colIndex++)
                {
                    newRow[colIndex] = (double)dtEigenM.Rows[colIndex][riskfun] * Math.Sqrt((Convert.ToDouble(dtEigenV.Rows[colIndex][0])));
                    //newRow[colIndex] = mathemticaresults[riskfun, colIndex];

                }
                PCAcomp.Rows.Add(newRow);
                newRow = PCAcomp.NewRow();

            }


            #region create the eigenvector cumalative table
            //rename column to Eigen Values
            dtEigenV.Columns[0].ColumnName = "Eigen Values";


            //add a row header column 
            setRowNumber(dtEigenV);

            //add the proportions column...
            dtEigenV.Columns.Add("Proportions", typeof(Double));
            eigproportions(dtEigenV, "Eigen Values");

            //add the cumulative column...
            dtEigenV.Columns.Add("Cumulative Proportion", typeof(Double));
            eigcumulative(dtEigenV, "Proportions", "Cumulative Proportion");
            #endregion

        }
        
        private void eigproportions(DataTable dt, String ColumnHead)
        {
            //find the perentage of total fo reach eigen vector
            string columnPlacement = "Proportions";
            calclass.propcalccolumn(dt, ColumnHead, columnPlacement); //find the perentage of total fo reach eigen vector
        }
        private void eigcumulative(DataTable dt, String Columnintput,string Columnoutput)
        {
            //find the perentage of total fo reach eigen vector
            calclass.Cumulativecalccolumn(dt, Columnintput, Columnoutput); //find the perentage of total fo reach eigen vector
        }
        private void setRowNumber(DataTable dt)
        {//add a new column and place first..

            dt.Columns.Add("Component", typeof(Int32)).SetOrdinal(0);
            for (int indexcount = 0; indexcount < dt.Rows.Count; indexcount++)
            {

                dt.Rows[indexcount]["Component"] = (int)(indexcount + 1);
            }

        }
    }
}
