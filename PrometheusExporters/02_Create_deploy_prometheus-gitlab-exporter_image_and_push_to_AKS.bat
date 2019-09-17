@ECHO OFF 
REM ## AZURE LOGIN 
set http_proxy=
set https_proxy=

REM CALL az login -u alberto.muriel.devops@hotmail.com -p Passw0rd10!


for /f %%i in ('CALL az acr credential show -g AgileMeterResourceGroup -n prometheusgitlabexporter --query passwords[0].value') do set prometheusgitlabexporterPassword=%%i

ECHO %prometheusgitlabexporterPassword%

REM ## LOGIN IN AZURE CONTAINER REGISTRY FROM DOCKER
docker login prometheusgitlabexporter.azurecr.io -u prometheusgitlabexporter -p %prometheusgitlabexporterPassword%


REM #################  REPLACE CRLF TO LF ON DOCKER-COMPOSE AND DOCKER FILES  #################
REM ## BUILD DOCKER LOCAL IMAGES
docker-compose -f docker-compose-AKS.yml build
docker images

REM ## PUSH LOCAL IMAGES TO AZURE CONTAINER REGISTRY
docker push prometheusgitlabexporter.azurecr.io/prometheus-gitlab-exporter:latest

REM ## LOGIN IN AZURE AND LIST CONTAINER REGISTRY IMAGES 
CALL az acr login --name prometheusgitlabexporter -u prometheusgitlabexporter -p %prometheusgitlabexporterPassword%

CALL az acr repository list --name prometheusgitlabexporter 


kubectl get all --namespace=monitoring

ECHO %URL%
ECHO %TOKEN%
ECHO %POLL_INTERVAL%
ECHO %BIND%


TIMEOUT /T 20