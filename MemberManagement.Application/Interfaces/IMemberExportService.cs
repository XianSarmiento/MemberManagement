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