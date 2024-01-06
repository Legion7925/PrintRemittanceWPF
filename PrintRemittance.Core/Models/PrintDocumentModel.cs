﻿using ControlPlateText;

namespace PrintRemittance.Core.Models;

public class PrintDocumentModel : AddDocumentModel
{
    public string P1 => PlateNumber.Length > 2 ? PlateNumber.Substring(0, 2).ToFaNumber() : "00";

    public string P2 => PlateNumber.Length > 3 ? PlateNumber.Substring(2, 1).ToFaNumber() : "0";

    public string P3 => PlateNumber.Length > 6 ? PlateNumber.Substring(3, 3).ToFaNumber() : "000";

    public string P4 => PlateNumber.Length >= 8 ? PlateNumber.Substring(6, 2).ToFaNumber() : "00";

    public string PrintNumber { get; set; } = string.Empty;

}
