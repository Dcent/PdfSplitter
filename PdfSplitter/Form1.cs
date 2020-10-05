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

namespace PdfSplitter
{
    public partial class Form1 : Form
    {
        private string filePath;

        public Form1()
        {
            InitializeComponent();
            btnRemoveSelected.Visible = false;
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "pdf files (*.pdf)|*.pdf";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    lblFilePath.Text = filePath;
                }
            }
        }

        private void btnAddSet_Click(object sender, EventArgs e)
        {
            if(txtbName.Text.Length != 0 && txtbPages.Text.Length != 0)
            {
                listbDataset.Items.Add($"{txtbName.Text} : {txtbPages.Text}");
                txtbName.Text = "";
                txtbPages.Text = "";
            }
        }

        private void listbDataset_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveSelected.Visible = true;
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            listbDataset.Items.Remove(listbDataset.Items[listbDataset.SelectedIndex]);
            if (listbDataset.Items.Count == 0)
            {
                btnRemoveSelected.Visible = false;
            }
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
           
            Dictionary<string, List<int>> pagesToExtract = new Dictionary<string, List<int>>();
            foreach (var listbDatasetItem in listbDataset.Items)
            {
               string fileContent = "";
               List<int> pageNumbers = new List<int>();
               string[] data = listbDatasetItem.ToString().Split(':');
               if (data.Length != 0)
               {
                   string name = data[0];
                   string pages = data[1];
                   foreach (string s in pages.Split(','))
                   {
                       if (s.Contains('-'))
                       {
                           string[] range = s.Split('-');
                           int startIdx = Int32.Parse(range[0]);
                           int endIdx = Int32.Parse(range[1]);
                           while (startIdx <= endIdx)
                           {
                               fileContent += startIdx.ToString() + " ";
                               pageNumbers.Add(startIdx);
                               startIdx++;
                           }
                       }
                       else
                       {
                           fileContent += s + " ";
                           pageNumbers.Add(Convert.ToInt32(s));
                       }
                   }
                   pagesToExtract.Add(name, pageNumbers);
                   MessageBox.Show(fileContent, "File Name will be: " + name, MessageBoxButtons.OK);
               }
            }
        }
    }
}
