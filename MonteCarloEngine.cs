using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ModellingTool.MC
{

    #region explanation
    //this class is the calculates the forward model for each maturity point, the info used by the MonteCarlo class to populated the results,the methhods are:
    //MCprocess, this method is called from the MonteCarlo class, main method of this class, it has multiple steps:
    //1. initials a random number from the random number class
    //2. find the random component of the discretised model by calling the calcrandomcomponent method
    //3. find the drift component of the discretised model by calling the driftcomponent method
    //4. combines the components of the discretised model and returns new value at maturity point to the MonteCarlo class calculate method.
    //calcrandomcomponent- this method is initialised from the MCprocess, calculates the random component of the discretised model, returns a double
    //calcdriftcomponent - this method is intialised from the MCprocess, calculates teh drift component fo the discretised model, returns a double.
    #endregion

    class MonteCarloEngine//implementation
    {
        //generates a random number with a mean of zero and variance of deltaT(1)
        MC.NextRandom newRandom = new MC.NextRandom();
        double driftvalue= 0;
        double randomvalue= 0;
      
        public MonteCarloEngine()
        {
        }

        public double MCprocess(Double initialvalue, double[] pcavalues,double TimeDeltas)
        {
            double dz = newRandom.randomGen(); //generates a random number with a mean of zero and variance of 1

            calcrandomcomponent(dz, pcavalues);//this find the random compenent of the discretised model
            calcdriftcomponent(pcavalues, TimeDeltas);//this find the drift component of the discretised model
            double tempresult = initialvalue * Math.Exp((randomvalue - (0.5*driftvalue)));//combines the model and returns new value at maturity.

            return tempresult;
        }

        public void calcrandomcomponent(double dz,double[] pcavlaues)
        {
            randomvalue = 0;
            foreach (double pcavar in pcavlaues)
            {
                randomvalue += (pcavar * dz);
            }
        }

        public void calcdriftcomponent(double[] pcavlaues, double dt)
        {
            driftvalue = 0;
            foreach (double pcavar in pcavlaues)
            {
                driftvalue += (Math.Pow(pcavar,2) * dt);
            }
        }

        }
}
