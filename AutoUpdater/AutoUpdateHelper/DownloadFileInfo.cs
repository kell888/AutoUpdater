using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KellAutoUpdater
{
    public class DownloadFileInfo
    {
        #region The private fields
        string downloadUrl = string.Empty;
        string fileName = string.Empty;
        string lastver = string.Empty;
        long size = 0;
        #endregion

        #region The public property
        public string DownloadUrl { get { return downloadUrl; } }
        public string FileFullName { get { return fileName; } }
        public string FileName { get { return Path.GetFileName(FileFullName); } }
        public string LastVer { get { return lastver; } set { lastver = value; } }
        public long Size { get { return size; } }
        #endregion

        #region The constructor of DownloadFileInfo
        public DownloadFileInfo(string url, string name, string ver, long size)
        {
            this.downloadUrl = url;
            this.fileName = name;
            this.lastver = ver;
            this.size = size;
        }
        #endregion

        public override string ToString()
        {
            return FileFullName;
        }
    }
}
