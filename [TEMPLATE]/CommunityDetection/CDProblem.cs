using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using GraphGenerator;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "CommunityDetection"; } }

        public override void TryMyCode()
        {            

            //Case1
            KeyValuePair<string, string>[] edges1 = new KeyValuePair<string, string>[2];
            edges1[0] = new KeyValuePair<string, string>("1", "4");
            edges1[1] = new KeyValuePair<string, string>("4", "5");

            int expected1 = 1;
            int output1 = CommunityDetection.CountCommunities(edges1);
            PrintCase(edges1, output1, expected1);

            //Case2
            KeyValuePair<string, string>[] edges2 = new KeyValuePair<string, string>[5];
            edges2[0] = new KeyValuePair<string, string>("1", "2");
            edges2[1] = new KeyValuePair<string, string>("3", "4");
            edges2[2] = new KeyValuePair<string, string>("5", "6");
            edges2[3] = new KeyValuePair<string, string>("7", "8");
            edges2[4] = new KeyValuePair<string, string>("9", "10");

            int expected2 = 5;
            int output2 = CommunityDetection.CountCommunities(edges2);
            PrintCase(edges2, output2, expected2);

            //Case3
            KeyValuePair<string, string>[] edges3 = new KeyValuePair<string, string>[6];
            edges3[0] = new KeyValuePair<string, string>("1", "2");
            edges3[1] = new KeyValuePair<string, string>("2", "3");
            edges3[2] = new KeyValuePair<string, string>("5", "4");
            edges3[3] = new KeyValuePair<string, string>("5", "6");
            edges3[4] = new KeyValuePair<string, string>("3", "5");
            edges3[5] = new KeyValuePair<string, string>("4", "2");
            int expected3 = 1;
            int output3 = CommunityDetection.CountCommunities(edges3);
            PrintCase(edges3, output3, expected3);

            //Case4
            KeyValuePair<string, string>[] edges4 = new KeyValuePair<string, string>[9];
            edges4[0] = new KeyValuePair<string, string>("1", "5");
            edges4[1] = new KeyValuePair<string, string>("1", "4");
            edges4[2] = new KeyValuePair<string, string>("1", "3");
            edges4[3] = new KeyValuePair<string, string>("1", "2");
            edges4[4] = new KeyValuePair<string, string>("2", "3");
            edges4[5] = new KeyValuePair<string, string>("3", "4");
            edges4[6] = new KeyValuePair<string, string>("4", "5");
            edges4[7] = new KeyValuePair<string, string>("5", "2");
            edges4[8] = new KeyValuePair<string, string>("6", "7");

            int expected4 = 2;
            int output4 = CommunityDetection.CountCommunities(edges4);
            PrintCase(edges4, output4, expected4);
 
        }

        

        Thread tstCaseThr;
        bool caseTimedOut ;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            int testCases;
            int actualResult = int.MinValue;
            int output = int.MinValue;

            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();
            testCases = int.Parse(line);
   
            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            int i = 1;
            while (testCases-- > 0)
            {
                line = sr.ReadLine();
                int e = int.Parse(line);
               
                var edges = new KeyValuePair<string, string>[e];
                for (int j = 0; j < e; j++)
                {
                    line = sr.ReadLine();
                    string[] lineParts = line.Split(',');
                    edges[j] = new KeyValuePair<string, string>(lineParts[0], lineParts[1]);
                }
                line = sr.ReadLine();

                actualResult = int.Parse(line);
                caseTimedOut = true;
                caseException = false;
                {
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            Stopwatch sw = Stopwatch.StartNew();
                            output = CommunityDetection.CountCommunities(edges);
                            sw.Stop();
                            //PrintCase(vertices,edges, output, actualResult);
                            Console.WriteLine("|E| = {0}, time in ms = {1}", edges.Length, sw.ElapsedMilliseconds);
                            Console.WriteLine("{0}", output);

                        }
                        catch
                        {
                            caseException = true;
                            output = int.MinValue;
                        }
                        caseTimedOut = false;
                    });

                    //StartTimer(timeOutInMillisec);
                    if (readTimeFromFile)
                    {
                        timeOutInMillisec = int.Parse(sr.ReadLine().Split(':')[1]);
                    }
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }

                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
					tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                else if (output == actualResult)    //Passed
                {
                    Console.WriteLine("Test Case {0} Passed!", i);
                    correctCases++;
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    Console.WriteLine(" your answer = {0}, correct answer = {1}", output, actualResult);
                    wrongCases++;
                }

                i++;
            }
            file.Close();
            sr.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0)); 
        }

        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();

        }

        #endregion

        #region Helper Methods
        private static void PrintCase(KeyValuePair<string, string>[] edges, int output, int expected)
        {
            
            Console.WriteLine("Edges: ");
            for (int i = 0; i < edges.Length; i++)
            {
                Console.WriteLine("{0}, {1}", edges[i].Key, edges[i].Value);
            }
            Console.WriteLine("Output: {0}", output);
            Console.WriteLine("Expected: {0}", expected);
            if (output == expected)    //Passed
            {
                Console.WriteLine("CORRECT");
            }
            else                    //WrongAnswer
            {
                Console.WriteLine("WRONG");
            }
            Console.WriteLine();
        }
        
        #endregion
   
    }
}
