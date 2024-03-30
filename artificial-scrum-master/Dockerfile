FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM node:21.7.1-bullseye AS build
WORKDIR /src
COPY . .
RUN wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN apt update
RUN apt install -y dotnet-sdk-8.0

RUN npm install

FROM build AS publish
WORKDIR /src
RUN npx nx build artificial.scrum.master
RUN npx nx build artificial.scrum.master.frontend --configuration=production

COPY ./dist/apps/artificial.scrum.master.frontend/browser ./dist/apps/artificial.scrum.master/Artificial.Scrum.Master/wwwroot

FROM base AS final
WORKDIR /app
COPY ./dist/apps/artificial.scrum.master/Artificial.Scrum.Master/net8.0 .
ENTRYPOINT ["dotnet", "Artificial.Scrum.Master.dll"]