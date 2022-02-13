using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Entities.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Entities.Exporting
{
    public class DirectMessagesExcelExporter : EpPlusExcelExporterBase, IDirectMessagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DirectMessagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDirectMessageForViewDto> directMessages)
        {
            return CreateExcelPackage(
                "DirectMessages.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("DirectMessages"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("text"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, directMessages,
                        _ => _.DirectMessage.text,
                        _ => _.UserName
                        );

					

                });
        }
    }
}
