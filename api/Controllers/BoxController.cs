﻿namespace boxCompany.Controllers;

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
    public IEnumerable<BoxQuery> Get()
    {
        return _boxService.GetBoxFeed();
    }
}