namespace PrintRemittance.Core.Models;

public class QueryParametersBase
{
    public int Page { get; set; } = 1;

    public int Size { get; set; } = int.MaxValue; //todo در صورت نیاز به اد شدن صفحه بندی مقدار متغیر به حالت پیشفرض برگدد

}
