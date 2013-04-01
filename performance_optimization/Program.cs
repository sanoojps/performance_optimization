using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using System.Runtime.InteropServices;
using System.ComponentModel;


namespace performance_optimization
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Console.WriteLine(
                System.IO.Path.GetPathRoot(
                Environment.GetFolderPath(Environment.SpecialFolder.System))
                );
           
            Console.WriteLine(
                Environment.GetFolderPath(Environment.SpecialFolder.Cookies)
                );

            Console.WriteLine(
               Environment.GetFolderPath(
               Environment.SpecialFolder.InternetCache)
               );


            performanceOptimization _perfOps =
                new performanceOptimization();

            //int binQueryStatus;
            //long binSize;
            //long binTotalMembers;


            //int returnValue = _perfOps.queryRecycleBin(
            //     out binQueryStatus,
            //     out binSize,
            //     out binTotalMembers,
            //     @"C:\"
            //     );

            //Console.WriteLine(binQueryStatus
            //    + "\n" + binSize
            //   + "\n" + binTotalMembers
            //+ "\n" + returnValue);


            //uint binClearStatus;

            //ulong return_Value = 
            //    _perfOps.shellEmptyRecycleBin(out binClearStatus);

            FSW_FileSystemWatcher _fsw = new FSW_FileSystemWatcher();
            _fsw.beginWatching();

            Console.ReadLine();
            _fsw.Dispose();
        }
    }
}
