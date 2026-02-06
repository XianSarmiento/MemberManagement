using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MemberManagement.Application.Business;
using MemberManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace MemberManagement.Application.Services
{
    public class MemberExportService : IMemberExportService
    {
        public byte[] GenerateExcel(IEnumerable<MemberDTO> members)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Members");

            // Header
            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "First Name";
            ws.Cell(1, 3).Value = "Last Name";
            ws.Cell(1, 4).Value = "Branch";
            ws.Cell(1, 5).Value = "Birth Date";
            ws.Cell(1, 6).Value = "Address";
            ws.Cell(1, 7).Value = "Contact No";
            ws.Cell(1, 8).Value = "Email";

            int row = 2;
            foreach (var m in members)
            {
                ws.Cell(row, 1).Value = m.MemberID;
                ws.Cell(row, 2).Value = m.FirstName;
                ws.Cell(row, 3).Value = m.LastName;
                ws.Cell(row, 4).Value = m.Branch;
                ws.Cell(row, 5).Value = m.BirthDate?.ToString("yyyy-MM-dd");
                ws.Cell(row, 6).Value = m.Address;
                ws.Cell(row, 7).Value = m.ContactNo;
                ws.Cell(row, 8).Value = m.EmailAddress;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] GeneratePdf(IEnumerable<MemberDTO> members)
        {
            var doc = new iTextSharp.text.Document();
            var stream = new MemoryStream();
            PdfWriter.GetInstance(doc, stream);
            doc.Open();

            var table = new PdfPTable(8) { WidthPercentage = 100 };
            table.AddCell("ID");
            table.AddCell("First Name");
            table.AddCell("Last Name");
            table.AddCell("Branch");
            table.AddCell("Birth Date");
            table.AddCell("Address");
            table.AddCell("Contact No");
            table.AddCell("Email");

            foreach (var m in members)
            {
                table.AddCell(m.MemberID.ToString());
                table.AddCell(m.FirstName);
                table.AddCell(m.LastName);
                table.AddCell(m.Branch);
                table.AddCell(m.BirthDate?.ToString("yyyy-MM-dd") ?? "");
                table.AddCell(m.Address);
                table.AddCell(m.ContactNo);
                table.AddCell(m.EmailAddress);
            }

            doc.Add(table);
            doc.Close();

            return stream.ToArray();
        }
    }
}
