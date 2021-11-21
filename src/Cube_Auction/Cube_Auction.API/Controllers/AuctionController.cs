using Cube_Auction.Application;
using Cube_Auction.Core.Repositories;
using Cube_Bid.API.Entities;
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
        public async Task<ActionResult<IEnumerable<AuctionResponse>>> GetAuctions_Note_MSSQL()
        {
            var auctions = await _repository.GetAuctions();
            return Ok(auctions);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AuctionResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<AuctionResponse>>> GetAuctionByName_Note_MSSQL(string name)
        {
            var auctions = await _repository.GetAuctionByName(name);
            return Ok(auctions);
        }


        [HttpPost]
        [ProducesResponseType(typeof(AuctionResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostAuctiontToQueue_Note_recordInMSSQL_thenSendToQueue_NotStoredInRedis([FromBody] AuctionCommand command)
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


        [HttpPost]
        [ProducesResponseType(typeof(AuctionResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostOneBidToQueue_Note_StoredInRedis([FromBody] BidCommand command)
        {


            //simulating a mapper from entity to event. At the moment is a speculat matching of attributes
            BidCreationEvent eventMessage = new BidCreationEvent();
            eventMessage.Id = command.Id;
            eventMessage.Amount = command.Amount;
            eventMessage.DateTime = DateTime.UtcNow;
            eventMessage.AuctionName = command.AuctionName;
            eventMessage.AuctionSubscriberName = command.AuctionSubscriberName;

            try
            {
                _eventBus.PublishBidCreation(EventBusConstants.BidCreationQueue, eventMessage); //need to create event object
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing event BID CREATION: {RequestId} from {Name}", eventMessage.Id, "Bid");
                throw;
            }

            return Ok();
        }

        //LD TO BE MOVED IN UT
        [HttpPost]
        [ProducesResponseType(typeof(AuctionResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostManyBidsToQueue_Note_StoredInRedis()
        {

            for (int i = 0; i < 10000; i++)
            {
                //simulating a mapper from entity to event. At the moment is a speculat matching of attributes
                BidCreationEvent eventMessage = new BidCreationEvent();
                eventMessage.Id = "IdBid="+i;
                eventMessage.Amount = 100;
                eventMessage.DateTime = DateTime.UtcNow;
                eventMessage.AuctionName = "a1";
                eventMessage.AuctionSubscriberName = "SubscriberLuca";

                try
                {
                    _eventBus.PublishBidCreation(EventBusConstants.BidCreationQueue, eventMessage); //need to create event object
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR Publishing event BID CREATION: {RequestId} from {Name}", eventMessage.Id, "Bid");
                    throw;
                }
            }
            

            return Ok();
        }

    }
}
