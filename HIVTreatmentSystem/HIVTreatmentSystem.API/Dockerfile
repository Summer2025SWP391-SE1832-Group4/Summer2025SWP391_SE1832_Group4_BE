FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY ./publish ./

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080


ENTRYPOINT ["dotnet", "HIVTreatmentSystem.API.dll"]
