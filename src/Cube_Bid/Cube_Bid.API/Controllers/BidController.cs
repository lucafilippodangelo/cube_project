using Cube_Bid.API.Entities;
using Cube_Bid.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace Cube_Bid.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IAuctionReposirory _repository;
        private readonly IBidReposirory _bidRepository;
        //private readonly EventBusRabbitMQProducer _eventBus;
        private readonly ILogger<BidController> _logger;

        public BidController(IAuctionReposirory repository, IBidReposirory bidRepository, ILogger<BidController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _bidRepository = bidRepository ?? throw new ArgumentNullException(nameof(_bidRepository));
            //_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            //_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public List<string> TestAuctionInsert()
        {
            return _repository.AuctionInsertTest();
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public void TestAuctionFlushAllDbAStar(string userName)
        {
            _repository.AuctionFlushTest();
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public List<string> GetByKeyPrefixPattern(string pattern)
        {
            return _bidRepository.GetBidsByPattern(pattern);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public string CreateBid(string key, string value)
        {
            return _bidRepository.InsertBid(key,value);
        }

    }
}
