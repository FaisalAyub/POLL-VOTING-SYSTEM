using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Entities.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Entities.Exporting
{
    public class PollsExcelExporter : EpPlusExcelExporterBase, IPollsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PollsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPollForViewDto> polls)
        {
            return CreateExcelPackage(
                "Polls.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Polls"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("Option1"),
                        L("Option2"),
                        L("Option3"),
                        L("Option4")
                        );

                    AddObjects(
                        sheet, 2, polls,
                        _ => _.Poll.Title,
                        _ => _.Poll.Option1,
                        _ => _.Poll.Option2,
                        _ => _.Poll.Option3,
                        _ => _.Poll.Option4
                        );

					

                });
        }
    }
}
