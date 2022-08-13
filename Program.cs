using System;

namespace catpdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainModule module = new MainModule(args);
                module.Execute();
            }
            catch (ShowUsageException t)
            {
                if (!string.IsNullOrEmpty(t.Message))
                {
                    Console.Error.WriteLine(t.Message);
                }
                Console.Error.WriteLine(Properties.Resources.Usage);
                Environment.Exit(1);
            }
            catch (Exception t)
            {
                Console.Error.WriteLine(t.Message);
                Environment.Exit(1);
            }
        }
    }
}
