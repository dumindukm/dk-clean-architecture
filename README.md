# dk-clean-architecture

# Update DB
dotnet ef migrations add InitialCreate -p ../Clean.Architecture.Infrastructure
dotnet ef database update


# todo
- [x] Unit of work
- [x] Api input data validation
- [ ] Domain data validation
- [x] Outbox pattern

# Reference
Initial code got from https://github.com/ardalis/CleanArchitecture
Data Validation, Outbox pattern https://github.com/dumindukm/sample-dotnet-core-cqrs-api
