using System;
using System.Collections.Generic;
using System.Text;

namespace Cube_Auction.Core.Entities.Base
{
    public interface IEntityBase<TId>
    {
        TId Id { get; }
    }
}
