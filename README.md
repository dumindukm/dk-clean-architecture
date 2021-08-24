# dk-clean-architecture

# Update DB
dotnet ef migrations add InitialCreate -p ../Clean.Architecture.Infrastructure
dotnet ef database update


# todo
- [ ] Unit of work
- [ ] Api input data validation
- [ ] Domain data validation
- [ ] Outbox pattern

# Reference
Initial code got from https://github.com/ardalis/CleanArchitecture
