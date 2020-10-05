using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PdfFileHandler.Interfaces;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PdfFileHandler.Logic
{
    class PdfDocumentHandler : IPdfDocumentHandler
    {
        public PdfDocument getPdfDocumentFromPath(string path)
        {
            PdfDocument inputDocument = PdfReader.Open(path, PdfDocumentOpenMode.Import);
            return inputDocument;
        }

        public List<PdfDocument> GetAllPdfDocuments(string folder)
        {
            List<PdfDocument> pdfs = new List<PdfDocument>();
            string[] files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                pdfs.Add(inputDocument);
            }

            return pdfs;
        }
    }
}
