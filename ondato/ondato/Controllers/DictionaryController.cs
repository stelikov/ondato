using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ondato.Attributes;
using ondato.DTO;
using ondato.Models;
using ondato.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ondato.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKeyAuthorize]
    public class DictionaryController : ControllerBase
    {
        private readonly ILogger<DictionaryController> _logger;
        private readonly IDictionaryService dictionaryService;

        public DictionaryController(ILogger<DictionaryController> logger, IDictionaryService dictionaryService)
        {
            _logger = logger;
            this.dictionaryService = dictionaryService;
        }

        // GET api/values/5  
        [HttpGet("key")]
        public IActionResult Get(string key)
        {
            try
            {
                return Ok(dictionaryService.GetData(key));

            } catch(Exception ex)
            {
                return BadRequest(new Response() { Status = "Error", Error = "Item not Found" });
            }
        }

        // POST api/values  
        [HttpPost]
        public void Create([FromBody] CreateViewModel createViewModel)
        {
            dictionaryService.AddData(createViewModel.Key, createViewModel.Data, createViewModel.Valid);
        }

        // PUT api/values/5  
        [HttpPut]
        public void Append(AppendViewModel appendViewModel)
        {
            dictionaryService.UpdateData(appendViewModel.Key, appendViewModel.Data);
        }

        // DELETE api/values/key 
        [HttpDelete("key")]
        public void Delete(string key)
        {
            dictionaryService.RemoveData(key);
        }
    }
}
