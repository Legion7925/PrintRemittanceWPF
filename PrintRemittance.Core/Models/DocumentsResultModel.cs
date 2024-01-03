using PersianDate.Standard;
using PrintRemittance.Core.Entities;

namespace PrintRemittance.Core.Models;

public class DocumentsResultModel : Document
{
    public string CreatedDateFa => CreatedDate.ToFa();
}
