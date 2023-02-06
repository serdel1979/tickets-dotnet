FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .
ENV DefoultConnection=postgres://gkjwumnl:tLWMgq3NwP8omPqoptLvvY2V4fxt7SA8@kesavan.db.elephantsql.com/gkjwumnl
ENV Secret=cl4v3-r3-s3c43t4-cl4v3-r3-s3c43t4-cl4v3-r3-s3c43t4-cl4v3-r3-s3c43t4-cl4v3-r3-s3c43t4
ENV UsuarioMail=sdlsysatic@gmail.com
ENV PasswordMail=shkrjccurliibsrd
ENV PortMail=587
ENV Ssl=true
ENV DefaultCredential=false

ENTRYPOINT ["dotnet", "tickets.dll"]