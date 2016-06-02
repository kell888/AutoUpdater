using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace KellAutoUpdater
{
    public class Config
    {
        #region The private fields
        private bool enabled = true;
        private string serverUrl = string.Empty;
        private UpdateFileList updateFileList;
        #endregion

        #region The public property
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        public string ServerUrl
        {
            get { return serverUrl; }
            set { serverUrl = value; }
        }
        public UpdateFileList UpdateFileList
        {
            get { return updateFileList; }
            set { updateFileList = value; }
        }

        public Config()
        {
            updateFileList = new UpdateFileList();
        }

        public Config(string file)
        {
            this.LoadConfig(file);
            if (updateFileList == null)
                updateFileList = new UpdateFileList();
        }
        #endregion

        #region The public method
        public void LoadConfig(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Config));
            using (StreamReader sr = new StreamReader(file))
            {
                Config config = xs.Deserialize(sr) as Config;
                if (config != null)
                {
                    this.Enabled = config.Enabled;
                    this.ServerUrl = config.ServerUrl;
                    this.UpdateFileList = config.UpdateFileList;
                }
            }
        }

        public void SaveConfig(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Config));
            using (StreamWriter sw = new StreamWriter(file))
            {
                xs.Serialize(sw, this);
            }
        }
        #endregion
    }

}
