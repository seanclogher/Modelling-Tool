using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MathNet.Numerics.LinearAlgebra;
using System.Numerics;

namespace ModellingTool.PCA
{
    class VectorMatrixCalc 
    {
        public VectorMatrixCalc()
        {
            //this class aims to provide support functionality, it is used by several other classes.
            //the methods in the class include:
            //arrayconvert- this converts a datatable into an array.
            //tableconvert - this converts an double array/single array (vector) into a datatable.
            //tableconvert (eigvales) - this converts an eignval object into a datatable
            //invertmatrix - this returns an array, allows for inversion of a matrix...
            // matrixVectorsubtraction - returns an array, allows fort eh subtraction of a vector from a matrix.
            //vectormultiplication - returns a double, allows for the multiplication of two vectors
            //sumvector - returns a double, sum the values in a vectr array.
            //sumcolumn - returns a double, sums the row entry in a specific column in a datatable
            //propcalccolumn - returns a datatable entry, using sumcolumn,finds a proportional weighting for the column in the datatable.
            //Cumulativecalccolumn - returns a datatable entry, adds rows accumalatively, from a specific column.
            //spreadsmatrix - returns an array(matrix), this method is used to calcualted the spreads between different months
            //in the forward curve, it also takes into account and injection and withdrawal costs.
        }

        public double[,] arrayconvert(DataTable initdata)
        {
           
            double[,] calcarray = new Double[initdata.Rows.Count, initdata.Columns.Count];
            //Now save DataTable values in array,
            //here we start from second row as ColumnNames are stored in first row
            for (int row = 0; row < initdata.Rows.Count; row++)
            {
                for (int col = 0; col < initdata.Columns.Count; col++)
                {
                    calcarray[row, col] = Math.Round((double)initdata.Rows[(row)][col], 6);
                }
            }
            //Return 2D-String Array
            return calcarray;
        }

        public DataTable tableconvert(double[,] initaray)
        {

            DataTable tableconvert = new DataTable();
           
            for (int columncount = 0; columncount < initaray.GetLength(1); columncount++)
            {
                tableconvert.Columns.Add("C" + columncount, typeof(System.Double));
            }

                //Multi Dimension Array into DataTable
            for (int outerIndex = initaray.GetLength(0)-1; outerIndex >= 0; outerIndex--)
                {
                    DataRow newRow = tableconvert.NewRow();
                    for (int innerIndex = 0; innerIndex < initaray.GetLength(1); innerIndex++)
                    {


                        newRow[innerIndex] = Convert.ToDouble(initaray[outerIndex, innerIndex]);


                    }
                    tableconvert.Rows.Add(newRow);
                }
                return tableconvert;
        }

        public DataTable tableconvert(double[] initaray)
        {

            DataTable table = new DataTable();
            //add the number of columns required: 1 for a vector...
            table.Columns.Add();
           

            //add each row of the array into the datatable.
           
                DataRow newRow = table.NewRow();
                for (int innerIndex = 0; innerIndex < initaray.GetLength(0); innerIndex++)
                {
                    newRow[0] = initaray[innerIndex];
                    table.Rows.Add(newRow);
                    newRow = table.NewRow();
               }
                            
            return table;
        }

        public DataTable tableconvert(Vector<Complex> eigvalues)
        {//this converts a complex vector from Math.Numerics into a dataset...

            DataTable table = new DataTable();
            //add the number of columns required: 1 for a vector...
            table.Columns.Add();


            //add each row of the array into the datatable.
             DataRow newRow = table.NewRow();
             for (int innerIndex = eigvalues.Count-1; innerIndex >= 0; innerIndex--)
            {
                 newRow[0] = eigvalues.Storage[innerIndex].Magnitude;
                 table.Rows.Add(newRow);
                newRow = table.NewRow();
            }

            return table;
        }
                
        public double[,] invertmatrix(double[,] init)
        {
            double[,] rhs = new Double[init.GetLength(0), init.GetLength(1)];

            for (int i = 0; i < init.GetLength(0); i++)
            {
                for (int j = 0; j < init.GetLength(0); j++)
                {
                    rhs[init.GetLength(0) - (i+1), init.GetLength(1)-(j+1)] = init[i, j]*-1;
                }
            }

            return rhs;
        }
        
        public double[,] matrixVectorsubtraction(double[,] lhs, double[] rhs)
        {//this method takes a matrix and vector, sutracts vector from matrix  
            double[,] tmp = new Double[lhs.GetLength(0), lhs.GetLength(1)];

            for (int i = 0; i < lhs.GetLength(0); i++)
            {
                for (int j = 0; j < rhs.GetLength(0); j++)
                {
                    tmp[i, j]= Math.Round(lhs[i, j] - rhs[j],6);
                }
            }
            return tmp;
        }

