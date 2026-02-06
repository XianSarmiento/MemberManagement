using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MemberManagement.Application.DTOs;
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

/* HOW IT WORKS:
  This service provides two different file-generation engines under one roof.

  1. CLOSEDXML (Excel Generation):
     - It creates a virtual 'XLWorkbook' and a worksheet named "Members."
     - It manually maps headers to the first row (Cell 1, 1-8).
     - It loops through the members, filling the grid starting from row 2.
     - Finally, it saves the file into a 'MemoryStream' and returns a byte 
       array. This is better than saving to a hard drive because it works 
       instantly in web environments without needing file permissions.

  2. ITEXTSHARP (PDF Generation):
     - It creates a 'Document' object and a 'PdfPTable' with 8 columns.
     - Cells are added sequentially (left-to-right, row-by-row).
     - Because PDFs are "drawn" rather than just filled like a grid, the 
       code opens the document, draws the table, and then closes it to 
       finalize the formatting.

  3. MEMORYSTREAM PATTERN: Both methods use 'MemoryStream'. This is a 
     performance-friendly way to handle binary data in RAM. Once the 
     data is converted to 'byte[]', it can be sent over HTTP as a 
     downloadable file (e.g., application/pdf or application/vnd.ms-excel).

  4. DATE FORMATTING: Note the use of .ToString("yyyy-MM-dd"). This 
     standardizes the date format for the reports so they look clean 
     regardless of the server's regional settings.
*/