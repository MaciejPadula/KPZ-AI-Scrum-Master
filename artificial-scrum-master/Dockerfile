ARG DLL_TO_RUN

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY ./dist .
ARG DLL_TO_RUN
RUN echo "dotnet $DLL_TO_RUN" > run.sh
RUN chmod +x run.sh
ENTRYPOINT ["sh", "run.sh"]