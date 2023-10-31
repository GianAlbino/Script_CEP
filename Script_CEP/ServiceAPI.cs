using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script_CEP
{
    public class ServiceAPI
    {
        private static ServiceAPI _service;

        public static ServiceAPI Instance() 
        { 
            _service ??= new ServiceAPI();

            return _service;
        }

        public async Task<bool> UpdateCandidate(Guid id, string zipCode)
        {
            var client = new RestClient("http://localhost:5012/api/");
            var request = new RestRequest($"Candidate/UpdateZipCode?id={id}&zipCode={zipCode}");
            var response = client.ExecuteGet(request);

            Console.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
