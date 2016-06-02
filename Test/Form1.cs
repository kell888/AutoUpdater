using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using KellAutoUpdater;
using System.Xml;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool bHasError = false;
            IAutoUpdater autoUpdater = new AutoUpdater(Application.ProductName);
            try
            {
                autoUpdater.Update();
            }
            //catch (WebException exp)
            //{
            //    MessageBox.Show("升级：下载过程出现错误：" + exp.Message);
            //    bHasError = true;
            //}
            //catch (XmlException exp)
            //{
            //    bHasError = true;
            //    MessageBox.Show("升级：解析XML配置文件出现错误：" + exp.Message);
            //}
            //catch (NotSupportedException exp)
            //{
            //    bHasError = true;
            //    MessageBox.Show("升级：出现未支持错误：" + exp.Message);
            //}
            //catch (ArgumentException exp)
            //{
            //    bHasError = true;
            //    MessageBox.Show("升级：参数错误：" + exp.Message);
            //}
            catch (Exception exp)
            {
                bHasError = true;
                MessageBox.Show("升级出错：" + exp.Message);
            }
            finally
            {
                if (bHasError)
                {
                    try
                    {
                        autoUpdater.RollBack();
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show("升级回滚失败：" + exp.Message);
                    }
                }
            }
        }
    }
}
