using Cube_Bid.API.Entities;
using Cube_Bid.API.Repositories;
using Cube_Bid.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cube_Bid.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IAuctionReposirory _repository;
        private readonly IAuctionsHistoryRepositoryRedis _auctionHistoryRepositoryRedis;
        private readonly IBidRepositoryMongo _bidRepositoryMongo;
        private readonly ILogger<BidController> _logger;



        public BidController(IAuctionReposirory repository, IAuctionsHistoryRepositoryRedis auctionHistoryRepositoryRedis, IBidRepositoryMongo bidRepositoryMongo, ILogger<BidController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _auctionHistoryRepositoryRedis = auctionHistoryRepositoryRedis ?? throw new ArgumentNullException(nameof(_auctionHistoryRepositoryRedis));
            _bidRepositoryMongo = bidRepositoryMongo ?? throw new ArgumentNullException(nameof(_bidRepositoryMongo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region redis
        /* */
        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public List<string> REDIS_TestAuctionInsert_ByAPI()
        {
            return _repository.AuctionInsertTest();
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public void REDIS_FlushDb(string userName)
        {
            _repository.AuctionFlushTest();
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public List<string> REDIS_GetByAuctionIdAndEventId(Guid AuctionId, int EventId)
        {
            return _auctionHistoryRepositoryRedis.GetAuctionsHistoriesBYAuctionIdAndEventId(AuctionId, EventId);
        }

        /*
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public string REDIS_CreateBid_ByAPI(string key, string value)
        {
            return _bidRepositoryRedis.InsertBid(key,value);
        }
        */
        #endregion redis

        #region mongo
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Bid>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> MONGO_GetAllBids()
        {
            var bids = await _bidRepositoryMongo.GetAllBids();
            return Ok(bids);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Bid>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> MONGO_GetBidsByAuctionId(Guid anAuctionId)
        {
            var bids = await _bidRepositoryMongo.GetBidsByAuctionId(anAuctionId);
            return Ok(bids);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Bid), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Bid>> MONGO_CreateBid([FromBody] Bid aBid)
        {
         
            await _bidRepositoryMongo.Create(aBid);

            return Ok(aBid);
        }


        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> MONGO_DeleteBidById(Guid id)
        {
            return Ok(await _bidRepositoryMongo.Delete(id));
        }


        [HttpDelete()]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> MONGO_DeleteAll()
        {
            return Ok(await _bidRepositoryMongo.DeleteAll());
        }
        #endregion

    }
}
