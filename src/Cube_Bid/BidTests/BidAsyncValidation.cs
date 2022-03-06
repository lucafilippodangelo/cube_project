using Cube_Bid.API;
using Cube_Bid.API.Entities;
using Cube_Bid.API.Repositories;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace BidTests
{
    //https://www.c-sharpcorner.com/article/unit-testing-with-xunit-and-moq-in-asp-net-core/

    
    public class BidAsyncValidation
    {
        

        //MOQ
        private readonly Mock<IAuctionsHistoryRepositoryRedis> service;
        public BidAsyncValidation()
        {
            service = new Mock<IAuctionsHistoryRepositoryRedis>();
        }

        public class TestDataGenerator : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
                {
                    //Case it is NOT VALID because bid is done after auction expires
                    new object[] {
                        new Guid("03359e80-02e2-4dba-9d7d-d941e9d96056"),
                        "03359e80-02e2-4dba-9d7d-d941e9d96056 3 06/03/2022 14:43:34 174",
                        Convert.ToDateTime("06/03/2022 14:43:35"),
                        400,
                        2},

                    //Case it is VALID because bid is done before auction expires
                    new object[] {
                        new Guid("03359e80-02e2-4dba-9d7d-d941e9d96056"),
                        "03359e80-02e2-4dba-9d7d-d941e9d96056 3 06/03/2022 14:43:34 174",
                        Convert.ToDateTime("06/03/2022 14:43:33"),
                        400, 
                        1},
                    
                    //Case it is VALID because bid is done before/"equal date" auction expires AND bidMilliseconds are <= to eventMilliseconds
                    new object[] {
                        new Guid("03359e80-02e2-4dba-9d7d-d941e9d96056"),
                        "03359e80-02e2-4dba-9d7d-d941e9d96056 3 06/03/2022 14:43:34 174",
                        Convert.ToDateTime("06/03/2022 14:43:34"),
                        174,
                        1},

                    //Case it is NOT VALID because bid is done before/"equal date" auction expires AND bidMilliseconds are > to eventMilliseconds
                    new object[] {
                        new Guid("03359e80-02e2-4dba-9d7d-d941e9d96056"),
                        "03359e80-02e2-4dba-9d7d-d941e9d96056 3 06/03/2022 14:43:34 174",
                        Convert.ToDateTime("06/03/2022 14:43:34"),
                        175,
                        2}
                };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void TestBidValidator(Guid eventId, string finalisedEventFromRedis, DateTime aBidDatetime, int aBidDatetimeMillisecond, int expectedResult)
        {
            //Arrange
            List<string> aListOfEvents = new List<string>() { finalisedEventFromRedis };
            

            //Moq -> of a specific event "3" on that "AuctionId"
            // so every time I call "GetAuctionsHistoriesBYAuctionIdAndEventId(aBid.AuctionId, 3)" of the mocked class "IAuctionsHistoryRepositoryRedis"
            // "aListOfEvents" will be returned.
            service.Setup(x => x.GetAuctionsHistoriesBYAuctionIdAndEventId(eventId, 3)).Returns(aListOfEvents);
            BidValidator _bidValidator = new BidValidator(service.Object);


            //Methid call
            var resultFromValidator = _bidValidator.ValidateInputBid(new Bid() {  AuctionId = eventId,  DateTime = aBidDatetime , DateTimeMilliseconds = aBidDatetimeMillisecond});

            Assert.Equal(resultFromValidator, expectedResult);

        }
    }
}



