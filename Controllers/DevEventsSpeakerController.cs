using AutoMapper;
using AwesomeDevEvents.API.Entities;
using AwesomeDevEvents.API.Models;
using AwesomeDevEvents.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API.Controllers
{
    [Route("api/dev-speakers")]
    [ApiController]
    public class DevEventsSpeakerController : ControllerBase
    {
        private readonly DevEventsDbContext _context;
        private readonly IMapper _mapper;

        public DevEventsSpeakerController(
                DevEventsDbContext context,
                IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /// <summary>
        /// Buscar todos Speakers
        /// </summary>
        /// <param name="id">Identificador do Speaker</param>
        /// <returns> Dados dos Speakers</returns>
        /// <response code ="200">Sucesso</response>
        /// <response code ="400">Não Encontrado</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllSpeaker() 
        {
            var devSpeaker = _context.DevEventSpeakers.FirstOrDefault();

            var viewModel = _mapper.Map<DevEventSpeakerViewModel>(devSpeaker);

            return Ok(viewModel);
        }
        /// <summary>
        /// Buscar todos Speakers
        /// </summary>
        /// <returns> Dados dos Speakers</returns>
        /// <response code ="200">Sucesso</response>
        /// <response code ="400">Não Encontrado</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetSpeakerById(Guid id) 
        {
            var devSpeaker = _context
                                .DevEventSpeakers
                                .SingleOrDefault(x => x.Id == id);

            if (devSpeaker == null)
                return NotFound();

            var viewModel = _mapper.Map<DevEventSpeakerViewModel>(devSpeaker);

            return Ok(viewModel);
        }

        /// <summary>
        /// Cadastrar um Speaker
        /// </summary>
        /// <remarks>
        /// { "name": "string", "talkTitle": "string", "talkDescription": "string", "linkedInProfile": "string" }
        /// </remarks>
        /// <param name="id">Identificador do Evento</param>
        /// <param name="input">Dados do palestrante</param>
        /// <returns>Nada.</returns>
        /// <response code = "204">Sucesso</response>
        /// <response code = "404">Não Encontrado</response>
        [HttpPost]
        [Route("{id}/speakers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PostSpeaker(Guid id, DevEventSpeakerInputModel input)
        {
            var speaker = _mapper.Map<DevEventSpeaker>(input);
            speaker.DevEventId = id;//Passando o valor da Foreign Key

            var devEvent = _context.DevEvents.Any(d => d.Id == id);

            if (!devEvent)
                return NotFound();

            _context.DevEventSpeakers.Add(speaker);
            _context.SaveChanges();

            return NoContent();
        }


        /// <summary>
        /// Atualizar um evento
        /// </summary>
        /// <remarks>
        ///  { "name": "string", "talkTitle": "string", "talkDescription": "string", "linkedInProfile": "string" }
        /// </remarks>
        /// <param name="id">Identificador do Speaker</param>
        /// <param name="input">Dados do Speaker</param>
        /// <returns>Nada</returns>
        /// <response code = "204">Sucesso</response>
        /// <response code = "404">Não Encontrado</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateSpeaker (Guid id, DevEventSpeakerInputModel input) 
        {
            var devEvent = _context.DevEventSpeakers.SingleOrDefault(x => x.Id == id);

            if (devEvent == null)
                return NotFound();

            devEvent.Update(input.Name, input.TalkTitle, input.TalkDescription, input.LinkedInProfile);

            _context.DevEventSpeakers.Update(devEvent);
            _context.SaveChanges();

            return NoContent();
        }


        /// <summary>
        /// Remover um Speaker
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Nada</returns>
        /// <response code = "204">Sucesso</response>
        /// <response code = "404">Não Encontrado</response>/ 
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteSpeakerById(Guid id)
        {
            var devEvent = _context.DevEventSpeakers.SingleOrDefault(x => x.Id == id);

            if (devEvent == null)
                return NotFound();

            _context.Remove(devEvent);

            _context.SaveChanges();

            return NoContent();

        }


    }
}
