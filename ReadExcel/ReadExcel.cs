using OfficeOpenXml;
using Excel = Microsoft.Office.Interop.Excel;

namespace ReadExcel
{
    public class ExcelClass
    {
        readonly Excel.Application excel;
        readonly Excel.Workbook workbook;
        readonly Excel.Worksheet sheet;
    }
}