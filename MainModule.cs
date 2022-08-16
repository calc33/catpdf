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
                            // missing filename after "-o"
                            throw new ShowUsageException();
                        }
                        if (OutputFileName != null)
                        {
                            // double "-o"
                            throw new ShowUsageException();
                        }
                        OutputFileName = args[i];
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
        private void LogFormat(string format, object arg0)
        {
            if (!LogDetail)
            {
                return;
            }
            Console.Out.WriteLine(string.Format(format, arg0));
        }
        private void LogFormat(string format, params object[] args)
        {
            if (!LogDetail)
            {
                return;
            }
            Console.Out.WriteLine(string.Format(format, args));
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
                    LogFormat(Properties.Resources.ReadingMessage, path);
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
                LogFormat(Properties.Resources.WritingMessage, OutputFileName);
                destination.Save(OutputFileName);
            }
            finally
            {
                destination.Close();
            }
        }
    }
    public class ShowUsageException : Exception
    {
        public ShowUsageException() : base(string.Empty) { }
    }
}
