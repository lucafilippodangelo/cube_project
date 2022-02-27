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
        public async Task<ActionResult<IEnumerable<AuctionResponse>>> MSSQL_AuctionsGet()
        {
            var auctions = await _repository.GetAuctions();
            return Ok(auctions);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AuctionResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<AuctionResponse>>> MSSQL_AuctionsAndHistoryGetByName(string name)
        {
            var auctions = await _repository.GetAuctionByName(name);
            return Ok(auctions);
        }

       
        /// <summary>
        /// I use this endpoint to create an auction with relate default history (creation and expire time)
        /// And send the events to a queue Bid service will listen
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(AuctionResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> MSSQL_AuctionCreation([FromBody] AuctionCommand command)
        {
            //LD when creating an auction we do create the auction itseld and the history records(one for create and one for finalise)
            var result = await _repository.PostAuction(command);


            //LD post creation event
            AuctionHistoryCommand anAuctionHistoryCommand = new AuctionHistoryCommand() 
            {  AuctionId = result.Id, 
               AuctionStatus =  AuctionStatus.Created, 
               DateTimeEvent =  DateTime.UtcNow };
            await _repository.PostAuctionHistory(anAuctionHistoryCommand);
            

            //LD post default expire time event. Will default to 30 seconds
            AuctionHistoryCommand anAuctionHistoryCommandTwo = new AuctionHistoryCommand()
            {   AuctionId = result.Id,
                AuctionStatus = AuctionStatus.Finalised,
                DateTimeEvent = anAuctionHistoryCommand.DateTimeEvent.AddSeconds(30),
            };
            await _repository.PostAuctionHistory(anAuctionHistoryCommandTwo);



            //LD posting messages in queues
            try
            {
                //simulating a mapper from entity to event. At the moment is a speculat matching of attributes
                AuctionEvent auctionEventMessage = new AuctionEvent();
                auctionEventMessage.Id = result.Id;
                auctionEventMessage.EventCode = (int)anAuctionHistoryCommand.AuctionStatus;
                auctionEventMessage.EventDateTime = anAuctionHistoryCommand.DateTimeEvent;
                _eventBus.PublishAuctionEvent(EventBusConstants.AuctionEventQueue, auctionEventMessage); 


                //simulating a mapper from entity to event. At the moment is a speculat matching of attributes
                AuctionEvent auctionEventMessageTwo = new AuctionEvent();
                auctionEventMessageTwo.Id = result.Id;
                auctionEventMessageTwo.EventCode = (int)anAuctionHistoryCommandTwo.AuctionStatus;
                auctionEventMessageTwo.EventDateTime = anAuctionHistoryCommandTwo.DateTimeEvent;
                _eventBus.PublishAuctionEvent(EventBusConstants.AuctionEventQueue, auctionEventMessageTwo); 

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing AUCTION EVENT");
                throw;
            }


            return Ok(result);
        }


        #region interaction with redis
        /*
                [HttpPost]
                [ProducesResponseType(typeof(AuctionResponse), (int)HttpStatusCode.OK)]
                public async Task<IActionResult> REDIS_PostOneBidToQueue_Note_StoredInRedis([FromBody] BidCommand command)
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
                        _eventBus.PublishBidCreation(EventBusConstants.BidCreationQueue_Redis, eventMessage); //need to create event object
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "ERROR Publishing event BID CREATION: {RequestId} from {Name}", eventMessage.Id, "Bid");
                        throw;
                    }

                    return Ok();
                }

                [HttpPost]
                [ProducesResponseType(typeof(AuctionResponse), (int)HttpStatusCode.OK)]
                public async Task<IActionResult> REDIS_PostManyBidsToQueue_Note_StoredIn_Redis()
                {
                    //add 5000 bids for "a1"
                    for (int i = 0; i < 5000; i++)
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
                            _eventBus.PublishBidCreation(EventBusConstants.BidCreationQueue_Redis, eventMessage); //need to create event object
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "ERROR Publishing event BID CREATION: {RequestId} from {Name}", eventMessage.Id, "Bid");
                            throw;
                        }
                    }

                    //add 2500 bids for "a2"
                    for (int i = 5001; i < 7500; i++)
                    {
                        //simulating a mapper from entity to event. At the moment is a speculat matching of attributes
                        BidCreationEvent eventMessage = new BidCreationEvent();
                        eventMessage.Id = "IdBid=" + i;
                        eventMessage.Amount = 100;
                        eventMessage.DateTime = DateTime.UtcNow;
                        eventMessage.AuctionName = "a2";
                        eventMessage.AuctionSubscriberName = "SubscriberLuca";

                        try
                        {
                            _eventBus.PublishBidCreation(EventBusConstants.BidCreationQueue_Redis, eventMessage); //need to create event object
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "ERROR Publishing event BID CREATION: {RequestId} from {Name}", eventMessage.Id, "Bid");
                            throw;
                        }
                    }

                    //add 500 bids for "a3"
                    for (int i = 7501; i < 8000; i++)
                    {
                        //simulating a mapper from entity to event. At the moment is a speculat matching of attributes
                        BidCreationEvent eventMessage = new BidCreationEvent();
                        eventMessage.Id = "IdBid=" + i;
                        eventMessage.Amount = 100;
                        eventMessage.DateTime = DateTime.UtcNow;
                        eventMessage.AuctionName = "a3";
                        eventMessage.AuctionSubscriberName = "SubscriberLuca";

                        try
                        {
                            _eventBus.PublishBidCreation(EventBusConstants.BidCreationQueue_Redis, eventMessage); //need to create event object
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "ERROR Publishing event BID CREATION: {RequestId} from {Name}", eventMessage.Id, "Bid");
                            throw;
                        }
                    }

                    return Ok();
                }

                */
        #endregion


        /// <summary>
        /// This method create a lot of bids, is used as a test
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(AuctionResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> MONGO_Post5000BidsToQueue_ThoseBidsWillBeStoredInMongo()
        {
            //5000 "a1"
            for (int i = 0; i < 5000; i++)
            {
                //simulating a mapper from entity to event. At the moment is a speculat matching of attributes
                BidCreationEvent eventMessage = new BidCreationEvent();
                eventMessage.Id = "IdBid=" + i;
                eventMessage.Amount = 100;
                eventMessage.DateTime = DateTime.UtcNow;
                eventMessage.AuctionName = "a1";
                eventMessage.AuctionSubscriberName = "SubscriberLuca";

                try
                {
                    _eventBus.PublishBidCreation(EventBusConstants.BidCreationQueue_Mongo, eventMessage); //need to create event object
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
