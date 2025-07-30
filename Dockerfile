FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything into the build container
COPY . ./

# Restore and publish using the correct project path
RUN dotnet restore LibraryManagementSystem/LibraryManagementSystem.csproj
RUN dotnet publish LibraryManagementSystem/LibraryManagementSystem.csproj -c Release -o /app/out

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/out .

# Expose the port and run the app
EXPOSE 80
ENTRYPOINT ["dotnet", "LibraryManagementSystem.dll"]
