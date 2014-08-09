itsUtils
========

Utility library for iTextSharp. It only has a PageEvent to add header, footer, page numbers and watermark to the pages.

Usage
-----

    PdfWriter.PageEvent=new CustomPageEvent(pageNumbers:true, headerMessage:"Testing");
    
In the constructor for CustomPageEvent you can specify the options (shown are the default values):

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
    int footerAlign = Element.ALIGN_LEFT
    
