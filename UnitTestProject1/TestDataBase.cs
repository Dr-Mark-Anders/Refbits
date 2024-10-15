using EngineThermo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class Parse
    {

        private string filename1 = @"P:\Thermodynamics\YAWS\EnthalpyFormation.txt";
        private string filename2 = @"P:\Thermodynamics\YAWS\Entropy.txt";
        private string filename3 = @"P:\Thermodynamics\YAWS\Gibbs Free Energy.txt";

        private string filename4 = @"P:\Thermodynamics\YAWS\CAS Numbers.txt";
        private string filename5 = @"P:\Thermodynamics\YAWS\Critical2.txt";
        private string filename6 = @"P:\Thermodynamics\YAWS\Heat Capacity.txt";

        private string filename1Out = @"P:\Thermodynamics\YAWS\EnthalpyFormation.out";
        private string filename2Out = @"P:\Thermodynamics\YAWS\Entropy.out";
        private string filename3Out = @"P:\Thermodynamics\YAWS\Gibbs Free Energy.out";

        private string filename4Out = @"P:\Thermodynamics\YAWS\CAS Numbers.out";
        private string filename5Out = @"P:\Thermodynamics\YAWS\Critical2.out";
        private string filename6Out = @"P:\Thermodynamics\YAWS\Heat Capacity.out";


        List<string> parsedtext = new List<string>();

        public void writeresults()
        {

        }

        [TestMethod]
        public void TestDataFile()
        {
            DataTable compData, encdata;
            /*compData = ComponentData.readdata("OldData.csv", false); // e.g. Hysys
            ComponentData.writedata("OldData.enc", compData, true);
            encdata = ComponentData.readdata("OldData.enc", true);

            compData = ComponentData.readdata("NewData.csv", false); // e.g. Hysys
            ComponentData.writedata("NewData.enc", compData, true);
            encdata = ComponentData.readdata("NewData.enc", true);*/

            ComponentData.readdata("DATABASE.csv", false); // e.g. Hysys
            ComponentData.writedata("DATABASE.enc", true);
            ComponentData.readdata("DATABASE.enc", true);
        }


        [TestMethod]
        public void YawsEnthalpy()
        {
            const Int32 BufferSize = 128;

            if (File.Exists(filename1))
            {
                using (var fileStream = File.OpenRead(filename1))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    string ParsedLine;
                    List<bool> isnumber = new List<bool>();
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        int start = 3;
                        line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                        string[] splits = line.Split(new char[] { ' ' });
                        {
                            int splitscount = splits.Count();
                            for (int i = 0; i < splitscount; i++)
                                isnumber.Add(double.TryParse(splits[i], out double res));

                            ParsedLine = splits[0] + ":"; // id
                            ParsedLine += splits[1] + ":"; // formula
                            ParsedLine += splits[2]; // name

                            if (!isnumber[3] && !isnumber[4])
                            {
                                ParsedLine += " " + splits[3] + " " + splits[4] + ":"; // name
                                start = 5;
                            }
                            else if (!isnumber[3])
                            {
                                ParsedLine += " " + splits[3] + ":"; // name
                                start = 4;
                            }
                            else
                            {
                                ParsedLine += ":"; // name
                                start = 3;
                            }



                            for (int i = start; i < splitscount; i++)
                            {
                                if (i < splitscount - 1 && splits[i + 1].Length > 0 && splits[i + 1][0] == '.') // number has been split by mistake
                                {
                                    ParsedLine += splits[i] + splits[i + 1] + ":";
                                    i++;
                                }
                                else
                                    ParsedLine += splits[i] + ":";
                            }
                        }
                        parsedtext.Add(ParsedLine);
                        Debug.Print(ParsedLine);
                        isnumber.Clear();
                    }
                }
            }

            if (File.Exists(filename1Out))
                File.Delete(filename1Out);

            using (var fileStream = File.OpenWrite(filename1Out))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                for (int i = 0; i < parsedtext.Count; i++)
                {
                    streamWriter.WriteLine(parsedtext[i]);
                }
                //fileStream.Close();
            }
        }

        [TestMethod]
        public void YawsEntropy()
        {
            const Int32 BufferSize = 128;

            if (File.Exists(filename2))
            {
                using (var fileStream = File.OpenRead(filename2))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    string ParsedLine;
                    List<bool> isnumber = new List<bool>();
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        int start = 3;
                        line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                        string[] splits = line.Split(new char[] { ' ' });
                        {
                            int splitscount = splits.Count();
                            for (int i = 0; i < splitscount; i++)
                                isnumber.Add(double.TryParse(splits[i], out double res));

                            ParsedLine = splits[0] + ":"; // id
                            ParsedLine += splits[1] + ":"; // formula
                            ParsedLine += splits[2]; // name

                            if (!isnumber[3] && !isnumber[4])
                            {
                                ParsedLine += " " + splits[3] + " " + splits[4] + ":"; // name
                                start = 5;
                            }
                            else if (!isnumber[3])
                            {
                                ParsedLine += " " + splits[3] + ":"; // name
                                start = 4;
                            }
                            else
                            {
                                ParsedLine += ":"; // name
                                start = 3;
                            }



                            for (int i = start; i < splitscount; i++)
                            {
                                if (i < splitscount - 1 && splits[i + 1].Length > 0 && splits[i + 1][0] == '.') // number has been split by mistake
                                {
                                    ParsedLine += splits[i] + splits[i + 1] + ":";
                                    i++;
                                }
                                else
                                    ParsedLine += splits[i] + ":";
                            }
                        }
                        parsedtext.Add(ParsedLine);
                        Debug.Print(ParsedLine);
                        isnumber.Clear();
                    }
                }
            }

            if (File.Exists(filename2Out))
                File.Delete(filename2Out);

            using (var fileStream = File.OpenWrite(filename2Out))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                for (int i = 0; i < parsedtext.Count; i++)
                {
                    streamWriter.WriteLine(parsedtext[i]);
                }
                //fileStream.Close();
            }
        }

        [TestMethod]
        public void YawsGibbs()
        {
            const Int32 BufferSize = 128;

            if (File.Exists(filename3))
            {
                using (var fileStream = File.OpenRead(filename3))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    string ParsedLine;
                    List<bool> isnumber = new List<bool>();
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        int start = 3;
                        line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                        string[] splits = line.Split(new char[] { ' ' });
                        {
                            int splitscount = splits.Count();
                            for (int i = 0; i < splitscount; i++)
                                isnumber.Add(double.TryParse(splits[i], out double res));

                            ParsedLine = splits[0] + ":"; // id
                            ParsedLine += splits[1] + ":"; // formula
                            ParsedLine += splits[2]; // name

                            if (!isnumber[3] && !isnumber[4])
                            {
                                ParsedLine += " " + splits[3] + " " + splits[4] + ":"; // name
                                start = 5;
                            }
                            else if (!isnumber[3])
                            {
                                ParsedLine += " " + splits[3] + ":"; // name
                                start = 4;
                            }
                            else
                            {
                                ParsedLine += ":"; // name
                                start = 3;
                            }

                            for (int i = start; i < splitscount; i++)
                            {
                                if (i < splitscount - 1 && splits[i + 1].Length > 0 && splits[i + 1][0] == '.') // number has been split by mistake
                                {
                                    ParsedLine += splits[i] + splits[i + 1] + ":";
                                    i++;
                                }
                                else
                                    ParsedLine += splits[i] + ":";
                            }
                        }
                        parsedtext.Add(ParsedLine);
                        Debug.Print(ParsedLine);
                        isnumber.Clear();
                    }
                }
            }

            if (File.Exists(filename3Out))
                File.Delete(filename3Out);

            using (var fileStream = File.OpenWrite(filename3Out))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                for (int i = 0; i < parsedtext.Count; i++)
                {
                    streamWriter.WriteLine(parsedtext[i]);
                }
                //fileStream.Close();
            }
        }

        [TestMethod]
        public void YawsCAS()
        {
            const Int32 BufferSize = 128;
            int count = 0;

            if (File.Exists(filename4))
            {
                using (var fileStream = File.OpenRead(filename4))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    string ParsedLine;
                    List<bool> isnumber = new List<bool>();
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        count++;
                        int start = 3;
                        line.Trim();
                        line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                        string[] splits = line.Split(new char[] { ' ' });
                        {
                            int splitscount = splits.Count();
                            for (int i = 0; i < splitscount; i++)
                                isnumber.Add(double.TryParse(splits[i], out double res));

                            ParsedLine = splits[0] + ":"; // frmuale
                            ParsedLine += splits[1] + ""; // name first part

                            if (splitscount > 4 && !Char.IsDigit(splits[2][0]) && splits[3].Length > 0 && !Char.IsDigit(splits[3][0]) && splits[3][0] != '-')
                            {
                                ParsedLine += " " + splits[2] + splits[3] + ":"; // name in two parts
                                start = 4;
                            }
                            else if (!Char.IsDigit(splits[2][0]))
                            {
                                ParsedLine += " " + splits[2] + ":"; // name
                                start = 3;
                            }
                            else
                            {
                                ParsedLine += ":"; // name
                                start = 2;
                            }

                            for (int i = start; i < splitscount; i++)
                            {
                                if (i < splitscount - 1 && splits[i + 1].Length > 0 && splits[i + 1][0] == '.') // number has been split by mistake
                                {
                                    ParsedLine += splits[i] + splits[i + 1] + ":";
                                    i++;
                                }
                                else
                                    ParsedLine += splits[i] + ":";
                            }
                        }
                        parsedtext.Add(ParsedLine);
                        Debug.Print(count + " " + ParsedLine);
                        isnumber.Clear();
                    }
                }
            }

            if (File.Exists(filename4Out))
                File.Delete(filename4Out);

            using (var fileStream = File.OpenWrite(filename4Out))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                for (int i = 0; i < parsedtext.Count; i++)
                {
                    streamWriter.WriteLine(parsedtext[i]);
                }
                //fileStream.Close();
            }
        }

        [TestMethod]
        public void YawsCritProps()
        {
            const Int32 BufferSize = 128;

            if (File.Exists(filename4))
            {
                using (var fileStream = File.OpenRead(filename5))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    string ParsedLine;
                    List<bool> isnumber = new List<bool>();
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        int start = 3;
                        line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                        string[] splits = line.Split(new char[] { ' ' });
                        {
                            int splitscount = splits.Count();
                            for (int i = 0; i < splitscount; i++)
                                isnumber.Add(double.TryParse(splits[i], out double res));

                            ParsedLine = splits[0] + ":"; // id
                            ParsedLine += splits[1] + ":"; // formula
                            try
                            {


                                if (splitscount > 3)
                                {
                                    ParsedLine += splits[2]; // name

                                    if (!isnumber[3] && !isnumber[4])
                                    {
                                        ParsedLine += " " + splits[3] + " " + splits[4] + ":"; // name
                                        start = 5;
                                    }
                                    else if (!isnumber[3])
                                    {
                                        ParsedLine += " " + splits[3] + ":"; // name
                                        start = 4;
                                    }
                                    else
                                    {
                                        ParsedLine += ":"; // name
                                        start = 3;
                                    }

                                    for (int i = start; i < splitscount; i++)
                                    {
                                        if (i < splitscount - 1 && splits[i + 1].Length > 0 && splits[i + 1][0] == '.') // number has been split by mistake
                                        {
                                            ParsedLine += splits[i] + splits[i + 1] + ":";
                                            i++;
                                        }
                                        else
                                            ParsedLine += splits[i] + ":";
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                        parsedtext.Add(ParsedLine);
                        Debug.Print(ParsedLine);
                        isnumber.Clear();
                    }
                }
            }

            if (File.Exists(filename5Out))
                File.Delete(filename5Out);

            using (var fileStream = File.OpenWrite(filename5Out))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                for (int i = 0; i < parsedtext.Count; i++)
                {
                    streamWriter.WriteLine(parsedtext[i]);
                }
                //fileStream.Close();
            }
        }

        [TestMethod]
        public void YawsCp()
        {
            const Int32 BufferSize = 128;

            if (File.Exists(filename6))
            {
                using (var fileStream = File.OpenRead(filename6))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    string ParsedLine;
                    List<bool> isnumber = new List<bool>();
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        int start = 3;
                        line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                        string[] splits = line.Split(new char[] { ' ' });
                        {
                            int splitscount = splits.Count();
                            for (int i = 0; i < splitscount; i++)
                                isnumber.Add(double.TryParse(splits[i], out double res));

                            ParsedLine = splits[0] + ":"; // id
                            ParsedLine += splits[1] + ":"; // formula
                            ParsedLine += splits[2]; // name

                            if (!isnumber[3] && !isnumber[4])
                            {
                                ParsedLine += " " + splits[3] + " " + splits[4] + ":"; // name
                                start = 5;
                            }
                            else if (!isnumber[3])
                            {
                                ParsedLine += " " + splits[3] + ":"; // name
                                start = 4;
                            }
                            else
                            {
                                ParsedLine += ":"; // name
                                start = 3;
                            }

                            for (int i = start; i < splitscount; i++)
                            {
                                if (i < splitscount - 1 && splits[i + 1].Length > 0 && splits[i + 1][0] == '.') // number has been split by mistake
                                {
                                    ParsedLine += splits[i] + splits[i + 1] + ":";
                                    i++;
                                }
                                else
                                    ParsedLine += splits[i] + ":";
                            }
                        }
                        parsedtext.Add(ParsedLine);
                        Debug.Print(ParsedLine);
                        isnumber.Clear();
                    }
                }
            }

            if (File.Exists(filename6Out))
                File.Delete(filename6Out);

            using (var fileStream = File.OpenWrite(filename6Out))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                for (int i = 0; i < parsedtext.Count; i++)
                {
                    streamWriter.WriteLine(parsedtext[i]);
                }
                //fileStream.Close();
            }
        }

    }
}


