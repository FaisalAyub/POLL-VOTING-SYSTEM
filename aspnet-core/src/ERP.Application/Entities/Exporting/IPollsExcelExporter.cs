using System.Collections.Generic;
using ERP.Entities.Dtos;
using ERP.Dto;

namespace ERP.Entities.Exporting
{
    public interface IPollsExcelExporter
    {
        FileDto ExportToFile(List<GetPollForViewDto> polls);
    }
}