        public double vectormultiplication(double[] lhs, double[] rhs)
        {
            if (rhs.GetLength(0) == lhs.GetLength(0))
            {
                double tmp = 0;
                for (int j = 0; j < rhs.GetLength(0); j++)
                {
                    tmp += Math.Round((lhs[j] * rhs[j]),6);
                }
                return tmp;
            }
            return 0;
        }

        public double sumvector(double[] lhs)
        {
            double tmp = 0;
            for (int j = 0; j < lhs.GetLength(0); j++)
            {
                tmp = +lhs[j];
            }
            return tmp;
        }

        //public double[,] partialcovariancecalc(double[,] lhs, double[,] rhs)
        //{//intakes the part1 calc output and the covariance matrix....

        //    //set up initial variables...
        //    double[] vectlhs = new double[lhs.GetLength(0)];
        //    double[] vectrhs = new double[lhs.GetLength(0)];
        //    double[,] results = new double[rhs.GetLength(0), rhs.GetLength(0)];


        //    if ((results.GetLength(0) * results.GetLength(1)) == 1)
        //    {//vector*vector problem
        //        results[0,0] = vectormultiplication(vectlhs, vectrhs);
        //        return results;
        //    }
        //    else
        //    {//matrix*vector problem
        //        int i = results.GetLength(0);
        //        for (int di = 0; di < i; di++)//populate the diagonals...
        //        {

        //            for (int j = 0; j < lhs.GetLength(0); j++)
        //                {
        //                    vectlhs[j] = lhs[j, di];//popuate temp of the dimension..
        //                }
        //            double tempvec = Math.Round(vectormultiplication(vectlhs, vectlhs)/ (double)(lhs.GetLength(0)-1),8);//do a vector*vector calculation...
        //            //update covaraince matrix with diagonal result...
        //            results[di, di] = tempvec;
        //        };
        //        for (int nodi = 0; nodi < i-1; nodi++)//populate the non diagonals symetmetrical matrix....
        //        {
        //            //it is important to get this relationship right, that is why I have split it rather than doing a combined version, 
        //            //step 1 populate vector..
        //            for (int j = 0; j < lhs.GetLength(0); j++)
        //            {
        //                vectlhs[j] = lhs[j, nodi];//populate first vector..
        //                //populate the next dimension vector..
        //                vectrhs[j] = lhs[j, nodi+1];
        //            }
        //            double tempvec = Math.Round(vectormultiplication(vectlhs, vectrhs)/(double)(lhs.GetLength(0)-1),8);//do a vector*vector calculation...
        //            //update covaraince matrix with diagonal result...
        //            results[nodi, nodi+1] = tempvec;
        //            results[nodi+1, nodi] = tempvec;
        //        };
        //        return results;
        //    }

        //}
        
        public double sumcolumn(DataTable dt, string columnid)
        {//this is used in working out the proportion per eigenvector
            double result = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result += Convert.ToDouble(dt.Rows[i][columnid]);  
            }
                           
            return result;

        }
      
        public void propcalccolumn(DataTable dt, string SumColumn, string columnPlacement)
        {
            double total = sumcolumn(dt, SumColumn);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][columnPlacement] = Convert.ToDouble(dt.Rows[i][SumColumn]) / total;
    
            }
        }
  
        public void Cumulativecalccolumn(DataTable dt, string InputCol, string columnoutput)
        {
            double results = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
              results += Convert.ToDouble(dt.Rows[i][InputCol]);
              dt.Rows[i][columnoutput] = results;
            }
        }

        public double[,] spreadsmatrix(DataTable initdata, double injcharge, double widthcharge)
        {

            //Declare ?D-String array
            //here we start from second row as ColumnNames are stored in first row
            double[,] initarray = new Double[initdata.Rows.Count, initdata.Columns.Count-1];
            double[,] resultarray = new Double[initdata.Columns.Count-1, initdata.Columns.Count - 1];
            //Now save DataTable values in array,

            for (int row = 0; row < initdata.Rows.Count; row++)
            {
                for (int col = 1; col < initdata.Columns.Count; col++)
                {
                    initarray[row, col-1] = Math.Round((double)initdata.Rows[(row)][col], 6);
                }
            }
            //find the discounted spreads for injection in month i and withdraw in month j
           // Each value shown in the table represents the discounted revenue the owner of the facility would receive by
           //injecting one unit of gas and withdrawing it at a later time, inclusive of injection costs and withdrawal costs...
            for (int i = 0; i < resultarray.GetLength(0); i++)
            {
                int ij = i;
                for (int j = initarray.GetLength(1); j > i; j--)
                {

                    resultarray[i, ij] = Math.Round(((initarray[0, j - 1] + widthcharge) - (initarray[0, i] - injcharge)), 5);
                    ij++;
                }
            }

            return resultarray;
        }
    }


}
