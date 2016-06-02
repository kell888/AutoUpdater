using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using KellAutoUpdater;
using System.Diagnostics;

namespace PublishUpgrade
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string srcDir;

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices != null && listBox1.SelectedIndices.Count > 0)
            {
                List<int> indexes = new List<int>();
                foreach (int index in listBox1.SelectedIndices)
                {
                    indexes.Add(index);
                }
                indexes.Sort();
                indexes.Reverse();
                for (int i = 0; i < indexes.Count; i++)
                    listBox1.Items.RemoveAt(indexes[i]);
            }
            else
            {
                MessageBox.Show("请选定需要删除的文件！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                if (MessageBox.Show("确定要清空所有文件么？", "清空提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    listBox1.Items.Clear();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "指定应用程序升级文件所在的源目录";
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox1.Items.Clear();
                srcDir = folderBrowserDialog1.SelectedPath;
                string[] files = Directory.GetFiles(srcDir,"*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    //if (!listBox1.Items.Contains(file))
                        listBox1.Items.Add(file);
                    //else
                    //    MessageBox.Show("文件[" + file + "]已经在列表中存在！忽略该文件。");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> files = GetFiles();
            if (files.Count > 0)
            {
                string dir = textBox1.Text.Trim();
                string lastver = textBox2.Text.Trim();
                string url = textBox3.Text.Trim();
                if (string.IsNullOrEmpty(url))
                {
                    MessageBox.Show("下载路径不能为空！");
                    textBox3.Focus();
                    textBox3.SelectAll();
                    return;
                }
                if (!url.EndsWith("\\"))
                    url += "\\";
                try
                {
                    Version ver = new Version(lastver);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("应用程序版本号不合规范：" + ex.Message);
                    textBox2.Focus();
                    textBox2.SelectAll();
                    return;
                }
                if (!string.IsNullOrEmpty(dir))
                {
                    List<RemoteFile> fs = CopyFilesToDir(files, dir, url, lastver, checkBox1.Checked);
                    BuildXml(dir, fs);
                    if (MessageBox.Show("发布成功！" + Environment.NewLine + Environment.NewLine + "是否要立即打开发布目录查看？", "成功提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        Process.Start(dir);
                    }
                }
                else
                {
                    MessageBox.Show("请指定发布的目录！");
                }
            }
            else
            {
                MessageBox.Show("请添加要更新的文件！");
            }
        }

        private List<RemoteFile> CopyFilesToDir(List<string> files, string dir, string urlBase, string lastver, bool needRestart)
        {
            List<RemoteFile> realFiles = new List<RemoteFile>();
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    int index = srcDir.Length;//D:\Test\Test.exe
                    string path = file.Substring(index + 1);
                    FileInfo fi = new FileInfo(file);
                    string dstFile = dir + "\\" + path;
                    string dstDir=Path.GetDirectoryName(dstFile);
                    if (!Directory.Exists(dstDir))
                        Directory.CreateDirectory(dstDir);
                    File.Copy(file, dstFile, true);
                    RemoteFile rf = new RemoteFile(path, urlBase + path, lastver, fi.Length, needRestart);
                    realFiles.Add(rf);
                }
            }
            return realFiles;
        }

        private List<string> GetFiles()
        {
            List<string> files = new List<string>();
            foreach (object o in listBox1.Items)
            {
                files.Add(o.ToString());
            }
            return files;
        }

        private void BuildXml(string dir, List<RemoteFile> files)//, string appName = null)
        {
            if (!Directory.Exists(dir))
            {
                if (MessageBox.Show("发布的目录不存在！是否让程序自动创建该目录？", "创建发布目录提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Directory.CreateDirectory(dir);
                }
            }
            //if (appName == null)
            //    appName = Application.ProductName;
            string filename = dir + "\\" + ConstFile.AUTOUPDATE;
            XmlDocument xDoc = new XmlDocument();
            XmlNode xNode;
            XmlElement xElem;
            try
            {
                XmlDeclaration xmlDeclaration = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xDoc.AppendChild(xmlDeclaration);
                xNode = xDoc.CreateElement("FileList");//appName);
                xDoc.AppendChild(xNode);
                foreach (RemoteFile file in files)
                {
                    xElem = xDoc.CreateElement("File");
                    XmlAttribute attr = xDoc.CreateAttribute("path");
                    attr.Value = file.Path;
                    xElem.Attributes.Append(attr);
                    XmlAttribute attr2 = xDoc.CreateAttribute("url");
                    attr2.Value = file.Url;
                    xElem.Attributes.Append(attr2);
                    XmlAttribute attr3 = xDoc.CreateAttribute("lastver");
                    attr3.Value = file.LastVer;
                    xElem.Attributes.Append(attr3);
                    XmlAttribute attr4 = xDoc.CreateAttribute("size");
                    attr4.Value = file.Size.ToString();
                    xElem.Attributes.Append(attr4);
                    XmlAttribute attr5 = xDoc.CreateAttribute("needRestart");
                    attr5.Value = file.NeedRestart ? "True" : "False";
                    xElem.Attributes.Append(attr5);
                    xNode.AppendChild(xElem);
                }
                xDoc.Save(filename);
            }
            catch (Exception e)
            {
                MessageBox.Show("生成XML配置文件时出错：" + e.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox3.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
