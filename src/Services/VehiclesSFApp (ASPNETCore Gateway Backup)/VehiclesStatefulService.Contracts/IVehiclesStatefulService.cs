using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace VehiclesStatefulService.Contracts
{ 
    public interface IVehiclesStatefulService : IService
    {
        //Task<TweetScore> GetAverageSentimentScore();
        //Task SetTweetSubject(string subject);

        Task<string> GetCounter();
    }

}
