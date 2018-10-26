using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentMerger2
{
    class Program
    {
        static void Main(string[] args)
        {            
            int counter;
            bool skip;
            int unopened = 0;
            string contents = null;
            string outputFile = args[args.Length - 1];
            List<string> fileList = new List<string>();
            List<string> unopenedList = new List<string>();

            if (args.Length < 3)
            {
                Console.WriteLine("Error, must have at least 2 input files to merge into the output file.\r");
                Console.WriteLine("Input should look like the following:\r");
                Console.WriteLine("DocumentMerger2 <inputFile1> <inputFile2> ... <inputFileN> <outputFile>");
                skip = true;
            }
            else
                skip = false;

            if (skip == false)
            {
                if (!outputFile.Contains(".txt"))
                    outputFile += ".txt";
                if (!File.Exists(outputFile))
                    using (StreamWriter output = new StreamWriter(outputFile))
                    {
                        File.Create(outputFile);
                        output.Close();
                    }
                File.WriteAllText(outputFile, "");

                for (counter = 0; counter < args.Length - 1; counter++)
                    fileList.Add(args[counter]);
                for (counter = 0; counter < args.Length - 1; counter++)
                    if (!fileList[counter].Contains(".txt"))
                        fileList[counter] += ".txt";

                Console.WriteLine("Document Merger 2");
                foreach (string file in fileList)
                {
                    if (!File.Exists(file))
                    {
                        Console.WriteLine("Error, {0} not found.", file);
                        unopenedList.Add(file);
                        unopened++;
                    }
                }

                try
                {
                    foreach (string fileName in fileList)
                    {
                        using (StreamReader file = new StreamReader(fileName))
                        {
                            contents = file.ReadToEnd();
                            Console.WriteLine("Contents of: {0}", fileName);
                            Console.WriteLine(contents);
                            Console.WriteLine("End of {0}...\n", fileName);
                        }
                    }
                }

                catch (Exception x)
                {
                    Console.WriteLine("\nError: {0}\n", x);
                }

                finally
                {
                    if (unopened != 0)
                    {
                        foreach (string files in fileList)
                            foreach (string failed in unopenedList)
                            {
                                if (files == failed)
                                {
                                    Console.WriteLine("File: {0} failed to merge...\r", files);
                                    continue;
                                }
                                else
                                    using (StreamReader file = new StreamReader(files))
                                    {
                                        contents = file.ReadToEnd();
                                        File.AppendAllText(outputFile, contents + Environment.NewLine);
                                        Console.WriteLine("File: {0} was merged successfully!\r", files);
                                    }
                            }
                    }

                    else
                        foreach (string files in fileList)
                            using (StreamReader file = new StreamReader(files))
                            {
                                contents = file.ReadToEnd();
                                File.AppendAllText(outputFile, contents + Environment.NewLine);
                                Console.WriteLine("File: {0} was merged successfully!\r", files);
                            }
                    Console.WriteLine("\nDocumentMerger2 closing...\r");
                }
            }
            Console.ReadKey();
        }
    }
}