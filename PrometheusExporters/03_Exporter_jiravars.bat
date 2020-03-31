@ECHO OFF 
REM ## AZURE LOGIN 
set http_proxy=
set https_proxy=

REM ## git clone https://github.com/zerok/jiravars.git

REM ## Logging in through command line is not supported. Is required to use 'az login' to authenticate through browser.
REM CALL az login -u alberto.muriel.devops@hotmail.com -p Passw0rd10!

REM ## CREATE COINTAINER REGISTRIES AND SHOW CREDENTIALS
CALL az acr create --resource-group AgileMeterResourceGroup --name jiravars --sku Basic --admin-enabled=true
CALL az acr credential show -g AgileMeterResourceGroup -n jiravars --query passwords[0].value

for /f %%i in ('CALL az acr credential show -g AgileMeterResourceGroup -n jiravars --query passwords[0].value') do set jiravarsPassword=%%i

ECHO %jiravarsPassword%

REM ## LOGIN IN AZURE CONTAINER REGISTRY FROM DOCKER
docker login jiravars.azurecr.io -u jiravars -p %jiravarsPassword%


REM #################  REPLACE CRLF TO LF ON DOCKER-COMPOSE AND DOCKER FILES  #################
REM ## BUILD DOCKER LOCAL IMAGES
docker-compose -f docker-compose-jiravars-AKS.yml build
docker images

REM ## PUSH LOCAL IMAGES TO AZURE CONTAINER REGISTRY
docker push jiravars.azurecr.io/jiravars-exporter:latest

REM ## LOGIN IN AZURE AND LIST CONTAINER REGISTRY IMAGES 
CALL az acr login --name jiravars -u jiravars -p %jiravarsPassword%

CALL az acr repository list --name jiravars 


REM ########## CREATE REGISTRY REPOSITORIES SECRETS IN AKS (REPLACE THE CREDENTIALS AND EMAIL) ##########
kubectl create secret docker-registry jiravars-exporter-regsecret --docker-server=jiravars.azurecr.io --docker-username=jiravars --docker-password=%jiravarsPassword% --docker-email=alberto.muriel.devops@hotmail.com --namespace=monitoring

REM ## DEPLOY YAML FILES TO AKS
kubectl apply -f "docker-compose-jiravars-AKS\templates\jiravars-exporter.yaml" --validate=false


kubectl get all --namespace=monitoring



TIMEOUT /T 20