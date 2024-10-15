using System.Collections.Generic;

namespace ModelEngine
{
    public partial class PipeFittings
    {
        public List<string> PipeIndex = new List<string>() { "0.5","0.75","1","1.5","2","3","4","6","8","10","12","14","16","18","20","24","26","28","30","32","34","36",
            "38","40","42","44","46","48","50","52","54","56","58","60","66","72","78","84","90","96","102","108","114","120"};

        public List<string> ScheduleList = new List<string>() { "0.375", "0.5", "1", "10", "20", "30", "40", "60", "80", "100", "120", "140", "160" };

        public List<string> NominalPipeSize = new List<string>() {"0.5","0.75","1","1.5","2","3","4","6","8","10","12","14","16","18","20","24","26","28","30",
            "32","34","36","38","40","42","44","46","48","50","52","54","56","58","60","66","72","78","84","90","96","102","108","114","120"};

        public Dictionary<int, double> PipeSchedule = new Dictionary<int, double> { { 1, 0.375 },{2,0.5},{3,1.0},{4,10},{5,20},{6,30},{7,40},
            {8,60},{9,80},{10,100},{11,120},{12,140},{13,160}};

        /*public  Dictionary<int , double > NominalPipeSize = new  Dictionary<int , double >{{1,0.5},{2,0.75},{3,1},{4,1.5},{5,2},{6,3},{7,4},{8,6},
            {9,8},{10,10},{11,12},{12,14},{13,16},{14,18},{15,20},{16,24},{17,26},{18,28},{19,30},{20,32},{21,34},{22,36},
            {23,38},{24,40},{25,42},{26,44},{27,46},{28,48},{29,50},{30,52},{31,54},{32,56},{33,58},{34,60},{35,66},{36,72},
            {37,78},{38,84},{39,90},{40,96},{41,102},{42,108},{43,114},{44,120}};*/

        public Dictionary<string, int> Schedule = new Dictionary<string, int> {{"0.375",1},{"0.5",2},{"1",3},{"10",4},{"20",5},{"30",6},{"40",7},{"60",8},{"80",9},{"100",10},
             {"120",11},{"140",12},{"160",13}};

