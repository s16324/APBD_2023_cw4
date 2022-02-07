using cwiczenia_4_s16324.Models;
using cwiczenia_4_s16324.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cwiczenia_4_s16324.Controllers
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalsController : ControllerBase
    {
        private IDatabaseService _dbService;

        public AnimalsController(IDatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult CreateAnimals(Animal newAnimal)
        {
            //_dbService.AddAnimal(newAnimal);
            return Ok(_dbService.AddAnimal(newAnimal));
        }

        [HttpGet("{orderBy}")]
        public IActionResult GetAnimals(string orderBy)
        {
            return Ok(_dbService.GetAnimals(orderBy));
        }
        [HttpGet]
        public IActionResult GetAnimals()
        {
            return Ok(_dbService.GetAnimals());
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnimals(int id)
        {
            return Ok(_dbService.DeleteAnimal(id));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAnimals(Animal newAnimal, int id)
        {
            return Ok(_dbService.UpdateAnimals(newAnimal, id));
        }
    }
}
