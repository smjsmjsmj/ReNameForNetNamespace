using DirLister;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RenameNetNamespace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string foldPath = this.textBox1.Text;
            var files = Test.FindFileDir(foldPath);
            GetFiles(files,this.SourceNamespace.Text,this.TarNamespace.Text);
            MessageBox.Show("转换成功");
        }


        /// <summary>  
        /// 获取路径下所有文件以及子文件夹中文件  
        /// </summary>  
        /// <param name="path">全路径根目录</param>  
        /// <param name="FileList">存放所有文件的全路径</param>  
        /// <param name="RelativePath"></param>  
        /// <returns></returns>  
        public static void GetFiles(string[]files,string srcString ,string tarString)
        {
            foreach (string file in files)
            {
                FileInfo f = new FileInfo(file);
                if (f == null) continue;
                long size = f.Length;
                var stream=f.OpenText();
                string line = "";
                List<string> lines = new List<string>();
                while (!string.IsNullOrEmpty(line = stream.ReadLine()))
                {
                    while (line!=null && !line.Contains("using ") && !line.Contains("namespace "))
                    {
                        lines.Add(line);
                        line = stream.ReadLine();
                        
                    }
                    while (line != null && line.Contains("using "))
                    {
                        
                        if (line.Contains(srcString))
                        {
                            line=line.Replace(srcString,tarString);
                        }
                        lines.Add(line);
                        line = stream.ReadLine();
                    }
                    while(line != null && !line.Contains("namespace "))
                    {
                        lines.Add(line);
                        line = stream.ReadLine();
                    }
                    if (line == null) continue;
                    if(line.Contains("namespace "))
                    line=line.Replace(srcString, tarString);
                    lines.Add(line);
                }
                stream.Close();
                var streamwrite = f.CreateText();
                var linesArr=lines.ToArray();
                foreach(var item in lines)
                {
                    streamwrite.WriteLine(item);
                }
                streamwrite.Close();
            }
        }


        private void select_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                this.textBox1.Text = foldPath;
            }
        }
    }
}
