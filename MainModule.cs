using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace catpdf
{
    public class MainModule
    {
        public bool LogDetail { get; private set; }
        public string[] InputFileNames { get; private set; }
        public string OutputFileName { get; private set; }
        public MainModule() { }
        public MainModule(string[] args)
        {
            List<string> files = new List<string>();
            for (int i = 0; i < args.Length; i++)
            {
                string a = args[i];
                switch (a)
                {
                    case "-o":
                        i++;
                        if (args.Length <= i)
                        {
                            // -o の後にファイル名がない
                            throw new ShowUsageException();
                        }
                        if (OutputFileName != null)
                        {
                            // -o を二重に指定した
                            throw new ShowUsageException();
                        }
                        string outfile = args[i];
                        OutputFileName = outfile;
                        break;
                    case "-v":
                        LogDetail = true;
                        break;
                    case "-h":
                    case "--help":
                        throw new ShowUsageException();
                    default:
                        files.Add(a);
                        break;
                }
            }
            if (files.Count == 0)
            {
                throw new ShowUsageException();
            }
            InputFileNames = files.ToArray();
        }

        private void Log(string message)
        {
            if (!LogDetail)
            {
                return;
            }
            Console.Out.WriteLine(message);
        }

        public void Execute()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (File.Exists(OutputFileName))
            {
                File.Delete(OutputFileName);
            }
            PdfDocument destination = new PdfDocument();
            try
            {
                foreach (string path in InputFileNames)
                {
                    Log(path + " を読込中");
                    PdfDocument source = PdfReader.Open(path, PdfDocumentOpenMode.Import);
                    try
                    {
                        foreach (PdfPage page in source.Pages)
                        {
                            destination.AddPage(page);
                        }
                    }
                    finally
                    {
                        source.Close();
                    }
                }
                Log(OutputFileName + " へ書出中");
                destination.Save(OutputFileName);
            }
            finally
            {
                destination.Close();
            }
            //if (File.Exists(OutputFileName))
            //{
            //    File.Delete(OutputFileName);
            //}
            //File.Copy(InputFileNames[0], OutputFileName);
            //PdfDocument destination = PdfReader.Open(OutputFileName, PdfDocumentOpenMode.Modify);
            //try
            //{
            //    for (int i = 1; i < InputFileNames.Length; i++)
            //    {
            //        PdfDocument source = PdfReader.Open(InputFileNames[i], PdfDocumentOpenMode.Import);
            //        try
            //        {

            //            foreach (PdfPage page in source.Pages)
            //            {
            //                destination.AddPage(page);
            //            }
            //        }
            //        finally
            //        {
            //            source.Close();
            //        }
            //    }
            //    destination.Save(OutputFileName);
            //}
            //finally
            //{
            //    destination.Close();
            //}
        }
    }
    public class ShowUsageException : Exception
    {
        public ShowUsageException() : base(string.Empty) { }
    }
}
