FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY ./dist/apps/artificial.scrum.master/Artificial.Scrum.Master/net8.0 .
ENTRYPOINT ["dotnet", "Artificial.Scrum.Master.dll"]