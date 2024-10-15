using ModelEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class Parse
    {
        private readonly string filename1 = @"P:\Thermodynamics\YAWS\EnthalpyFormation.txt";
        private readonly string filename2 = @"P:\Thermodynamics\YAWS\Entropy.txt";
        private readonly string filename3 = @"P:\Thermodynamics\YAWS\Gibbs Free Energy.txt";

        private readonly string filename4 = @"P:\Thermodynamics\YAWS\CAS Numbers.txt";
        private readonly string filename5 = @"P:\Thermodynamics\YAWS\Critical2.txt";
        private readonly string filename6 = @"P:\Thermodynamics\YAWS\Heat Capacity.txt";

        private readonly string filename1Out = @"P:\Thermodynamics\YAWS\EnthalpyFormation.out";
        private readonly string filename2Out = @"P:\Thermodynamics\YAWS\Entropy.out";
        private readonly string filename3Out = @"P:\Thermodynamics\YAWS\Gibbs Free Energy.out";

        private readonly string filename4Out = @"P:\Thermodynamics\YAWS\CAS Numbers.out";
        private readonly string filename5Out = @"P:\Thermodynamics\YAWS\Critical2.out";
        private readonly string filename6Out = @"P:\Thermodynamics\YAWS\Heat Capacity.out";
        private readonly List<string> parsedtext = new();

        [TestMethod]
        public void TestDataFile()
        {
            ComponentData.readdata(@"C:\Users\MarkA\Desktop\Refbits Files\RefBitsNet6\ModelThermo\DATABASE.csv", false); // e.g. Hysys
            ComponentData.writedata(@"C:\Users\MarkA\Desktop\Refbits Files\RefBitsNet6\ModelThermo\DATABASE.enc", true);
            ComponentData.readdata(@"C:\Users\MarkA\Desktop\Refbits Files\RefBitsNet6\ModelThermo\DATABASE.enc", true);
        }

        [TestMethod]
        public void YawsEnthalpy()
        {
            const Int32 BufferSize = 128;

            if (File.Exists(filename1))
            {
                using var fileStream = File.OpenRead(filename1);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
                string line;
                string ParsedLine;
                List<bool> isnumber = new();
                while ((line = streamReader.ReadLine()) != null)
                {
                    line = line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                    string[] splits = line.Split(new char[] { ' ' });
                    {
                        int splitscount = splits.Length;
                        for (int i = 0; i < splitscount; i++)
                            isnumber.Add(double.TryParse(splits[i], out double res));

                        ParsedLine = splits[0] + ":"; // id
                        ParsedLine += splits[1] + ":"; // formula
                        ParsedLine += splits[2]; // name

                        int start;
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

            if (File.Exists(filename1Out))
                File.Delete(filename1Out);

            using (var fileStream = File.OpenWrite(filename1Out))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                for (int i = 0; i < parsedtext.Count; i++)
                {
                    streamWriter.WriteLine(parsedtext[i]);
                }
            }
        }

        [TestMethod]
        public void YawsEntropy()
        {
            const int BufferSize = 128;

            if (File.Exists(filename2))
            {
                using var fileStream = File.OpenRead(filename2);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
                string ParsedLine;
                List<bool> isnumber = new();
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    line = line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                    string[] splits = line.Split(new char[] { ' ' });
                    {
                        int splitscount = splits.Length;
                        for (int i = 0; i < splitscount; i++)
                            isnumber.Add(double.TryParse(splits[i], out double res));

                        ParsedLine = splits[0] + ":"; // id
                        ParsedLine += splits[1] + ":"; // formula
                        ParsedLine += splits[2]; // name

                        int start;
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

            if (File.Exists(filename2Out))
                File.Delete(filename2Out);

            using (var fileStream = File.OpenWrite(filename2Out))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                for (int i = 0; i < parsedtext.Count; i++)
                {
                    streamWriter.WriteLine(parsedtext[i]);
                }
            }
        }

        [TestMethod]
        public void YawsGibbs()
        {
            const Int32 BufferSize = 128;

            if (File.Exists(filename3))
            {
                using var fileStream = File.OpenRead(filename3);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
                List<bool> isnumber = new();
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    line = line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                    string[] splits = line.Split(new char[] { ' ' });
                    string ParsedLine;
                    {
                        int splitscount = splits.Length;
                        for (int i = 0; i < splitscount; i++)
                            isnumber.Add(double.TryParse(splits[i], out double res));

                        ParsedLine = splits[0] + ":"; // id
                        ParsedLine += splits[1] + ":"; // formula
                        ParsedLine += splits[2]; // name

                        int start;
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

            if (File.Exists(filename3Out))
                File.Delete(filename3Out);

            using (var fileStream = File.OpenWrite(filename3Out))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                for (int i = 0; i < parsedtext.Count; i++)
                {
                    streamWriter.WriteLine(parsedtext[i]);
                }
            }
        }

        [TestMethod]
        public void YawsCAS()
        {
            const int BufferSize = 128;
            int count = 0;

            if (File.Exists(filename4))
            {
                using var fileStream = File.OpenRead(filename4);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
                string ParsedLine;
                List<bool> isnumber = new();
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    count++;
                    int start = 3;
                    line = line.Trim();
                    line = line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                    string[] splits = line.Split(new char[] { ' ' });
                    {
                        int splitscount = splits.Length;
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
            const int BufferSize = 128;

            if (File.Exists(filename4))
            {
                using var fileStream = File.OpenRead(filename5);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
                string ParsedLine;
                List<bool> isnumber = new();
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    int start = 3;
                    line = line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                    string[] splits = line.Split(new char[] { ' ' });
                    {
                        int splitscount = splits.Length;
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

            if (File.Exists(filename5Out))
                File.Delete(filename5Out);

            using (var fileStream = File.OpenWrite(filename5Out))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                for (int i = 0; i < parsedtext.Count; i++)
                {
                    streamWriter.WriteLine(parsedtext[i]);
                }
            }
        }

        [TestMethod]
        public void YawsCp()
        {
            const Int32 BufferSize = 128;

            if (File.Exists(filename6))
            {
                using var fileStream = File.OpenRead(filename6);
                using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
                string ParsedLine;
                List<bool> isnumber = new();
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    int start;
                    line = line.Trim(new char[] { '"' }).Replace("\n", "").Replace("\r", "");
                    string[] splits = line.Split(new char[] { ' ' });
                    {
                        int splitscount = splits.Length;
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