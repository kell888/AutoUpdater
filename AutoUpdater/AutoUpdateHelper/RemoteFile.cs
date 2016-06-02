using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace KellAutoUpdater
{
    public class RemoteFile
    {
        #region The private fields
        private string path = "";
        private string url = "";
        private string lastver = "";
        private long size = 0;
        private bool needRestart = false;
        #endregion

        #region The public property
        public string Path { get { return path; } }
        public string Url { get { return url; } }
        public string LastVer { get { return lastver; } }
        public long Size { get { return size; } }
        public bool NeedRestart { get { return needRestart; } }
        #endregion

        #region The constructor of AutoUpdater
        public RemoteFile(string path, string url, string lastver, long size, bool needRestart)
        {
            this.path = path;
            this.url = url;
            this.lastver = lastver;
            this.size = size;
            this.needRestart = needRestart;
        }
        public RemoteFile(XmlNode node)
        {
            this.path = node.Attributes["path"].Value;
            this.url = node.Attributes["url"].Value;
            this.lastver = node.Attributes["lastver"].Value;
            this.size = Convert.ToInt64(node.Attributes["size"].Value);
            this.needRestart = Convert.ToBoolean(node.Attributes["needRestart"].Value);
        }
        #endregion
    }
}
