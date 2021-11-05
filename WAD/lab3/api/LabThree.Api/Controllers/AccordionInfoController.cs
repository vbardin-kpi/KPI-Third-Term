using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LabThree.Api.Dal;
using LabThree.Api.Dal.Entities;
using LabThree.Api.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabThree.Api.Controllers
{
    [ApiController]
    [Route("/api/accordions")]
    public class AccordionInfoController : Controller
    {
        private readonly ApiDbContext _dbContext;

        public AccordionInfoController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<AccordionInfo>> GetAll()
        {
            return await _dbContext.Accordions
                .Select(x => new AccordionInfo()
                {
                    Title = x.Title,
                    Body = x.Body,
                }).ToListAsync();
        }

        [HttpPost]
        [Route("/add")]
        public async Task<StatusCodeResult> AddAccordionInfo(AccordionInfo accordionInfo)
        {
            if (accordionInfo is null) return BadRequest();

            var accordionEnt = new Accordion
            {
                Title = accordionInfo.Title,
                Body = accordionInfo.Body,
            };

            var r =  await _dbContext.Accordions.AddAsync(accordionEnt);
            await _dbContext.SaveChangesAsync();

            return r != null ? Ok() : BadRequest();
        }
    }
}
