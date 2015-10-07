using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingTool.MC
{
    public class NextRandom
    {
        public NextRandom()
        {

        }
        //   Generates normally distributed numbers, with a mean = 0, variance =1.
        
        public double randomGen()
        {
            double mu = 0;//mean of 0
            double sigma = 1; //variance of 1
            var r = new Random();
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }
    }
}
