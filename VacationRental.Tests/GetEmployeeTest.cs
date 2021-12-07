using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Models;
using Xunit;

namespace VacationRental.Tests
{
    [Collection("Integration")]
    public class GetEmployeeTest
    {
        private readonly HttpClient _client;

        public GetEmployeeTest(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        //When something is  found
        [Fact]
        public async Task Test1_Get_All_With_Result()
        {
            using (var postRentalResponse = await _client.GetAsync($"/api/v1/employees/1"))
            {
                postRentalResponse.EnsureSuccessStatusCode();
                var employee = await postRentalResponse.Content.ReadAsAsync<Employee>();

                //var getCalendarResult = await postRentalResponse.Content.ReadAsAsync<Employee>();
            }

        }
    }
}
