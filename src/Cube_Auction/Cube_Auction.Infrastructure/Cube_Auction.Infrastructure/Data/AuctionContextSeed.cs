using Cube_Auction.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube_Auction.Infrastructure.Data
{
    // NOTE in order to get the migration I did:
    // - set as startup project Cube_Auction.AP
    // - from "Package manager console" I did select in dropdown "default project" the project "Cuce_Auction.infrastructure"
    //   this because in this project I do have DbContext and Seed of DbContext  
    // - did execure "PM> Add-Migration InitialCreate" and "update-database". Then the table was created in the
    // - db previously ran in a container
    public class AuctionContextSeed
    {
        public static async Task SeedAsync(AuctionContext auctionContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
            // INFO: Run this if using a real database. Used to automaticly migrate docker image of sql server db.
            auctionContext.Database.Migrate();
            //auctionContext.Database.EnsureCreated();

            if (!auctionContext.Auctions.Any())
                {
                auctionContext.Auctions.AddRange(GetPreconfiguredAuctions());
                    await auctionContext.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                if (retryForAvailability < 5)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<AuctionContextSeed>();
                    log.LogError(exception.Message);
                    await SeedAsync(auctionContext, loggerFactory, retryForAvailability);
                }
                throw;
            }


        }



        //LD entities to seed

        private static IEnumerable<Auction> GetPreconfiguredAuctions()
        {
            return new List<Auction>()
            {
                new Auction() { Name = "Auction_001",  ExpirationDateTime = DateTime.UtcNow.AddSeconds(30) },
                new Auction() { Name = "Auction_002",  ExpirationDateTime = DateTime.UtcNow.AddMinutes(1) },
                new Auction() { Name = "Auction_003",  ExpirationDateTime = DateTime.UtcNow.AddMinutes(2) }
            };
        }
    }
}
