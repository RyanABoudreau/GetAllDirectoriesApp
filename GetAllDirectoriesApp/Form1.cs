using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GetAllDirectoriesApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string folderPath = "notYetDefined";
        List<Folder> folderList = new List<Folder>();
        long tempSize = 0;
        
                                                    
        public void button1_Click(object sender, EventArgs e)
        {
            GetRootFolder();
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            folderList.Clear();
            if (GetDirectorySize(folderPath) >= numericUpDown1.Value)
            {
                folderList.Add(new Folder(GetName(textBox1.Text), textBox1.Text, GetDirectorySize(textBox1.Text)));
            }
            GetSubdirectoriesList(folderPath);
            ConvertSubdirectoriesListToTxtFile();
        }
        public void ConvertSubdirectoriesListToTxtFile()
        {
            string header = ("Root Directory Path:" + folderPath + "\nDate/Time document was created:" + DateTime.Now + "\nSize Threshold:>=" + numericUpDown1.Value +"MB\n\nDirectory Size \t \t \t \t  Directory Path \n");
            folderList = folderList.OrderByDescending(x => x.size).ToList();            
            string filePath = folderPath + "\\" + "FolderBeholder_SizeLog_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";           
            using (StreamWriter outputFile = new StreamWriter(filePath))
            {
                outputFile.Write(header);
                        
                foreach (var Folder in folderList)
                {                    
                    outputFile.WriteLine(string.Format("Size:{0} MB,  Path: {2}", ((Folder.size).ToString()).PadLeft(7), Folder.name, Folder.path));
                }
            }
            if (File.Exists(filePath))
            {
                Process.Start("explorer.exe", filePath);
            }
        }
        public void GetRootFolder()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;
                folderPath = textBox1.Text;
            }
        }
        public void GetSubdirectoriesList(string folderPath)
        {            
            try
            {
                foreach (string d in Directory.GetDirectories(folderPath))                    
                {
                    try
                    {
                        int directoryCount = Directory.GetDirectories(d, "*.*", SearchOption.AllDirectories).Length;
                        tempSize = GetDirectorySize(d);
                        if (directoryCount < 1)                        
                        {                       
                            if (tempSize >= numericUpDown1.Value)
                            {
                                folderList.Add(new Folder(GetName(d), (d), tempSize));
                            }
                        }
                        else
                        {
                            try
                            {                            
                                if (tempSize >= numericUpDown1.Value)
                                {
                                    folderList.Add(new Folder(GetName(d), (d), tempSize));
                                    GetSubdirectoriesList(d);
                                }
                                else
                                {
                                    GetSubdirectoriesList(d);
                                }
                            }
                            catch (System.Exception)
                            {
                                
                            }
                        }
                    }
                    catch (System.Exception)
                    {
                        
                    }
                }
            }
            catch (System.Exception)
            {
               
            }

        }
        public string GetName(string folderPath)
        {
            string name = Path.GetFileName(folderPath);
            return name;
        }
        public static long GetDirectorySize(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            long bytes = di.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly).Sum(fi => fi.Length);

            double mb = Convert.ToDouble(bytes) / 1000000;
            return Convert.ToInt64(Math.Round(mb));
        }

    }
}

