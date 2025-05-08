namespace WebUI.Areas.Admin.Models.Personel;
public class QrCodeDto
{
    public string FullName { get; set; }
    public byte[] QRCode { get; set; }

    public string Code { get; set; }
}

