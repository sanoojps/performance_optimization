﻿using System;
using System.Collections.Generic;
using System.Text;

namespace performance_optimization
{
    /// <summary>
    /// Class Organization
    /// Constructor - to initialize and set properties
    /// Dispose function - to stop & dispose all objects
    /// Event handler function for all events to be monitored
    /// start monitoring
    /// </summary>
    class FSW_FileSystemWatcher
    {

        ///Instantiating the FileSystemWatcher
        System.IO.FileSystemWatcher _FSW =
                new System.IO.FileSystemWatcher();

        public FSW_FileSystemWatcher()        
        {
            ///setting properties

            setFilters();

            _FSW.Filter = "*.*";

            _FSW.IncludeSubdirectories = true;

            _FSW.InternalBufferSize = 8192;

            _FSW.Path = System.IO.Path.GetTempPath().ToString();

            //Adding event Handlers

            _FSW.Changed += _FSW_Changed;

            _FSW.Created += _FSW_Created;

            _FSW.Deleted += _FSW_Deleted;

            _FSW.Renamed += _FSW_Renamed;

            _FSW.Error += _FSW_Error;

            //_FSW.Disposed += _FSW_Disposed;


        }

        //void _FSW_Disposed(object sender, EventArgs e)
        //{
        //    Dispose();
        //}

        void _FSW_Error(object sender, System.IO.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        void _FSW_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            Console.WriteLine("_FSW_Renamed"
                 + "\n" + e.ChangeType
                 + "\n" + e.FullPath
                 + "\n" + e.Name
                 + "\n" + e.OldFullPath
                 + "\n" + e.OldName
                  );
        }

        void _FSW_Deleted(object sender, System.IO.FileSystemEventArgs e)
        {
           
            Console.WriteLine("_FSW_Deleted"
                +"\n" + e.Name
                + "\n" + e.FullPath
                + "\n" + e.ChangeType
                );

        }

        void _FSW_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            Console.WriteLine("_FSW_Created"
                + "\n" + e.ChangeType
                + "\n" + e.FullPath
                + "\n" + e.Name
                );
        }

        void _FSW_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            Console.WriteLine("_FSW_Changed"
               + "\n" + e.ChangeType
               + "\n" + e.FullPath
               + "\n" + e.Name
               );


            //temp dir Size;

            DirSize nu = new DirSize();

            //for %temp%
            System.IO.DirectoryInfo path =
                new System.IO.DirectoryInfo(System.IO.Path.GetTempPath());
            nu.WalkDirectoryTree(path);

             Console.WriteLine("Dir: " + System.IO.Path.GetTempPath() +
             Environment.NewLine + "Size: " + nu.Number / (1000 * 1000) + " MB ");

            long tempDirSize = nu.Number;



        }


        public void beginWatching()
        {
            try
            {
                _FSW.EnableRaisingEvents = true;
            }

            catch (System.ObjectDisposedException) { }
            catch (System.PlatformNotSupportedException) { }
            catch (System.IO.FileNotFoundException) { }

        }


        private void setFilters()
        {
            try
            {
                //filter onlySizeChange
                _FSW.NotifyFilter =
                    //System.IO.NotifyFilters.Attributes |
                    //System.IO.NotifyFilters.CreationTime |
                    //System.IO.NotifyFilters.DirectoryName |
                    //System.IO.NotifyFilters.FileName |
                    //System.IO.NotifyFilters.LastAccess |
                    //System.IO.NotifyFilters.LastWrite |
                    //System.IO.NotifyFilters.Security |
                    System.IO.NotifyFilters.Size
                    ;
            }

            catch (System.ArgumentException) { }

        }

        public void Dispose()
        {
            //stop monitoring
            _FSW.EnableRaisingEvents = false;
            //dispose the monitor object
            _FSW.Dispose();
        }
    }
}
