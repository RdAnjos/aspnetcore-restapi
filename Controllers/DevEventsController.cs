using AutoMapper;
using AwesomeDevEvents.API.Entities;
using AwesomeDevEvents.API.Models;
using AwesomeDevEvents.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventsDbContext _context;
        private readonly IMapper _mapper;

        public DevEventsController(
                DevEventsDbContext context,
                IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Obter todos eventos
        /// </summary>
        /// <returns>Coleçãao de eventos</returns>
        /// <response code ="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var devEvents = _context.DevEvents.Where(d => !d.IsDeleted).ToList();

            var viewModel = _mapper.Map<List<DevEventViewModel>>(devEvents);

            return Ok(viewModel);
        }


        /// <summary>
        /// Obter um evento
        /// </summary>
        /// <param name="id">Identificador do Evento</param>
        /// <returns>Dados do evento</returns>
        /// <response code ="200">Sucesso</response>
        /// <response code ="400">Não Encontrado</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(Guid id)
        {
            var devEvent = _context.DevEvents
                .Include(d => d.Speakers)
                .SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
                return NotFound();

            var viewModel = _mapper.Map<DevEventViewModel>(devEvent);//Apenas um modelo de saida

            return Ok(viewModel);
        }

        /// <summary>
        /// Cadastrar um evento
        /// </summary>
        /// <remarks>
        /// {"title": "string", "description": "string", "startDate": "2024-09-28T19:39:51.687Z", "endDate": "2024-09-28T19:39:51.687Z"}
        /// </remarks>
        /// <param name="input">Dados do Evento</param>
        /// <returns>Objeto recem criado</returns>
        /// <response code ="201">Sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(DevEventInputModel input)
        {
            var devEvent = _mapper.Map<DevEvent>(input);
            _context.DevEvents.Add(devEvent);
            _context.SaveChanges(); //Usando EF, precisamos usar essa chamada para salvar no banco de dados

            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);
        }


        /// <summary>
        /// Atualizar um evento
        /// </summary>
        /// <remarks>
        ///  { "title": "string", "description": "string", "startDate": "2024-09-28T19:48:14.663Z", "endDate": "2024-09-28T19:48:14.663Z" }
        /// </remarks>
        /// <param name="id">Identificador do Evento</param>
        /// <param name="input">Dados do evento</param>
        /// <returns>Nada</returns>
        /// <response code = "204">Sucesso</response>
        /// <response code = "404">Não Encontrado</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(Guid id, DevEventInputModel input)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(x => x.Id == id);

            if (devEvent == null)
                return NotFound();

            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);

            _context.DevEvents.Update(devEvent);
            _context.SaveChanges();
            return NoContent();

        }

        /// <summary>
        /// Deletar um evento
        /// </summary>
        /// <param name="id">Identificador do evento</param>
        /// <returns>Nada</returns>
        /// <response code = "204">Sucesso</response>
        /// <response code = "404">Não Encontrado</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(x => x.Id == id);
            if (devEvent == null)
                return NotFound();

            devEvent.Delete();

            _context.SaveChanges();
            return NoContent();
        }


    }
}
