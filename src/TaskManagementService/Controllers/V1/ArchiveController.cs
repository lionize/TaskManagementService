﻿using Lionize.TaskManagement.ApiModels.V1;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace TIKSN.Lionize.TaskManagementService.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class ArchiveController : ControllerBase
    {
        [HttpGet]
        public async Task<ArchiveTask[]> Get()
        {
            throw new NotImplementedException();
        }
    }
}
