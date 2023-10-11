using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using boxCompany.Filters;
using boxCompany.TransferModels;

namespace boxCompany.Controllers;

using infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using service;

[ApiController]
public class BoxController : ControllerBase
{

    private readonly ILogger<BoxController> _logger;
    private readonly BoxService _boxService;

    public BoxController(ILogger<BoxController> logger,
        BoxService boxService)
    {
        _logger = logger;
        _boxService = boxService;
    }



    [HttpGet]
    [Route("/api/boxes")]
    public ResponseDto Get()
    {
        return new ResponseDto()
        {
            ResponseData = _boxService.GetBoxFeed()
        };
    }
    
    [HttpPut]
    [ValidationModel]
    [Route("/api/boxes/{boxId}")]
    public ResponseDto Put([FromRoute] int boxId,
        [FromBody] UpdateBoxRequestDto dto)
    {
        HttpContext.Response.StatusCode = 201;
        return new ResponseDto()
        {
            MessageToClient = "Successfully updated",
            ResponseData =
                _boxService.UpdateBox(dto.BoxName, dto.BoxId, dto.BoxDescription, dto.BoxImgUrl)
        };

    }

    [HttpPost]
    [ValidationModel]
    [Route("/api/boxes")]
    public ResponseDto Post([FromBody] CreateBoxRequestDto dto)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status201Created;
        
        return new ResponseDto()
        {
            MessageToClient = "Box created",
            ResponseData = _boxService.CreateBox(dto.BoxName, dto.BoxDescription, dto.BoxImgUrl)
        };
    }
    [HttpDelete]
    [Route("/api/books/{bookId}")]
    public ResponseDto Delete([FromRoute] int boxId)
    {
        _boxService.DeleteBox(boxId);
        return new ResponseDto()
        {
            MessageToClient = "Successfully deleted"
        };

    }
}