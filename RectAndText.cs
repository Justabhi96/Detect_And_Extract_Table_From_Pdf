using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace itextPdfTextCoordinates
{
    public class RectAndText
    {
        public iTextSharp.text.Rectangle Rect;
        public String Text;
        public RectAndText(iTextSharp.text.Rectangle rect, String text)
        {
            this.Rect = rect;
            this.Text = text;
        }

    }
}