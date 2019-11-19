using System;
using System.IO;

namespace RectorLocal
{
    class Config
    {
        public static string connection_string = @"Data Source=68.193.38.138,9999\WIN-JHM3H4KID52; Initial Catalog=Dslrentals; User ID=Test; Password=Mp2664311;";
        public static string GetRectorFolderPath()
        {
            string progfiles = ProgramFilesx86();
            string progfolder = progfiles + "\\DSLRentals\\RectorLocal\\";
            if (!Directory.Exists(progfolder))
            {
                Directory.CreateDirectory(progfolder);
            }
            return progfolder;
        }
        public static string GetRectorUpdateFolderPath()
        {
            string progfiles = ProgramFilesx86();
            string progfolder = progfiles + "\\DSLRentals\\RectorUpdate\\";
            if (!Directory.Exists(progfolder))
            {
                Directory.CreateDirectory(progfolder);
            }
            return progfolder;
        }
        public static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }
    }
}
