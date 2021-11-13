using Cube_Auction.Application;
using Cube_Auction.Core.Repositories;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cube_Auction.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionRepository _repository;
        private readonly ILogger<AuctionController> _logger;
        private readonly EventBusRabbitMQProducer _eventBus;

        public AuctionController(IAuctionRepository repository, EventBusRabbitMQProducer eventBus, ILogger<AuctionController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AuctionResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<AuctionResponse>>> GetAuctions()
        {
            var auctions = await _repository.GetAuctions();
            return Ok(auctions);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AuctionResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<AuctionResponse>>> GetAuctionByName(string name)
        {
            var auctions = await _repository.GetAuctionByName(name);
            return Ok(auctions);
        }

        //Added for testing purpose
        [HttpPost]
        [ProducesResponseType(typeof(AuctionResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostAuction([FromBody] AuctionCommand command)
        {
            var result = await _repository.PostAuction(command);

            //simulating a mapper from entity to event. At the moment is a speculat matching of attributes
            AuctionCreationEvent eventMessage = new AuctionCreationEvent();
            eventMessage.Id = result.Id;
            eventMessage.Name = result.Name;
            eventMessage.ExpirationDateTime = result.ExpirationDateTime;
            eventMessage.RequestId = Guid.NewGuid();

            try
            {
                _eventBus.PublishAuctionCreation(EventBusConstants.AuctionCreationQueue, eventMessage); //need to create event object
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing event AUCTION CREATION: {RequestId} from {Name}", eventMessage.Id, "Auction");
                throw;
            }

            return Ok(result);
        }

    }
}
