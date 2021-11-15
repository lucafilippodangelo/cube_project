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
        private readonly IBidReposirory _repository;
        //private readonly EventBusRabbitMQProducer _eventBus;
        private readonly ILogger<BidController> _logger;

        public BidController(IBidReposirory repository, ILogger<BidController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            //_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            //_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public List<string> TestBidGet()
        {
            return _repository.BidGetTest();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public List<string> TestBidInsert(string userName)
        {
            return _repository.BidInsertTest();
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public void TestBidFlushAllDb(string userName)
        {
            _repository.BidFlushTest();
        }

    }
}
