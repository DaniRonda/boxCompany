using System.ComponentModel.DataAnnotations;
using boxCompany.CustomDataAnnotations;

namespace boxCompany.TransferModels;

public class CreateBoxRequestDto
{
    [MinLength(5)]
    public string BoxName { get; set; }
    [MinLength(10)]
    public string BoxDescription  { get; set; }
    [Url]
    public string BoxImgUrl { get; set; }   
    
    
}