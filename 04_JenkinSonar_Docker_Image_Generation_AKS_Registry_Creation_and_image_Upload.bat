@ECHO OFF 
REM ## AZURE LOGIN 
set http_proxy=
set https_proxy=

REM ## Logging in through command line is not supported. Is required to use 'az login' to authenticate through browser.
REM CALL az login -u alberto.muriel.devops@hotmail.com -p Passw0rd10!

REM ## CREATE COINTAINER REGISTRIES AND SHOW CREDENTIALS
CALL az acr create --resource-group AgileMeterResourceGroup --name jenkinsonar --sku Basic --admin-enabled=true
CALL az acr credential show -g AgileMeterResourceGroup -n jenkinsonar --query passwords[0].value

for /f %%i in ('CALL az acr credential show -g AgileMeterResourceGroup -n jenkinsonar --query passwords[0].value') do set jenkinsonarPassword=%%i
ECHO %jenkinsonarPassword%

REM ## LOGIN IN AZURE CONTAINER REGISTRY FROM DOCKER
docker login jenkinsonar.azurecr.io -u jenkinsonar -p %jenkinsonarPassword%

REM #################  REPLACE CRLF TO LF ON DOCKER-COMPOSE AND DOCKER FILES  #################
REM ## BUILD DOCKER LOCAL IMAGES
docker-compose -f JenkinSonar\docker-compose-AKS.yml build
docker images

REM ## PUSH LOCAL IMAGES TO AZURE CONTAINER REGISTRY
docker push jenkinsonar.azurecr.io/jenkinsonar:latest

REM ## LOGIN IN AZURE AND LIST CONTAINER REGISTRY IMAGES 
CALL az acr login --name jenkinsonar -u jenkinsonar -p %jenkinsonarPassword%
CALL az acr repository list --name jenkinsonar 

REM ## CREATE NAMESPACE
kubectl create namespace ci-cd

REM ########## CREATE REGISTRY REPOSITORIES SECRETS IN AKS (REPLACE THE CREDENTIALS AND EMAIL) ##########
kubectl create secret docker-registry jenkinsonar-regsecret --docker-server=jenkinsonar.azurecr.io --docker-username=jenkinsonar --docker-password=%jenkinsonarPassword% --docker-email=alberto.muriel.devops@hotmail.com --namespace=ci-cd

REM ## DEPLOY YAML FILES TO AKS
kubectl apply -f JenkinSonar\docker-compose\templates\ --validate=false
kubectl get all --namespace=ci-cd


TIMEOUT /T 20