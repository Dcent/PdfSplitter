using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Pdf;

namespace PdfFileHandler.Interfaces
{
   public interface IPdfDocumentHandler
   {
       PdfDocument getPdfDocumentFromPath(string path);

       List<PdfDocument> GetAllPdfDocuments(string folder);
   }
}
