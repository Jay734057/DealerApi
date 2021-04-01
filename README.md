In the web api, no endpoint is provided to create new dealer now and admins. Hence, for testing purposes, some test data is configured.

**test dealer**

1. A
```json
{ "Name" : "A", "Password" : "password" }
```

2. B
```json
{ "Name" : "B", "Password" : "password" }
```

To make you own test data, please configure in [SeedData method](TaskV3.Service/Startup.cs):

```csharp
private static void SeedData(ApiContext context, IAuthenticationProvider authenticationProvider)
        {
            var password = "password";
            authenticationProvider.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var dealer = new Dealer
            {
                Name = "A",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            context.Add(dealer);

            var car = new Car
            {
                Make = "Audi",
                Model = "A4",
                Year = 2020
            };

            context.Add(car);

            var stock = new Stock
            {
                CarId = car.Id,
                DealerId = dealer.Id,
                Amount = 5
            };

            context.Add(stock);

            context.SaveChanges();
        }
```

## To run with docker image

```console
docker pull jianyu734057/dealerapi
docker run -it -p [port]:80 jianyu734057/dealerapi
```

## To run with Terminal

```console
git clone https://github.com/Jay734057/DealerApi.git
cd DealerApi
dotnet restore
dotnet run --project TaskV3.Service
```


## Swagger api document

Please get the service up and running:
* visit https://localhost:[port]/swagger/index.html

## Note
* Unit test covers only CarService, more tests should be provided for DealerService and AuthenticationProvider.
* Logs to be added to the api for better monitoring and problem diagnosis.
* Run test command should be added to the Dockerfile.
