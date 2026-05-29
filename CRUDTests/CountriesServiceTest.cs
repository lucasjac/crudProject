using System;
using System.Collections.Generic;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Moq;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        // Constructor
        public CountriesServiceTest()
        {
            var countriesInitialData = new List<Country>() { };
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
            );

            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            _countriesService = new CountriesService(dbContext);
        }

        #region AddCountry
        // When CountryAddRequest is null, it should ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            // Arrange
            CountryAddRequest? request = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _countriesService.AddCountry(request);
            });
        }

        // When the CountryName is null, it should throw ArgumentExcepetion
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest()
            { CountryName = null };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _countriesService.AddCountry(request);
            });

        }

        // When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            // Arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest()
            { CountryName = "USA" };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });

        }

        // When you supply proper country name, it should insert (add) the country to the existing list of countries
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

            // Act
            CountryResponse response = await _countriesService.AddCountry(request);
            List<CountryResponse> countriesFromGetAllCountries = await _countriesService.GetAllCountries();

            // Assert
            Assert.True(response.CountryId != Guid.Empty);
            Assert.Contains(response, countriesFromGetAllCountries);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        // The List of countries should be empty by default (before adding any countries)
        public async Task GetAllCountries_EmptyList()
        {
            // Acts
            List<CountryResponse> current_countries_response_list = await _countriesService.GetAllCountries();

            // Assert
            Assert.Empty(current_countries_response_list);
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            // Arrange
            List<CountryAddRequest> countryRequestList = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "USA" },
                new CountryAddRequest() { CountryName = "UK" },
            };

            // Act
            List<CountryResponse> countriesListFromAddCountry = new List<CountryResponse>();
            foreach(CountryAddRequest countryRequest in countryRequestList)
            {
                countriesListFromAddCountry.Add(await _countriesService.AddCountry(countryRequest));
            }

            List<CountryResponse> currentCountryResponseList = await _countriesService.GetAllCountries();
            // Read each element from countriesListFromAddCountry
            foreach(CountryResponse expectedCountry in countriesListFromAddCountry)
            {
                Assert.Contains(expectedCountry, currentCountryResponseList);
            }
        }
        #endregion

        #region GetCountryByCountryId
        // If we supply null as CountryId, it should return null also
        [Fact]
        public async Task GetCountryByCountryId_NullCountryId()
        {
            // Arrange
            Guid? countryId = null;

            // Act
            CountryResponse? countryResponseFromGetMethod = await _countriesService.GetCountryByCountryId(countryId);

            // Assert
            Assert.Null(countryResponseFromGetMethod);
        }

        // If we supply a valid CountryId, it should return the matching country details as CountryResponse object
        [Fact]
        public async Task GetCountryByCountryId_ValidCountryId()
        {
            // Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "China" };

            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);

            // Act
            CountryResponse? countryResponseFromGetMethod = await _countriesService.GetCountryByCountryId(countryResponseFromAdd.CountryId);

            // Assert
            Assert.Equal(countryResponseFromGetMethod, countryResponseFromAdd);
        }
        #endregion
    }
}
