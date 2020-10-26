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
using System.Xml;
using PdfFileHandler.Interfaces;
using PdfFileHandler.Logic;
using PdfSharp.Pdf;

namespace PdfSplitter
{
    public partial class Form1 : Form
    {
        private string filePath;
        private IPdfDocumentHandler _pdfDocHandler = new PdfDocumentHandler();
        private IPageExtractor _pageExtractor = new PageExtractor();
        private string outputFolder = "c:/PDF";

        public Form1()
        {
            InitializeComponent();
            btnRemoveSelected.Visible = false;
            outputLbl.Text = "";
            lblFilePath.Text = "";
        }

        private void outputBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog openFolderDialog = new FolderBrowserDialog())
            {
                DialogResult result = openFolderDialog.ShowDialog();
                outputLbl.Text = openFolderDialog.SelectedPath;
                outputFolder = openFolderDialog.SelectedPath;
            }
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
           
            Dictionary<string, List<int>> pagesToExtract = CreatePdfFilesList();

            extractAndCreatePdfs(pagesToExtract);

        }

        private void extractAndCreatePdfs(Dictionary<string, List<int>> pagesToExtract)
        {
            if (pagesToExtract.Count != 0)
            {
                PdfDocument pd = _pdfDocHandler.GetPdfDocumentFromPath(filePath);
                foreach (var item in _pageExtractor.PageExtraction(pd, pagesToExtract))
                {
                    PdfDocument newPDF = new PdfDocument();
                    foreach (var page in item.Value)
                    {
                        newPDF.AddPage(page);
                    }
                    CreateOutputFolder();
                    _pdfDocHandler.SavePdfFile(outputFolder + "/" + item.Key + ".pdf", newPDF);
                }
            }
        }

        private Dictionary<string, List<int>> CreatePdfFilesList()
        {
            Dictionary<string, List<int>> pagesToExtract = new Dictionary<string, List<int>>();
            foreach (var listbDatasetItem in listbDataset.Items)
            {
                string[] data = listbDatasetItem.ToString().Split(':');
                if (data.Length != 0)
                {
                    List<int> pageNumbers = PageIdentifier(data[1]);
                    string name = data[0];

                    pagesToExtract.Add(name, pageNumbers);
                    MessageBox.Show(ConvertPageNumbersToString(pageNumbers), "File Name will be: " + name, MessageBoxButtons.OK);
                }
            }
            return pagesToExtract;
        }

        private List<int> PageIdentifier(string pages)
        {
            List<int> pageNumbers = new List<int>();
            foreach (string s in pages.Split(','))
            {
                if (s.Contains('-'))
                {
                    string[] range = s.Split('-');
                    int startIdx = Int32.Parse(range[0]);
                    int endIdx = Int32.Parse(range[1]);
                    while (startIdx <= endIdx)
                    {
                        pageNumbers.Add(startIdx);
                        startIdx++;
                    }
                }
                else
                {
                    pageNumbers.Add(Convert.ToInt32(s));
                }
            }
            return pageNumbers;
        }

        private string ConvertPageNumbersToString(List<int> pageNumbers)
        {
            string fileContent = "";
            foreach (int pn in pageNumbers)
            {
                fileContent += pn + ", ";
            }
            return fileContent;
        }

        private void CreateOutputFolder()
        {
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
        }
       
    }
}