        /// <summary>
        /// Fist Column is pipe OD, Columns are schedule,
        /// </summary>
        public double[,] PipeinternalSizes = new double[,]{
            {0.375,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,0.622,double.NaN,0.546,double.NaN,double.NaN,double.NaN,0.466},
            {double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,0.824,double.NaN,0.742,double.NaN,double.NaN,double.NaN,0.612},
            {double.NaN,0.565,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,1.049,double.NaN,0.957,double.NaN,double.NaN,double.NaN,0.815},
            {0.565,1.15,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,1.61,double.NaN,1.5,double.NaN,double.NaN,double.NaN,1.338},
            {1.15,1.625,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,2.067,double.NaN,1.939,double.NaN,double.NaN,double.NaN,1.687},
            {1.625,2.75,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,3.068,double.NaN,2.9,double.NaN,double.NaN,double.NaN,2.624},
            {2.75,3.75,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,4.026,double.NaN,3.826,double.NaN,3.624,double.NaN,3.438},
            {3.75,5.875,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,6.065,double.NaN,5.761,double.NaN,5.501,double.NaN,5.187},
            {5.875,7.875,7.625,double.NaN,double.NaN,8.125,8.071,7.981,7.813,7.625,7.437,7.187,7.001,6.8713},
            {7.875,10,9.75,double.NaN,double.NaN,10.25,10.136,10.02,9.75,9.562,9.312,9.062,8.75,8.5},
            {10,12,11.75,double.NaN,double.NaN,12.25,12.06,11.938,11.626,11.374,11.062,10.75,10.5,10.126},
            {12,13.25,13,double.NaN,13.5,13.376,13.25,13.124,12.812,12.5,12.124,11.812,11.5,11.188},
            {13.25,15.25,15,double.NaN,15.5,15.376,15.25,15,14.688,14.312,13.938,13.562,13.124,12.812},
            {15.25,17.25,17,double.NaN,17.5,17.376,17.124,16.876,16.5,16.124,15.688,15.25,14.876,14.438},
            {17.25,19.25,19,double.NaN,19.5,19.25,19,18.812,18.376,17.938,17.438,17,16.5,16.062},
            {19.25,23.25,23,double.NaN,23.5,23.25,22.876,22.624,22.062,21.562,20.938,20.376,19.876,19.312},
            {23.25,25.25,25,double.NaN,25.376,25,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {25.25,27.25,27,double.NaN,27.376,27,26.75,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {27.25,29.25,29,double.NaN,29.376,29,28.75,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {29.25,31.25,31,double.NaN,31.376,31,30.75,30.624,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {31.25,33.25,33,double.NaN,33.312,33,32.75,32.624,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {33.25,35.25,35,double.NaN,35.376,35,34.75,34.5,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {35.25,37.25,37,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {37.25,39.25,39,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {39.25,41.25,41,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {41.25,43.25,43,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {43.25,45.25,45,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {45.25,47.25,47,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {47.25,49.25,49,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {49.25,51.25,51,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {51.25,53.25,53,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {53.25,55.25,55,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {55.25,57.25,57,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {57.25,59.25,59,58,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {59.25,65.25,65,64,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {65.25,71.25,71,70,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {71.25,77.25,77,76,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {77.25,83.25,83,82,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {83.25,89.25,89,88,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {89.25,95.25,95,94,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {95.25,101.25,101,100,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {101.25,107.25,107,106,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {107.25,113.25,113,112,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN},
            {113.25,119.25,119,118,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN,double.NaN}};
    }

    public class PipeFitting
    {
        private double paramC = double.NaN;
        private double paramA = double.NaN;
        private double parmaB = double.NaN;

        public virtual double ParamA { get => paramA; set => paramA = value; }
        public virtual double ParmaB { get => parmaB; set => parmaB = value; }
        public virtual double ParamC { get => paramC; set => paramC = value; }
    }

    public class Elbows90LR : PipeFitting
    {
        public override double ParamA { get; set; } = 800;
        public override double ParmaB { get; set; } = 0.2;
        public override double ParamC { get; set; } = 25.4;
    }

    public class Elbows45LR : PipeFitting
    {
        public override double ParamA { get; set; } = 500;
        public override double ParmaB { get; set; } = 0.15;
        public override double ParamC { get; set; } = 25.4;
    }

    public class TeesThru : PipeFitting
    {
        public override double ParamA { get; set; } = 150;
        public override double ParmaB { get; set; } = 0.5;
        public override double ParamC { get; set; } = 25.4;
    }

    public class TeesBranch : PipeFitting
    {
        public override double ParamA { get; set; } = 800;
        public override double ParmaB { get; set; } = 0.8;
        public override double ParamC { get; set; } = 25.4;
    }

    public class BallValves : PipeFitting
    {
        public override double ParamA { get; set; } = 500;
        public override double ParmaB { get; set; } = 0.15;
        public override double ParamC { get; set; } = 25.4;
    }

    public class ButterflyValves : PipeFitting
    {
        public override double ParamA { get; set; } = 800;
        public override double ParmaB { get; set; } = 0.25;
        public override double ParamC { get; set; } = 25.4;
    }

    public class GateValves : PipeFitting
    {
        public override double ParamA { get; set; } = 300;
        public override double ParmaB { get; set; } = 0.1;
        public override double ParamC { get; set; } = 25.4;
    }

    public class GlobeValves : PipeFitting
    {
        public override double ParamA { get; set; } = 1500;
        public override double ParmaB { get; set; } = 4;
        public override double ParamC { get; set; } = 25.4;
    }

    public class CheckValves : PipeFitting
    {
        public override double ParamA { get; set; } = 1500;
        public override double ParmaB { get; set; } = 1.5;
        public override double ParamC { get; set; } = 25.4;
    }

    public class PlugValves : PipeFitting
    {
        public override double ParamA { get; set; } = 1000;
        public override double ParmaB { get; set; } = 0.25;
        public override double ParamC { get; set; } = 25.4;
    }

    public class PipeEntrance : PipeFitting
    {
        public override double ParamA { get; set; } = 160;
        public override double ParmaB { get; set; } = 0.5;
        public override double ParamC { get; set; } = 0;
    }
}