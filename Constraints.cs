using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ModellingTool
{
    public class Constraints 
    {//used in the optimisation process....

        private double janImport = Convert.ToDouble(ConfigurationManager.AppSettings["JanIMCap"]);
        public double JanImport
        {
            get { return janImport; }
            set { janImport = value; }
        }

        //  public double JanImport { get; set; }
        private double janExport = Convert.ToDouble(ConfigurationManager.AppSettings["JanExport"]);
        public double JanExport
        {
            get { return janExport; }
            set { janExport = value; }
        }
        private double febImport = Convert.ToDouble(ConfigurationManager.AppSettings["FebIMCap"]);
        public double FebImport
        {
            get { return febImport; }
            set { febImport = value; }
        }
        private double febExport = Convert.ToDouble(ConfigurationManager.AppSettings["FebExport"]);
        public double FebExport
        {
            get { return febExport; }
            set { febExport = value; }
        }
        private double marImport = Convert.ToDouble(ConfigurationManager.AppSettings["MarIMCap"]);
        public double MarImport
        {
            get { return marImport; }
            set { marImport = value; }
        }
        private double marExport = Convert.ToDouble(ConfigurationManager.AppSettings["MarExport"]);
        public double MarExport
        {
            get { return marExport; }
            set { marExport = value; }
        }
        private double aprImport = Convert.ToDouble(ConfigurationManager.AppSettings["AprIMCap"]);
        public double AprImport
        {
            get { return aprImport; }
            set { aprImport = value; }
        }
        private double aprExport = Convert.ToDouble(ConfigurationManager.AppSettings["AprExport"]);
        public double AprExport
        {
            get { return aprExport; }
            set { aprExport = value; }
        }
        private double mayImport = Convert.ToDouble(ConfigurationManager.AppSettings["MayIMCap"]);
        public double MayImport
        {
            get { return mayImport; }
            set { mayImport = value; }
        }
        private double mayExport = Convert.ToDouble(ConfigurationManager.AppSettings["MayExport"]);
        public double MayExport
        {
            get { return mayExport; }
            set { mayExport = value; }
        }
        private double junImport = Convert.ToDouble(ConfigurationManager.AppSettings["JunIMCap"]);
        public double JunImport
        {
            get { return junImport; }
            set { junImport = value; }
        }
        private double junExport = Convert.ToDouble(ConfigurationManager.AppSettings["JunExport"]);
        public double JunExport
        {
            get { return junExport; }
            set { junExport = value; }
        }
        private double julImport = Convert.ToDouble(ConfigurationManager.AppSettings["JulIMCap"]);
        public double JulImport
        {
            get { return julImport; }
            set { julImport = value; }
        }
        private double julExport = Convert.ToDouble(ConfigurationManager.AppSettings["JulExport"]);
        public double JulExport
        {
            get { return julExport; }
            set { julExport = value; }
        }
        private double augImport = Convert.ToDouble(ConfigurationManager.AppSettings["AugIMCap"]);
        public double AugImport
        {
            get { return augImport; }
            set { augImport = value; }
        }
        private double augExport = Convert.ToDouble(ConfigurationManager.AppSettings["AugExport"]);
        public double AugExport
        {
            get { return augExport; }
            set { augExport = value; }
        }
        private double septImport = Convert.ToDouble(ConfigurationManager.AppSettings["SeptIMCap"]);
        public double SeptImport
        {
            get { return septImport; }
            set { septImport = value; }
        }
        private double septExport = Convert.ToDouble(ConfigurationManager.AppSettings["SeptExport"]);
        public double SeptExport
        {
            get { return septExport; }
            set { septExport = value; }
        }

        private double octImport = Convert.ToDouble(ConfigurationManager.AppSettings["OctIMCap"]);
        public double OctImport
        {
            get { return octImport; }
            set { octImport = value; }
        }
        private double octExport = Convert.ToDouble(ConfigurationManager.AppSettings["OctExport"]);
        public double OctExport
        {
            get { return octExport; }
            set { octExport = value; }
        }
        private double novImport = Convert.ToDouble(ConfigurationManager.AppSettings["NovIMCap"]);
        public double NovImport
        {
            get { return novImport; }
            set { novImport = value; }
        }
        private double novExport = Convert.ToDouble(ConfigurationManager.AppSettings["NovExport"]);
        public double NovExport
        {
            get { return novExport; }
            set { novExport = value; }
        }
        private double decImport = Convert.ToDouble(ConfigurationManager.AppSettings["DecIMCap"]);
        public double DecImport
        {
            get { return decImport; }
            set { decImport = value; }
        }
        private double decExport = Convert.ToDouble(ConfigurationManager.AppSettings["DecExport"]);

        public double DecExport
        {

            get { return decExport; }
            set { decExport = value; }


        }
        private double maxCap = Convert.ToDouble(ConfigurationManager.AppSettings["maxcap"]);
        public double MaxCap
        {
            get { return maxCap; }
            set { maxCap = value; }
        }

        private double injection = Convert.ToDouble(ConfigurationManager.AppSettings["Injectioncharge"]);
        public double Injection
        {
            get { return injection; }
            set { injection = value; }
        }
        private double widthdrawl = Convert.ToDouble(ConfigurationManager.AppSettings["Withdrawlcharge"]);
        public double Widthdrawl
        {
            get { return widthdrawl; }
            set { widthdrawl = value; }
        }
        private double rollingdays = Convert.ToDouble(ConfigurationManager.AppSettings["RevaluationDays"]);
        public double Rollingdays
        {
            get { return rollingdays; }
            set { rollingdays = value; }
        }
        public Constraints()
        {
            
        }
    }
}
