using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace itsUtils.PageEvents
{
    public class CustomPageEvent : PdfPageEventHelper
    {
        private int _page;
        private readonly string _headerMessage;
        private readonly string _footerMessage;
        private readonly int _headerAlign;
        private readonly int _footerAlign;
        private readonly bool _pageNumbers;
        private readonly string _watermark;
        private readonly BaseColor _watermarkFillColor;
        private readonly BaseColor _watermarkStrokeColor;
        private readonly int _watermarkTextRenderMode;
        private readonly float _angleWatermark;
        private readonly float _watermarkFontSize;
        private readonly Rectangle _margins;
        private static readonly Font FontHf = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9);

        private readonly Dictionary<int, float> _posicions;

        public CustomPageEvent(
            string headerMessage = null,
            string footerMessage = null,
            bool pageNumbers = false,
            string watermark = null,
            float angleWatermark = 45,
            BaseColor watermarkFillColor = null,
            BaseColor watermarkStrokeColor = null,
            float watermarkFontSize = 80,
            int watermarkTextRenderMode = PdfContentByte.TEXT_RENDER_MODE_FILL,
            Rectangle pageSize = null,
            int headerAlign = Element.ALIGN_LEFT,
            int footerAlign = Element.ALIGN_LEFT)
        {
            _headerMessage = headerMessage;
            _watermarkFontSize = watermarkFontSize;
            _watermarkFillColor = watermarkFillColor ?? BaseColor.LIGHT_GRAY;
            _watermarkStrokeColor = watermarkStrokeColor ?? BaseColor.LIGHT_GRAY;
            _watermarkTextRenderMode = watermarkTextRenderMode;
            _angleWatermark = angleWatermark;
            _footerAlign = footerAlign;
            _headerAlign = headerAlign;
            Rectangle pageSize1 = pageSize ?? PageSize.A4;
            _margins = new Rectangle(10, 10, pageSize1.Width - 20, pageSize1.Height - 20);
            _watermark = watermark;
            _pageNumbers = pageNumbers;
            _footerMessage = footerMessage;

            _posicions = new Dictionary<int, float>
                             {
                                 {Element.ALIGN_LEFT, _margins.Left + 10},
                                 {Element.ALIGN_RIGHT, _margins.Right - 10},
                                 {Element.ALIGN_CENTER, (_margins.Left + _margins.Right)/2}
                             };
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            _page++;
            if (_watermark != null)
            {
                var xPosition = (_margins.Left + _margins.Right) / 2;
                var yPosition = (_margins.Top + _margins.Bottom) / 2;
                var under = writer.DirectContentUnder;
                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                under.BeginText();
                under.SetColorFill(_watermarkFillColor);
                under.SetColorStroke(_watermarkStrokeColor);
                under.SetTextRenderingMode(_watermarkTextRenderMode);
                under.SetFontAndSize(baseFont, _watermarkFontSize);
                under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, _watermark, xPosition, yPosition, _angleWatermark);
                under.EndText();
            }
            base.OnStartPage(writer, document);
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            _page = 0;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            if (_headerMessage != null)
                ColumnText.ShowTextAligned(writer.DirectContent, _headerAlign, new Phrase(string.Format("{0}", _headerMessage), FontHf), _posicions[_headerAlign], _margins.Top - 10, 0);
            if (_footerMessage != null)
                ColumnText.ShowTextAligned(writer.DirectContent, _footerAlign, new Phrase(string.Format("{0}", _footerMessage), FontHf), _posicions[_footerAlign], _margins.Bottom + 10, 0);
            if (_pageNumbers)
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase(string.Format("{0}", _page), FontHf), _posicions[Element.ALIGN_RIGHT], _margins.Bottom + 10, 0);
            base.OnEndPage(writer, document);
        }
    }
}