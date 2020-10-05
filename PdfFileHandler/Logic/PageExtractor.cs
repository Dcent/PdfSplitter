using System;
using System.Collections.Generic;
using System.Text;
using PdfFileHandler.Interfaces;
using PdfSharp.Pdf;

namespace PdfFileHandler.Logic
{
    public class PageExtractor : IPageExtractor
    {
        public Dictionary<string, List<PdfPage>> PageExtraction(PdfDocument pd, Dictionary<string, List<int>> pagesToExtract)
        {
            Dictionary<string, List<PdfPage>> extractedPages = new Dictionary<string, List<PdfPage>>();
            int count = pd.PageCount;
            if (count != 0)
            {
                foreach (KeyValuePair<string, List<int>> items in pagesToExtract)
                {
                    List<PdfPage> pages = new List<PdfPage>();
                    foreach (int pageNum in items.Value)
                    {
                        PdfPage page = pd.Pages[pageNum -1];
                        pages.Add(page);
                    }
                    extractedPages.Add(items.Key, pages);
                }
            }
            return extractedPages;
        }

        public PdfPage SinglePageToExtract(PdfDocument pd, int pageNum)
        {
            return pd.Pages[pageNum];
        }
    }
}
