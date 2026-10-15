# Service Instance Isolation & Data Consistency Issue

## Problem Summary

This project demonstrates a **Dependency Injection design flaw** where two separate instances of the same service (`CountriesService`) are created — one in the test class and one inside `PersonsService`. This causes data lookups to fail because they query different instances.

### The Bug

**CountryID values work correctly** (they're just copied around), but **CountryName lookups fail** because PersonsService queries its own empty CountriesService instance instead of the test's instance where countries were registered.

---

## Scenario 1: Where It Works (ID Only)

```csharp
// CRUDTests/PersonsServiceTest.cs
[Fact]
public void AddPerson_WithRealCountry_CountryID_Works()
{
	// Arrange: Add country to TEST's CountriesService
	var countryRequest = new CountryAddRequest { CountryName = "Greece" };
	var countryResponse = _countriesService.AddCountry(countryRequest);

	// Create person linked to that country
	var personRequest = new PersonAddRequest
	{
		PersonName = "Matt",
		CountryID = countryResponse.CountryID,  // ✓ This ID is just a value, gets copied fine
		Email = "matt@mail.com",
		Address = "Sample Address",
		DateOfBirth = DateTime.Parse("2000-01-01"),
		Gender = GenderOptions.Male,
		RecieveNewsLetter = true
	};

	// Act
	var result = _personService.AddPerson(personRequest);

	// Assert: CountryID is populated correctly
	Assert.Equal(countryResponse.CountryID, result.CountryID);  // ✓ PASSES
}
```

---

## Scenario 2: Where It Breaks (Name Lookup)

```csharp
// CRUDTests/PersonsServiceTest.cs
[Fact]
public void AddPerson_WithRealCountry_ShouldReturnCountryNameInResponse()
{
	// Arrange: Add country to TEST's CountriesService
	var countryRequest = new CountryAddRequest { CountryName = "Greece" };
	var countryResponse = _countriesService.AddCountry(countryRequest);  // Added to test's instance

	// Create person linked to that country
	var personRequest = new PersonAddRequest
	{
		PersonName = "Matt",
		CountryID = countryResponse.CountryID,  // ID is correct
		Email = "matt@mail.com",
		Address = "Sample Address",
		DateOfBirth = DateTime.Parse("2000-01-01"),
		Gender = GenderOptions.Male,
		RecieveNewsLetter = true
	};

	// Act: Add person via PersonsService
	var result = _personService.AddPerson(personRequest);

	// Printing CountryResponse Variables to Get the _countriesService Data
	_testOutputHelper.WriteLine($"CountryName Added: {countryResponse.CountryName}");
	_testOutputHelper.WriteLine($"CountryID Added: {countryResponse.CountryID}");

	// Printing Varaiables for the country associated with the Person
	_testOutputHelper.WriteLine($"PersonResponse CountryName Added: {result.Country}");
	_testOutputHelper.WriteLine($"PersonResponse CountryID Added: {result.CountryID}");

	// Assert: Country name should be "Greece"
	Assert.NotNull(result.Country);
	Assert.Equal("Greece", result.Country);  // ❌ FAILS - result.Country is NULL

	// Why it fails:
	// 1. "Greece" was added to test's _countriesService
	// 2. PersonsService has its own separate _countriesService (created in its constructor)
	// 3. When AddPerson() calls ConvertPersonToPersonResponse(), it runs:
	//    response.Country = _countriesService.GetCountryByID(response.CountryID)?.CountryName;
	//    ↑ This _countriesService is PersonsService's instance (empty, no "Greece")
	// 4. GetCountryByID returns null -> Country name is null
}
```

---

## Root Cause

**Services\PersonsService.cs (line 13-16):**
```csharp
public PersonsService()
{
	_persons = new List<Person>();
	_countriesService = new CountriesService();  // ❌ Creates its own instance
}
```

**CRUDTests\PersonsServiceTest.cs (line 17-22):**
```csharp
public PersonsServiceTest(ITestOutputHelper testOutputHelper)
{
	_personService = new PersonsService();       // PersonsService creates its own CountriesService
	_countriesService = new CountriesService();  // Test creates a separate CountriesService
	_testOutputHelper = testOutputHelper;
	// ❌ Now there are TWO different CountriesService instances with no shared data
}
```

---

## Solution

Inject `ICountriesService` into `PersonsService` constructor so both use the same instance:

```csharp
// Services/PersonsService.cs
public PersonsService(ICountriesService countriesService)
{
	_persons = new List<Person>();
	_countriesService = countriesService;  // ✓ Uses injected instance
}
```

```csharp
// CRUDTests/PersonsServiceTest.cs
public PersonsServiceTest(ITestOutputHelper testOutputHelper)
{
	_countriesService = new CountriesService();
	_personService = new PersonsService(_countriesService);  // ✓ Same instance for both
	_testOutputHelper = testOutputHelper;
}
```

Now both test and service use the same `CountriesService` instance, and country lookups work correctly.

---

## Key Takeaway

**You cannot hide dependency injection problems with separate instances.** When services need to share data access (like country lookups), they must use the same dependency instance, not create their own parallel instances.
