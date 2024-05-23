ARG DLL_TO_RUN

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY ./dist .
ARG DLL_TO_RUN
ENTRYPOINT ["dotnet", "$DLL_TO_RUN"]