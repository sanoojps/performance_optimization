using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace performance_optimization
{
    class performanceOptimization
    {

        #region SHQueryRecycleBin

        /*
         * SHQueryRecycleBin function
         * [url]
         * [http://msdn.microsoft.com/en-us/library/windows/desktop/bb762241(v=vs.85).aspx]
         * 
         * Retrieves the size of the Recycle Bin and the number of items in it, 
         * for a specified drive.
         * 
         * HRESULT SHQueryRecycleBin(
    _In_opt_  LPCTSTR pszRootPath,
         * The address of a null-terminated string of maximum length MAX_PATH 
         * to contain the path of the root drive on which the Recycle Bin is located.
         * This parameter can contain the address of a string formatted with the drive, folder, and subfolder names (C:\Windows\System...).
    _Inout_   LPSHQUERYRBINFO pSHQueryRBInfo
         * The address of a SHQUERYRBINFO structure that receives 
         * the Recycle Bin information. 
         * The cbSize member of the structure must be set to the size of the structure 
         * before calling this API.
);
         
         * SHQUERYRBINFO structure
         * [url]
         * [http://msdn.microsoft.com/en-us/library/windows/desktop/bb759803(v=vs.85).aspx]
         * 
         * Contains the size and item count information retrieved by the 
         * SHQueryRecycleBin function.
         * 
         * typedef struct {
  DWORD   cbSize;
         * The size of the structure, in bytes. 
         * This member must be filled in prior to calling the function.
  __int64 i64Size;
         * The total size of all the objects in the specified Recycle Bin, in bytes. 
  __int64 i64NumItems;
         * The total number of items in the specified Recycle Bin.
} SHQUERYRBINFO, *LPSHQUERYRBINFO;
         */


        /// <summary>
        /// SHQUERYRBINFO
        /// </summary>
        [System.Runtime.InteropServices.StructLayoutAttribute
           (System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct SHQUERYRBINFO
        {

            /// DWORD->unsigned int
            public uint cbSize;

            /// __int64
            public long i64Size;

            /// __int64
            public long i64NumItems;
        }


        /// Return Type: HRESULT->LONG->int
        ///pszRootPath: LPCWSTR->WCHAR*
        ///pSHQueryRBInfo: LPSHQUERYRBINFO->_SHQUERYRBINFO*
        [System.Runtime.InteropServices.DllImportAttribute(
            "shell32.dll",
            EntryPoint = "SHQueryRecycleBinW",
            CallingConvention =
            System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int SHQueryRecycleBinW(
            [System.Runtime.InteropServices.InAttribute()]
            [System.Runtime.InteropServices.MarshalAsAttribute(
                System.Runtime.InteropServices.UnmanagedType.LPWStr)] 
            string pszRootPath,
            ref SHQUERYRBINFO pSHQueryRBInfo);

        /// <summary>
        /// queryRecycleBin 
        /// obtain number of items &
        /// total size
        /// 
        /// Function - skeleton
        /// -------------------
        ///  public int queryRecycleBin(
        ///  out int binQueryStatus,
        ///  out long binSize,
        ///  out long binTotalMembers,
        ///  string pszRootPath = @"C:\"
        ///  )
        ///  
        /// </summary>
        /// <param name="binQueryStatus"></param>
        /// <param name="binSize"></param>
        /// <param name="binTotalMembers"></param>
        /// <param name="pszRootPath"></param>
        /// <returns>HRESULT_return_value</returns>
        public int queryRecycleBin(
            out int binQueryStatus,
              out long binSize,
              out long binTotalMembers,
                string pszRootPath = @"C:\"
            )
        {
            ///variable to asssign LastWin32Error
            binQueryStatus = int.MinValue;

            ///variable to assign RecycleBin Size 
            binSize = long.MinValue;

            ///variable to assign total number of bin members
            binTotalMembers = long.MinValue;

            ///SHQueryRecycleBin function return value
            int HRESULT_return_value = int.MinValue;

            ///filling in the SHQUERYRBINFO structure

            SHQUERYRBINFO pSHQueryRBInfo = new SHQUERYRBINFO();
            pSHQueryRBInfo.cbSize = (uint)
                System.Runtime.InteropServices.Marshal.SizeOf(
                typeof(SHQUERYRBINFO));
            
            ///check if drive exists
            if (System.IO.Directory.Exists(pszRootPath))
            {
                int _HRESULT = SHQueryRecycleBinW(pszRootPath, ref pSHQueryRBInfo);
                ///getGetLastWin32Error 
                int errorOfFunc = Convert.ToInt32(
              System.Runtime.InteropServices.Marshal.GetLastWin32Error());

                binQueryStatus = errorOfFunc;

                HRESULT_return_value = _HRESULT;

                ///assign RecycleBin Size
                binSize = pSHQueryRBInfo.i64Size;

                ///assign total number of bin members
                binTotalMembers = pSHQueryRBInfo.i64NumItems;

                System.Diagnostics.Debug.WriteLine("\n" + "pSHQueryRBInfo.cbSize: "
                    + pSHQueryRBInfo.cbSize 
                    );
                System.Diagnostics.Debug.WriteLine("pSHQueryRBInfo.i64Size: "
                    + pSHQueryRBInfo.i64Size
                    );
                System.Diagnostics.Debug.WriteLine("pSHQueryRBInfo.i64NumItems: "
                    + pSHQueryRBInfo.i64NumItems 
                    );
                System.Diagnostics.Debug.WriteLine("a: "
                   + _HRESULT 
                   );
                System.Diagnostics.Debug.WriteLine("errorOfFunc: "
                  + errorOfFunc + "\n"
                  );
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(
                    pszRootPath
                    + "\n"
                    + "Drive letter does not exit"
                     + "\n"
                    );
            }

            return HRESULT_return_value;
        
        }

        #endregion


        #region shellEmptyRecycleBin

        /*
        [url][http://msdn.microsoft.com/en-us/library/windows/desktop/bb762160(v=vs.85).aspx]
     
         HRESULT SHEmptyRecycleBin(
  _In_opt_  HWND hwnd,
         * A handle to the parent window of any dialog boxes 
         * that might be displayed during the operation. 
         * This parameter can be NULL.
  _In_opt_  LPCTSTR pszRootPath,
         * The address of a null-terminated string of maximum length 
         * MAX_PATH that contains the path of the root drive on which the Recycle Bin 
         * is located. 
         * This parameter can contain the address of a string formatted with the drive,
         * folder, and subfolder names, for example c:\windows\system\. 
         * It can also contain an empty string or NULL. 
         * If this value is an empty string or NULL, 
         * all Recycle Bins on all drives will be emptied.
  DWORD dwFlags
         * One or more of the following values.
         * SHERB_NOCONFIRMATION
         * No dialog box confirming the deletion of the objects will be displayed.
         * SHERB_NOPROGRESSUI
         * No dialog box indicating the progress will be displayed.
         * SHERB_NOSOUND
         * No sound will be played when the operation is complete.
         * // flags for SHEmptyRecycleBin
         * //
         * #define SHERB_NOCONFIRMATION    0x00000001
         * #define SHERB_NOPROGRESSUI      0x00000002
         * #define SHERB_NOSOUND           0x00000004
);
         * Return value
         * Type: HRESULT
         * If this function succeeds, 
         * it returns S_OK. 
         * Otherwise, it returns an HRESULT error code.
         * 
         * Minimum supported client
         * Windows 2000 Professional, Windows XP [desktop apps only]
         * 
         * Header
         * Shellapi.h
         * Library
         * Shell32.lib
         * DLL
         * Shell32.dll (version 4.71 or later)
         * Unicode and ANSI names
         * SHEmptyRecycleBinW (Unicode) and 
         * SHEmptyRecycleBinA (ANSI)
         */
        /// <summary>
        /// SHEmptyRecycleBin
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="pszRootPath"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("Shell32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern ulong SHEmptyRecycleBin(
        [In] IntPtr hwnd,
            [param: MarshalAs(UnmanagedType.LPTStr)]
        [In] string pszRootPath,
        [In] uint dwFlags
            );

        private const int SHERB_NOCONFIRMATION = 0x00000001;
        private const int SHERB_NOPROGRESSUI = 0x00000002;
        private const int SHERB_NOSOUND = 0x00000004;
        private const int NONE_OF_THE_ABOVE = 0X00000000; //myown

        /*
//HRESULT return values
//Name	                Description	                            Value
//S_OK	                Operation successful	                0x00000000
//E_ABORT	            Operation aborted	                    0x80004004
//E_ACCESSDENIED	    General access denied error	            0x80070005
//E_FAIL	            Unspecified failure	                    0x80004005
//E_HANDLE	            Handle that is not valid	            0x80070006
//E_INVALIDARG	        One or more arguments are not valid	    0x80070057
//E_NOINTERFACE	        No such interface supported	            0x80004002
//E_NOTIMPL	            Not implemented	                        0x80004001
//E_OUTOFMEMORY	        Failed to allocate necessary memory	    0x8007000E
//E_POINTER	            Pointer that is not valid	            0x80004003
//E_UNEXPECTED	        Unexpected failure	                    0x8000FFFF
        */

        /// <summary>
        /// shellEmptyRecycleBin
        /// (
        ///  out uint binClearStatus,string pszRootPath = @"C:\"
        /// )
        /// </summary>
        /// <param name="binClearStatus">out</param>
        /// <param name="pszRootPath">in</param>
        /// <returns>
        /// uint binClearStatus
        /// LastWin32Error
        /// </returns>
        public ulong shellEmptyRecycleBin(
            out uint binClearStatus,
            string pszRootPath = @"C:\")
        {
            ///variable to asssign LastWin32Error
            binClearStatus = 999999;

            ///SHEmptyRecycleBin function return value
            ulong _SHEmptyRecycleBin = ulong.MinValue;

            ///check if drive exists
            if (System.IO.Directory.Exists(pszRootPath))
            {
                ///empty recycle bin of the specified Drive
                ///no confirmation
                _SHEmptyRecycleBin = SHEmptyRecycleBin(
                IntPtr.Zero,
                  pszRootPath,
                SHERB_NOCONFIRMATION
                );
                ///getGetLastWin32Error 
                int errorOfFunc = Convert.ToInt32(
              System.Runtime.InteropServices.Marshal.GetLastWin32Error());

                binClearStatus = (uint)errorOfFunc;

            }
            else
            {
                //binClearStatus = 1;
                //_SHEmptyRecycleBin = 1;
                System.Diagnostics.Debug.WriteLine(
                   pszRootPath
                   + "\n"
                   + "Drive letter does not exit"
                    + "\n"
                   );
            }

            return _SHEmptyRecycleBin;
        }
        #endregion

    }
}
