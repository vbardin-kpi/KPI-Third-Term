using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LabThree.Data;
using LabThree.Data.Entities;
using LabThree.Models.Requests;
using LabThree.Models.Responses;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabThree.Controllers;

[ApiController]
[Route($"/api/{ApiRoutingDefaults.CurrentApiVersion}/accordions")]
public class AccordionsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AccordionsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("all")]
    public async Task<List<AccordionResponse>> GetAll()
    {
        return await _dbContext.Accordions.Select(x => new AccordionResponse()
        {
            Id = x.Id,
            Title = x.Title,
            Body = x.Body,
        }).ToListAsync();
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add([FromBody] CreateAccordionRequest accordion)
    {
        if (accordion is null) return BadRequest("Inconsistent data error.");

        try
        {
            var addedEnt = await _dbContext.Accordions.AddAsync(
                new AccordionEntity()
                {
                    Title = accordion.Title,
                    Body = accordion.Body,
                }
            );
            await _dbContext.SaveChangesAsync();

            return Ok(addedEnt.Entity.Id);
        }
        catch (Exception)
        {
            return BadRequest("An error occured while creating an accordion.");
        }
    }

    [HttpPatch]
    [Route("patch")]
    public async Task<IActionResult> Update([FromBody] PatchAccordionRequest patchedAccordion)
    {
        if (patchedAccordion is null)
            return BadRequest("Patched accordion can't be empty. To remove an accordion use /delete endpoint");

        try
        {
            var ent = await _dbContext.Accordions.FirstOrDefaultAsync(x => x.Id == patchedAccordion.Id);

            if (ent is null)
                return NotFound($"Accordion with id {patchedAccordion.Id} is not exists.");

            if (!string.IsNullOrEmpty(patchedAccordion.Title) &&
                !string.IsNullOrWhiteSpace(patchedAccordion.Title))
                ent.Title = patchedAccordion.Title;

            if (!string.IsNullOrEmpty(patchedAccordion.Body) &&
                !string.IsNullOrWhiteSpace(patchedAccordion.Body))
                ent.Body = patchedAccordion.Body;

            _dbContext.Update(ent);
            await _dbContext.SaveChangesAsync();
            return Ok(ent.Id);
        }
        catch (Exception)
        {
            return BadRequest("An error occured while patching the accordion.");
        }
    }

    [HttpDelete]
    [Route("delete/{accordionId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid accordionId)
    {
        try
        {
            var ent = await _dbContext.Accordions.FirstOrDefaultAsync(x => x.Id == accordionId);
            if (ent is null)
                return NotFound($"Accordion with id: {accordionId} wasn't found.");

            _dbContext.Accordions.Remove(ent);
            await _dbContext.SaveChangesAsync();
            return Ok(ent.Id);
        }
        catch (Exception)
        {
            return BadRequest($"An error occured while deleting an accordion with id: {accordionId}");
        }
    }
}