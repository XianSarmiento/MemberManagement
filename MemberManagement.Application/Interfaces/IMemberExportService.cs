using MemberManagement.Application.DTOs;
using System.Collections.Generic;

namespace MemberManagement.Application.Interfaces
{
    public interface IMemberExportService
    {
        byte[] GenerateExcel(IEnumerable<MemberDTO> members);
        byte[] GeneratePdf(IEnumerable<MemberDTO> members);
    }
}

/* HOW IT WORKS:
  This interface acts as a contract for services that transform member data 
  into binary files for the user to download or print.

  1. INPUT (IEnumerable<MemberDTO>): Both methods take a collection of DTOs. 
     This is important because it means the export service doesn't need to 
     talk to the database—it only cares about formatting the data it is given.

  2. OUTPUT (byte[]): Instead of returning a file saved on a disk, it returns 
     a "byte array." This is the most flexible format for web applications 
     because this raw data can be sent directly to a user's browser, which 
     then prompts them to "Save As" an .xlsx or .pdf file.

  3. SEPARATION OF CONCERNS: By putting these methods in their own interface 
     (rather than inside IMemberService), you follow the "Interface Segregation 
     Principle." A class that only needs to save members doesn't have to 
     know anything about Excel or PDF logic.

  4. PLUGGABLE FORMATTING: Because this is an interface, you can have 
     different implementations. For example, your 'GenerateExcel' might 
     use a library like 'EPPlus' or 'ClosedXML', but the rest of your 
     application doesn't need to know which one is being used.
*/