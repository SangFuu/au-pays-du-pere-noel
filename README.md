# Santa Workshop

I created this project in C# with Adnane Serrai as a pair project during my computer science degree.

We developed a simulation for making, packing and delivering gifts. The program manages employees, gifts and sleighs. This project helped me practise object-oriented programming and relationships between classes.

## Run the project

With .NET 8 or a compatible SDK:

```sh
dotnet run --project AuPaysDuPereNoel.csproj
```

The program starts in the terminal with an interactive menu.

## How it works

The simulation follows gifts from production to delivery. It takes sleigh capacity, working time and employee rest into account. The menu lets you configure and restart a simulation.

## Build and tests

```sh
dotnet build AuPaysDuPereNoel.csproj
dotnet run --project tests/Tests.csproj
```

The tests cover deliveries, costs, gift storage and invalid input. They do not replace a full check of every menu interaction.
