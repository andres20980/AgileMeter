@ECHO OFF 
REM ## AZURE LOGIN 
set http_proxy=
set https_proxy=

REM ## Logging in through command line is not supported. Is required to use 'az login' to authenticate through browser.
REM CALL az login -u alberto.muriel.devops@hotmail.com -p Passw0rd10!

REM ## CREATE COINTAINER REGISTRIES AND SHOW CREDENTIALS
CALL az acr create --resource-group AgileMeterResourceGroup --name prometheusgitlabexporter --sku Basic --admin-enabled=true

CALL az acr credential show -g AgileMeterResourceGroup -n prometheusgitlabexporter --query passwords[0].value



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


REM ########## CREATE REGISTRY REPOSITORIES SECRETS IN AKS (REPLACE THE CREDENTIALS AND EMAIL) ##########
kubectl create secret docker-registry prometheus-gitlab-exporter-regsecret --docker-server=prometheusgitlabexporter.azurecr.io --docker-username=prometheusgitlabexporter --docker-password=%prometheusgitlabexporterPassword% --docker-email=alberto.muriel.devops@hotmail.com --namespace=monitoring

REM ## DEPLOY YAML FILES TO AKS
kubectl apply -f "docker-compose-AKS\templates\prometheus-gitlab-exporter.yaml" --validate=false

kubectl get all --namespace=monitoring



TIMEOUT /T 20