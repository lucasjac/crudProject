using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;
using FluentAssertions;
using AutoFixture;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture; 

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            var countriesInitialData = new List<Country>() { };
            var personsInitialData = new List<Person>() { };

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
            );

            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);

            _countriesService = new CountriesService(dbContext);

            _personService = new PersonsService(dbContext, _countriesService);
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        // When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            // Arrange
            PersonAddRequest? personAddRequest = null;

            //Act
            /*await Assert.ThrowsAsync<ArgumentNullException>(async() =>
            {
                await _personService.AddPerson(personAddRequest);
            });*/

            //await Assert.ThrowsAsync<ArgumentNullException>;

            // Fluent assertion
            Func<Task> action = async () =>
            {
                await _personService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        // When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            // Arrange
            //PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, null as string).Create();

            //Act
            /*await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personService.AddPerson(personAddRequest);
            });*/

            // Fluent assertion
            Func<Task> action = async () =>
            {
                await _personService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        // When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse, wich includes with the newly generated person id
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            // Arrange
            /*PersonAddRequest? personAddRequest = new PersonAddRequest() { 
                PersonName = "Lucas Jacoby",
                Email = "lucas@gmail.com",
                Address = "Sample address",
                CountryId = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@example.com").Create();

            // Act
            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);

            List<PersonResponse> personsList = await _personService.GetAllPersons();

            // Assert
            //Assert.True(personResponseFromAdd.PersonId != Guid.Empty);

            // Fluent assertion
            personResponseFromAdd.PersonId.Should().NotBe(Guid.Empty);

            Assert.Contains(personResponseFromAdd, personsList);
        }

        #endregion

        #region GetPersonPersonId

        // If we supply null as PersonId, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonId_NullPersonId()
        {
            // Arrange
            Guid? personId = null;

            // Act
            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonId(personId);

            // Assert
            Assert.Null(personResponseFromGet);
        }

        // If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonByPersonId_WithPersonId()
        {
            // Arrange
            //CountryAddRequest? countryRequest = new CountryAddRequest() { CountryName = "Canada" };
            CountryAddRequest? countryRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

            /*PersonAddRequest personRequest = new PersonAddRequest()
            {
                PersonName = "Lucas Jacoby",
                Email = "lucas@gmail.com",
                Address = "Sample address",
                CountryId = countryResponse.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false,
            };*/

            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>.With(temp => temp.Email, "email@sample.com").Create();

            PersonResponse personResponseFromAdd = await _personService.AddPerson(personRequest);

            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonId(personResponseFromAdd.PersonId);

            // Assert
            Assert.Equal(personResponseFromAdd, personResponseFromGet);
        }
        #endregion

        #region GetAllPersons

        // The GetAllPersons() should return an empty list by default
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            // Act
            List<PersonResponse> personsFromGet = await _personService.GetAllPersons();

            // Assert
            Assert.Empty(personsFromGet);
        }

        // First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            // Arrange
            //CountryAddRequest? countryRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? countryRequest1 = _fixture.Create<CountryAddRequest>();

            //CountryAddRequest? countryRequest2 = new CountryAddRequest() { CountryName = "Paraguay" };
            CountryAddRequest? countryRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryRequest2);

            /*PersonAddRequest personRequest1 = new PersonAddRequest() {
                PersonName = "Lucas",
                Email = "lucas@gmail.com",
                Gender = GenderOptions.Male,
                Address = "Sample address",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_1@example.com").Create();

            /*PersonAddRequest personRequest2 = new PersonAddRequest()
            {
                PersonName = "Gabriela",
                Email = "gabriela@gmail.com",
                Gender = GenderOptions.Female,
                Address = "Sample address 2",
                CountryId = countryResponse2.CountryId,
                DateOfBirth = DateTime.Parse("2001-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_2@example.com").Create();

            /*PersonAddRequest personRequest3 = new PersonAddRequest()
            {
                PersonName = "Manuel",
                Email = "manuel@gmail.com",
                Gender = GenderOptions.Male,
                Address = "Sample address 3",
                CountryId = countryResponse2.CountryId,
                DateOfBirth = DateTime.Parse("2003-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_3@example.com").Create();

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>() { personRequest1, personRequest2, personRequest3 };

            List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();
            
            foreach(PersonAddRequest personRequest in personRequests)
            {
                PersonResponse personResponse = await _personService.AddPerson(personRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            // Print personResponseListFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach(PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            // Act
            List<PersonResponse> personsListFromGet = await _personService.GetAllPersons();

            // Print personsListFromGet
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponseFromGet in personsListFromGet)
            {
                _testOutputHelper.WriteLine(personResponseFromGet.ToString());
            }

            // Assert
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                Assert.Contains(personResponseFromAdd, personsListFromGet);
            }
        }

        #endregion

        #region GetFilteredPersons

        // If the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            // Arrange
            //CountryAddRequest? countryRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? countryRequest1 = _fixture.Create<CountryAddRequest>();

            //CountryAddRequest? countryRequest2 = new CountryAddRequest() { CountryName = "Paraguay" };
            CountryAddRequest? countryRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryRequest2);

            /*PersonAddRequest personRequest1 = new PersonAddRequest() {
                PersonName = "Lucas",
                Email = "lucas@gmail.com",
                Gender = GenderOptions.Male,
                Address = "Sample address",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_1@example.com").Create();

            /*PersonAddRequest personRequest2 = new PersonAddRequest()
            {
                PersonName = "Gabriela",
                Email = "gabriela@gmail.com",
                Gender = GenderOptions.Female,
                Address = "Sample address 2",
                CountryId = countryResponse2.CountryId,
                DateOfBirth = DateTime.Parse("2001-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_2@example.com").Create();

            /*PersonAddRequest personRequest3 = new PersonAddRequest()
            {
                PersonName = "Manuel",
                Email = "manuel@gmail.com",
                Gender = GenderOptions.Male,
                Address = "Sample address 3",
                CountryId = countryResponse2.CountryId,
                DateOfBirth = DateTime.Parse("2003-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_3@example.com").Create();

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>() { personRequest1, personRequest2, personRequest3 };

            List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest personRequest in personRequests)
            {
                PersonResponse personResponse = await _personService.AddPerson(personRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            // Print personResponseListFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            // Act
            List<PersonResponse> personsListFromSearch = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            // Print personsListFromGet
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponseFromGet in personsListFromSearch)
            {
                _testOutputHelper.WriteLine(personResponseFromGet.ToString());
            }

            // Assert
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                Assert.Contains(personResponseFromAdd, personsListFromSearch);
            }
        }

        // First we will add few persons; and the we will search based on person name with some search string. It should return the matching persons
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            // Arrange
            //CountryAddRequest? countryRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? countryRequest1 = _fixture.Create<CountryAddRequest>();

            //CountryAddRequest? countryRequest2 = new CountryAddRequest() { CountryName = "Paraguay" };
            CountryAddRequest? countryRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryRequest2);

            /*PersonAddRequest personRequest1 = new PersonAddRequest() {
                PersonName = "Lucas",
                Email = "lucas@gmail.com",
                Gender = GenderOptions.Male,
                Address = "Sample address",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Name, "Rahman")
                .With(temp => temp.Email, "someone_1@example.com").Create();

            /*PersonAddRequest personRequest2 = new PersonAddRequest()
            {
                PersonName = "Gabriela",
                Email = "gabriela@gmail.com",
                Gender = GenderOptions.Female,
                Address = "Sample address 2",
                CountryId = countryResponse2.CountryId,
                DateOfBirth = DateTime.Parse("2001-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Name, "Mary")
                .With(temp => temp.Email, "someone_2@example.com").Create();

            /*PersonAddRequest personRequest3 = new PersonAddRequest()
            {
                PersonName = "Manuel",
                Email = "manuel@gmail.com",
                Gender = GenderOptions.Male,
                Address = "Sample address 3",
                CountryId = countryResponse2.CountryId,
                DateOfBirth = DateTime.Parse("2003-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Name, "Lucas")
                .With(temp => temp.Email, "someone_3@example.com").Create();

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>() { personRequest1, personRequest2, personRequest3 };

            List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest personRequest in personRequests)
            {
                PersonResponse personResponse = await _personService.AddPerson(personRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            // Print personResponseListFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            // Act
            List<PersonResponse> personsListFromSearch = await _personService.GetFilteredPersons(nameof(Person.PersonName), "Man");

            // Print personsListFromGet
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponseFromGet in personsListFromSearch)
            {
                _testOutputHelper.WriteLine(personResponseFromGet.ToString());
            }

            // Assert
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                if(personResponseFromAdd.PersonName != null)
                {
                    if(personResponseFromAdd.PersonName.Contains("Man", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(personResponseFromAdd, personsListFromSearch);
                    }
                }
            }
        }

        #endregion

        #region GetSortedPersons

        // When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        [Fact]
        public async Task GetSortedPersons()
        {
            // Arrange
            //CountryAddRequest? countryRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? countryRequest1 = _fixture.Create<CountryAddRequest>();

            //CountryAddRequest? countryRequest2 = new CountryAddRequest() { CountryName = "Paraguay" };
            CountryAddRequest? countryRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryRequest2);

            /*PersonAddRequest personRequest1 = new PersonAddRequest() {
                PersonName = "Lucas",
                Email = "lucas@gmail.com",
                Gender = GenderOptions.Male,
                Address = "Sample address",
                CountryId = countryResponse1.CountryId,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.Email, "someone_1@example.com").Create();

            /*PersonAddRequest personRequest2 = new PersonAddRequest()
            {
                PersonName = "Gabriela",
                Email = "gabriela@gmail.com",
                Gender = GenderOptions.Female,
                Address = "Sample address 2",
                CountryId = countryResponse2.CountryId,
                DateOfBirth = DateTime.Parse("2001-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Mary")
                .With(temp => temp.Email, "someone_2@example.com").Create();

            /*PersonAddRequest personRequest3 = new PersonAddRequest()
            {
                PersonName = "Manuel",
                Email = "manuel@gmail.com",
                Gender = GenderOptions.Male,
                Address = "Sample address 3",
                CountryId = countryResponse2.CountryId,
                DateOfBirth = DateTime.Parse("2003-05-06"),
                ReceiveNewsLetters = true,
            };*/

            PersonAddRequest personRequest3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Email, "someone_3@example.com").Create();

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>() { personRequest1, personRequest2, personRequest3 };

            List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest personRequest in personRequests)
            {
                PersonResponse personResponse = await _personService.AddPerson(personRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            // Print personResponseListFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            List<PersonResponse> allPersons = await _personService.GetAllPersons();

            // Act
            List<PersonResponse> personsListFromSort = await _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            // Print personsListFromGet
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse personResponseFromGet in personsListFromSort)
            {
                _testOutputHelper.WriteLine(personResponseFromGet.ToString());
            }

            personResponseListFromAdd = personResponseListFromAdd.OrderByDescending(temp => temp.PersonName).ToList();

            // Assert
            for(int i = 0; i < personResponseListFromAdd.Count; i++)
            {
                Assert.Equal(personResponseListFromAdd[i], personsListFromSort[i]);
            }
        }

        #endregion

        #region UpdatePerson

        // When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            // Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }

        // When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonId()
        {
            // Arrange
            /*PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() 
            {
                PersonId = Guid.NewGuid(),
            };*/

            PersonUpdateRequest? personUpdateRequest = _fixture.Build<PersonUpdateRequest>().Create();

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _personService.UpdatePerson(personUpdateRequest);
            });

        }


        // When the personName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            // Arrange
            //CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "UK" };
            CountryAddRequest? countryRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryRequest);

            /*PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "John",
                Email = "lucas@gmail.com",
                Address = "Sample address",
                Gender = GenderOptions.Male,
                CountryId = countryResponseFromAdd.CountryId
            };*/
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.Email, "someone_1@example.com").Create();
            PersonResponse personResponseFromAdd = await _personService.AddPerson(personRequest);


            PersonUpdateRequest? personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }

        // First, add a new person and try to update the person name and email
        [Fact]
        public async Task UpdatePerson_PersonFullDetails()
        {
            // Arrange
            //CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "UK" };
            CountryAddRequest? countryRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryRequest);

            /*PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "John",
                CountryId = countryResponseFromAdd.CountryId,
                Address = "Abc Road",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "lucas@example.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };*/
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.Email, "someone_1@example.com").Create();
            PersonResponse personResponseFromAdd = await _personService.AddPerson(personRequest);

            PersonUpdateRequest? personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "William";
            personUpdateRequest.Email = "william@example.com";

            // Act
            PersonResponse personResponseFromUpdate = await _personService.UpdatePerson(personUpdateRequest);

            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonId(personResponseFromUpdate.PersonId);

            // Assert
            Assert.Equal(personResponseFromGet, personResponseFromUpdate);
        }

        #endregion

        #region DeletePerson

        // If you supply a valid PersonId, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonId()
        {
            // Arrange
            //CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);

            /*PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Lucas",
                Address = "Sample Address",
                CountryId = countryResponseFromAdd.CountryId,
                DateOfBirth = Convert.ToDateTime("2010-01-01"),
                Email = "lucas@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };*/
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.Email, "someone_1@example.com").Create();
            PersonResponse personResponseFromAdd = await _personService.AddPerson(personRequest);

            // Act
            bool isDeleted = await _personService.DeletePerson(personResponseFromAdd.PersonId);

            // Assert
            Assert.True(isDeleted);
        }

        // If you supply a invalid PersonId, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonId()
        {
            // Act
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

            // Assert
            Assert.False(isDeleted);
        }

        #endregion
    }
}
