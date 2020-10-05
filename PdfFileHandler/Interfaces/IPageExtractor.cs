using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Pdf;

namespace PdfFileHandler.Interfaces
{
    public interface IPageExtractor
    {
        Dictionary<string, List<PdfPage>> PageExtraction(PdfDocument pd, Dictionary<string, List<int>> pagesToExtract);

        PdfPage SinglePageToExtract(PdfDocument pd, int pageNum);
    }
}
