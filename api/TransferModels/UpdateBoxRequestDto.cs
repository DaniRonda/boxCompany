using System.ComponentModel.DataAnnotations;

namespace boxCompany.TransferModels;

public class UpdateBoxRequestDto
{
    public int BoxId { get; set; }
    [MinLength(5)]
    public string BoxName { get; set; }
    [MinLength(10)]
    public string BoxDescription  { get; set; }
    [Url]
    public string BoxImgUrl { get; set; }  
}